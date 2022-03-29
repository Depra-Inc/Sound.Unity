using System;
using Depra.Sound.Runtime.Configuration.Implementations;
using JetBrains.Annotations;

namespace Depra.Sound.Runtime.Systems
{
    public interface ISoundSystem
    {
        [PublicAPI]
        void PlayFx(string key);

        [PublicAPI]
        void PlayFx(string key, [NotNull] Action endAction);

        [PublicAPI]
        void PlayUnplayedFx(string key);

        [PublicAPI]
        void PlayNonRepeatingFx(string key);

        [PublicAPI]
        void PlayLoopFx(string key);

        [PublicAPI]
        void StopLoopFx(string key);

        [PublicAPI]
        SoundEntry GetRandomSound(string soundName);
    }
}