// SPDX-License-Identifier: Apache-2.0
// © 202-2025 Depra <n.melnikov@depra.org>

using System;
using System.Collections.Generic;
using Depra.SerializeReference.Extensions;

namespace Depra.Sound.Configuration
{
	[Serializable]
	[SerializeReferenceMenuPath(nameof(AudioTrackBag))]
	public sealed class AudioTrackBag : IAudioTrack
	{
		[SerializeReferenceDropdown]
		[UnityEngine.SerializeReference]
		private IAudioTrack[] _tracks;

		public AudioTrackBag() { }
		public AudioTrackBag(params IAudioTrack[] tracks) => _tracks = tracks;

		void IAudioTrack.ExtractSegments(IList<AudioTrackSegment> segments)
		{
			foreach (var track in _tracks)
			{
				track.ExtractSegments(segments);
			}
		}
	}
}