using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Unknown.Manager.AudioManager {

    // Category Settings
    public partial class SoundManager : MonoBehaviour {

        private struct AudioTypeValues {
            public string volumeAudioString;        // string for audio mixer
            public bool isMuted;                    // true & false
            public float previousValue;             // value from 0 - 1
            public AudioMixerGroup mixerGroup;
        }

        private Dictionary<string, AudioTypeValues> audioTypesVolumeStringLookup = new();
        private List<AudioType> allValidAudioTypes = new();

        private const float MinVolumeDB = -80f;

        private void InitializeSoundManagerCategorySettings() {
            AudioCategory audioCategory = Resources.Load(nameof(AudioCategory)) as AudioCategory;

            if(audioCategory == null) {
                Debug.LogError("AudioCategory not found in Resources.");
                return;
            }

            for(int i = 0; i < audioCategory.categories.Length; i++) {
                string audioType = audioCategory.categories[i];

                if(string.IsNullOrEmpty(audioType))
                    continue;

                if(audioTypesVolumeStringLookup.ContainsKey(audioType)) {
                    Debug.LogWarning("Duplicate audio type : " + audioType);
                    continue;
                }

                AudioTypeValues audioTypeValues = new AudioTypeValues {
                    volumeAudioString = UnknownNames.Type + i + "Vol",
                    isMuted = false,
                    previousValue = 1f
                };

                AudioMixerGroup[] groups = audioMixer.FindMatchingGroups(UnknownNames.Type + i.ToString());

                if(groups.Length > 0) {
                    audioTypeValues.mixerGroup = groups[0];
                }

                allValidAudioTypes.Add(AudioTypeMask.NameToAudioType(audioType));
                audioTypesVolumeStringLookup[audioType] = audioTypeValues;
            }
        }

        #region Audio Type

        #region Public Methods for string input

        /// <summary>
        /// Sets the volume of an audio type (0-1).
        /// </summary>
        public void SetVolume(string audioType, float volume) {
            if(!TryGetAudioTypeValues(audioType, out AudioTypeValues values))
                return;

            volume = Mathf.Clamp01(volume);

            values.previousValue = volume;

            audioTypesVolumeStringLookup[audioType] = values;

            float volumeDB = volume <= 0f ? MinVolumeDB : Mathf.Log10(volume) * 20f;

            // applying volume only if not muted.
            if(!values.isMuted)
                audioMixer.SetFloat(audioType, volumeDB);
        }

        /// <summary>
        /// Gets the volume of an audio type (0-1).
        /// </summary>
        public float GetVolume(string audioType) {
            if(!TryGetAudioTypeValues(audioType, out AudioTypeValues values))
                return 0f;

            return values.previousValue;
        }

        /// <summary>
        /// Sets the mute state of an audio type.
        /// </summary>
        public void SetMute(string audioType, bool isMuted) {
            if(!TryGetAudioTypeValues(audioType, out AudioTypeValues values))
                return;

            if(values.isMuted == isMuted)
                return;

            values.isMuted = isMuted;

            audioTypesVolumeStringLookup[audioType] = values;

            if(isMuted) {
                audioMixer.SetFloat(audioType, MinVolumeDB);
            } else {
                float volumeDB = values.previousValue <= 0f ? MinVolumeDB : Mathf.Log10(values.previousValue) * 20f;

                audioMixer.SetFloat(audioType, volumeDB);
            }
        }

        /// <summary>
        /// Toggles the mute state of an audio type.
        /// </summary>
        public void ToggleMute(string audioType) {
            if(!TryGetAudioTypeValues(audioType, out AudioTypeValues values))
                return;

            SetMute(audioType, !values.isMuted);
        }

        /// <summary>
        /// Checks whether an audio type is muted.
        /// </summary>
        public bool IsMuted(string audioType) {
            if(!TryGetAudioTypeValues(audioType, out AudioTypeValues values))
                return false;

            return values.isMuted;
        }

        #endregion

        #region Private Methods

        private bool TryGetAudioTypeValues(string audioType, out AudioTypeValues values) {

            if(string.IsNullOrEmpty(audioType)) {
                values = default;
                Debug.LogWarning("Audio type cannot be null or empty.");
                return false;
            }

            if(!audioTypesVolumeStringLookup.TryGetValue(audioType, out values)) {
                Debug.LogWarning("Audio type not found: " + audioType);
                return false;
            }

            return true;
        }

        #endregion

        #endregion


        #region Audio Mask

        #region Public Methods 

        /// <summary>
        /// Sets the volume of all audio types in mask
        /// </summary>
        public void SetVolume(AudioTypeMask audioTypeMask, float volume) {
            ForAllAudioTypesInMask(audioTypeMask, (audioType) => {
                SetVolume(audioType.audioTypeName, volume);
            });
        }

        /// <summary>
        /// Set mute to every audio type in mask
        /// </summary>
        public void SetMute(AudioTypeMask audioTypeMask, bool isMuted) {
            ForAllAudioTypesInMask(audioTypeMask, (audioType) => {
                SetMute(audioType.audioTypeName, isMuted);
            });
        }

        /// <summary>
        /// Toggles mute of every audio type in mask
        /// </summary>
        public void ToggleMute(AudioTypeMask audioTypeMask) {
            ForAllAudioTypesInMask(audioTypeMask, (audioType) => {
                ToggleMute(audioType.audioTypeName);
            });
        }

        #endregion

        #region Private Methods 

        private void ForAllAudioTypesInMask(AudioTypeMask audioTypeMask, Action<AudioType> method) {
            foreach(AudioType audioType in audioTypeMask.GetAudioTypes(allValidAudioTypes)) {
                method?.Invoke(audioType);
            }
        }

        #endregion

        #endregion

        #region Helper Methods

        private AudioMixerGroup GetMixer(string audioType) {
            if(!audioTypesVolumeStringLookup.ContainsKey(audioType)) 
                return null;

            return audioTypesVolumeStringLookup[audioType].mixerGroup;
        }

        #endregion

    }
}