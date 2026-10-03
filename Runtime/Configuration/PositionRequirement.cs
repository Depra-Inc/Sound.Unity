using System;
using Depra.SerializeReference.Extensions;

namespace Depra.Sound
{
	[Serializable]
	[SerializeReferenceIcon("d_Transform Icon")]
	public sealed class PositionRequirement : IAudioParamDescription
	{
		public AudioParam Compile()
		{
			throw new NotImplementedException();
		}
	}
}