using UnityEngine;

namespace Unknown.Manager.AudioManager {

    // master settings
    public partial class SoundManager : MonoBehaviour {

        // strings
        private string MASTER_VOLUME;

        // private variables
        private bool isMasterVolumeMuted;
        private float masterPreviousVolume = 1f;

        private void InitializeSoundManagerMasterSettings() {
            MASTER_VOLUME = UnknownNames.Master + "Vol";
        }

        #region Public Methods

        /// <summary>
        /// Sets the volume for master
        /// </summary>
        /// <param name="volume">Between 0 - 1</param>
        public void SetMasterVolume(float volume) {
            volume = Mathf.Clamp01(volume);

            float volumeDB = volume <= 0.0001f ? -80f : Mathf.Log10(volume) * 20f;

            audioMixer.SetFloat(MASTER_VOLUME, volumeDB);
            masterPreviousVolume = volume;
        }

        /// <summary>
        /// Returns the current mater volume
        /// </summary>
        public float GetMasterVolume() {
            audioMixer.GetFloat(MASTER_VOLUME, out float volumeDB);

            return Mathf.Pow(10f, volumeDB / 20f);
        }

        public void SetMasterMute(bool mute) {
            if(isMasterVolumeMuted == mute)
                return;

            if(mute) {
                masterPreviousVolume = GetMasterVolume();
                audioMixer.SetFloat(MASTER_VOLUME, -80f);
            } else {
                SetMasterVolume(masterPreviousVolume);
            }

            isMasterVolumeMuted = mute;
        }

        public void ToggleMuteMaster() {
            SetMasterMute(!isMasterVolumeMuted);
        }

        public bool IsMasterMuted() {
            return isMasterVolumeMuted;
        }

        #endregion

    }
}