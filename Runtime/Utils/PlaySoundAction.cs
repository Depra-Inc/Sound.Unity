using UnityEngine;

namespace Depra.Sound.Runtime.Utils
{
    /// <summary>
    /// Plays sound by key. Uses <see cref="SoundPlayer"/>.
    /// </summary>
    public class PlaySoundAction : MonoBehaviour
    {
        [SerializeField] private string _clipName;

        public void Fire()
        {
            SoundPlayer.PlaySoundFx(_clipName);
        }
    }
}