// SPDX-License-Identifier: Apache-2.0
// © 2024-2025 Depra <n.melnikov@depra.org>

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Depra.Sound.Module;

// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable ForCanBeConvertedToForeach
namespace Depra.Sound.Configuration
{
	[CreateAssetMenu(menuName = MENU_PATH + FILE_NAME, fileName = FILE_NAME, order = DEFAULT_ORDER)]
	public sealed class PersistentAudioBank : ScriptableObject, IAudioBank
	{
		[SerializeField] private List<Entry> _events;

		private const string FILE_NAME = "Audio Bank";

		public bool Contains(AudioEventId id) => _events.Exists(entry => entry.Id == id);

		public bool TryGet(AudioEventId eventId, out IAudioEventDescription description)
		{
			var index = eventId.Value;
			if ((uint)index >= (uint)_events.Count)
			{
				description = null;
				return false;
			}

			description = _events[index].Description;
			return description != null;
		}

#if UNITY_EDITOR
		[ContextMenu(nameof(Sort))]
		internal void Sort()
		{
			_events.Sort((a, b) => a.Id.Value.CompareTo(b.Id.Value));
			EditorUtility.SetDirty(this);
		}
#endif

		[Serializable]
		public struct Entry
		{
			public AudioEventId Id;
			[UnityEngine.SerializeReference]
			public IAudioEventDescription Description;
		}
	}
}