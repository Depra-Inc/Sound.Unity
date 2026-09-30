using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Depra.Sound.Runtime
{
	// Pure POCOs built once from the authoring data at startup,
	// with no UnityEngine.Object/serialization overhead - safe to read from the hot path.
	public sealed class RuntimeAudioTable : IAudioTable
	{
		private readonly Dictionary<AudioEventId, IAudioEventDescription> _events = new();

		internal RuntimeAudioTable(IReadOnlyList<AudioBankAsset> banks)
		{
			foreach (var bank in banks)
			{
				if (bank != null)
				{
					bank.Compile(_events);
				}
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		bool IAudioTable.TryResolve(AudioEventId eventId, out IAudioEventDescription description) =>
			_events.TryGetValue(eventId, out description);
	}
}