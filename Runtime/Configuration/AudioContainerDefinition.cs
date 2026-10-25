// SPDX-License-Identifier: Apache-2.0
// © 2024-2026 Depra <n.melnikov@depra.org>

using System;

namespace Depra.Sound
{
	[Serializable]
	public struct AudioContainerDefinition
	{
		public AudioEventId Id;
		public AudioEventContainer Container;
	}
}