using Depra.Sound.Runtime.Effects;

namespace Depra.Sound.Runtime.Factory
{
    public interface ISoundFactory
    {
        SoundFx Create(SoundFx prefab);
    }
}