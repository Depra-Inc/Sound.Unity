// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Collections.Generic;
using Depra.SerializeReference.Extensions;
using UnityEngine;
using static Depra.Sound.Module;

namespace Depra.Sound.Source
{
	[AddComponentMenu(MENU_PATH + nameof(AudioSourceProxy), DEFAULT_ORDER)]
	public sealed class AudioSourceProxy : MonoBehaviour, IAudioSource
	{
		[SerializeReferenceDropdown]
		[UnityEngine.SerializeReference]
		private IAudioSource _audioSource;

		event Action IAudioSource.Started
		{
			add => _audioSource.Started += value;
			remove => _audioSource.Started -= value;
		}

		event Action<AudioStopReason> IAudioSource.Stopped
		{
			add => _audioSource.Stopped += value;
			remove => _audioSource.Stopped -= value;
		}

		bool IAudioSource.IsPlaying => _audioSource.IsPlaying;
		IAudioClip IAudioSource.Current => _audioSource.Current;
		IEnumerable<Type> IAudioSource.SupportedTracks => _audioSource.SupportedTracks;

		void IAudioSource.Stop() => _audioSource.Stop();
		void IAudioSource.Play(IAudioTrack track) => _audioSource.Play(track);

		TParameter IAudioSource.Read<TParameter>() => _audioSource.Read<TParameter>();
		IAudioClipParameter IAudioSource.Read(Type parameterType) => _audioSource.Read(parameterType);
		IEnumerable<IAudioClipParameter> IAudioSource.EnumerateParameters() => _audioSource.EnumerateParameters();
	}
}