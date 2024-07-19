// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System.Collections.Generic;

namespace Depra.Sound.Source
{
	public abstract class AudioSourceParameterConverter<TSource, TTarget>
	{
		public abstract TTarget Convert(TSource source);

		public IEnumerable<TTarget> Convert(IEnumerable<IAudioSourceParameter> parameters)
		{
			foreach (var parameter in parameters)
			{
				if (parameter is TSource source)
				{
					yield return Convert(source);
				}
			}
		}
	}
}