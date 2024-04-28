// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Collections.Generic;
using Depra.Sound.Clip;
using UnityEngine;
using static Depra.Sound.Module;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Depra.Sound.Configuration
{
	[CreateAssetMenu(menuName = MENU_PATH + FILE_NAME, fileName = FILE_NAME, order = DEFAULT_ORDER)]
	public sealed class AudioConfig : ScriptableObject, IAudioBank
	{
		[SerializeField] private List<SoundEntry> _sounds;

		private const string FILE_NAME = nameof(AudioConfig);

		public bool Contains(SoundId id) => _sounds.Exists(x => x.Key == id.ToString());

		public IAudioClip Get(SoundId id)
		{
			foreach (var sound in _sounds)
			{
				if (sound.Key == id.ToString())
				{
					return sound.Container.Next();
				}
			}

			throw new ArgumentException($"No clip found for id: {id}");
		}

		public IEnumerable<IAudioClip> GetAll() => _sounds.ConvertAll(x => x.Container.Next());

#if UNITY_EDITOR
		[ContextMenu(nameof(Sort))]
		public void Sort()
		{
			_sounds.Sort((a, b) => string.Compare(a.Key, b.Key, StringComparison.Ordinal));
			EditorUtility.SetDirty(this);
		}
#endif
	}
}