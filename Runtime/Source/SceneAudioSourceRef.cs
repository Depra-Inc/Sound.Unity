// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using UnityEngine;

namespace Depra.Sound.Source
{
	[Serializable]
	public sealed class SceneAudioSourceRef : IAudioSource, IAudioSourceFactory
	{
		[SerializeField] private SceneAudioSource _gameObject;
		private IAudioSource _source;

		event Action IAudioSource.Started
		{
			add => Source.Started += value;
			remove => Source.Started -= value;
		}

		event Action<AudioStopReason> IAudioSource.Stopped
		{
			add => Source.Stopped += value;
			remove => Source.Stopped -= value;
		}

		bool IAudioSource.IsPlaying => Source.IsPlaying;
		IAudioClip IAudioSource.Current => Source.Current;
		private IAudioSource Source => _source ??= _gameObject.GetComponent<IAudioSource>();

		public void Play(IAudioClip clip) => Source?.Play(clip);

		public void Play(IAudioClip clip, ReadOnlySpan<AudioParameter> staticParams,
			ReadOnlySpan<AudioParameter> dynamicParams) => Source?.Play(clip, staticParams, dynamicParams);

		void IAudioSource.Stop() => Source?.Stop();

		IAudioSource IAudioSourceFactory.Create() => Source;
		void IAudioSourceFactory.Destroy(IAudioSource source) { }
	}
}