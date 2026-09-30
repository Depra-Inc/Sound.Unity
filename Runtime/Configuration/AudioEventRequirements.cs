using System;
using System.Collections.Generic;
using Depra.SerializeReference.Extensions;

namespace Depra.Sound
{
	public interface IAudioEventRequirement
	{
		bool Validate(ReadOnlySpan<AudioParam> parameters, out string error);
	}

	public sealed class AudioEventRequirements : IAudioEventContract
	{
		private readonly List<IAudioEventRequirement> _requirements;
		public AudioEventRequirements(List<IAudioEventRequirement> requirements) => _requirements = requirements;

		bool IAudioEventContract.Validate(ReadOnlySpan<AudioParam> parameters, out string error)
		{
			foreach (var requirement in _requirements)
			{
				if (!requirement.Validate(parameters, out error))
				{
					return false;
				}
			}

			error = null;
			return true;
		}
	}

	[Serializable]
	[SerializeReferenceIcon("d_Transform Icon")]
	public struct PositionRequirement : IAudioEventRequirement
	{
		bool IAudioEventRequirement.Validate(ReadOnlySpan<AudioParam> parameters, out string error)
		{
			foreach (var parameter in parameters)
			{
				if (parameter.Id == UnityAudioParamId.Position || parameter.Id == UnityAudioParamId.Transform)
				{
					error = null;
					return true;
				}
			}

			error = "A 3D audio event requires a position or transform parameter.";
			return false;
		}
	}
}