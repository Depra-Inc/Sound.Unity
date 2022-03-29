using System;
using Depra.Sound.Runtime.Effects;

namespace Depra.Sound.Runtime.Configuration.Interfaces
{
    internal interface ISoundSystemConfiguration
    {
        Func<SoundFx> Factory { get; }
    }
}