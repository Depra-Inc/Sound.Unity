// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Object = UnityEngine.Object;

namespace Depra.Sound.Source
{
	[Serializable]
	public sealed class DefaultAudioSourceFactory : IAudioSourceFactory
	{
		private readonly UnityAudioSource _original;

		public DefaultAudioSourceFactory(UnityAudioSource original) => _original = original;

		IAudioSource IAudioSourceFactory.Create(Type type) => Object.Instantiate(_original);

		void IAudioSourceFactory.Destroy(IAudioSource source) => Object.Destroy((Object) source);
	}
}