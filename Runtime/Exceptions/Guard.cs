// SPDX-License-Identifier: Apache-2.0
// © 2024-2026 Depra <n.melnikov@depra.org>

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Depra.Sound.Exceptions
{
	public static class Guard
	{
		[Conditional("SOUND_DEBUG")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AgainstNull(object value, string parameterName)
		{
			if (value == null)
			{
				throw new ArgumentNullException(parameterName);
			}
		}

		[Conditional("SOUND_DEBUG")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AgainstUnsupportedType(Type actual, Type required)
		{
			if (actual != required)
			{
				throw new UnsupportedClipTypeException(actual, required);
			}
		}
	}
}