using System;
using Depra.SerializeReference.Extensions;

namespace Depra.Sound.Configuration
{
	public interface IAudioEventRequirement
	{
		bool Validate(ReadOnlySpan<AudioParameter> parameters, out string error);
	}

	[Serializable]
	[SerializeReferenceIcon("d_Transform Icon")]
	public struct PositionRequirement : IAudioEventRequirement
	{
		bool IAudioEventRequirement.Validate(ReadOnlySpan<AudioParameter> parameters, out string error)
		{
			foreach (var parameter in parameters)
			{
				if (parameter.Id == Audio3DParameterId.Position || parameter.Id == Audio3DParameterId.Transform)
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