// SPDX-License-Identifier: Apache-2.0
// © 2024-2025 Depra <n.melnikov@depra.org>

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
	public sealed class UnityAudioSource : SceneAudioSource, IAudioSource<UnityAudioClip>
	{
		private static readonly Type SUPPORTED_CLIP = typeof(UnityAudioClip);
		private static readonly Type[] SUPPORTED_CLIPS = { SUPPORTED_CLIP };
		private static readonly Type[] SUPPORTED_PARAMETERS =
		{
			typeof(PanParameter),
			typeof(LoopParameter),
			typeof(PitchParameter),
			typeof(EmptyParameter),
			typeof(VolumeParameter),
			typeof(PositionParameter),
			typeof(TransformParameter)
		};

		private AudioSource _source;

		public event Action Started;
		public event Action<AudioStopReason> Stopped;

		public bool IsPlaying => Source.isPlaying;
		public UnityAudioClip Current { get; private set; }

		IAudioClip IAudioSource.Current => Current;
		IEnumerable<Type> IAudioSource.SupportedClips => SUPPORTED_CLIPS;
		private AudioSource Source => _source ??= GetComponent<AudioSource>();

		public void Stop()
		{
			Source.Stop();
			Stopped?.Invoke(AudioStopReason.STOPPED);
		}

		public void Play(UnityAudioClip clip, IEnumerable<IAudioSourceParameter> parameters)
		{
			Source.clip = Current = clip;
			foreach (var parameter in parameters)
			{
				Write(parameter);
			}

			Source.Play();
			Started?.Invoke();
#if SOUND_EVENTS
			Invoke(nameof(OnFinished), clip.Duration);
#endif
		}

		public void Write(IAudioSourceParameter parameter)
		{
			switch (parameter)
			{
				case EmptyParameter:
					break;
				case LoopParameter loop:
					_source.loop = loop.Value;
					break;
				case VolumeParameter volume:
					_source.volume = volume.Value;
					break;
				case PitchParameter pitch:
					_source.pitch = pitch.Value;
					break;
				case PanParameter pan:
					_source.panStereo = pan.Value;
					break;
				case PositionParameter position:
					_source.transform.position = position.Value;
					break;
				case TransformParameter transformation:
					_source.transform.position = transformation.Value.position;
					_source.transform.rotation = transformation.Value.rotation;
					break;
				default:
					VerboseError(
						$"Parameter '{parameter.GetType().Name}' cannot be applied to '{_source.name}' ({nameof(AudioSource)})");
					break;
			}
		}

		public IAudioSourceParameter Read(Type type) => type switch
		{
			_ when type == typeof(LoopParameter) => new LoopParameter(_source.loop),
			_ when type == typeof(PanParameter) => new PanParameter(_source.panStereo),
			_ when type == typeof(PitchParameter) => new PitchParameter(_source.pitch),
			_ when type == typeof(VolumeParameter) => new VolumeParameter(_source.volume),
			_ when type == typeof(TransformParameter) => new TransformParameter(_source.transform),
			_ when type == typeof(PositionParameter) => new PositionParameter(_source.transform.position),
			_ => new NullParameter()
		};

		void IAudioSource.Play(IAudioClip clip, IList<IAudioSourceParameter> parameters)
		{
			Guard.AgainstUnsupportedType(clip.GetType(), SUPPORTED_CLIP);
			Play((UnityAudioClip)clip, parameters);
		}

		IEnumerable<IAudioSourceParameter> IAudioSource.EnumerateParameters() => SUPPORTED_PARAMETERS.Select(Read);

#if SOUND_EVENTS
		private void OnFinished() => Stopped?.Invoke(AudioStopReason.FINISHED);
#endif

		[Conditional("SOUND_DEBUG")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void VerboseError(string message) => Debug.LogErrorFormat(LOG_FORMAT, message);
	}
}