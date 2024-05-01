// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.SerializeReference.Extensions;
using Depra.Sound.Clip;
using Depra.Sound.Parameter;
using UnityEngine;
using static Depra.Sound.Module;

namespace Depra.Sound.Source
{
	[AddComponentMenu(MENU_PATH + nameof(AudioSourceProxy), DEFAULT_ORDER)]
	public sealed class AudioSourceProxy : MonoBehaviour, IAudioSource
	{
		[SerializeReferenceDropdown]
		[UnityEngine.SerializeReference]
		private IAudioSource _audioSource;

		event IAudioSource.PlayDelegate IAudioSource.Started
		{
			add => _audioSource.Started += value;
			remove => _audioSource.Started -= value;
		}

		event IAudioSource.StopDelegate IAudioSource.Stopped
		{
			add => _audioSource.Stopped += value;
			remove => _audioSource.Stopped -= value;
		}

		bool IAudioSource.IsPlaying => _audioSource.IsPlaying;

		IAudioClipParameters IAudioSource.Parameters => _audioSource.Parameters;

		void IAudioSource.Stop() => _audioSource.Stop();

		void IAudioSource.Play(IAudioClip clip) => _audioSource.Play(clip);
	}
}