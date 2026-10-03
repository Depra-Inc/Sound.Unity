using System;
using System.Collections.Generic;

namespace Depra.Sound
{
	public sealed class AudioEventContract : IAudioEventContract
	{
		public static readonly IAudioEventContract EMPTY = new Empty();

		private readonly AudioParam[] _resultParams;
		private readonly AudioParam[] _optionalParams;
		private readonly Dictionary<AudioParamType, int> _optionalIndices;

		public AudioEventContract(AudioParam[] defaultParams, AudioParam[] optionalParams)
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

		ReadOnlySpan<AudioParam> IAudioEventContract.Merge(ReadOnlySpan<AudioParam> parameters)
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

		private sealed class Empty : IAudioEventContract
		{
			ReadOnlySpan<AudioParam> IAudioEventContract.Merge(ReadOnlySpan<AudioParam> parameters) => parameters;
		}
	}

	// [Serializable]
	// [SerializeReferenceIcon("d_Transform Icon")]
	// public struct PositionRequirement : IAudioEventRequirement
	// {
	// 	bool IAudioEventRequirement.Validate(ReadOnlySpan<AudioParam> parameters, out string error)
	// 	{
	// 		foreach (var parameter in parameters)
	// 		{
	// 			if (parameter.Id == UnityAudioParamId.Position || parameter.Id == UnityAudioParamId.Transform)
	// 			{
	// 				error = null;
	// 				return true;
	// 			}
	// 		}
	//
	// 		error = "A 3D audio event requires a position or transform parameter.";
	// 		return false;
	// 	}
	// }
}