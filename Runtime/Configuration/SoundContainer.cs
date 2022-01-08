using System;
using System.Collections.Generic;
using UnityEngine;

namespace Depra.Toolkit.SoundSystem.Runtime.Configuration
{
    [Serializable]
    public class SoundContainer
    {
        [field: SerializeField] public string Key { get; private set; }

        [field: Min(0f)]
        [field: Range(0f, 1f)]
        [field: SerializeField]
        public float Volume { get; private set; } = 1.0f;

        [field: SerializeField] public List<AudioClip> Clips { get; private set; } = new List<AudioClip>();
    }
}