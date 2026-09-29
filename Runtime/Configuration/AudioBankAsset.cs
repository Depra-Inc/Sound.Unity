using UnityEngine;
using System.Collections.Generic;

namespace Depra.Sound.Configuration
{
	public abstract class AudioBankAsset : ScriptableObject
	{
		public abstract bool Contains(AudioEventId id);
		public abstract IEnumerable<(ulong id, string label)> GetAllEventNames();
		public abstract void Compile(IDictionary<AudioEventId, IAudioEventDescription> map);
	}
}