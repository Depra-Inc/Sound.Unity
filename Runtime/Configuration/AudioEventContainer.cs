using System;
using System.Collections.Generic;
using Depra.SerializeReference.Extensions;
using Depra.Sound.Runtime;
using UnityEngine;
using static Depra.Sound.Module;

namespace Depra.Sound
{
	[CreateAssetMenu(fileName = "New Audio Container", menuName = MENU_PATH + "Audio Container", order = DEFAULT_ORDER)]
	public sealed class AudioEventContainer : ScriptableObject
	{
		[SerializeReferenceDropdown]
		[UnityEngine.SerializeReference]
		private List<IAudioClip> _clips;

		[SerializeField] private PlaybackMode _playbackMode;

		[SerializeReferenceDropdown]
		[UnityEngine.SerializeReference]
		private List<IAudioParamDescription> _parameters;

		[SerializeReferenceDropdown]
		[UnityEngine.SerializeReference]
		private List<IAudioEventRequirement> _requirements;

		public IAudioEventDescription Compile()
		{
			if (_clips.Count == 0)
			{
				Debug.LogError("An audio event container must contain at least one variant.");
				return null;
			}

			var parameters = new AudioParam[_parameters.Count];
			for (var index = 0; index < parameters.Length; index++)
			{
				parameters[index] = _parameters[index].Compile();
			}

			return new RuntimeAudioEventContainer(_playbackMode, _clips,
				new AudioEventRequirements(_requirements), parameters);
		}

		public enum PlaybackMode
		{
			[InspectorName("Random")] RANDOM,
			[InspectorName("Sequence")] SEQUENCE
		}
	}

	[Serializable]
	public struct AudioContainerEntry
	{
		public string Name;
		public AudioEventId Id;
		public AudioEventContainer Container;
	}
}