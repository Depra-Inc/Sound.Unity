﻿// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

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

		public void Play(IAudioSource source)
		{
			foreach (var track in _tracks)
			{
				track.Play(source);
			}
		}

		AudioTrackSegment[] IAudioTrack.Deconstruct()
		{
			var result = new List<AudioTrackSegment>();
			foreach (var track in _tracks)
			{
				result.AddRange(track.Deconstruct());
			}

			return result.ToArray();
		}
	}
}