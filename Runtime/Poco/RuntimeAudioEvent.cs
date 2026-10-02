using System.Runtime.CompilerServices;

namespace Depra.Sound.Runtime
{
	/// <summary>
	/// Compiled, hot-path-friendly audio event: a clip plus ready-to-apply static parameters.
	/// </summary>
	public sealed class RuntimeAudioEvent : IAudioEventDescription
	{
		private readonly IAudioClip _clip;
		private readonly IAudioEventContract _contract;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public RuntimeAudioEvent(IAudioClip clip, IAudioEventContract contract)
		{
			_clip = clip;
			_contract = contract;
		}

		IAudioClip IAudioEventDescription.Clip
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _clip;
		}

		IAudioEventContract IAudioEventDescription.Contract
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => _contract;
		}
	}
}