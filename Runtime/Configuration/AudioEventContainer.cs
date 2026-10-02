using System;
using System.Collections.Generic;
using Depra.Sound.Runtime;
using UnityEngine;
using static Depra.Sound.Module;

namespace Depra.Sound
{
	[CreateAssetMenu(fileName = "New Audio Container", menuName = MENU_PATH + "Audio Container", order = DEFAULT_ORDER)]
	public sealed class AudioEventContainer : ScriptableObject
	{
		[SerializeField] private List<AudioEventId> _events;
		[SerializeField] private PlaybackMode _playbackMode;

		public IAudioEventDescription Compile(IDictionary<AudioEventId, IAudioEventDescription> map)
		{
			if (map == null)
			{
				Debug.LogError($"Audio event container '{name}' compile map is null.");
				return null;
			}

			if (_events == null || _events.Count == 0)
			{
				Debug.LogError("An audio event container must contain at least one event reference.");
				return null;
			}

			var count = 0;
			var descriptions = new IAudioEventDescription[_events.Count];
			foreach (var eventId in _events)
			{
				if (map.TryGetValue(eventId, out var description) && description != null)
				{
					descriptions[count++] = description;
					continue;
				}

				Debug.LogError($"Audio event container '{name}' references unresolved event id '{eventId.Value}'.");
			}

			if (count == 0)
			{
				return null;
			}

			if (count != descriptions.Length)
			{
				Array.Resize(ref descriptions, count);
			}

			if (_playbackMode == PlaybackMode.BAG)
			{
				return new RuntimeAudioBagEventContainer(descriptions);
			}

			return new RuntimeAudioEventContainer(_playbackMode, descriptions);
		}

		public enum PlaybackMode
		{
			[InspectorName("Bag")] BAG,
			[InspectorName("Random")] RANDOM,
			[InspectorName("Sequence")] SEQUENCE
		}
	}
}