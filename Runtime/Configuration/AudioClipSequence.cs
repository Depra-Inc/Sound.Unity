// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.Inspector.SerializedReference;
using Depra.Sound.Clip;
using UnityEngine;

namespace Depra.Sound.Configuration
{
	[SubtypeAlias(nameof(AudioClipSequence))]
	public sealed class AudioClipSequence : IAudioContainer
	{
		[SubtypeDropdown] [SerializeReference] private IAudioClip[] _clips;

		private int _lastIndex = -1;

		public AudioClipSequence() { }

		public AudioClipSequence(params IAudioClip[] clips) => _clips = clips;

		IAudioClip IAudioContainer.Next() => _clips[++_lastIndex % _clips.Length];
	}
}