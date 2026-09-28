using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Depra.Sound.Configuration;
using UnityEngine;
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

				foreach (var entry in bank.Events)
				{
					RegisterBank(bank, entry);
				}
			}
		}

		public bool TryResolve(AudioEventId eventId, out IAudioEventDescription description) =>
			_events.TryGetValue(eventId, out description);

		private void RegisterBank(AudioBankAsset bank, AudioBankEntry entry)
		{
			if (entry.Description == null)
			{
				Debug.LogError($"Audio event '{entry.Name}' ({entry.Id.Value}) has no description.", bank);
				return;
			}

			var runtimeEvent = entry.Description.Compile();
			if (runtimeEvent == null)
			{
				Debug.LogError($"Audio event '{entry.Name}' ({entry.Id.Value}) could not be compiled.", bank);
				return;
			}

			if (!_events.TryAdd(entry.Id, runtimeEvent))
			{
				Debug.LogError($"Duplicate audio event ID '{entry.Id}' in bank '{bank.name}'.", bank);
			}
		}
	}

	/// <summary>
	/// Compiled, hot-path-friendly audio event: a clip plus ready-to-apply static parameters.
	/// </summary>
	public sealed class RuntimeAudioEvent : IAudioEventDescription
	{
		private readonly AudioParameter[] _parameters;

		public RuntimeAudioEvent(IAudioClip clip, IAudioEventContract contract, AudioParameter[] parameters)
		{
			Clip = clip;
			Contract = contract;
			_parameters = parameters ?? Array.Empty<AudioParameter>();
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

		public ReadOnlySpan<AudioParameter> StaticParameters
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
		private readonly IAudioEventDescription[] _variants;
		private readonly AudioEventContainer.Strategy _strategy;

		private int _nextSequenceIndex;
		private int _selectedVariantIndex = -1;

		public RuntimeAudioEventContainer(AudioEventContainer.Strategy strategy, List<IAudioEventDescription> variants,
			IAudioEventContract contract)
		{
			Contract = contract;
			_strategy = strategy;
			_variants = new IAudioEventDescription[variants.Count];
			for (var index = 0; index < _variants.Length; index++)
			{
				_variants[index] = variants[index];
			}
		}

		public IAudioClip Clip
		{
			get
			{
				_selectedVariantIndex = SelectVariantIndex();
				return _selectedVariantIndex >= 0 ? _variants[_selectedVariantIndex].Clip : null;
			}
		}

		public IAudioEventContract Contract { get; }

		public ReadOnlySpan<AudioParameter> StaticParameters =>
			(uint)_selectedVariantIndex < (uint)_variants.Length
				? _variants[_selectedVariantIndex].StaticParameters
				: ReadOnlySpan<AudioParameter>.Empty;

		private int SelectVariantIndex()
		{
			if (_variants.Length == 0)
			{
				return -1;
			}

			if (_strategy == AudioEventContainer.Strategy.Random)
			{
				return Random.Range(0, _variants.Length);
			}

			var selected = _nextSequenceIndex;
			_nextSequenceIndex = (_nextSequenceIndex + 1) % _variants.Length;

			return selected;
		}
	}
}