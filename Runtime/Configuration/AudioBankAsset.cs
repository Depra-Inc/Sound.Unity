using UnityEngine;
using System.Collections.Generic;

namespace Depra.Sound.Configuration
{
	public abstract class AudioBankAsset : ScriptableObject
	{
		public abstract IReadOnlyList<AudioBankEntry> Events { get; }
	}
}