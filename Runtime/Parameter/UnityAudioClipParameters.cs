﻿// SPDX-License-Identifier: Apache-2.0
// © 2024 Nikolay Melnikov <n.melnikov@depra.org>

using System;
using System.Collections.Generic;
using UnityEngine;

namespace Depra.Sound.Parameter
{
	internal sealed class UnityAudioClipParameters : IAudioClipParameters
	{
		private readonly AudioSource _source;

		public UnityAudioClipParameters(AudioSource source) => _source = source;

		IEnumerable<Type> IAudioClipParameters.SupportedTypes() => new[]
		{
			typeof(PanParameter),
			typeof(LoopParameter),
			typeof(PitchParameter),
			typeof(EmptyParameter),
			typeof(VolumeParameter),
			typeof(PositionParameter),
			typeof(RuntimePositionParameter)
		};

		IAudioClipParameter IAudioClipParameters.Get(Type type) => type switch
		{
			_ when type == typeof(LoopParameter) => new LoopParameter(_source.loop),
			_ when type == typeof(PanParameter) => new PanParameter(_source.panStereo),
			_ when type == typeof(PitchParameter) => new PitchParameter(_source.pitch),
			_ when type == typeof(VolumeParameter) => new VolumeParameter(_source.volume),
			_ when type == typeof(PositionParameter) => new PositionParameter(_source.transform.position),
			_ when type == typeof(RuntimePositionParameter) => new PositionParameter(_source.transform.position),
			_ => new NullParameter()
		};

		void IAudioClipParameters.Set(IAudioClipParameter parameter)
		{
			switch (parameter)
			{
				case EmptyParameter:
					break;
				case LoopParameter loop:
					_source.loop = loop.Value;
					break;
				case VolumeParameter volume:
					_source.volume = volume.Value;
					break;
				case PitchParameter pitch:
					_source.pitch = pitch.Value;
					break;
				case PanParameter pan:
					_source.panStereo = pan.Value;
					break;
				case PositionParameter position:
					_source.transform.position = position.Value;
					break;
				default:
					Debug.LogError($"Failed to set parameter {parameter.GetType().Name}!");
					break;
			}
		}
	}
}