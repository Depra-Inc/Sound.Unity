// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.Inspector.SerializedReference;
using Depra.Sound.Clip;
using UnityEngine;

namespace Depra.Sound.Configuration
{
	[Serializable]
	[SubtypeAlias(nameof(SingleAudioClip))]
	public sealed class SingleAudioClip : IAudioContainer
	{
		[SubtypeDropdown] [SerializeReference] private IAudioClip _clip;

		public SingleAudioClip() { }

		public SingleAudioClip(IAudioClip clip) => _clip = clip;

		IAudioClip IAudioContainer.Next() => _clip;
	}
}