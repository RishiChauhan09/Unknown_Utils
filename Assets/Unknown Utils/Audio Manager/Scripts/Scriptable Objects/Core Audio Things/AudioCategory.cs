using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unknown.Manager.AudioManager {

    //[CreateAssetMenu(fileName = "AudioCategory", menuName = "Unknown/Audio Manager/AudioCategory")]
    public class AudioCategory : ScriptableObject {

        private const int MaxCategories = 32;

        private static readonly string[] DefaultCategories = {
            "Default",
            "SFX",
            "UI",
            "Ambient",
            "Music",
            "Voice"
        };

        [SerializeField]
        public string[] categories = new string[MaxCategories];

        private void Reset() {
            categories = new string[MaxCategories];

            Array.Copy(DefaultCategories, categories, DefaultCategories.Length);
        }


        private void OnValidate() {
            if(categories.Length != MaxCategories) {
                Reset();
            }

            foreach(string s in categories) {
                if(!string.IsNullOrEmpty(s)) {
                    return;
                }
            }

            Reset();
        }

        public int GetIndex(string audioType) {
            if(string.IsNullOrEmpty(audioType))
                return -1;

            for(int i = 0; i < categories.Length; i++) {
                if(string.IsNullOrEmpty(categories[i]))
                    continue;

                if(string.Equals(audioType, categories[i]))
                    return i;
            }

            return -1;
        }

        public string GetString(int index) {
            if(index >= categories.Length || index < 0)
                return null;

            return categories[index];
        }

        public static List<string> GetAllTypes() {
            AudioCategory audioCategory = Resources.Load<AudioCategory>(nameof(AudioCategory));

            List<string> allTypes = new();

            foreach(string s in audioCategory.categories) {
                if(s == "" || string.IsNullOrEmpty(s))
                    continue;

                allTypes.Add(s);
            }

            return allTypes;
        }

        public static class NameOf {
            public static string CATEGORIES_LIST = nameof(categories);
        }
    }
}