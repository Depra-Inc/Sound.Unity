// SPDX-License-Identifier: Apache-2.0
// © 2024-2026 Depra <n.melnikov@depra.org>

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using static Depra.Sound.Module;
using Debug = UnityEngine.Debug;

namespace Depra.Sound.Unity.Builtin
{
	[RequireComponent(typeof(AudioSource))]
	[AddComponentMenu(MENU_PATH + nameof(UnityAudioSource), DEFAULT_ORDER)]
	public sealed class UnityAudioSource : SceneAudioSource, IAudioSource
	{
		private static readonly Type SUPPORTED_CLIP = typeof(UnityAudioClip);

		private AudioSource _source;
		private UnityAudioClip _current;

		public event Action Started;
		public event Action<AudioStopReason> Stopped;

		public bool IsPlaying => Source.isPlaying;
		IAudioClip IAudioSource.Current => _current;
		private AudioSource Source => _source ??= GetComponent<AudioSource>();

		public void Stop()
		{
			Source.Stop();
			Stopped?.Invoke(AudioStopReason.STOPPED);
		}

		public void Play(IAudioClip clip)
		{
			Guard.AgainstUnsupportedType(clip, SUPPORTED_CLIP);
			Source.clip = _current = (UnityAudioClip)clip;

			Source.Play();
			Started?.Invoke();
#if SOUND_EVENTS
			Invoke(nameof(OnFinished), clip.Duration);
#endif
		}

		public void Play(IAudioClip clip, ReadOnlySpan<AudioParam> parameters)
		{
			Guard.AgainstUnsupportedType(clip, SUPPORTED_CLIP);
			Source.clip = _current = (UnityAudioClip)clip;

			foreach (var parameter in parameters)
			{
				SetParameter(parameter);
			}

			Source.Play();
			Started?.Invoke();
#if SOUND_EVENTS
			Invoke(nameof(OnFinished), clip.Duration);
#endif
		}

		private void SetParameter(in AudioParam parameter)
		{
			var parameterId = parameter.Id;
			if (parameterId == AudioParamId.Volume && parameter.Type == AudioParamType.FLOAT)
			{
				_source.volume = parameter.FloatValue;
			}
			else if (parameterId == AudioParamId.Loop && parameter.Type == AudioParamType.BOOL)
			{
				_source.loop = parameter.IntegerValue != 0;
			}
			else if (parameterId == AudioParamId.Pan && parameter.Type == AudioParamType.FLOAT)
			{
				_source.panStereo = parameter.FloatValue;
			}
			else if (parameterId == AudioParamId.Pitch && parameter.Type == AudioParamType.FLOAT)
			{
				_source.pitch = parameter.FloatValue;
			}
			else if (parameterId == UnityAudioParamId.Position && parameter.Type == AudioParamType.FLOAT3)
			{
				_source.transform.position = new Vector3(parameter.Float0, parameter.Float1, parameter.Float2);
			}
			else if (parameterId == UnityAudioParamId.Transform && parameter is
				         { Type: AudioParamType.REFERENCE, ReferenceValue: Transform target })
			{
				_source.transform.position = target.position;
				_source.transform.rotation = target.rotation;
			}
			else
			{
				VerboseError($"Parameter '{parameterId}' has unexpected type '{parameter.Type}' for '{_source.name}'");
			}
		}

#if SOUND_EVENTS
		private void OnFinished() => Stopped?.Invoke(AudioStopReason.FINISHED);
#endif

		[Conditional("SOUND_DEBUG")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void VerboseError(string message) => Debug.LogErrorFormat(LOG_FORMAT, message);
	}
}