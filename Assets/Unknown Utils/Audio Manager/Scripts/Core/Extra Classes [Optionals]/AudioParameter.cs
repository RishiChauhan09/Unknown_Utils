using UnityEngine;

namespace Unknown.Manager.AudioManager {

    [System.Serializable]
    public class AudioParameter {

        public float volume = 1;
        public float pitch = 1;
        public float delay = 0;
        public float normalizedStartPos = 0;
        public float normalizedEndPos = 1;

    }

}