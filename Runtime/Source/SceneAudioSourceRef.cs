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

		private IAudioSource Source => _source ??= _gameObject.GetComponent<IAudioSource>();

		bool IAudioSource.IsPlaying => Source.IsPlaying;
		IAudioClip IAudioSource.Current => Source.Current;
		IEnumerable<Type> IAudioSource.SupportedClips => Source.SupportedClips;

		void IAudioSource.Stop() => Source?.Stop();
		void IAudioSource.Play(IAudioClip clip, IEnumerable<IAudioSourceParameter> parameters) => Source?.Play(clip, parameters);

		IAudioSourceParameter IAudioSource.Read(Type parameterType) => Source.Read(parameterType);
		IEnumerable<IAudioSourceParameter> IAudioSource.EnumerateParameters() => Source.EnumerateParameters();
	}
}