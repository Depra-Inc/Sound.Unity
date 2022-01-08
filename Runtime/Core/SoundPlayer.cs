using System;
using Depra.Toolkit.SoundSystem.Runtime.Background;
using Depra.Toolkit.SoundSystem.Runtime.Configuration;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;

namespace Depra.Toolkit.SoundSystem.Runtime.Core
{
    /// <summary>
    /// Static utility class that provides easy access to the current
    /// scene's sound system.
    /// </summary>
    public static class SoundPlayer
    {
        private const string SoundActiveKey = "Sound_Enabled";
        private const string MusicActiveKey = "Music_Enabled";
        
        public static bool SoundEnabled
        {
            get => PlayerPrefs.GetInt(SoundActiveKey, 1) == 1;
            set => PlayerPrefs.SetFloat(SoundActiveKey, value ? 1 : 0);
        }
        
        public static bool MusicEnabled
        {
            get => PlayerPrefs.GetInt(MusicActiveKey, 1) == 1;
            set
            {
                PlayerPrefs.SetInt(MusicActiveKey, value ? 1 : 0);
                
                var bgMusic = Object.FindObjectOfType<BackgroundMusic>();
                
                if (bgMusic)
                    bgMusic.GetComponent<AudioSource>().mute = !value;
            }
        }
        
        private static SoundSystem _soundSystem;

        public delegate void UpdateSoundDelegate();
        public static event UpdateSoundDelegate UpdateSound;
        public delegate void UpdateMusicDelegate();
        
        public static event UpdateMusicDelegate UpdateMusic;

        public static void InvokeEvents()
        {
            UpdateMusic?.Invoke();
            UpdateSound?.Invoke();
        }

        public static void Initialize()
        {
            _soundSystem = Object.FindObjectOfType<SoundSystem>();
            Assert.IsNotNull(_soundSystem);
        }
        
        public static void Initialize(SoundSystem system)
        {
            _soundSystem = system;
            Assert.IsNotNull(_soundSystem);
        }

        public static void PlaySoundFx(string soundName)
        {
            if (string.IsNullOrWhiteSpace(soundName))
            {
                return;
            }

            if (_soundSystem != null && _soundSystem)
            {
                _soundSystem.PlaySoundFx(soundName);
            }
        }

        public static void PlayLoopFX(string soundName)
        {
            if (string.IsNullOrWhiteSpace(soundName))
            {
                return;
            }

            if (_soundSystem != null && _soundSystem)
            {
                _soundSystem.PlayLoopFx(soundName);
            }
        }

        public static void StopLoopFX(string soundName)
        {
            if (string.IsNullOrWhiteSpace(soundName))
            {
                return;
            }

            if (_soundSystem != null && _soundSystem)
            {
                _soundSystem.StopLoop(soundName);
            }
        }
        
        public static SoundContainer GetContainer(string name)
        {
            if (_soundSystem != null && _soundSystem)
            {
                return _soundSystem.GetRandomSound(name);
            }

            return null;
        }

        public static void PlayCoroSoundFX(string name, Action onComplete)
        {
            if (_soundSystem != null && _soundSystem)
            {
                _soundSystem.PlayEndAction(name, onComplete);
            }
            else
            {
                onComplete?.Invoke();
            }
        }
        
        public static void PlayNotRepeatSoundFx(string soundName)
        {
            if (_soundSystem != null && _soundSystem)
            {
                _soundSystem.PlayNotRepeatSoundFx(soundName);
            }
        }

        public static void PlayNotPlaying(string soundName)
        {
            if (_soundSystem != null && _soundSystem)
            {
                _soundSystem.PlayNotPlaying(soundName);
            }
        }
    }
}