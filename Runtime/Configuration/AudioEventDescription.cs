using System;
using Depra.SerializeReference.Extensions;
using UnityEngine;

namespace Depra.Sound.Configuration
{
	public sealed class AudioEventContainer : IAudioEventDescription
	{
		[SerializeField] private Mode _mode;

		public IAudioClip Clip { get; }

		public void ApplyStaticParameters(IAudioSource source)
		{
			throw new NotImplementedException();
		}

		public enum Mode
		{
			BAG,
			RANDOM
		}
	}

	[Serializable]
	public sealed class AudioEventDescription : IAudioEventDescription
	{
		[field: SerializeReferenceDropdown, UnityEngine.SerializeReference]
		public IAudioClip Clip { get; private set; }

		[SerializeField] private AudioParameter[] _parameters;

		void IAudioEventDescription.ApplyStaticParameters(IAudioSource source)
		{
			foreach (var parameter in _parameters)
			{
				source.SetParameter(parameter);
			}
		}
	}
}