using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Depra.SoundSystem.Core;
using Depra.Toolkit.Serialization.Interfaces.Runtime;
using Depra.Toolkit.SoundSystem.Runtime.Configuration;
using UnityEngine;

namespace Depra.Toolkit.SoundSystem.Runtime.Core
{
    /// <summary>
    /// The sound system handles the sound pool of the current scene.
    /// </summary>
    public class SoundSystem : MonoBehaviour
    {
        [SerializeField] private SoundFx _prefab;

        [SerializeField] private InterfaceReference<ISoundLibrary> _library;
        //private ObjectPool _pool;

        private Dictionary<string, SoundFx> _playingClips = new Dictionary<string, SoundFx>();
        private readonly Dictionary<string, SoundFx> _fxLoop = new Dictionary<string, SoundFx>();

        private readonly Dictionary<string, List<SoundContainer>> _nameToSoundList =
            new Dictionary<string, List<SoundContainer>>();

        private bool _isInit;

        private void Awake()
        {
            StartCoroutine(WaitPool());
        }

        private void Start()
        {
            //_pool.Initialize();
        }

        public void PlayNotRepeatSoundFx(string soundName)
        {
            if (_isInit == false || _nameToSoundList.ContainsKey(soundName) == false)
            {
                return;
            }

            var sounds = _nameToSoundList[soundName].RandomElement();
            if (sounds?.Clips == null || sounds.Clips.All(t => t == null))
            {
                return;
            }

            var jobClips = sounds.Clips.Where(t => t != null).ToList();
            if (jobClips.Count <= 1)
            {
                PlaySoundFx(soundName);
                return;
            }

            var oldName = PlayerPrefs.GetString("OldSoundName_" + soundName, "_");
            var clip = jobClips.Where(t => t.name != oldName).RandomElement();

            if (clip == null)
            {
                return;
            }

            PlayerPrefs.SetString("OldSoundName_" + soundName, clip.name);
            PlaySoundFx(clip, sounds.Volume);
        }

        public void PlayEndAction(string soundName, Action endAction)
        {
            if (_isInit == false)
            {
                endAction?.Invoke();
                return;
            }

            if (_nameToSoundList.ContainsKey(soundName))
            {
                var clip = _nameToSoundList[soundName].RandomElement();
                if (clip != null)
                {
                    PlaySoundFx(clip.Clips.RandomElement(), clip.Volume);
                    StartCoroutine(PlaySoundFxIE(clip.Clips.RandomElement(), endAction, clip.Volume));
                }
                else
                {
                    endAction?.Invoke();
                }
            }
            else
            {
                endAction?.Invoke();
            }
        }

        private IEnumerator PlaySoundFxIE(AudioClip clip, Action endAction, float volume = 1.0f)
        {
            if (_isInit == false)
            {
                endAction?.Invoke();
                yield break;
            }

            if (SoundPlayer.SoundEnabled && clip != null)
            {
                //var fx = _pool.GetObject().GetComponent<SoundFx>();
                var fx = Instantiate(_prefab);
                fx.Play(clip, volume);

                while (fx.AudioSource.isPlaying)
                {
                    yield return null;
                }
            }

            endAction?.Invoke();
        }

        public void ReformateNull()
        {
            var clips = new Dictionary<string, SoundFx>();
            foreach (var oldClips in _playingClips)
            {
                if (oldClips.Value == null)
                {
                    continue;
                }

                if (clips.ContainsKey(oldClips.Key) == false)
                {
                    clips.Add(oldClips.Key, oldClips.Value);
                }
            }

            _playingClips = clips;
        }

        public void PlayNotPlaying(string soundName)
        {
            if (_isInit == false)
            {
                return;
            }

            ReformateNull();

            if (_nameToSoundList.ContainsKey(soundName) == false)
            {
                return;
            }

            var clip = _nameToSoundList[soundName].RandomElement();
            if (clip == null)
            {
                return;
            }

            var randomClip = clip.Clips.RandomElement();

            if (_playingClips.ContainsKey(soundName) && 
                _playingClips[soundName].AudioSource.clip == randomClip &&
                _playingClips[soundName].AudioSource.isPlaying)
            {
                return;
            }

            if (_playingClips.ContainsKey(soundName) == false)
            {
                _playingClips.Add(soundName, null);
            }

            _playingClips[soundName] = PlaySoundFxReturn(randomClip, clip.Volume);
        }

        public void PlaySoundFx(string soundName)
        {
            if (_isInit == false || _nameToSoundList.ContainsKey(soundName) == false)
            {
                return;
            }

            var clip = _nameToSoundList[soundName].RandomElement();
            if (clip == null)
            {
                return;
            }

            PlaySoundFx(clip.Clips.RandomElement(), clip.Volume);
        }

        public void PlayLoopFx(string soundName)
        {
            if (_isInit == false || _nameToSoundList.ContainsKey(soundName) == false)
            {
                return;
            }

            var clip = _nameToSoundList[soundName].RandomElement();
            if (clip == null)
            {
                return;
            }

            PlayLoop(clip.Clips.RandomElement(), soundName, clip.Volume);
        }

        public SoundContainer GetRandomSound(string soundNam)
        {
            if (_isInit == false || _nameToSoundList.ContainsKey(soundNam) == false)
            {
                return null;
            }

            var clip = _nameToSoundList[soundNam].RandomElement();
            return clip;
        }

        private void PlaySoundFx(AudioClip clip, float volume = 1.0f)
        {
            if (_isInit == false || SoundPlayer.SoundEnabled == false || clip == null)
            {
                return;
            }

            //_pool.GetObject().GetComponent<SoundFx>().Play(clip, volume);
            var fx = Instantiate(_prefab);
            fx.Play(clip, volume);
        }

        public void PlayLoop(AudioClip clip, string key, float volume = 1.0f)
        {
            if (_isInit == false || SoundPlayer.SoundEnabled == false || clip == null || _fxLoop.ContainsKey(key))
            {
                return;
            }

            //var fx = _pool.GetObject().GetComponent<SoundFx>();
            var fx = Instantiate(_prefab);
            fx.PlayLoop(clip, volume);
            _fxLoop.Add(key, fx);
        }

        public void StopLoop(string key)
        {
            if (_fxLoop.ContainsKey(key) == false)
            {
                return;
            }

            _fxLoop[key].StopLoop();
            _fxLoop.Remove(key);
        }

        private SoundFx PlaySoundFxReturn(AudioClip clip, float volume = 1.0f)
        {
            if (_isInit == false || SoundPlayer.SoundEnabled == false || clip == null)
            {
                return null;
            }

            //var obj = _pool.GetObject();
            //var sfx = obj.GetComponent<SoundFx>();
            var sfx = Instantiate(_prefab);
            sfx.Play(clip, volume);

            return sfx;
        }

        private IEnumerator WaitPool()
        {
            foreach (var sound in _library.Value.GetAllSound())
            {
                if (_nameToSoundList.ContainsKey(sound.Key) == false)
                {
                    _nameToSoundList.Add(sound.Key, new List<SoundContainer>());
                }

                _nameToSoundList[sound.Key].Add(sound);
            }

            SoundPlayer.Initialize(this);
            _isInit = true;

            yield return null;
        }
    }
}