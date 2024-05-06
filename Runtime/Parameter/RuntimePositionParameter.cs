// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using Depra.SerializeReference.Extensions;

namespace Depra.Sound.Parameter
{
	[Serializable]
	[SerializeReferenceIcon("d_Transform Icon")]
	public sealed class RuntimePositionParameter : IAudioClipParameter { }
}