// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.SerializeReference.Extensions;
using Depra.Sound.Clip;

namespace Depra.Sound.Configuration
{
	[Serializable]
	[SerializeReferenceMenuPath(nameof(SingleAudioClip))]
	public sealed class SingleAudioClip : IAudioContainer
	{
		[SerializeReferenceDropdown] [UnityEngine.SerializeReference] private IAudioClip _clip;

		public SingleAudioClip() { }

		public SingleAudioClip(IAudioClip clip) => _clip = clip;

		IAudioClip IAudioContainer.Next() => _clip;
	}
}