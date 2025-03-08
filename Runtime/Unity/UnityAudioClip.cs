// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using UnityEngine;

namespace Depra.Sound.Unity
{
	public readonly struct UnityAudioClip : IAudioClip
	{
		public static implicit operator AudioClip(UnityAudioClip clip) => clip._value;

		private readonly AudioClip _value;

		public UnityAudioClip(AudioClip raw) => _value = raw;

		public string Name => _value?.name ?? string.Empty;
		public float Duration => _value?.length ?? 0;
	}
}