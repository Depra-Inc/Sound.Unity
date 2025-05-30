﻿// SPDX-License-Identifier: Apache-2.0
// © 2024-2025 Depra <n.melnikov@depra.org>

using System;
using System.Collections.Generic;
using System.Linq;
using Depra.SerializeReference.Extensions;
using Depra.Sound.Clip;
using UnityEngine;
using static Depra.Sound.Module;

// ReSharper disable LoopCanBeConvertedToQuery
// ReSharper disable ForCanBeConvertedToForeach
namespace Depra.Sound.Configuration
{
	[CreateAssetMenu(menuName = MENU_PATH + FILE_NAME, fileName = FILE_NAME, order = DEFAULT_ORDER)]
	public sealed class PersistentAudioBank : ScriptableObject, IAudioBank
	{
		[SerializeField] private List<SoundKvp> _sounds;

		private const string FILE_NAME = nameof(PersistentAudioBank);

		public bool Contains(TrackId id)
		{
			for (var index = 0; index < _sounds.Count; index++)
			{
				if (_sounds[index].Key == id.ToString())
				{
					return true;
				}
			}

			return false;
		}

		public IAudioTrack Get(TrackId id)
		{
			for (var index = 0; index < _sounds.Count; index++)
			{
				var sound = _sounds[index];
				if (sound.Key == id.ToString())
				{
					return sound.Track;
				}
			}

			return new NullAudioTrack();
		}

		public IEnumerable<IAudioTrack> Enumerate() => from sound in _sounds where sound.IsValid() select sound.Track;

#if UNITY_EDITOR
		[ContextMenu(nameof(Sort))]
		internal void Sort()
		{
			_sounds.Sort((a, b) => string.Compare(a.Key, b.Key, StringComparison.Ordinal));
			UnityEditor.EditorUtility.SetDirty(this);
		}
#endif

		[Serializable]
		private struct SoundKvp
		{
			[field: SerializeField] public string Key { get; private set; }

			[field: SerializeReferenceDropdown, UnityEngine.SerializeReference]
			public IAudioTrack Track { get; private set; }

			public bool IsValid() => string.IsNullOrEmpty(Key) == false && Track != null;
		}
	}
}