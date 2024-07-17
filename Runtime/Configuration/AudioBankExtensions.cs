// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System.Runtime.CompilerServices;

namespace Depra.Sound.Configuration
{
	public static class AudioBankExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool TryGetClip(this IAudioBank self, TrackId id, out IAudioTrack track)
		{
			if (self.Contains(id))
			{
				track = self.Get(id);
				return true;
			}

			track = null;
			return false;
		}
	}
}