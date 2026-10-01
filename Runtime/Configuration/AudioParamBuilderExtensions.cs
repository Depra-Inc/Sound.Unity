using System.Runtime.CompilerServices;
using UnityEngine;

namespace Depra.Sound
{
	public static class AudioParamBuilderExtensions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AudioParam.Builder Position(this AudioParam.Builder self, Vector3 position) =>
			self.Add(AudioParam.Vector3(UnityAudioParamId.Position, position.x, position.y, position.z));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static AudioParam.Builder Transform(this AudioParam.Builder self, Transform transform) =>
			self.Add(AudioParam.CustomRef(UnityAudioParamId.Transform, UnityAudioParamId.Transform, transform));
	}
}