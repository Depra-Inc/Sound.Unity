// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Collections.Generic;
using Depra.Sound.Clip;
using Depra.Sound.Parameter;
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
		[SerializeField] private List<SoundEntry> _entries;

		private const string FILE_NAME = nameof(AudioConfig);

		public bool Contains(SoundId id) => _entries.Exists(x => x.Key == id.ToString());

		public IAudioClip GetClip(SoundId id)
		{
			foreach (var sound in _entries)
			{
				if (sound.Key == id.ToString())
				{
					return sound.Container.Next();
				}
			}

			throw new ArgumentException($"No clip found for id: {id}");
		}

		public IEnumerable<IAudioClip> GetAllClips() => _entries.ConvertAll(x => x.Container.Next());

		public IAudioClipParameter[] GetParameters(SoundId id) =>
			_entries.Find(x => x.Key == id.ToString()).Parameters;

#if UNITY_EDITOR
		[ContextMenu(nameof(Sort))]
		public void Sort()
		{
			_entries.Sort((a, b) => string.Compare(a.Key, b.Key, StringComparison.Ordinal));
			EditorUtility.SetDirty(this);
		}
#endif
	}
}