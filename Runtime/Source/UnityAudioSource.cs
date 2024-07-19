// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Collections.Generic;
using System.Linq;
using Depra.Sound.Clip;
using Depra.Sound.Configuration;
using Depra.Sound.Exceptions;
using UnityEngine;
using static Depra.Sound.Module;

namespace Depra.Sound.Source
{
	[RequireComponent(typeof(AudioSource))]
	[AddComponentMenu(MENU_PATH + nameof(UnityAudioSource), DEFAULT_ORDER)]
	public sealed class UnityAudioSource : SceneAudioSource, IAudioSource<UnityAudioClip>
	{
		private static readonly Type SUPPORTED_TRACK = typeof(UnityAudioTrack);
		private static readonly Type[] SUPPORTED_TRACKS = { SUPPORTED_TRACK };

		private AudioSource _source;

		public event Action Started;
		public event Action<AudioStopReason> Stopped;

		public bool IsPlaying => Source.isPlaying;
		public UnityAudioClip Current { get; private set; }

		private AudioSource Source => _source ??= GetComponent<AudioSource>();

		IAudioClip IAudioSource.Current => Current;
		IEnumerable<Type> IAudioSource.SupportedTracks => SUPPORTED_TRACKS;

		public void Stop()
		{
			Source.Stop();
			Stopped?.Invoke(AudioStopReason.STOPPED);
		}

		public void Play(IAudioTrack track)
		{
			var clip = track.Play(this);
			Guard.AgainstUnsupportedType(clip.GetType(), SUPPORTED_TRACK);

			Current = (UnityAudioClip) clip;
		}

		public void Play(UnityAudioClip clip, IEnumerable<IAudioSourceParameter> parameters)
		{
			Source.clip = clip;
			foreach (var parameter in parameters)
			{
				Write(parameter);
			}

			Source.Play();
			Started?.Invoke();
			Invoke(nameof(OnFinished), clip.Duration);
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
				default:
					Debug.LogErrorFormat(LOG_FORMAT,
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
			_ when type == typeof(PositionParameter) => new PositionParameter(_source.transform.position),
			_ when type == typeof(RuntimePositionParameter) => new PositionParameter(_source.transform.position),
			_ => new NullParameter()
		};

		private void OnFinished() => Stopped?.Invoke(AudioStopReason.FINISHED);

		private IEnumerable<Type> SupportedTypes() => new[]
		{
			typeof(PanParameter),
			typeof(LoopParameter),
			typeof(PitchParameter),
			typeof(EmptyParameter),
			typeof(VolumeParameter),
			typeof(PositionParameter),
			typeof(RuntimePositionParameter)
		};

		void IAudioSource.Play(IAudioClip clip, IEnumerable<IAudioSourceParameter> parameters)
		{
			Guard.AgainstUnsupportedType(clip.GetType(), SUPPORTED_TRACK);
			Play((UnityAudioClip) clip, parameters);
		}

		IEnumerable<IAudioSourceParameter> IAudioSource.EnumerateParameters() => SupportedTypes().Select(Read);
	}
}