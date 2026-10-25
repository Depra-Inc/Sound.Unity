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
		private List<IAudioParamDescription> _defaultParameters = new();

		[SerializeReferenceDropdown]
		[UnityEngine.SerializeReference]
		private List<IAudioParamDescription> _optionalParameters = new();

		public IAudioEventDescription Compile()
		{
			var defaultParams = new AudioParam[_defaultParameters.Count];
			for (var index = 0; index < defaultParams.Length; index++)
			{
				defaultParams[index] = _defaultParameters[index].Compile();
			}

			var optionalParams = new AudioParam[_optionalParameters.Count];
			for (var index = 0; index < optionalParams.Length; index++)
			{
				optionalParams[index] = _optionalParameters[index].Compile();
			}

			return new RuntimeAudioEvent(_clip, new RuntimeAudioEventContract(defaultParams, optionalParams));
		}
	}
}