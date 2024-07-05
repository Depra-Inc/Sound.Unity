// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using UnityEngine;

namespace Depra.Sound.Parameter
{
	[Serializable]
	public sealed class IntegerParameter : IAudioClipParameter
	{
		[field: SerializeField] public string Name { get; private set; }
		[field: SerializeField] public int Value { get; private set; }

		public IntegerParameter() { }

		public IntegerParameter(string name, int value)
		{
			Name = name;
			Value = value;
		}
	}
}