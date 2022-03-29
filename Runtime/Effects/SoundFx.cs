using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

namespace Depra.Sound.Runtime.Effects
{
    /// <summary>
    /// An abstraction over Unity's audio source component. It is used
    /// in a sound pool that avoids having to dynamically create and
    /// destroy sounds dynamically.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class SoundFx : MonoBehaviour
    {
        [field: SerializeField] public AudioSource AudioSource { get; private set; }

        private void Awake()
        {
            UpdateSound();
            SoundPlayer.UpdateSound += UpdateSound;
        }

        private void OnDestroy()
        {
            SoundPlayer.UpdateSound -= UpdateSound;
        }

        private void UpdateSound()
        {
            AudioSource.mute = SoundPlayer.SoundEnabled == false;
        }

        public void Play(AudioClip clip, float volume)
        {
            if (clip == null)
            {
                throw new NullReferenceException("Audio clip is null!");
            }
            
            AudioSource.clip = clip;
            AudioSource.volume = volume;
            AudioSource.loop = false;
            AudioSource.Play();
            
            Invoke(nameof(KillSoundFx), clip.length + 0.1f);
        }
        
        public void Play(AudioClip clip)
        {
            if (clip == null)
            {
                throw new NullReferenceException("Audio clip is null!");
            }
            
            AudioSource.clip = clip;
            AudioSource.Play();
            Invoke(nameof(KillSoundFx), clip.length + 0.1f);
        }

        public void PlayLoop(AudioClip clip, float volume)
        {
            if (clip == null)
            {
                throw new NullReferenceException("Audio clip is null!");
            }
            
            AudioSource.clip = clip;
            AudioSource.volume = volume;
            AudioSource.loop = true;
            AudioSource.Play();
        }

        public void StopLoop()
        {
            KillSoundFx();
        }

        private void KillSoundFx()
        {
            Destroy(gameObject);
        }

        [Button]
        public void Reset()
        {
            AudioSource = GetComponent<AudioSource>();
        }

        private void OnValidate()
        {
            Assert.IsNotNull(AudioSource);
        }
    }
}