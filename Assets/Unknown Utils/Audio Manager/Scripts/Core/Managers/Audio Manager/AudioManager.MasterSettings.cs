using UnityEngine;

namespace Unknown.Manager.AudioManager {

    public static partial class AudioManager {

        /// <summary>
        /// Sets the master volume of audio mixer
        /// </summary>
        /// <param name="volume">Value of audio in normalized 0 - 1</param>
        public static void SetMasterVolume(float volume) {
            SoundManager.Instance.SetMasterVolume(volume);
        }


        /// <summary>
        /// Returns the normalized master volume
        /// </summary>
        public static float GetMasterVolume() {
            return SoundManager.Instance.GetMasterVolume();
        }


        /// <summary>
        /// Toggle the mute of master volume
        /// </summary>
        public static void ToggleMuteMaster() {
            SoundManager.Instance.ToggleMuteMaster();
        }


        /// <summary>
        /// Returns if the master audio mixer is muted or not
        /// </summary>
        public static bool IsMaterMuted() {
            return SoundManager.Instance.IsMasterMuted();
        }


        /// <summary>
        /// Sets the master audio mixer to mute
        /// </summary>
        public static void SetMasterMute(bool mute) {
            SoundManager.Instance.SetMasterMute(mute);
        }

    }

}