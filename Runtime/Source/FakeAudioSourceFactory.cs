// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using UnityEngine;

namespace Depra.Sound.Source
{
	[Serializable]
	public sealed class FakeAudioSourceFactory : IAudioSourceFactory
	{
		[SerializeField] private SceneAudioSource _instance;

		IAudioSource IAudioSourceFactory.Create() => (IAudioSource) _instance;

		void IAudioSourceFactory.Destroy(IAudioSource source) { }
	}
}