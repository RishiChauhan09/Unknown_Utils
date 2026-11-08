using System;
using UnityEngine;

namespace Unknown.Manager.AudioManager {

    [CreateAssetMenu(fileName = "AudioCategory", menuName = "Unknown/Audio Manager/AudioCategory")]
    public class AudioCategory : ScriptableObject {

        private const int MaxCategories = 32;

        private static readonly string[] DefaultCategories = {
            "SFX",
            "UI",
            "Ambient",
            "Music",
            "Voice"
        };

        [SerializeField]
        private string[] categories = new string[MaxCategories];

        private void Reset() {
            categories = new string[MaxCategories];

            Array.Copy(DefaultCategories, categories, DefaultCategories.Length);
        }


        private void OnValidate() {
            if(categories.Length == MaxCategories)
                return;

            string[] arr = new string[MaxCategories];
            Array.Copy(categories, arr, MaxCategories);
            categories = arr;
        }
    }
}