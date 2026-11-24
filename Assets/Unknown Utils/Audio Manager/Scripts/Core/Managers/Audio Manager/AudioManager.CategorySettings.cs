using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Unknown.Manager.AudioManager {
    
    public static partial class AudioManager {


        #region Audio type String

        /// <summary>
        /// Changes the volume of audio type specified 
        /// </summary>
        /// <param name="volume">0 - 1 Normalized value</param>
        public static void SetVolume(string audioType, float volume) {
            SoundManager.Instance.SetVolume(audioType, volume);
        }


        /// <summary>
        /// Returns the current volume of that audio type 0 - 1
        /// </summary>
        public static float GetVolume(string audioType) {
            return SoundManager.Instance.GetVolume(audioType);
        }


        /// <summary>
        /// Sets the audio type to mute
        /// </summary>
        public static void SetMute(string audioType, bool muteValue) {
            SoundManager.Instance.SetMute(audioType, muteValue);
        }


        /// <summary>
        /// Toggles the mute of that audio type
        /// </summary>
        public static void ToggleMute(string audioType) {
            SoundManager.Instance.ToggleMute(audioType);
        }


        /// <summary>
        /// Retusn if that audio type is muted or not
        /// </summary>
        public static bool IsMuted(string audioType) {
            return SoundManager.Instance.IsMuted(audioType);
        }

        #endregion



        #region Audio Type Object

        /// <summary>
        /// Changes the volume of audio type specified 
        /// </summary>
        /// <param name="volume">0 - 1 Normalized value</param>
        public static void SetVolume(AudioType audioType, float volume) {
            SoundManager.Instance.SetVolume(audioType.audioTypeName, volume);
        }


        /// <summary>
        /// Returns the current volume of that audio type 0 - 1
        /// </summary>
        public static float GetVolume(AudioType audioType) {
            return SoundManager.Instance.GetVolume(audioType.audioTypeName);
        }


        /// <summary>
        /// Sets the audio type to mute
        /// </summary>
        public static void SetMute(AudioType audioType, bool muteValue) {
            SoundManager.Instance.SetMute(audioType.audioTypeName, muteValue);
        }


        /// <summary>
        /// Toggles the mute of that audio type
        /// </summary>
        public static void ToggleMute(AudioType audioType) {
            SoundManager.Instance.ToggleMute(audioType.audioTypeName);
        }


        /// <summary>
        /// Retusn if that audio type is muted or not
        /// </summary>
        public static bool IsMuted(AudioType audioType) {
            return SoundManager.Instance.IsMuted(audioType.audioTypeName);
        }

        #endregion



        #region Audio Mask

        /// <summary>
        /// Sets volume for audio types in mask | volume value should be between 0 & 1
        /// </summary>
        /// <param name="volume">Value sbetween 0 & 1</param>
        public static void SetVolume(AudioTypeMask audioTypeMask, float volume) {
            SoundManager.Instance.SetVolume(audioTypeMask, volume);
        }


        /// <summary>
        /// Sets mute to audio types in mask 
        /// </summary>
        public static void SetMute(AudioTypeMask audioTypeMask, bool muteValue) {
            SoundManager.Instance.SetMute(audioTypeMask, muteValue);
        }


        /// <summary>
        /// Toggles the mute state for the specified audio types.
        /// </summary>
        /// <param name="audioTypeMask">AudioTypeMask specifying which audio types to toggle mute for.</param>
        public static void ToggleMute(AudioTypeMask audioTypeMask) {
            SoundManager.Instance.ToggleMute(audioTypeMask);
        }

        #endregion

    }

}