using System.Runtime.CompilerServices;
using Random = UnityEngine.Random;

namespace Depra.Sound.Runtime
{
	/// <summary>
	/// Picks one of several compiled variants each time the clip is requested.
	/// </summary>
	public sealed class RuntimeAudioEventContainer : IAudioEventDescription
	{
		private readonly IAudioEventDescription[] _events;
		private readonly AudioEventContainer.PlaybackMode _playbackMode;

		private int _nextSequenceIndex;
		private int _selectedVariantIndex = -1;

		internal RuntimeAudioEventContainer(AudioEventContainer.PlaybackMode playbackMode,
			IAudioEventDescription[] events)
		{
			_events = events;
			_playbackMode = playbackMode;
		}

		public IAudioClip Clip
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GetSelectedClip();
		}

		public IAudioEventContract Contract
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GetSelectedContract();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private IAudioClip GetSelectedClip()
		{
			_selectedVariantIndex = SelectVariantIndex();
			return _selectedVariantIndex >= 0 ? _events[_selectedVariantIndex].Clip : null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private IAudioEventContract GetSelectedContract()
		{
			if (_selectedVariantIndex < 0 || _selectedVariantIndex >= _events.Length)
			{
				_selectedVariantIndex = SelectVariantIndex();
			}

			return _selectedVariantIndex >= 0
				? _events[_selectedVariantIndex].Contract
				: AudioEventContract.EMPTY;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int SelectVariantIndex()
		{
			if (_events.Length == 0)
			{
				return -1;
			}

			if (_playbackMode == AudioEventContainer.PlaybackMode.RANDOM)
			{
				return Random.Range(0, _events.Length);
			}

			var selected = _nextSequenceIndex;
			_nextSequenceIndex = (_nextSequenceIndex + 1) % _events.Length;

			return selected;
		}
	}
}