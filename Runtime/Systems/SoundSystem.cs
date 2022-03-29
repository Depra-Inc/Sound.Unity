using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Depra.Extensions.CsharpTypes;
using Depra.Extensions.Exceptions;
using Depra.Sound.Runtime.Configuration.Implementations;
using Depra.Sound.Runtime.Configuration.Interfaces;
using Depra.Sound.Runtime.Effects;
using Depra.Toolkit.Serialization.Interfaces.Runtime;
using UnityEngine;

namespace Depra.Sound.Runtime.Systems
{
    /// <summary>
    /// The sound system handles the sound pool of the current scene.
    /// </summary>
    public class SoundSystem : MonoBehaviour, ISoundSystem, ISoundFactoryUser
    {
        private const string OldSoundPrefix = "OldSoundName_";

        [SerializeField] private InterfaceReference<ISoundLibrary> _library;

        private Func<SoundFx> _factory;

        private Dictionary<string, SoundFx> _playingClips = new();
        private readonly Dictionary<string, SoundFx> _fxLoop = new();
        private readonly Dictionary<string, List<SoundEntry>> _nameToSoundList = new();

        private bool _isInitialized;

        private void Awake()
        {
            StartCoroutine(InitCoroutine());
        }

        public void Setup(Func<SoundFx> soundFactory)
        {
            _factory = soundFactory;
        }

        public void PlayFx(string key, Action endAction)
        {
            if (CheckInitialization() == false || CheckSoundKeyValidity(key) == false)
            {
                endAction?.Invoke();
                return;
            }

            if (TryGetRandomClipByKey(key, out var randomClip, out var volume))
            {
                PlayClip(randomClip, volume);
                StartCoroutine(PlayClipCoroutine(randomClip, endAction, volume));
            }
            else
            {
                endAction?.Invoke();
            }
        }

        public void PlayFx(string key)
        {
            if (CheckInitialization() == false || CheckSoundKeyValidity(key) == false)
            {
                return;
            }

            if (TryGetRandomClipByKey(key, out var randomClip, out var volume))
            {
                PlayClip(randomClip, volume);
            }
        }

        public void PlayLoopFx(string key)
        {
            if (CheckInitialization() == false || CheckSoundKeyValidity(key) == false)
            {
                return;
            }

            if (TryGetRandomClipByKey(key, out var randomClip, out var volume))
            {
                PlayLoopClip(randomClip, key, volume);
            }
        }

        public void PlayUnplayedFx(string key)
        {
            if (CheckInitialization() == false || CheckSoundKeyValidity(key) == false)
            {
                return;
            }

            if (TryGetRandomClipByKey(key, out var randomClip, out var volume) == false)
            {
                return;
            }

            _playingClips = _playingClips.RemoveNullValues();

            if (_playingClips.ContainsKey(key) && 
                _playingClips[key].AudioSource.clip == randomClip &&
                _playingClips[key].AudioSource.isPlaying)
            {
                return;
            }

            if (_playingClips.ContainsKey(key) == false)
            {
                _playingClips.Add(key, null);
            }

            _playingClips[key] = PlayClipReturn(randomClip, volume);
        }

        public void PlayNonRepeatingFx(string key)
        {
            if (CheckInitialization() == false || CheckSoundKeyValidity(key) == false)
            {
                return;
            }

            var sounds = _nameToSoundList[key].RandomElement();
            if (CheckSoundEntryValidity(sounds) == false)
            {
                return;
            }

            if (sounds.Clips.Count <= 1)
            {
                PlayFx(key);
                return;
            }

            var oldName = PlayerPrefs.GetString(OldSoundPrefix + key, "_");
            var clip = sounds.Clips.Where(clip => clip.name != oldName).RandomElement();
            if (clip == null)
            {
                return;
            }

            PlayerPrefs.SetString(OldSoundPrefix + key, clip.name);
            PlayClip(clip, sounds.Volume);
        }

        public SoundEntry GetRandomSound(string soundName)
        {
            if (_isInitialized == false || _nameToSoundList.ContainsKey(soundName) == false)
            {
                return default;
            }

            var sounds = _nameToSoundList[soundName].RandomElement();
            return sounds;
        }

        private void PlayClip([NotNull] AudioClip clip, float volume = 1.0f)
        {
            var soundFx = _factory.Invoke();
            soundFx.Play(clip, volume);
        }

        private SoundFx PlayClipReturn([NotNull] AudioClip clip, float volume = 1.0f)
        {
            var soundFx = _factory.Invoke();
            soundFx.Play(clip, volume);
            return soundFx;
        }

        private void PlayLoopClip([NotNull] AudioClip clip, string key, float volume = 1.0f)
        {
            if (_fxLoop.ContainsKey(key))
            {
                return;
            }

            var soundFx = _factory.Invoke();
            soundFx.PlayLoop(clip, volume);
            _fxLoop.Add(key, soundFx);
        }

        public void StopLoopFx(string key)
        {
            if (_fxLoop.ContainsKey(key) == false)
            {
                return;
            }

            _fxLoop[key].StopLoop();
            _fxLoop.Remove(key);
        }

        private IEnumerator PlayClipCoroutine([NotNull] AudioClip clip, Action endAction, float volume = 1.0f)
        {
            var soundFx = _factory.Invoke();
            soundFx.Play(clip, volume);

            while (soundFx.AudioSource.isPlaying)
            {
                yield return null;
            }

            endAction?.Invoke();
        }

        private IEnumerator InitCoroutine()
        {
            foreach (var soundEntry in _library.Value.GetAllSound())
            {
                soundEntry.RemoveNulls();
                
                if (_nameToSoundList.ContainsKey(soundEntry.Key) == false)
                {
                    _nameToSoundList.Add(soundEntry.Key, new List<SoundEntry>());
                }

                _nameToSoundList[soundEntry.Key].Add(soundEntry);
            }

            SoundPlayer.Initialize(this);
            _isInitialized = true;

            yield return null;
        }

        private bool CheckInitialization()
        {
            if (_isInitialized == false)
            {
                throw new InitializationException($"{nameof(SoundSystem)} is not initialized!");
            }

            return true;
        }

        private bool CheckSoundKeyValidity(string key)
        {
            if (key.IsNullOrEmpty() == false)
            {
                throw new ArgumentException($"{key} is not valid!");
            }

            if (_nameToSoundList.ContainsKey(key) == false)
            {
                Debug.LogWarning($"{key} not found!");
                return false;
            }

            return true;
        }

        private bool TryGetRandomClipByKey(string key, out AudioClip randomClip, out float volume)
        {
            randomClip = default;
            volume = 1f;

            var sounds = _nameToSoundList[key].RandomElement();
            if (CheckSoundEntryValidity(sounds) == false)
            {
                return false;
            }

            randomClip = sounds.Clips.RandomElement();
            volume = sounds.Volume;

            return randomClip != null;
        }

        private bool CheckSoundEntryValidity(SoundEntry entry)
        {
            if (entry.IsValid() == false)
            {
                Debug.LogWarning($"{nameof(SoundEntry)} with key:{entry.Key} is not valid!");
                return false;
            }

            return true;
        }
    }
}