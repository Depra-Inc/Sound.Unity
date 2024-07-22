// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.Sound.Source;

namespace Depra.Sound.Clip
{
	internal sealed class NullAudioTrack : IAudioTrack<UnityAudioSource>
	{
		void IAudioTrack.Play(IAudioSource source) => throw new NotImplementedException();
		void IAudioTrack<UnityAudioSource>.Play(UnityAudioSource source) => throw new NotImplementedException();
		AudioTrackSegment[] IAudioTrack.Deconstruct() => throw new NotImplementedException();
	}
}