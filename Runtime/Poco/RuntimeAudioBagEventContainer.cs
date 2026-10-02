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

		IAudioEventContract IAudioEventDescription.Contract
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _events.Length > 0 ? _events[0].Contract : AudioEventContract.EMPTY;
		}

		public int EventCount
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _events.Length;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public IAudioEventDescription GetEvent(int index) =>
			(uint)index < (uint)_events.Length ? _events[index] : null;
	}
}




