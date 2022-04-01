using Depra.Tools.Singletons.Runtime.Core.Attributes;
using Depra.Tools.Singletons.Runtime.Unity;
using Depra.Tools.Utils.Runtime.Singletons.Runtime.Unity;
using UnityEngine;

namespace Depra.Sound.Runtime.Background
{
    /// <summary>
    /// This class manages the background music of the game.
    /// </summary>
    [Singleton]
    [RequireComponent(typeof(AudioSource))]
    public class BackgroundMusic : MonoSingleton<BackgroundMusic>
    {
        private AudioSource _source;
        
        protected override void InitializeOverride()
        {
            _source = GetComponent<AudioSource>();
            SetMute(SoundPlayer.MusicEnabled == false);
        }

        public void SetMute(bool mute)
        {
            _source.mute = mute;
        }

        public void SetVolume(float volume)
        {
            _source.volume = volume;
        }
    }
}