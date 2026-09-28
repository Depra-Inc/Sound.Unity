using System;
using System.Collections.Generic;
using Depra.SerializeReference.Extensions;
using Depra.Sound.Runtime;

namespace Depra.Sound.Configuration
{
	[Serializable]
	public sealed class AudioEventDescription : IAudioEventVariant
	{
		[SerializeReferenceDropdown]
		[UnityEngine.SerializeReference]
		private IAudioClip _clip;

		[SerializeReferenceDropdown]
		[UnityEngine.SerializeReference]
		private List<IAudioEventParameter> _parameters = new();

		[SerializeReferenceDropdown]
		[UnityEngine.SerializeReference]
		private List<IAudioEventRequirement> _requirements = new();

		public IAudioEventDescription Compile()
		{
			var parameters = new AudioParam[_parameters?.Count ?? 0];
			for (var index = 0; index < parameters.Length; index++)
			{
				parameters[index] = _parameters[index].Compile();
			}

			return new RuntimeAudioEvent(_clip, new AudioEventRequirements(_requirements), parameters);
		}
	}

	/// <summary>
	/// Authoring source for a single playable audio event or a set of alternative variants.
	/// Compiles into a <see cref="RuntimeAudioEvent"/> once, at application start.
	/// </summary>
	public interface IAudioEventVariant
	{
		IAudioEventDescription Compile();
	}
}