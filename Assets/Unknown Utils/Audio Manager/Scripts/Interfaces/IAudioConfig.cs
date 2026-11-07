using UnityEngine;

namespace Unknown.Manager.AudioManager {

    public interface IAudioConfig {

        public AudioID GetAudioID();
        public AudioType GetAudioType();
        public float GetMasterVolume();
        public float GetPitch();
        public bool IsLooped();
        public UnknownAudioClip PickNewClip();
        public void ResetClipSelectionStrategy();
        public IAudioClip[] GetAudioClips();

    }
}