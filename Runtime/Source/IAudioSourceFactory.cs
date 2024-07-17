// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

namespace Depra.Sound.Source
{
	public interface IAudioSourceFactory
	{
		IAudioSource Create();

		void Destroy(IAudioSource source);
	}
}