// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.SerializeReference.Extensions;
using UnityEngine;

namespace Depra.Sound.Configuration
{
	[Serializable]
	public sealed class IntegerParameter : IAudioSourceParameter
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

	[Serializable]
	public sealed class SingleParameter : IAudioSourceParameter
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

	[Serializable]
	[SerializeReferenceIcon("d_FilterByLabel")]
	public sealed class LabelParameter : IAudioSourceParameter
	{
		[field: SerializeField] public string Name { get; private set; }
		[field: SerializeField] public string Value { get; private set; }

		public LabelParameter() { }

		public LabelParameter(string name, string value)
		{
			Name = name;
			Value = value;
		}
	}

	[Serializable]
	[SerializeReferenceIcon("d_Transform Icon")]
	public sealed class PositionParameter : IAudioSourceParameter
	{
		[field: SerializeField] public Vector3 Value { get; private set; }

		public PositionParameter() { }
		public PositionParameter(Vector3 value) => Value = value;
	}

	[Serializable]
	[SerializeReferenceIcon("d_Transform Icon")]
	public sealed class RuntimePositionParameter : IAudioSourceParameter { }

	[Serializable]
	[SerializeReferenceIcon("d_Transform Icon")]
	public sealed class TransformParameter : IAudioSourceParameter
	{
		[field: SerializeField] public Transform Value { get; private set; }

		public TransformParameter() { }
		public TransformParameter(Transform value) => Value = value;
	}

	[Serializable]
	[SerializeReferenceIcon("d_Transform Icon")]
	public sealed class RuntimeTransformParameter : IAudioSourceParameter { }
}