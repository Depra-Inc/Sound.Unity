// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.Inspector.SerializedReference;
using Depra.Sound.Clip;
using JetBrains.Annotations;
using UnityEngine;

namespace Depra.Sound.Clips
{
	[Serializable]
	[SubtypeAlias(nameof(UnityAudioClip))]
	public sealed class UnityAudioClip : IAudioClip
	{
		[SerializeField] private AudioClip _value;

		public static implicit operator AudioClip(UnityAudioClip clip) => clip._value;

		public UnityAudioClip() { }

		public UnityAudioClip(AudioClip raw) => _value = raw;

		[UsedImplicitly]
		public AudioClip Raw => _value;

		public string Name => _value?.name;

		public float Duration => _value.length;
	}
}