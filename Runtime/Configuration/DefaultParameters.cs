// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.SerializeReference.Extensions;
using UnityEngine;

namespace Depra.Sound.Configuration
{
	[Serializable]
	public struct IntegerParameter : IAudioSourceParameter
	{
		[field: SerializeField] public string Name { get; private set; }
		[field: SerializeField] public int Value { get; private set; }

		public IntegerParameter(string name, int value)
		{
			Name = name;
			Value = value;
		}
	}

	[Serializable]
	public struct SingleParameter : IAudioSourceParameter
	{
		[field: SerializeField] public string Name { get; private set; }
		[field: SerializeField] public float Value { get; private set; }

		public SingleParameter(string name, float value)
		{
			Name = name;
			Value = value;
		}
	}

	[Serializable]
	[SerializeReferenceIcon("d_FilterByLabel")]
	public struct LabelParameter : IAudioSourceParameter
	{
		[field: SerializeField] public string Name { get; private set; }
		[field: SerializeField] public string Value { get; private set; }

		public LabelParameter(string name, string value)
		{
			Name = name;
			Value = value;
		}
	}

	[Serializable]
	[SerializeReferenceIcon("d_Transform Icon")]
	public struct PositionParameter : IAudioSourceParameter
	{
		[field: SerializeField] public Vector3 Value { get; private set; }

		public PositionParameter(Vector3 value) => Value = value;
	}

	[Serializable]
	[SerializeReferenceIcon("d_Transform Icon")]
	public readonly struct RuntimePositionParameter : IAudioSourceParameter { }

	[Serializable]
	[SerializeReferenceIcon("d_Transform Icon")]
	public struct TransformParameter : IAudioSourceParameter
	{
		[field: SerializeField] public Transform Value { get; private set; }

		public TransformParameter(Transform value) => Value = value;
	}

	[Serializable]
	[SerializeReferenceIcon("d_Transform Icon")]
	public readonly struct RuntimeTransformParameter : IAudioSourceParameter { }
}