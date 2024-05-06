// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using UnityEngine;

namespace Depra.Sound.Parameter
{
	public sealed class IntegerParameter : IAudioClipParameter
	{
		[field: SerializeField] public string Name { get; private set; }
		[field: SerializeField] public int Value { get; private set; }

		public IntegerParameter(string name, int value)
		{
			Name = name;
			Value = value;
		}
	}
}