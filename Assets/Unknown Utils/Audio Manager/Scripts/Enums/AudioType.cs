using UnityEngine;

namespace Unknown.Manager.AudioManager {

    [System.Serializable]
    public class AudioType {

        public string audioTypeName;
        public int index;

        public uint Mask => 1u << index;

        public static class NameOf {
            public static string AudioTypeName = nameof(audioTypeName);
            public static string Index = nameof(index);
        }

    }
}