using System.Runtime.CompilerServices;
using UnityEngine;

namespace Depra.Sound.Configuration
{
	public readonly struct Audio3DParameterId
	{
		public static readonly AudioParameterId Position = new(101);
		public static readonly AudioParameterId Transform = new(102);
		public static readonly AudioParameterId NamedInt = new(103);
		public static readonly AudioParameterId NamedFloat = new(104);
		public static readonly AudioParameterId NamedString = new(105);
	}

	public readonly struct UnityAudioParameters
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AudioParameter Transform(Transform transform) =>
			AudioParameter.CustomReference(Audio3DParameterId.Transform, Audio3DParameterId.Transform, transform);
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AudioParameter NamedInteger(string name, int value) =>
			AudioParameter.CustomReference(Audio3DParameterId.NamedInt, Audio3DParameterId.NamedInt, name,
				integerValue: value);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AudioParameter NamedFloat(string name, float value) =>
			AudioParameter.CustomReference(Audio3DParameterId.NamedFloat, Audio3DParameterId.NamedFloat, name,
				float0: value);

		// [MethodImpl(MethodImplOptions.AggressiveInlining)]
		// public static AudioParameter NamedString(string name, string value) =>
		// 	AudioParameter.CustomReference(Audio3DParameterId.NamedString, Audio3DParameterId.NamedString, name,
		// 		referenceValue: value);
	}
}