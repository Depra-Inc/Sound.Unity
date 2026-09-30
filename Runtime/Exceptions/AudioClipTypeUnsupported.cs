// SPDX-License-Identifier: Apache-2.0
// © 2024-2026 Depra <n.melnikov@depra.org>

using System;

namespace Depra.Sound
{
	public sealed class AudioClipTypeUnsupported : Exception
	{
		public AudioClipTypeUnsupported(Type clipType, Type sourceType) :
			base($"Clip type {clipType.Name} is not supported by source type {sourceType.Name}") { }
	}
}