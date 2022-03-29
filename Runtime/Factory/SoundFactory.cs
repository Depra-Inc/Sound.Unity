using System;
using Depra.Sound.Runtime.Effects;
using Object = UnityEngine.Object;

namespace Depra.Sound.Runtime.Factory
{
    [Serializable]
    [AddTypeMenu("Manual")]
    public class SoundFactory : ISoundFactory
    {
        public SoundFx Create(SoundFx prefab) => Object.Instantiate(prefab);
    }
}