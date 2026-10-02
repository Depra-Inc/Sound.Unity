// SPDX-License-Identifier: Apache-2.0
// © 2024-2026 Depra <n.melnikov@depra.org>

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Depra.Sound.Module;

namespace Depra.Sound
{
	[CreateAssetMenu(menuName = MENU_PATH + "Audio Bank", fileName = "New Audio Bank", order = DEFAULT_ORDER)]
	public sealed class AudioEventBank : AudioBankAsset
	{
		[SerializeField] private List<AudioEventDefinition> _events;
		[SerializeField] private List<AudioContainerDefinition> _containers;

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

		public override IEnumerable<(ulong id, string label)> GetAllEventNames()
		{
			foreach (var entry in _events)
			{
				var eventName = string.IsNullOrWhiteSpace(entry.Name)
					? "Unnamed Event"
					: entry.Name;

				yield return (entry.Id.Value, $"{eventName} ({entry.Id.Value})");
			}

			foreach (var entry in _containers)
			{
				var containerName = string.IsNullOrWhiteSpace(entry.Container.name)
					? "Unnamed Container"
					: entry.Container.name;

				yield return (entry.Id.Value, $"{containerName} ({entry.Id.Value})");
			}
		}
	}
}