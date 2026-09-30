using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Random = UnityEngine.Random;

namespace Depra.Sound.Runtime
{
	/// <summary>
	/// Picks one of several compiled variants each time the clip is requested.
	/// </summary>
	public sealed class RuntimeAudioEventContainer : IAudioEventDescription
	{
		private readonly IAudioClip[] _clips;
		private readonly AudioParam[] _staticParams;
		private readonly AudioEventContainer.PlaybackMode _playbackMode;

		private int _nextSequenceIndex;
		private int _selectedVariantIndex = -1;

		internal RuntimeAudioEventContainer(AudioEventContainer.PlaybackMode playbackMode, List<IAudioClip> clips,
			IAudioEventContract contract, AudioParam[] staticParams)
		{
			Contract = contract;
			_clips = clips.ToArray();
			_playbackMode = playbackMode;
			_staticParams = staticParams;
		}

		public IAudioClip Clip
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => GetSelectedClip();
		}

		public IAudioEventContract Contract { get; }

		ReadOnlySpan<AudioParam> IAudioEventDescription.StaticParameters
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _staticParams;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private IAudioClip GetSelectedClip()
		{
			_selectedVariantIndex = SelectVariantIndex();
			return _selectedVariantIndex >= 0 ? _clips[_selectedVariantIndex] : null;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private int SelectVariantIndex()
		{
			if (_clips.Length == 0)
			{
				return -1;
			}

			if (_playbackMode == AudioEventContainer.PlaybackMode.RANDOM)
			{
				return Random.Range(0, _clips.Length);
			}

			var selected = _nextSequenceIndex;
			_nextSequenceIndex = (_nextSequenceIndex + 1) % _clips.Length;

			return selected;
		}
	}
}