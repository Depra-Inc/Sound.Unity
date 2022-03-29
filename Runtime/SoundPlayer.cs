using System;
using Depra.Sound.Runtime.Background;
using Depra.Sound.Runtime.Configuration.Implementations;
using Depra.Sound.Runtime.Configuration.Interfaces;
using Depra.Sound.Runtime.Systems;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace Depra.Sound.Runtime
{
    /// <summary>
    /// Static utility class that provides easy access to the current scene's sound system.
    /// </summary>
    public static class SoundPlayer
    {
        private const string SoundActiveKey = "Sound_Enabled";
        private const string MusicActiveKey = "Music_Enabled";

        private static ISoundSystem _soundSystem;
        private static readonly ISoundSystemConfiguration Config;

        public static bool SoundEnabled => PlayerPrefs.GetInt(SoundActiveKey, 1) == 1;
        public static bool MusicEnabled => PlayerPrefs.GetInt(MusicActiveKey, 1) == 1;

        public delegate void UpdateSoundDelegate();
        public static event UpdateSoundDelegate UpdateSound;

        public delegate void UpdateMusicDelegate();
        public static event UpdateMusicDelegate UpdateMusic;

        public static void Initialize() => Initialize(Object.FindObjectOfType<SoundSystem>());

        public static void Initialize(SoundSystem system)
        {
            _soundSystem = system;

            Assert.IsNotNull(_soundSystem);

            system.Setup(Config.Factory);
        }

        public static void SetSoundEnabled(bool enabled)
        {
            PlayerPrefs.SetFloat(SoundActiveKey, enabled ? 1 : 0);
            UpdateSound?.Invoke();
        }

        public static void SetMusicEnabled(bool enabled)
        {
            PlayerPrefs.SetInt(MusicActiveKey, enabled ? 1 : 0);

            var bgMusic = Object.FindObjectOfType<BackgroundMusic>();
            if (bgMusic)
            {
                bgMusic.SetMute(enabled == false);
            }
            
            UpdateMusic?.Invoke();
        }
        
        public static void InvokeEvents()
        {
            UpdateMusic?.Invoke();
            UpdateSound?.Invoke();
        }

        public static void PlaySoundFx(string soundName)
        {
            if (SoundEnabled)
            {
                _soundSystem?.PlayFx(soundName);
            }
        }

        public static void PlayLoopFX(string soundName)
        {
            if (SoundEnabled)
            {
                _soundSystem?.PlayLoopFx(soundName);
            }
        }

        public static void StopLoopFX(string soundName)
        {
            if (SoundEnabled)
            {
                _soundSystem?.StopLoopFx(soundName);
            }
        }

        public static void PlayCoroSoundFX(string soundName, Action onComplete)
        {
            if (SoundEnabled && _soundSystem != null)
            {
                _soundSystem.PlayFx(soundName, onComplete);
            }
            else
            {
                onComplete?.Invoke();
            }
        }

        public static void PlayNotRepeatSoundFx(string soundName)
        {
            if (SoundEnabled)
            {
                _soundSystem?.PlayNonRepeatingFx(soundName);
            }
        }

        public static void PlayNotPlaying(string soundName)
        {
            if (SoundEnabled)
            {
                _soundSystem?.PlayUnplayedFx(soundName);
            }
        }

        public static SoundEntry GetContainer(string soundName) => _soundSystem?.GetRandomSound(soundName) ?? default;

        static SoundPlayer()
        {
            Config = SoundConfig.Instance;
        }
    }
}