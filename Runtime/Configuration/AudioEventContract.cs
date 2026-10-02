using System;

namespace Depra.Sound
{
	public sealed class EmptyContract : IAudioEventContract
	{
		ReadOnlySpan<AudioParam> IAudioEventContract.GetDefaultParameters() => ReadOnlySpan<AudioParam>.Empty;
		ReadOnlySpan<AudioParam> IAudioEventContract.Apply(ReadOnlySpan<AudioParam> parameters) => parameters;
	}

	public sealed class AudioEventContract : IAudioEventContract
	{
		public static readonly IAudioEventContract EMPTY = new Empty();

		private readonly AudioParam[] _defaultParams;
		private readonly AudioParam[] _optionalParams;

		public AudioEventContract(AudioParam[] defaultParams, AudioParam[] optionalParams)
		{
			_defaultParams = defaultParams;
			_optionalParams = optionalParams;
		}

		ReadOnlySpan<AudioParam> IAudioEventContract.GetDefaultParameters() => _defaultParams.AsSpan();

		public ReadOnlySpan<AudioParam> Apply(ReadOnlySpan<AudioParam> parameters)
		{
			return parameters;
		}

		private sealed class Empty : IAudioEventContract
		{
			ReadOnlySpan<AudioParam> IAudioEventContract.GetDefaultParameters() => ReadOnlySpan<AudioParam>.Empty;
			ReadOnlySpan<AudioParam> IAudioEventContract.Apply(ReadOnlySpan<AudioParam> parameters) => parameters;
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