// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.SerializeReference.Extensions;
using Depra.Sound.Clip;
using Random = UnityEngine.Random;

namespace Depra.Sound.Configuration
{
	[Serializable]
	[SerializeReferenceMenuPath(nameof(RandomAudioClip))]
	public sealed class RandomAudioClip : IAudioContainer
	{
		[SerializeReferenceDropdown]
		[UnityEngine.SerializeReference]
		private IAudioClip[] _clips;

		public RandomAudioClip() { }

		public RandomAudioClip(params IAudioClip[] clips) => _clips = clips;

		IAudioClip IAudioContainer.Next() => _clips[Random.Range(0, _clips.Length)];
	}
}