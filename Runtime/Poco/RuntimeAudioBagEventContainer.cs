using System;
using System.Runtime.CompilerServices;

namespace Depra.Sound.Runtime
{
	/// <summary>
	/// Runtime POCO for BAG playback: exposes all variants in one call for multi-play backends.
	/// </summary>
	public sealed class RuntimeAudioBagEventContainer : IAudioEventDescription, IAudioEventBatchDescription
	{
		private readonly IAudioEventDescription[] _events;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal RuntimeAudioBagEventContainer(IAudioEventDescription[] events) => _events = events;

		IAudioClip IAudioEventDescription.Clip
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _events.Length > 0 ? _events[0].Clip : null;
		}

		int IAudioEventBatchDescription.EventCount
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _events.Length;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		IAudioEventDescription IAudioEventBatchDescription.GetEvent(int index) =>
			(uint)index < (uint)_events.Length ? _events[index] : null;

		ReadOnlySpan<AudioParam> IAudioEventDescription.Overlay(ReadOnlySpan<AudioParam> parameters) =>
			ReadOnlySpan<AudioParam>.Empty;
	}
}