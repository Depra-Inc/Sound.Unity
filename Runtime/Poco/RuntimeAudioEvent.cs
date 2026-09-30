using System;
using System.Runtime.CompilerServices;

namespace Depra.Sound.Runtime
{
	/// <summary>
	/// Compiled, hot-path-friendly audio event: a clip plus ready-to-apply static parameters.
	/// </summary>
	public sealed class RuntimeAudioEvent : IAudioEventDescription
	{
		private readonly AudioParam[] _parameters;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public RuntimeAudioEvent(IAudioClip clip, IAudioEventContract contract, AudioParam[] parameters)
		{
			Clip = clip;
			Contract = contract;
			_parameters = parameters ?? Array.Empty<AudioParam>();
		}

		public IAudioClip Clip
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get;
		}

		public IAudioEventContract Contract
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get;
		}

		public ReadOnlySpan<AudioParam> StaticParameters
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _parameters;
		}
	}
}