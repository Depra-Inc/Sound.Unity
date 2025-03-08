// SPDX-License-Identifier: Apache-2.0
// © 2024-2025 Depra <n.melnikov@depra.org>

using System;
using System.Collections.Generic;
using Depra.SerializeReference.Extensions;

namespace Depra.Sound.Configuration
{
	[Serializable]
	[SerializeReferenceMenuPath(nameof(AudioTrackSequence))]
	public sealed class AudioTrackSequence : IAudioTrack
	{
		[SerializeReferenceDropdown]
		[UnityEngine.SerializeReference]
		private IAudioTrack[] _tracks;

		private int _lastIndex = -1;

		public AudioTrackSequence() { }
		public AudioTrackSequence(params IAudioTrack[] tracks) => _tracks = tracks;

		void IAudioTrack.ExtractSegments(IList<AudioTrackSegment> segments) =>
			_tracks[++_lastIndex % _tracks.Length].ExtractSegments(segments);
	}
}