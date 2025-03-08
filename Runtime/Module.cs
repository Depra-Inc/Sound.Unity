// SPDX-License-Identifier: Apache-2.0
// © 2024-2025 Depra <n.melnikov@depra.org>

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Depra.Sound.Unity")]

namespace Depra.Sound
{
	internal static class Module
	{
		public const int DEFAULT_ORDER = 52;
		public const string MENU_PATH = "Sound/";
		public const string LOG_FORMAT = "[Sound] {0}";
	}
}