using System;
using Depra.Sound.Runtime.Effects;

namespace Depra.Sound.Runtime.Systems
{
    internal interface ISoundFactoryUser
    {
        void Setup(Func<SoundFx> soundFactory);
    }
}