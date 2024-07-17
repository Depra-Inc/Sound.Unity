// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System.Collections.Generic;

namespace Depra.Sound.Configuration
{
	public interface IAudioBank
	{
		bool Contains(TrackId id);

		IAudioTrack Get(TrackId id);

		IEnumerable<IAudioTrack> Enumerate();
	}
}