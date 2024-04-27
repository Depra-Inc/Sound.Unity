// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.Inspector.SerializedReference;
using UnityEngine;

namespace Depra.Sound.Configuration
{
	[Serializable]
	public struct SoundEntry
	{
		[field: SerializeField] public string Key { get; private set; }
		[field: Range(0f, 1f), SerializeField] public float Volume { get; private set; }

		[field: SubtypeDropdown, SerializeReference]
		public IAudioContainer Container { get; private set; }

		public bool IsValid() => string.IsNullOrEmpty(Key) == false && Container != null;
	}
}