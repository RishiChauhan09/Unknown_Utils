using UnityEngine;

namespace Unknown.Manager.AudioManager {
    public interface IEffectPlayer {

#if !UNITY_WEBGL

        /// <summary>
        /// Sets the lowpass value for all other audio types other than dominators
        /// </summary>
        public IEffectPlayer LowPassOthers(float lowpassValue);

        /// <summary>
        /// Sets the highpass value for all other audio types other than dominators
        /// </summary>
        public IEffectPlayer HighpassOthers(float highPassValue);

        /// <summary>
        /// Sets silent to other audio types other than dominators
        /// </summary>
        public IEffectPlayer SilentOthers();

        /// <summary>
        /// Sets the value of volume to other audio types leaving dominators
        /// </summary>
        /// <param name="volume">Volume value between 0 - 1</param>
        public IEffectPlayer VolumeOthers(float volume);


        /// <summary>
        /// Resets the effects
        /// </summary>
        public void ResetEffects();

#endif

    }
}