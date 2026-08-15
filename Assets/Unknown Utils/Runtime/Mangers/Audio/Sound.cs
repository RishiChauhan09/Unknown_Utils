using UnityEngine;

namespace Unknown.Audio {

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



        // hide in inspector variables
        [HideInInspector] public bool sequenceForward = true;
        [HideInInspector] public int lastAudioIndex;
        [HideInInspector] public float lastPlayTime;
        [HideInInspector] public int lastAudioPlayed;


        #region Get Methods 
        public bool IsCooldownCompleted() {
            if(Time.time - lastPlayTime > cooldownTime)
                return true;

            return false;
        }


        public AudioClip GetClip() {
            if(!IsCooldownCompleted())
                return null;

            if(clips == null || clips.Length == 0) {
                Debug.LogError("Make sure you provide some clips");
                return null;
            }

            lastPlayTime = Time.time;

            if(clips.Length == 1)
                return clips[0];

            switch(defaultSelectionMode) {
                case ClipSelectionMode.First:
                    return clips[0];

                case ClipSelectionMode.Random:
                    return GetRandomClip();

                case ClipSelectionMode.Shuffle:
                    return GetShuffelClip();

                case ClipSelectionMode.Sequence:
                    return GetSequenceClip();

                case ClipSelectionMode.PingPong:
                    return GetPingPongClip();

                default:
                    return clips[0];
            }
        }

        public float GetVolume() {
            if(useRandomVolume) {
                return Random.Range(randomVolumeRange.x, randomVolumeRange.y);
            } else {
                return volume;
            }
        }

        public float GetPitch() {
            if(useRandomPitch) {
                return Random.Range(randomPitchRange.x, randomPitchRange.y);
            } else {
                return pitch;
            }
        }

        // getting clips methods
        private AudioClip GetRandomClip() {
            return clips[Random.Range(0, clips.Length)];
        }

        private AudioClip GetSequenceClip() {
            if(Time.time - lastPlayTime > sequenceResetTimer)
                lastAudioIndex = 0;

            AudioClip clip = clips[lastAudioIndex];

            lastAudioIndex = (lastAudioIndex + 1) % clips.Length;

            return clip;
        }

        private AudioClip GetPingPongClip() {
            if(Time.time - lastPlayTime > sequenceResetTimer) {
                lastAudioIndex = 0;
                sequenceForward = true;
            }

            AudioClip clip = clips[lastAudioIndex];

            if(sequenceForward) {
                if(lastAudioIndex >= clips.Length - 1) {
                    sequenceForward = false;
                    lastAudioIndex--;
                } else {
                    lastAudioIndex++;
                }
            } else {
                if(lastAudioIndex <= 0) {
                    sequenceForward = true;
                    lastAudioIndex++;
                } else {
                    lastAudioIndex--;
                }
            }

            return clip;
        }

        private AudioClip GetShuffelClip() {
            int randomIndex = Random.Range(0, clips.Length);
            while(randomIndex == lastAudioIndex) {
                randomIndex = Random.Range(0, clips.Length);
            }

            lastAudioIndex = randomIndex;
            return clips[randomIndex];
        }

        #endregion

        private void OnValidate() {
            id = name;
        }

    }
}