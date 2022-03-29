using System.Collections.Generic;
using Depra.Sound.Runtime.Configuration.Implementations;
using JetBrains.Annotations;

namespace Depra.Sound.Runtime.Configuration.Interfaces
{
    public interface ISoundLibrary
    {
        [PublicAPI]
        IEnumerable<SoundEntry> GetAllSound();
    }
}