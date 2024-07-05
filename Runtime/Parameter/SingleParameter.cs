// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using UnityEngine;

namespace Depra.Sound.Parameter
{
	[Serializable]
	public sealed class SingleParameter : IAudioClipParameter
	{
		[field: SerializeField] public string Name { get; private set; }
		[field: SerializeField] public float Value { get; private set; }

		public SingleParameter() { }

		public SingleParameter(string name, float value)
		{
			Name = name;
			Value = value;
		}
	}
}