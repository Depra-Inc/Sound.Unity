// SPDX-License-Identifier: Apache-2.0
// © 2024-2026 Depra <n.melnikov@depra.org>

using System;
using System.Runtime.CompilerServices;
using Depra.SerializeReference.Extensions;
using UnityEngine;

namespace Depra.Sound
{
	// Editor (authoring) types: serialized in the inspector,
	// each compiles into a single AudioParam.

	public interface IAudioParamDescription
	{
		AudioParam Compile();
	}

	[Serializable]
	[SerializeReferenceMenuPath("Volume")]
	public struct VolumeParamDescription : IAudioParamDescription
	{
		[SerializeField] private float _value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AudioParam Compile(float value) => AudioParam.Float(AudioParamId.Volume, value);

		public readonly AudioParam Compile() => Compile(_value);
	}

	[Serializable]
	[SerializeReferenceMenuPath("Loop")]
	[SerializeReferenceIcon("d_preAudioLoopOff")]
	public struct LoopParamDescription : IAudioParamDescription
	{
		[SerializeField] private bool _value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AudioParam Compile(bool value) => AudioParam.Bool(AudioParamId.Loop, value);

		public readonly AudioParam Compile() => Compile(_value);
	}

	[Serializable]
	[SerializeReferenceMenuPath("Pan")]
	public struct PanParamDescription : IAudioParamDescription
	{
		[SerializeField] private float _value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AudioParam Compile(float value) => AudioParam.Float(AudioParamId.Pan, value);

		public readonly AudioParam Compile() => Compile(_value);
	}

	[Serializable]
	[SerializeReferenceMenuPath("Pitch")]
	public struct PitchParamDescription : IAudioParamDescription
	{
		[SerializeField] private float _value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AudioParam Compile(float value) => AudioParam.Float(AudioParamId.Pitch, value);

		public readonly AudioParam Compile() => Compile(_value);
	}

	[Serializable]
	[SerializeReferenceMenuPath("Labeled Float")]
	[SerializeReferenceIcon("d_FilterByLabel")]
	public struct LabeledFloatParamDescription : IAudioParamDescription
	{
		[SerializeField] private string _name;
		[SerializeField] private float _value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AudioParam Compile(string name, float value) =>
			AudioParam.NamedFloat(AudioParamId.Unknown, name, value);

		public readonly AudioParam Compile() => Compile(_name, _value);
	}

	[Serializable]
	[SerializeReferenceMenuPath("Labeled Integer")]
	[SerializeReferenceIcon("d_FilterByLabel")]
	public struct LabeledIntegerParamDescription : IAudioParamDescription
	{
		[SerializeField] private string _name;
		[SerializeField] private int _value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AudioParam Compile(string name, int value) =>
			AudioParam.NamedInt(AudioParamId.Unknown, name, value);

		public readonly AudioParam Compile() => Compile(_name, _value);
	}

	[Serializable]
	[SerializeReferenceMenuPath("Labeled String")]
	[SerializeReferenceIcon("d_FilterByLabel")]
	public struct LabeledStringParamDescription : IAudioParamDescription
	{
		[SerializeField] private string _name;
		[SerializeField] private string _value;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AudioParam Compile(string name, string value) =>
			AudioParam.NamedString(AudioParamId.Unknown, name, value);

		public readonly AudioParam Compile() => Compile(_name, _value);
	}

	[Serializable]
	[SerializeReferenceIcon("d_Transform Icon")]
	[SerializeReferenceMenuPath("Position Required")]
	public sealed class PositionParamDescription : IAudioParamDescription
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AudioParam Compile(Vector3 position) =>
			AudioParam.Vector3(UnityAudioParamId.Position, position.x, position.y, position.z);

		AudioParam IAudioParamDescription.Compile() => Compile(Vector3.zero);
	}

	[Serializable]
	[SerializeReferenceIcon("d_Transform Icon")]
	[SerializeReferenceMenuPath("Transform Required")]
	public struct TransformParamDescription : IAudioParamDescription
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AudioParam Compile(Transform transform) =>
			AudioParam.CustomRef(UnityAudioParamId.Transform, transform);

		AudioParam IAudioParamDescription.Compile() => Compile(null);
	}
}