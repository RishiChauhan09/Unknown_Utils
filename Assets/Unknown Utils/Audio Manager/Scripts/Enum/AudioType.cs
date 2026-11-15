using UnityEngine;

namespace Unknown.Manager.AudioManager {

    [System.Serializable]
    public class AudioType {

        public string audioTypeName;
        public int index;

        public uint Mask => 1u << index;

        /// <summary>
        /// Returns the audio type from string of name
        /// </summary>
        public static AudioType NameToAudioType(string name) {
            AudioCategory audioCategory = Resources.Load(nameof(AudioCategory)) as AudioCategory;
            int index = audioCategory.GetIndex(name);
            if(index < 0)
                return null;

            return new AudioType() {
                audioTypeName = name,
                index = index
            };
        }

        public static class NameOf {
            public static string AudioTypeName = nameof(audioTypeName);
            public static string Index = nameof(index);
        }

    }
}