// SPDX-License-Identifier: Apache-2.0
// © 2024-2025 Depra <n.melnikov@depra.org>

using System;
using System.Collections.Generic;
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
		private IAudioTrack _randomTrack;

		public RandomAudioTrack() { }
		public RandomAudioTrack(params IAudioTrack[] tracks) => _tracks = tracks;

		void IAudioTrack.ExtractSegments(IList<AudioTrackSegment> segments)
		{
			var randomIndex = Random.Range(0, _tracks.Length);
			_randomTrack = _tracks[randomIndex];
			_randomTrack.ExtractSegments(segments);
		}
	}
}