// SPDX-License-Identifier: Apache-2.0
// © 2024-2025 Depra <n.melnikov@depra.org>

using UnityEngine;
using Object = UnityEngine.Object;

namespace Depra.Sound.Source
{
	[System.Serializable]
	public sealed class DefaultAudioSourceFactory : IAudioSourceFactory
	{
		[SerializeField] private SceneAudioSource _original;

		public DefaultAudioSourceFactory() { }
		public DefaultAudioSourceFactory(SceneAudioSource original) => _original = original;

		IAudioSource IAudioSourceFactory.Create() => Object.Instantiate(_original).GetComponent<IAudioSource>();

		void IAudioSourceFactory.Destroy(IAudioSource source) => Object.Destroy((Object)source);
	}
}