using System;
using Depra.Sound.Runtime.Effects;

namespace Depra.Sound.Runtime.Factory
{
    [Serializable]
    [AddTypeMenu("Pool [Not implemented]")]
    public class SoundPool : ISoundFactory
    {
        public SoundFx Create(SoundFx prefab)
        {
            throw new System.NotImplementedException();
        }
    }
}