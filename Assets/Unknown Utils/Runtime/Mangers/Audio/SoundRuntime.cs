using UnityEngine;
using static Unknown.Manager.Sound;

namespace Unknown.Manager {
    public class SoundRuntime {
        public Sound sound;

        public bool sequenceForward = true;
        public int lastAudioIndex = 0;
        public float lastPlayTime = 0;
        public float lastSequencePlayedTime = 0;

        public SoundRuntime(Sound sound) {
            this.sound = sound;
        }

        #region Get Methods 
        public bool IsCooldownCompleted() {
            if (Time.time - lastPlayTime >= sound.cooldownTime)
                return true;

            return false;
        }


        public AudioClip GetClip() {
            if (!IsCooldownCompleted())
                return null;

            if (sound.clips == null || sound.clips.Length == 0) {
                Debug.LogError("Make sure you provide some clips");
                return null;
            }

            lastPlayTime = Time.time;

            if (sound.clips.Length == 1)
                return sound.clips[0];

            switch (sound.defaultSelectionMode) {
                case ClipSelectionMode.First:
                    return sound.clips[0];

                case ClipSelectionMode.Random:
                    return GetRandomClip();

                case ClipSelectionMode.Shuffle:
                    return GetShuffelClip();

                case ClipSelectionMode.Sequence:
                    return GetSequenceClip();

                case ClipSelectionMode.PingPong:
                    return GetPingPongClip();

                default:
                    return sound.clips[0];
            }
        }


        public float GetVolume() {
            if (sound.useRandomVolume) {
                return Random.Range(sound.randomVolumeRange.x, sound.randomVolumeRange.y);
            } else {
                return sound.volume;
            }
        }

        public float GetPitch() {
            if (sound.useRandomPitch) {
                return Random.Range(sound.randomPitchRange.x, sound.randomPitchRange.y);
            } else {
                return sound.pitch;
            }
        }

        // getting clips methods
        private AudioClip GetRandomClip() {
            return sound.clips[Random.Range(0, sound.clips.Length)];
        }

        private AudioClip GetSequenceClip() {
            if (Time.time - lastSequencePlayedTime > sound.sequenceResetTimer)
                lastAudioIndex = 0;

            AudioClip clip = sound.clips[lastAudioIndex];

            lastAudioIndex = (lastAudioIndex + 1) % sound.clips.Length;

            lastSequencePlayedTime = Time.time;
            return clip;
        }

        private AudioClip GetPingPongClip() {
            if (Time.time - lastSequencePlayedTime > sound.sequenceResetTimer) {
                lastAudioIndex = 0;
                sequenceForward = true;
            }

            AudioClip clip = sound.clips[lastAudioIndex];

            if (sequenceForward) {
                if (lastAudioIndex >= sound.clips.Length - 1) {
                    sequenceForward = false;
                    lastAudioIndex--;
                } else {
                    lastAudioIndex++;
                }
            } else {
                if (lastAudioIndex <= 0) {
                    sequenceForward = true;
                    lastAudioIndex++;
                } else {
                    lastAudioIndex--;
                }
            }

            lastSequencePlayedTime = Time.time;
            return clip;
        }

        private AudioClip GetShuffelClip() {
            int randomIndex = Random.Range(0, sound.clips.Length);
            while (randomIndex == lastAudioIndex) {
                randomIndex = Random.Range(0, sound.clips.Length);
            }

            lastAudioIndex = randomIndex;
            return sound.clips[randomIndex];
        }

        #endregion

    }
}