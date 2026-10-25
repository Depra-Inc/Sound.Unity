using System;
using System.Collections.Generic;
using Depra.Sound.Runtime;
using UnityEngine;

namespace Depra.Sound
{
	// Editor (authoring) type: the single ScriptableObject saved under Project Settings.
	// Holds the project's banks and allocates unique event IDs across all of them. Never touched at runtime.
	public sealed class AudioProjectSettings : ScriptableObject
	{
		[SerializeField] private List<AudioBankAsset> _banks = new();
		[SerializeField, HideInInspector] private uint _nextEventIdLow;
		[SerializeField, HideInInspector] private uint _nextEventIdHigh;
		[SerializeField, HideInInspector] private bool _eventIdCounterExhausted;

		public List<AudioBankAsset> Banks => _banks;

		public RuntimeAudioTable Compile() => new(_banks);

		public ulong AllocateEventId(ISet<ulong> reservedIds = null, AudioBankAsset excludedBank = null)
		{
			if (_eventIdCounterExhausted)
			{
				throw new InvalidOperationException("No event IDs are available.");
			}

			var candidate = GetNextEventId();
			while (true)
			{
				if (!ContainsEventId(candidate, excludedBank) &&
				    (reservedIds == null || !reservedIds.Contains(candidate)))
				{
					AdvanceEventIdCounter(candidate);
					return candidate;
				}

				if (candidate == ulong.MaxValue)
				{
					_eventIdCounterExhausted = true;
					throw new InvalidOperationException("No event IDs are available.");
				}

				candidate++;
			}
		}

		public bool ContainsEventId(ulong id, AudioBankAsset excludedBank = null)
		{
			foreach (var bank in _banks)
			{
				if (!bank || bank == excludedBank)
				{
					continue;
				}

				if (bank.Contains(id))
				{
					return true;
				}
			}

			return false;
		}

		private ulong GetNextEventId() => ((ulong)_nextEventIdHigh << 32) | _nextEventIdLow;

		private void AdvanceEventIdCounter(ulong allocatedId)
		{
			if (allocatedId == ulong.MaxValue)
			{
				_eventIdCounterExhausted = true;
				return;
			}

			var nextId = allocatedId + 1;
			_nextEventIdLow = (uint)nextId;
			_nextEventIdHigh = (uint)(nextId >> 32);
		}
	}
}