// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.Sound.Clip;
using Depra.Sound.Parameter;
using UnityEngine;

namespace Depra.Sound.Source
{
	[RequireComponent(typeof(AudioSource))]
	public sealed class UnityAudioSource : MonoBehaviour, IAudioSource
	{
		private AudioSource _unitySource;

		public event IAudioSource.PlayDelegate Started;
		public event IAudioSource.StopDelegate Stopped;

		public bool IsPlaying => UnitySource.isPlaying;
		private AudioSource UnitySource => _unitySource ??= GetComponent<AudioSource>();

		public void Play(IAudioClip clip)
		{
			var unityClip = (UnityAudioClip) clip;
			UnitySource.clip = unityClip;
			UnitySource.Play();

			Started?.Invoke();
			Invoke(nameof(OnFinished), unityClip.Duration);
		}

		public void Stop()
		{
			UnitySource.Stop();
			Stopped?.Invoke(AudioStopReason.STOPPED);
		}

		public IAudioClipParameter GetParameter(Type type) => type switch
		{
			_ when type == typeof(LoopParameter) => new LoopParameter(UnitySource.loop),
			_ when type == typeof(VolumeParameter) => new VolumeParameter(UnitySource.volume),
			_ when type == typeof(PitchParameter) => new PitchParameter(UnitySource.pitch),
			_ when type == typeof(PanParameter) => new PanParameter(UnitySource.panStereo),
			_ => new NullParameter()
		};

		public void SetParameter(IAudioClipParameter parameter)
		{
			switch (parameter)
			{
				case LoopParameter loop:
					UnitySource.loop = loop.Value;
					break;
				case VolumeParameter volume:
					UnitySource.volume = volume.Value;
					break;
				case PitchParameter pitch:
					UnitySource.pitch = pitch.Value;
					break;
				case PanParameter pan:
					UnitySource.panStereo = pan.Value;
					break;
			}
		}

		private void OnFinished() => Stopped?.Invoke(AudioStopReason.FINISHED);
	}
}