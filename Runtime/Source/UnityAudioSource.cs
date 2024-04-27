// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.Sound.Clip;
using Depra.Sound.Clips;
using Depra.Sound.Source;
using UnityEngine;

namespace Depra.Source
{
	[RequireComponent(typeof(AudioSource))]
	public sealed class UnityAudioSource : MonoBehaviour, IAudioSource
	{
		private AudioSource _unitySource;

		private AudioSource UnitySource => _unitySource ??= GetComponent<AudioSource>();

		public bool IsPlaying => UnitySource.isPlaying;

		public float Volume
		{
			get => UnitySource.volume;
			set => UnitySource.volume = value;
		}

		public void Play(IAudioClip clip)
		{
			var unityClip = clip as UnityAudioClip;
			UnitySource.clip = unityClip;
			//UnitySource.volume = volume;
			//UnitySource.loop = loop;
			UnitySource.Play();
		}

		public void Stop() => UnitySource.Stop();
	}
}