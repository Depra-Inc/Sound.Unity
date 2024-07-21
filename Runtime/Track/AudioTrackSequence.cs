// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
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

		public void Play(IAudioSource source)
		{
			var track = _tracks[++_lastIndex % _tracks.Length];
			track.Play(source);
		}

		void IAudioTrack.Deconstruct(out AudioTrackSegment[] segments)
		{
			var track = _tracks[++_lastIndex % _tracks.Length];
			track.Deconstruct(out segments);
		}
	}
}