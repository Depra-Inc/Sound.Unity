// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.Sound.Clip;
using Depra.Sound.Parameter;
using UnityEngine;

namespace Depra.Sound.Source
{
	[Serializable]
	public sealed class SceneAudioSourceRef : IAudioSource
	{
		[SerializeField] private GameObject _gameObject;
		private IAudioSource _audioSource;

		private IAudioSource AudioSource => _audioSource ??= _gameObject.GetComponent<IAudioSource>();

		event IAudioSource.PlayDelegate IAudioSource.Started
		{
			add => AudioSource.Started += value;
			remove => AudioSource.Started -= value;
		}

		event IAudioSource.StopDelegate IAudioSource.Stopped
		{
			add => AudioSource.Stopped += value;
			remove => AudioSource.Stopped -= value;
		}

		bool IAudioSource.IsPlaying => AudioSource.IsPlaying;

		IAudioClipParameters IAudioSource.Parameters => AudioSource?.Parameters;

		void IAudioSource.Stop() => AudioSource?.Stop();

		void IAudioSource.Play(IAudioClip clip) => AudioSource?.Play(clip);
	}
}