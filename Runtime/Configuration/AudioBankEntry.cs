using System;
using Depra.SerializeReference.Extensions;

namespace Depra.Sound.Configuration
{
	[Serializable]
	public struct AudioBankEntry
	{
		public string Name;
		public AudioEventId Id;

		[SerializeReferenceDropdown]
		[UnityEngine.SerializeReference]
		public IAudioEventVariant Description;
	}
}