// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Reflection;

namespace Depra.Sound.Exceptions
{
	internal sealed class UnsupportedClipTypeException : Exception
	{
		public UnsupportedClipTypeException(MemberInfo clipType, MemberInfo sourceType) :
			base($"Clip type {clipType.Name} is not supported by source type {sourceType.Name}") { }
	}
}