// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.SerializeReference.Extensions;
using UnityEngine;

namespace Depra.Sound.Clip
{
	[Serializable]
	[SerializeReferenceIcon("d_AudioClip Icon")]
	[SerializeReferenceMenuPath(nameof(UnityAudioClip))]
	public sealed class UnityAudioClip : IAudioClip
	{
		[SerializeField] private AudioClip _value;

		public static implicit operator AudioClip(UnityAudioClip clip) => clip._value;

		public UnityAudioClip() { }
		public UnityAudioClip(AudioClip raw) => _value = raw;

		public string Name => _value?.name;
		public float Duration => _value.length;
	}
}