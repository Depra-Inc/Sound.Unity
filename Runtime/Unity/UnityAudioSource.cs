// SPDX-License-Identifier: Apache-2.0
// © 2024-2026 Depra <n.melnikov@depra.org>

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Depra.Sound.Configuration;
using Depra.Sound.Exceptions;
using UnityEngine;
using static Depra.Sound.Module;
using Debug = UnityEngine.Debug;

namespace Depra.Sound.Unity
{
	[RequireComponent(typeof(AudioSource))]
	[AddComponentMenu(MENU_PATH + nameof(UnityAudioSource), DEFAULT_ORDER)]
	public sealed class UnityAudioSource : SceneAudioSource, IAudioSource
	{
		private static readonly Type SUPPORTED_CLIP = typeof(UnityAudioClip);

		private AudioSource _source;

		public event Action Started;
		public event Action<AudioStopReason> Stopped;

		public bool IsPlaying => Source.isPlaying;
		public UnityAudioClip Current { get; private set; }
		IAudioClip IAudioSource.Current => Current;

		private AudioSource Source => _source ??= GetComponent<AudioSource>();

		public void Stop()
		{
			Source.Stop();
			Stopped?.Invoke(AudioStopReason.STOPPED);
		}

		public void Play(IAudioClip clip)
		{
			Guard.AgainstUnsupportedType(clip.GetType(), SUPPORTED_CLIP);
			Source.clip = Current = (UnityAudioClip)clip;

			Source.Play();
			Started?.Invoke();
#if SOUND_EVENTS
			Invoke(nameof(OnFinished), clip.Duration);
#endif
		}

		public void Play(IAudioClip clip, ReadOnlySpan<AudioParameter> staticParams, ReadOnlySpan<AudioParameter> dynamicParams)
		{
			Guard.AgainstUnsupportedType(clip.GetType(), SUPPORTED_CLIP);
			Source.clip = Current = (UnityAudioClip)clip;

			foreach (var parameter in staticParams)
			{
				SetParameter(parameter);
			}

			foreach (var parameter in dynamicParams)
			{
				SetParameter(parameter);
			}
			
			Source.Play();
			Started?.Invoke();
#if SOUND_EVENTS
			Invoke(nameof(OnFinished), clip.Duration);
#endif
		}

		public void SetParameter(in AudioParameter parameter)
		{
			var parameterId = parameter.Id;
			if (parameterId == AudioParameterId.Volume && parameter.Type == AudioParameterType.FLOAT)
			{
				_source.volume = parameter.FloatValue;
			}
			else if (parameterId == AudioParameterId.Loop && parameter.Type == AudioParameterType.BOOL)
			{
				_source.loop = parameter.IntegerValue != 0;
			}
			else if (parameterId == AudioParameterId.Pan && parameter.Type == AudioParameterType.FLOAT)
			{
				_source.panStereo = parameter.FloatValue;
			}
			else if (parameterId == AudioParameterId.Pitch && parameter.Type == AudioParameterType.FLOAT)
			{
				_source.pitch = parameter.FloatValue;
			}
			else if (parameterId == Audio3DParameterId.Position && parameter.Type == AudioParameterType.VECTOR3)
			{
				_source.transform.position = new Vector3(parameter.Float0, parameter.Float1, parameter.Float2);
			}
			else if (parameterId == Audio3DParameterId.Transform && parameter is
				         { Type: AudioParameterType.REFERENCE, ReferenceValue: Transform target })
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