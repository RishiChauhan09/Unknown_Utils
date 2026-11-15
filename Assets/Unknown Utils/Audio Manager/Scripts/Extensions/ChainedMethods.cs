using UnityEngine;

namespace Unknown.Manager.AudioManager {

    public static class ChainedMethods {

#if !UNITY_WEBGL
        #region Effects

        /// <summary>
        /// Plays audio player as Dominator 
        /// </summary>
        public static IEffectPlayer AsDominator(this AudioPlayer audioPlayer) {
            return SoundManager.Instance.AsDominator(audioPlayer);
        }

        #endregion
#endif

    }
}