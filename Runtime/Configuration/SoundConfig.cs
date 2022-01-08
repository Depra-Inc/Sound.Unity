using System.Collections.Generic;
using Depra.Toolkit.Configuration.Runtime;
using UnityEditor;
using UnityEngine;

namespace Depra.Toolkit.SoundSystem.Runtime.Configuration
{
    /// <summary>
    /// A collection of sounds. We use two collections in the game: one for the
    /// menu sounds and another for the game sounds.
    /// </summary>
    [CreateAssetMenu(fileName = "SoundConfig", menuName = "Game/Sounds", order = 51)]
    public class SoundConfig : ObjectConfig, ISoundLibrary
    {
        [SerializeField] private List<SoundContainer> _sounds;

        public IEnumerable<SoundContainer> GetAllSound() => _sounds;

#if UNITY_EDITOR
        [ContextMenu("Sort")]
        public void SortFunc()
        {
            var sortedDictionary = new SortedDictionary<string, List<SoundContainer>>();
            foreach (var item in _sounds)
            {
                if (sortedDictionary.ContainsKey(item.Key) == false)
                {
                    sortedDictionary.Add(item.Key, new List<SoundContainer>());
                }

                sortedDictionary[item.Key].Add(item);
            }

            UpdateSoundLibrary(sortedDictionary);
            EditorUtility.SetDirty(this);
        }

        private void UpdateSoundLibrary(SortedDictionary<string, List<SoundContainer>> dictionary)
        {
            var finalList = new List<SoundContainer>();
            foreach (var pair in dictionary)
            {
                finalList.AddRange(pair.Value);
            }

            _sounds = finalList;
        }
#endif
    }
}