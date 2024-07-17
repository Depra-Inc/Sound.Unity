// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.Sound.Source;

namespace Depra.Sound.Clip
{
	internal sealed class NullAudioTrack : IAudioTrack<UnityAudioSource>
	{
		IAudioClip IAudioTrack.Play(IAudioSource source) => throw new NotImplementedException();
		IAudioClip IAudioTrack<UnityAudioSource>.Play(UnityAudioSource source) => throw new NotImplementedException();
	}
}