// SPDX-License-Identifier: Apache-2.0
// © 2024-2025 Depra <n.melnikov@depra.org>

using System.Collections.Generic;

namespace Depra.Sound.Clip
{
	internal readonly struct NullAudioTrack : IAudioTrack
	{
		void IAudioTrack.ExtractSegments(IList<AudioTrackSegment> segments) { }
	}
}