using System;
using System.Collections.Generic;
using System.Linq;
using Depra.Sound.Runtime.Configuration.Interfaces;
using UnityEngine;

namespace Depra.Sound.Runtime.Configuration.Implementations
{
    [Serializable]
    public struct SoundEntry : ISoundData
    {
        private const float DefaultVolume = 1f;

        [field: SerializeField] public string Key { get; private set; }
        [field: Range(0f, 1f), SerializeField] public float Volume { get; private set; }
        [field: SerializeField] public List<AudioClip> Clips { get; private set; }

        public SoundEntry(string key, float volume = DefaultVolume, List<AudioClip> clips = null)
        {
            Key = key;
            Volume = volume;
            Clips = clips;
        }

        public bool IsValid()
        {
            return string.IsNullOrEmpty(Key) == false && Clips != null;
        }

        internal void RemoveNulls()
        {
            Clips = Clips.Where(clip => clip != null).ToList();
        }
    }
}