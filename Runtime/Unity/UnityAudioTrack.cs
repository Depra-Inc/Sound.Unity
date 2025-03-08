// SPDX-License-Identifier: Apache-2.0
// © 2024-2025 Depra <n.melnikov@depra.org>

using System;
using System.Collections.Generic;
using Depra.SerializeReference.Extensions;
using UnityEngine;

namespace Depra.Sound.Unity
{
	[Serializable]
	[SerializeReferenceIcon("d_AudioClip Icon")]
	[SerializeReferenceMenuPath(nameof(UnityAudioTrack))]
	public sealed class UnityAudioTrack : IAudioTrack
	{
		[SerializeField] private AudioClip _value;

		[SerializeReferenceDropdown]
		[UnityEngine.SerializeReference]
		private IAudioSourceParameter[] _parameters;

		public UnityAudioTrack() { }
		public UnityAudioTrack(AudioClip raw) => _value = raw;

		void IAudioTrack.ExtractSegments(IList<AudioTrackSegment> segments) =>
			segments.Add(new AudioTrackSegment(new UnityAudioClip(_value), _parameters));
	}
}