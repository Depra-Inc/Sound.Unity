using UnityEngine;
using UnityEngine.Assertions;

namespace Depra.Sound.Runtime.Utils
{
    /// <summary>
    /// On/off audio sources on objects.
    /// </summary>
    [AddComponentMenu("Audio/Audio Muter Component")]
    public class AudioMuter : MonoBehaviour
    {
        [SerializeField] private AudioSource _source;
        [SerializeField] [Tooltip("This flag tells our class whether the AudioSource is sound or music.")]
        private bool _isMusic;

        private void Start()
        {
            OnMusicSettingChanged();
        }

        private void OnEnable()
        {
            SoundPlayer.UpdateSound += OnSoundSettingChanged;
            SoundPlayer.UpdateMusic += OnMusicSettingChanged;
        }

        private void OnDisable()
        {
            SoundPlayer.UpdateSound -= OnSoundSettingChanged;
            SoundPlayer.UpdateMusic -= OnMusicSettingChanged;
        }

        private void OnMusicSettingChanged()
        {
            if (_isMusic)
            {
                SetMute(SoundPlayer.MusicEnabled == false);
            }
        }

        private void OnSoundSettingChanged()
        {
            if (_isMusic == false)
            {
                SetMute(SoundPlayer.SoundEnabled == false);
            }
        }

        private void SetMute(bool muteStatus)
        {
            _source.mute = muteStatus;
        }

        private void Reset()
        {
            _source = GetComponent<AudioSource>();
        }

        private void OnValidate()
        {
            Assert.IsNotNull(_source);
        }
    }
}