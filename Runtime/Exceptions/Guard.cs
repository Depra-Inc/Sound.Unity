﻿// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Depra.Sound.Exceptions;

namespace Depra.Sound.Exceptions
{
	public static class Guard
	{
		[Conditional("DEBUG")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AgainstNull(object value, string parameterName) =>
			Against(value == null, () => new ArgumentNullException(parameterName));

		[Conditional("DEBUG")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void AgainstUnsupportedType(MemberInfo actual, MemberInfo required) =>
			Against(actual != required, () => new UnsupportedClipTypeException(actual, required));

		[Conditional("DEBUG")]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private static void Against(bool condition, Func<Exception> exception)
		{
			if (condition)
			{
				throw exception();
			}
		}
	}
}