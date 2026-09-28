using System.Runtime.CompilerServices;
using UnityEngine;

namespace Depra.Sound.Configuration
{
	public readonly struct Audio3DParameterId
	{
		public static readonly AudioParameterId Position = new(101);
		public static readonly AudioParameterId Transform = new(102);
		public static readonly AudioParameterId LabeledInt = new(103);
		public static readonly AudioParameterId LabeledFloat = new(104);
		public static readonly AudioParameterId LabeledString = new(105);
	}

	public readonly struct UnityAudioParameters
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AudioParameter Position(Vector3 position) =>
			AudioParameter.Vector3(Audio3DParameterId.Position, position.x, position.y, position.z);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AudioParameter Transform(Transform transform) =>
			AudioParameter.CustomReference(Audio3DParameterId.Transform, Audio3DParameterId.Transform, transform);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AudioParameter LabeledInt(string name, int value) =>
			AudioParameter.CustomReference(Audio3DParameterId.LabeledInt, Audio3DParameterId.LabeledInt, name,
				integerValue: value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AudioParameter LabeledFloat(string name, float value) =>
			AudioParameter.CustomReference(Audio3DParameterId.LabeledFloat, Audio3DParameterId.LabeledFloat, name,
				float0: value);

		// [MethodImpl(MethodImplOptions.AggressiveInlining)]
		// public static AudioParameter NamedString(string name, string value) =>
		// 	AudioParameter.CustomReference(Audio3DParameterId.NamedString, Audio3DParameterId.NamedString, name,
		// 		referenceValue: value);
	}
}