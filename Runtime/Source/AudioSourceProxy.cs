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
		private IAudioSource _source;

		event Action IAudioSource.Started
		{
			add => _source.Started += value;
			remove => _source.Started -= value;
		}

		event Action<AudioStopReason> IAudioSource.Stopped
		{
			add => _source.Stopped += value;
			remove => _source.Stopped -= value;
		}

		bool IAudioSource.IsPlaying => _source.IsPlaying;
		IAudioClip IAudioSource.Current => _source.Current;
		IEnumerable<Type> IAudioSource.SupportedClips => _source.SupportedClips;

		void IAudioSource.Stop() => _source.Stop();
		void IAudioSource.Play(IAudioClip clip, IEnumerable<IAudioSourceParameter> parameters) => _source.Play(clip, parameters);

		IAudioSourceParameter IAudioSource.Read(Type parameterType) => _source.Read(parameterType);
		IEnumerable<IAudioSourceParameter> IAudioSource.EnumerateParameters() => _source.EnumerateParameters();
	}
}