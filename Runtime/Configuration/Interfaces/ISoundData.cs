using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace Depra.Sound.Runtime.Configuration.Interfaces
{
    public interface ISoundData
    {
        [PublicAPI] string Key { get; }

        [PublicAPI] float Volume { get; }

        [PublicAPI] List<AudioClip> Clips { get; }
    }
}