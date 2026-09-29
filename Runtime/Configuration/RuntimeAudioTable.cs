using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Depra.Sound.Configuration;
using Random = UnityEngine.Random;

namespace Depra.Sound.Runtime
{
	// Runtime types below: pure POCOs built once from the authoring data at startup, with no
	// UnityEngine.Object/serialization overhead — safe to read from the hot path.
	public sealed class RuntimeAudioTable : IAudioTable
	{
		private readonly Dictionary<AudioEventId, IAudioEventDescription> _events = new();

		internal RuntimeAudioTable(IReadOnlyList<AudioBankAsset> banks)
		{
			foreach (var bank in banks)
			{
				if (bank == null)
				{
					continue;
				}

				bank.Compile(_events);
			}
		}

		public bool TryResolve(AudioEventId eventId, out IAudioEventDescription description) =>
			_events.TryGetValue(eventId, out description);
	}

	/// <summary>
	/// Compiled, hot-path-friendly audio event: a clip plus ready-to-apply static parameters.
	/// </summary>
	public sealed class RuntimeAudioEvent : IAudioEventDescription
	{
		private readonly AudioParam[] _parameters;

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

		public RuntimeAudioEventContainer(AudioEventContainer.PlaybackMode playbackMode, List<IAudioClip> clips,
			IAudioEventContract contract, AudioParam[] staticParams)
		{
			Contract = contract;
			_clips = clips.ToArray();
			_playbackMode = playbackMode;
			_staticParams = staticParams;
		}

		public IAudioClip Clip
		{
			get
			{
				_selectedVariantIndex = SelectVariantIndex();
				return _selectedVariantIndex >= 0 ? _clips[_selectedVariantIndex] : null;
			}
		}

		public IAudioEventContract Contract { get; }
		ReadOnlySpan<AudioParam> IAudioEventDescription.StaticParameters => _staticParams;

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