// SPDX-License-Identifier: Apache-2.0
// © 2024-2025 Depra <n.melnikov@depra.org>

using System;
using Depra.Sound.Exceptions;

namespace Depra.Sound.Source
{
	public sealed class LambdaBasedAudioSourceFactory : IAudioSourceFactory
	{
		private readonly Action<IAudioSource> _destroyFunc;
		private readonly Func<IAudioSource> _createFunc;

		public LambdaBasedAudioSourceFactory(Func<IAudioSource> createFunc, Action<IAudioSource> destroyFunc)
		{
			Guard.AgainstNull(createFunc, nameof(createFunc));
			Guard.AgainstNull(destroyFunc, nameof(destroyFunc));

			_createFunc = createFunc;
			_destroyFunc = destroyFunc;
		}

		IAudioSource IAudioSourceFactory.Create() => _createFunc();
		void IAudioSourceFactory.Destroy(IAudioSource source) => _destroyFunc(source);
	}
}