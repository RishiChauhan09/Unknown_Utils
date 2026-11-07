using UnityEngine;

namespace Unknown.Manager.AudioManager {

    public interface IAudioClip {

        public float delay { get; set; }
        public float volume { get; set; }

        public float startPosition { get; set; }
        public float endPosition { get; set; }

        public AudioClip GetAudioClip();
        public bool IsValid();

    }
}