using System;
using System.Collections.Generic;

namespace Depra.Sound
{
	public sealed class RuntimeAudioEventContract
	{
		private readonly AudioParam[] _resultParams;
		private readonly AudioParam[] _optionalParams;
		private readonly Dictionary<AudioParamType, int> _optionalIndices;

		public RuntimeAudioEventContract(AudioParam[] defaultParams, AudioParam[] optionalParams)
		{
			defaultParams ??= Array.Empty<AudioParam>();
			optionalParams ??= Array.Empty<AudioParam>();

			_optionalParams = optionalParams;
			_resultParams = new AudioParam[defaultParams.Length + optionalParams.Length];

			defaultParams.AsSpan().CopyTo(_resultParams);
			optionalParams.AsSpan().CopyTo(_resultParams.AsSpan(defaultParams.Length));

			_optionalIndices = new Dictionary<AudioParamType, int>(optionalParams.Length);
			for (var index = 0; index < optionalParams.Length; index++)
			{
				_optionalIndices.Add(optionalParams[index].Type, index);
			}
		}

		public ReadOnlySpan<AudioParam> Overlay(ReadOnlySpan<AudioParam> parameters)
		{
			var optionalOffset = _resultParams.Length - _optionalParams.Length;
			_optionalParams.AsSpan().CopyTo(_resultParams.AsSpan(optionalOffset));

			for (int index = 0, count = parameters.Length; index < count; index++)
			{
				var parameter = parameters[index];
				if (_optionalIndices.TryGetValue(parameter.Type, out var optionalIndex))
				{
					_resultParams[optionalOffset + optionalIndex] = parameter;
				}
			}

			return _resultParams;
		}
	}
}