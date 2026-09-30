// SPDX-License-Identifier: Apache-2.0
// © 2024-2026 Depra <n.melnikov@depra.org>

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Depra.Sound.Module;

namespace Depra.Sound
{
	[CreateAssetMenu(menuName = MENU_PATH + "Audio Bank", fileName = "New Audio Bank", order = DEFAULT_ORDER)]
	public sealed class AudioClipBank : AudioBankAsset
	{
		[SerializeField] private List<EventEntry> _events;
		[SerializeField] private List<AudioContainerEntry> _containers;

		public override bool Contains(AudioEventId id) =>
			_events.Any(entry => entry.Id.Value == id) ||
			_containers.Any(entry => entry.Id.Value == id);

		public override void Compile(IDictionary<AudioEventId, IAudioEventDescription> map)
		{
			foreach (var entry in _events)
			{
				map.TryAdd(entry.Id, entry.Description.Compile());
			}

			foreach (var entry in _containers)
			{
				map.TryAdd(entry.Id, entry.Container.Compile());
			}
		}

		public override IEnumerable<(ulong id, string label)> GetAllEventNames() => from entry in _events
			let eventName = string.IsNullOrWhiteSpace(entry.Name) ? "Unnamed Event" : entry.Name
			select (entry.Id.Value, $"{eventName} ({entry.Id.Value}) - {eventName}");

		[Serializable]
		public struct EventEntry
		{
			public string Name;
			public AudioEventId Id;
			public AudioEventDescription Description;
		}
	}
}