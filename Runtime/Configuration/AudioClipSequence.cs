// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.SerializeReference.Extensions;
using Depra.Sound.Clip;

namespace Depra.Sound.Configuration
{
	[Serializable]
	[SerializeReferenceMenuPath(nameof(AudioClipSequence))]
	public sealed class AudioClipSequence : IAudioContainer
	{
		[SerializeReferenceDropdown]
		[UnityEngine.SerializeReference]
		private IAudioClip[] _clips;

		private int _lastIndex = -1;

		public AudioClipSequence() { }

		public AudioClipSequence(params IAudioClip[] clips) => _clips = clips;

		IAudioClip IAudioContainer.Next() => _clips[++_lastIndex % _clips.Length];
	}
}