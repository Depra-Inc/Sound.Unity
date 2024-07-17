// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Depra.Sound.Source
{
	[Serializable]
	public sealed class SceneAudioSourceRef : IAudioSource
	{
		[SerializeField] private SceneAudioSource _gameObject;
		private IAudioSource _audioSource;

		event Action IAudioSource.Started
		{
			add => AudioSource.Started += value;
			remove => AudioSource.Started -= value;
		}

		event Action<AudioStopReason> IAudioSource.Stopped
		{
			add => AudioSource.Stopped += value;
			remove => AudioSource.Stopped -= value;
		}

		private IAudioSource AudioSource => _audioSource ??= _gameObject.GetComponent<IAudioSource>();

		bool IAudioSource.IsPlaying => AudioSource.IsPlaying;
		IAudioClip IAudioSource.Current => AudioSource.Current;
		IEnumerable<Type> IAudioSource.SupportedTracks => AudioSource.SupportedTracks;

		void IAudioSource.Stop() => AudioSource?.Stop();
		void IAudioSource.Play(IAudioTrack track) => AudioSource?.Play(track);

		IAudioClipParameter IAudioSource.Read(Type parameterType) => AudioSource.Read(parameterType);
		TParameter IAudioSource.Read<TParameter>() => AudioSource.Read<TParameter>();
		IEnumerable<IAudioClipParameter> IAudioSource.EnumerateParameters() => AudioSource.EnumerateParameters();
	}
}