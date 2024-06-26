﻿// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using Depra.SerializeReference.Extensions;
using UnityEngine;

namespace Depra.Sound.Parameter
{
	[SerializeReferenceIcon("d_FilterByLabel")]
	public sealed class LabelParameter : IAudioClipParameter
	{
		[field: SerializeField] public string Name { get; private set; }
		[field: SerializeField] public string Value { get; private set; }

		public LabelParameter(string name, string value)
		{
			Name = name;
			Value = value;
		}
	}
}