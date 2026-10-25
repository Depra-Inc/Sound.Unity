using System;
using System.Runtime.CompilerServices;

namespace Depra.Sound.Runtime
{
	/// <summary>
	/// Compiled, hot-path-friendly audio event: a clip plus ready-to-apply static parameters.
	/// </summary>
	public sealed class RuntimeAudioEvent : IAudioEventDescription
	{
		private readonly IAudioClip _clip;
		private readonly RuntimeAudioEventContract _contract;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public RuntimeAudioEvent(IAudioClip clip, RuntimeAudioEventContract contract)
		{
			_clip = clip;
			_contract = contract;
		}

		IAudioClip IAudioEventDescription.Clip
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _clip;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlySpan<AudioParam> Overlay(ReadOnlySpan<AudioParam> parameters) => _contract.Overlay(parameters);
	}
}