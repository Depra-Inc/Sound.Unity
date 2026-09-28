using System;
using System.Collections.Generic;
using Depra.SerializeReference.Extensions;
using Depra.Sound.Runtime;
using UnityEngine;

namespace Depra.Sound.Configuration
{
	[Serializable]
	public sealed class AudioEventContainer : IAudioEventVariant
	{
		[SerializeField] private Strategy _strategy;
		[SerializeField] private List<AudioEventDescription> _variants = new();

		[SerializeReferenceDropdown]
		[UnityEngine.SerializeReference]
		private List<IAudioEventRequirement> _requirements = new();

		public IAudioEventDescription Compile()
		{
			var variants = new List<IAudioEventDescription>();
			foreach (var variant in _variants)
			{
				if (variant == null)
				{
					continue;
				}

				var compiled = variant.Compile();
				if (compiled == null)
				{
					return null;
				}

				variants.Add(compiled);
			}

			if (variants.Count == 0)
			{
				Debug.LogError("An audio event container must contain at least one variant.");
				return null;
			}

			return new RuntimeAudioEventContainer(_strategy, variants, new AudioEventRequirements(_requirements));
		}

		public enum Strategy
		{
			Random,
			Sequence
		}
	}
}