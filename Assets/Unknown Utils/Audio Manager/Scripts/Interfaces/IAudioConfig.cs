using UnityEngine;

namespace Unknown.Manager.AudioManager {

    public interface IAudioConfig {

        /// <summary>
        /// Returns audio id of this audio config
        /// </summary>
        public AudioID GetAudioID();

        /// <summary>
        /// Returns audio type of this auido config
        /// </summary>
        public AudioType GetAudioType();

        /// <summary>
        /// Returns master volume of this audio config
        /// </summary>
        public float GetMasterVolume();

        /// <summary>
        /// Returns pitch of this audio config
        /// </summary>
        public float GetPitch();

        /// <summary>
        /// Returns if this audio is looped
        /// </summary>
        public bool IsLooped();

        /// <summary>
        /// Returns if loop is same audio 
        /// </summary>
        public bool IsSameAudioLooped();

        /// <summary>
        /// Returns new audio clip by picking up new audio clip
        /// </summary>
        public UnknownAudioClip PickNewClip();

        /// <summary>
        /// Resets clip selection strategy
        /// </summary>
        public void ResetClipSelectionStrategy();

        /// <summary>
        /// Returns IAudioClip
        /// </summary>
        public IAudioClip[] GetAudioClips();

        /// <summary>
        /// Returns Audio3DSettings
        /// </summary>
        public Audio3DSettings GetAudio3DSettings();

    }
}