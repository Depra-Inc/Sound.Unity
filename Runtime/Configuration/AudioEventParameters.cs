// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.SerializeReference.Extensions;
using UnityEngine;

namespace Depra.Sound.Configuration
{
	// Editor (authoring) types: serialized in the inspector,
	// each compiles into a single AudioParam.

	public interface IAudioEventParameter
	{
		AudioParam Compile();
	}

	[Serializable]
	[SerializeReferenceMenuPath("Volume")]
	public struct VolumeParameter : IAudioEventParameter
	{
		[SerializeField] private float _value;

		public VolumeParameter(float value) => _value = value;

		readonly AudioParam IAudioEventParameter.Compile() => AudioParam.Float(AudioParamId.Volume, _value);
	}

	[Serializable]
	[SerializeReferenceMenuPath("Loop")]
	[SerializeReferenceIcon("d_preAudioLoopOff")]
	public struct LoopParameter : IAudioEventParameter
	{
		[SerializeField] private bool _value;

		public LoopParameter(bool value) => _value = value;

		readonly AudioParam IAudioEventParameter.Compile() => AudioParam.Bool(AudioParamId.Loop, _value);
	}

	[Serializable]
	[SerializeReferenceMenuPath("Pan")]
	public struct PanParameter : IAudioEventParameter
	{
		[SerializeField] private float _value;

		public PanParameter(float value) => _value = value;

		readonly AudioParam IAudioEventParameter.Compile() => AudioParam.Float(AudioParamId.Pan, _value);
	}

	[Serializable]
	[SerializeReferenceMenuPath("Pitch")]
	public struct PitchParameter : IAudioEventParameter
	{
		[SerializeField] private float _value;

		public PitchParameter(float value) => _value = value;

		readonly AudioParam IAudioEventParameter.Compile() => AudioParam.Float(AudioParamId.Pitch, _value);
	}

	[Serializable]
	[SerializeReferenceMenuPath("Labeled Float")]
	[SerializeReferenceIcon("d_FilterByLabel")]
	public struct LabeledFloatParameter : IAudioEventParameter
	{
		[SerializeField] private string _name;
		[SerializeField] private float _value;

		readonly AudioParam IAudioEventParameter.Compile() => AudioParam.CustomRef(
			UnityAudioParamId.LabeledFloat, UnityAudioParamId.LabeledFloat,
			_name, float0: _value);
	}

	[Serializable]
	[SerializeReferenceMenuPath("Labeled Integer")]
	[SerializeReferenceIcon("d_FilterByLabel")]
	public struct LabeledIntegerParameter : IAudioEventParameter
	{
		[SerializeField] private string _name;
		[SerializeField] private int _value;

		readonly AudioParam IAudioEventParameter.Compile() => AudioParam.CustomRef(
			UnityAudioParamId.LabeledInt, UnityAudioParamId.LabeledInt,
			_name, integerValue: _value);
	}
}