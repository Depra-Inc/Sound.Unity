﻿// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.SerializeReference.Extensions;
using UnityEngine;

namespace Depra.Sound.Parameter
{
	[Serializable]
	[SerializeReferenceIcon("d_Transform Icon")]
	public struct PositionParameter : IAudioClipParameter
	{
		[field: SerializeField] public Vector3 Value { get; private set; }

		public PositionParameter(Vector3 value) => Value = value;
	}
}