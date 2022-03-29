using System;
using System.Collections.Generic;
using Depra.Configuration.Runtime.Attributes;
using Depra.Configuration.Runtime.SO;
using Depra.Sound.Runtime.Configuration.Interfaces;
using Depra.Sound.Runtime.Effects;
using Depra.Sound.Runtime.Factory;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Depra.Sound.Runtime.Configuration.Implementations
{
    /// <summary>
    /// A collection of sounds. We use two collections in the game: one for the
    /// menu sounds and another for the game sounds.
    /// </summary>
    [Config("Sound")]
    [CreateAssetMenu(fileName = "SoundConfig", menuName = "Game/Sounds", order = 51)]
    public class SoundConfig : ObjectConfig<SoundConfig>, ISoundLibrary, ISoundSystemConfiguration
    {
        [Required] [SerializeField] private SoundFx _soundFxPrefab;

        [SubclassSelector] [SerializeReference]
        private ISoundFactory _factory = new SoundFactory();

        [SerializeField, BoxGroup] private List<SoundEntry> _sounds;
        
        public IEnumerable<SoundEntry> GetAllSound() => _sounds;
        
        Func<SoundFx> ISoundSystemConfiguration.Factory => () => _factory.Create(_soundFxPrefab);

#if UNITY_EDITOR
        [Button]
        [ContextMenu("Sort")]
        public void Sort()
        {
            var sortedDictionary = new SortedDictionary<string, List<SoundEntry>>();
            foreach (var item in _sounds)
            {
                if (sortedDictionary.ContainsKey(item.Key) == false)
                {
                    sortedDictionary.Add(item.Key, new List<SoundEntry>());
                }

                sortedDictionary[item.Key].Add(item);
            }

            UpdateSoundLibrary(sortedDictionary);
            EditorUtility.SetDirty(this);
        }

        private void UpdateSoundLibrary(IDictionary<string, List<SoundEntry>> dictionary)
        {
            var finalList = new List<SoundEntry>();
            foreach (var pair in dictionary)
            {
                finalList.AddRange(pair.Value);
            }

            _sounds = finalList;
        }
#endif

        private void OnValidate()
        {
            Assert.IsNotNull(_soundFxPrefab);
            Assert.IsNotNull(_factory);
        }
    }
}