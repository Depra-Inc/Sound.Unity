// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Runtime.CompilerServices;
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

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AudioParam Compile(float value) => AudioParam.Float(AudioParamId.Volume, value);

		public readonly AudioParam Compile() => Compile(_value);
	}

	[Serializable]
	[SerializeReferenceMenuPath("Loop")]
	[SerializeReferenceIcon("d_preAudioLoopOff")]
	public struct LoopParameter : IAudioEventParameter
	{
		[SerializeField] private bool _value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AudioParam Compile(bool value) => AudioParam.Bool(AudioParamId.Loop, value);

		public readonly AudioParam Compile() => Compile(_value);
	}

	[Serializable]
	[SerializeReferenceMenuPath("Pan")]
	public struct PanParameter : IAudioEventParameter
	{
		[SerializeField] private float _value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AudioParam Compile(float value) => AudioParam.Float(AudioParamId.Pan, value);

		public readonly AudioParam Compile() => Compile(_value);
	}

	[Serializable]
	[SerializeReferenceMenuPath("Pitch")]
	public struct PitchParameter : IAudioEventParameter
	{
		[SerializeField] private float _value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AudioParam Compile(float value) => AudioParam.Float(AudioParamId.Pitch, value);

		public readonly AudioParam Compile() => Compile(_value);
	}

	[Serializable]
	[SerializeReferenceMenuPath("Labeled Float")]
	[SerializeReferenceIcon("d_FilterByLabel")]
	public struct LabeledFloatParameter : IAudioEventParameter
	{
		[SerializeField] private string _name;
		[SerializeField] private float _value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AudioParam Compile(string name, float value) => AudioParam.CustomRef(
			UnityAudioParamId.LabeledFloat, UnityAudioParamId.LabeledFloat,
			name, float0: value);

		public readonly AudioParam Compile() => Compile(_name, _value);
	}

	[Serializable]
	[SerializeReferenceMenuPath("Labeled Integer")]
	[SerializeReferenceIcon("d_FilterByLabel")]
	public struct LabeledIntegerParameter : IAudioEventParameter
	{
		[SerializeField] private string _name;
		[SerializeField] private int _value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AudioParam Compile(string name, int value) => AudioParam.CustomRef(
			UnityAudioParamId.LabeledInt, UnityAudioParamId.LabeledInt,
			name, integerValue: value);

		public readonly AudioParam Compile() => Compile(_name, _value);
	}
}