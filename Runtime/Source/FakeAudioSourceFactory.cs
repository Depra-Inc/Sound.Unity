// SPDX-License-Identifier: Apache-2.0
// © 2024-2025 Depra <n.melnikov@depra.org>

using System;
using UnityEngine;

namespace Depra.Sound.Source
{
	[Serializable]
	[Obsolete("Use SceneAudioSourceRef instead.")]
	public sealed class FakeAudioSourceFactory : IAudioSourceFactory
	{
		[SerializeField] private SceneAudioSource _instance;

		IAudioSource IAudioSourceFactory.Create() => (IAudioSource) _instance;
		void IAudioSourceFactory.Destroy(IAudioSource source) { }
	}
}