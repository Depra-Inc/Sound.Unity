// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.Sound.Source;
using Depra.Source;
using UnityEngine;

namespace Depra.Sound.Runtime.Sources
{
	public sealed class DefaultAudioSourceFactory : IAudioSourceFactory
	{
		private readonly UnityAudioSource _original;

		public DefaultAudioSourceFactory(UnityAudioSource original) => _original = original;

		IAudioSource IAudioSourceFactory.Create() => Object.Instantiate(_original);

		void IAudioSourceFactory.Destroy(IAudioSource source) => Object.Destroy((Object) source);
	}
}