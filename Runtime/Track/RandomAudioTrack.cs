// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.SerializeReference.Extensions;
using Random = UnityEngine.Random;

namespace Depra.Sound.Configuration
{
	[Serializable]
	[SerializeReferenceMenuPath(nameof(RandomAudioTrack))]
	public sealed class RandomAudioTrack : IAudioTrack
	{
		[SerializeReferenceDropdown]
		[UnityEngine.SerializeReference]
		private IAudioTrack[] _tracks;

		public RandomAudioTrack() { }
		public RandomAudioTrack(params IAudioTrack[] tracks) => _tracks = tracks;

		IAudioClip IAudioTrack.Play(IAudioSource source)
		{
			var randomIndex = Random.Range(0, _tracks.Length);
			var randomTrack = _tracks[randomIndex];
			source.Play(randomTrack);

			return source.Current;
		}
	}
}