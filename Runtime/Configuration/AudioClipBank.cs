// SPDX-License-Identifier: Apache-2.0
// © 2024-2026 Depra <n.melnikov@depra.org>

using System.Collections.Generic;
using UnityEngine;
using static Depra.Sound.Module;

namespace Depra.Sound.Configuration
{
	[CreateAssetMenu(menuName = MENU_PATH + FILE_NAME, fileName = FILE_NAME, order = DEFAULT_ORDER)]
	public sealed class AudioClipBank : AudioBankAsset
	{
		[SerializeField] private List<AudioBankEntry> _events = new();

		private const string FILE_NAME = "Audio Bank";

		public override IReadOnlyList<AudioBankEntry> Events => _events;
	}
}