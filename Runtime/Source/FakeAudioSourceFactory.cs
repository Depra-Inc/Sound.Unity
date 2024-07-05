// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using UnityEngine;

namespace Depra.Sound.Source
{
	[Serializable]
	public sealed class FakeAudioSourceFactory : IAudioSourceFactory
	{
		[SerializeField] private UnityAudioSource _instance;

		IAudioSource IAudioSourceFactory.Create() => _instance;

		void IAudioSourceFactory.Destroy(IAudioSource source) { }
	}
}