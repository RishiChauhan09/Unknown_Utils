using UnityEngine;

namespace Unknown.Manager.AudioManager {

    public static partial class AudioManager {

        #region Play

        public static AudioPlayer Play(AudioID audioID, float delay = 0) {
            return SoundManager.Instance.Play(audioID, delay);
        }

        public static AudioPlayer Play(AudioID audioID, Vector3 position, float delay = 0) {
            return SoundManager.Instance.Play(audioID, position, delay);
        }

        public static AudioPlayer Play(AudioClip audioClip, AudioParameter audioparameter = null) {
            return SoundManager.Instance.Play(audioClip, audioparameter);
        }

        #endregion

        #region Stop

        public static void Stop(AudioID audioID) {
            SoundManager.Instance.Stop(audioID);
        }

        public static void Stop(AudioType audioType) {
            SoundManager.Instance.Stop(audioType);
        }

        public static void StopAll() {
            SoundManager.Instance.StopAll();
        }

        #endregion

        #region Pause 

        public static void Pause(AudioID audioID) {
            SoundManager.Instance.Pause(audioID);
        }

        public static void Pause(AudioType audioType) {
            SoundManager.Instance.Pause(audioType);
        }

        public static void PauseAll() {
            SoundManager.Instance.PauseAll();
        }

        #endregion

        #region Resume 

        public static void Resume(AudioID audioID) {
            SoundManager.Instance.Resume(audioID);
        }

        public static void Resume(AudioType audioType) {
            SoundManager.Instance.Resume(audioType);
        }

        public static void ResumeAll() {
            SoundManager.Instance.ResumeAll();
        }

        #endregion

    }
}