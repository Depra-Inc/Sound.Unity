// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.SerializeReference.Extensions;
using Depra.Sound.Source;
using UnityEngine;

namespace Depra.Sound.Clip
{
	[Serializable]
	[SerializeReferenceIcon("d_AudioClip Icon")]
	[SerializeReferenceMenuPath(nameof(UnityAudioTrack))]
	public sealed class UnityAudioTrack : IAudioTrack<UnityAudioSource>
	{
		[SerializeField] private AudioClip _value;

		[SerializeReferenceDropdown]
		[UnityEngine.SerializeReference]
		private IAudioSourceParameter[] _parameters;

		public UnityAudioTrack() { }
		public UnityAudioTrack(AudioClip raw) => _value = raw;

		public void Play(UnityAudioSource source) => source.Play(new UnityAudioClip(_value), _parameters);

		void IAudioTrack.Play(IAudioSource source) => Play((UnityAudioSource) source);

		AudioTrackSegment[] IAudioTrack.Deconstruct() => new[]
			{ new AudioTrackSegment(new UnityAudioClip(_value), _parameters) };
	}
}