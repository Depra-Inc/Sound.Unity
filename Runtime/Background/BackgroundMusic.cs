using Depra.Toolkit.Singletons;
using Depra.Toolkit.Singletons.Runtime.Mono;
using Depra.Toolkit.SoundSystem.Runtime.Core;
using UnityEngine;

namespace Depra.Toolkit.SoundSystem.Runtime.Background
{
    /// <summary>
    /// This class manages the background music of the game.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class BackgroundMusic : MonoSingleton<BackgroundMusic>
    {
        protected override void OnRegistration()
        {
            if (SoundPlayer.MusicEnabled == false)
            {
                GetComponent<AudioSource>().mute = true;
            }
        }
    }
}