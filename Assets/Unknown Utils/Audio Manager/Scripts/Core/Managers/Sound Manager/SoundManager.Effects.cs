using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Unknown.Manager.AudioManager {

    public partial class SoundManager : MonoBehaviour {

        // constants 
        private const float DEFAULT_LOWPASS = 22000f;
        private const float DEFAULT_HIGHPASS = 10f;
        private const float DEFAULT_VOLUME = 0f;


        // audio mixer paramters strings
        private const string HIGH_PASS_EFFCT = "Highpass";
        private const string LOW_PASS_EFFCT = "Lowpass";
        private const string EFFECTS_VOL = "EffectVol";

        // private variables
        private Queue<AudioMixerGroup> dominatorsQueue;
        private EffectPlayer effectPlayer;
        private const int dominatorsCount = 4;

        private void InitializeSoundManagerEffects() {

            dominatorsQueue = new Queue<AudioMixerGroup>(dominatorsCount);

            for(int i = 1; i <= dominatorsCount; i++) {
                AudioMixerGroup[] groups = audioMixer.FindMatchingGroups($"{UnknownNames.Dominator}{i}");

                if(groups.Length == 0)
                    continue;

                dominatorsQueue.Enqueue(groups[0]);
            }

            effectPlayer = new EffectPlayer(this);
        }

#if !UNITY_WEBGL
        #region Public Methods

        /// <summary>
        /// Plays audio player as dominator
        /// </summary>
        public IEffectPlayer AsDominator(AudioPlayer audioPlayer) {
            if(dominatorsQueue.Count <= 0)
                return effectPlayer;

            AudioMixerGroup dominatorGrp = dominatorsQueue.Dequeue();

            audioPlayer.GetAudioSource().outputAudioMixerGroup = dominatorGrp;

            audioPlayer.onAudioEnded += () => {
                ResetEffects();
                dominatorsQueue.Enqueue(dominatorGrp);
            };

            return effectPlayer;
        }

        /// <summary>
        /// Low pass effect
        /// </summary>
        public void LowPassEffect(float lowPassValue) {
            audioMixer.SetFloat(LOW_PASS_EFFCT, lowPassValue);
        }

        /// <summary>
        /// Highpass effect
        /// </summary>
        public void HighPassEffect(float highPassValue) {
            audioMixer.SetFloat(HIGH_PASS_EFFCT, highPassValue);
        }

        /// <summary>
        /// Takes value from 0 - 1
        /// </summary>
        public void SetEffectsVolume(float volume) {
            float volumeDB = volume <= 0f ? MinVolumeDB : Mathf.Log10(volume) * 20f;

            audioMixer.SetFloat(EFFECTS_VOL, volumeDB);
        }

        /// <summary>
        /// Resets the effects applied in effects
        /// </summary>
        public void ResetEffects() {
            audioMixer.SetFloat(HIGH_PASS_EFFCT, DEFAULT_HIGHPASS);
            audioMixer.SetFloat(LOW_PASS_EFFCT, DEFAULT_LOWPASS);
            audioMixer.SetFloat(EFFECTS_VOL, DEFAULT_VOLUME);
        }

        #endregion
#endif


    }
}