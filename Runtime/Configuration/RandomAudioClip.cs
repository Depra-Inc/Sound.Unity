// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.Inspector.SerializedReference;
using Depra.Sound.Clip;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Depra.Sound.Configuration
{
	[Serializable]
	[SubtypeAlias(nameof(RandomAudioClip))]
	public sealed class RandomAudioClip : IAudioContainer
	{
		[SubtypeDropdown] [SerializeReference] private IAudioClip[] _clips;

		public RandomAudioClip() { }

		public RandomAudioClip(params IAudioClip[] clips) => _clips = clips;

		IAudioClip IAudioContainer.Next() => _clips[Random.Range(0, _clips.Length)];
	}
}