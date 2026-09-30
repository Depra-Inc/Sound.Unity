using System;
using System.Collections.Generic;
using Depra.SerializeReference.Extensions;
using Depra.Sound.Runtime;

namespace Depra.Sound
{
	[Serializable]
	public sealed class AudioEventDescription
	{
		[SerializeReferenceDropdown]
		[UnityEngine.SerializeReference]
		private IAudioClip _clip;

		[SerializeReferenceDropdown]
		[UnityEngine.SerializeReference]
		private List<IAudioParamDescription> _parameters = new();

		[SerializeReferenceDropdown]
		[UnityEngine.SerializeReference]
		private List<IAudioEventRequirement> _requirements = new();

		public IAudioEventDescription Compile()
		{
			var parameters = new AudioParam[_parameters.Count];
			for (var index = 0; index < parameters.Length; index++)
			{
				parameters[index] = _parameters[index].Compile();
			}

			return new RuntimeAudioEvent(_clip, new AudioEventRequirements(_requirements), parameters);
		}
	}
}