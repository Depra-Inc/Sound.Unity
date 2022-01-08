using System.Collections.Generic;

namespace Depra.Toolkit.SoundSystem.Runtime.Configuration
{
    public interface ISoundLibrary
    {
        IEnumerable<SoundContainer> GetAllSound();
    }
}
