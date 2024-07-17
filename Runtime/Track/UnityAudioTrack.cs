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
		private IAudioClipParameter[] _parameters;

		public UnityAudioTrack() { }
		public UnityAudioTrack(AudioClip raw) => _value = raw;

		public IAudioClip Play(UnityAudioSource source)
		{
			var clip = new UnityAudioClip(_value);
			source.Play(clip, _parameters);

			return clip;
		}

		IAudioClip IAudioTrack.Play(IAudioSource source) => Play((UnityAudioSource) source);
	}
}