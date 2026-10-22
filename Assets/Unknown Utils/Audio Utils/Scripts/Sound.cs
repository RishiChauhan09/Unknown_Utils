using UnityEngine;

namespace Unknown.Manager {

    [CreateAssetMenu(fileName = "Sound", menuName = "Unknown/Sound")]
    public class Sound : ScriptableObject {
        public string id;

        // volume
        public bool useRandomVolume;
        public Vector2 randomVolumeRange = new Vector2(.9f, 1f);
        public float volume = 1f;

        // pitch
        public bool useRandomPitch;
        public float pitch = 1;
        public Vector2 randomPitchRange = new Vector2(.95f, 1.05f);

        // other settings
        public float cooldownTime;

        // clip mode
        public ClipSelectionMode defaultSelectionMode = ClipSelectionMode.First;
        public float sequenceResetTimer = 1f;

        // clips
        public AudioClip[] clips;

        public enum ClipSelectionMode {
            First,                  // first clip provided in array
            Random,                 // random clip from clips have chances of being repeated
            Shuffle,                // make sure there is no repeated clip 
            Sequence,               // 0 -> clip length and then repeat 
            PingPong,               // 0 -> clip length and then clip length -> 0
        }

        private void OnValidate() {
            id = name;
        }
    }
}