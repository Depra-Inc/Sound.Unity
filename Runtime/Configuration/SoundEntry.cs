// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.SerializeReference.Extensions;
using Depra.Sound.Parameter;
using UnityEngine;

namespace Depra.Sound.Configuration
{
	[Serializable]
	public struct SoundEntry
	{
		[field: SerializeField] public string Key { get; private set; }

		[field: SerializeReferenceDropdown, UnityEngine.SerializeReference]
		public IAudioContainer Container { get; private set; }

		[field: SerializeReferenceDropdown, UnityEngine.SerializeReference]
		public IAudioClipParameter[] Parameters { get; private set; }

		public bool IsValid() => string.IsNullOrEmpty(Key) == false && Container != null;
	}
}