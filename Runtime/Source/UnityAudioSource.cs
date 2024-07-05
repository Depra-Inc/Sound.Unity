// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.Sound.Clip;
using Depra.Sound.Parameter;
using UnityEngine;
using static Depra.Sound.Module;

namespace Depra.Sound.Source
{
	[RequireComponent(typeof(AudioSource))]
	[AddComponentMenu(MENU_PATH + nameof(UnityAudioSource), DEFAULT_ORDER)]
	public sealed class UnityAudioSource : SceneAudioSource, IAudioSource
	{
		private AudioSource _unitySource;
		private UnityAudioClipParameters _parameters;

		public event IAudioSource.PlayDelegate Started;
		public event IAudioSource.StopDelegate Stopped;

		public bool IsPlaying => UnitySource.isPlaying;
		private AudioSource UnitySource => _unitySource ??= GetComponent<AudioSource>();
		public IAudioClipParameters Parameters => _parameters ??= new UnityAudioClipParameters(UnitySource);

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

		private void OnFinished() => Stopped?.Invoke(AudioStopReason.FINISHED);
	}
}