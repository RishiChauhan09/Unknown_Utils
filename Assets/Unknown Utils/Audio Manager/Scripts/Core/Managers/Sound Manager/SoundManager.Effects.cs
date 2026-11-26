using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Unknown.Manager.AudioManager {

    public partial class SoundManager : MonoBehaviour {

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
                AudioMixerGroup[] groups = audioMixer.FindMatchingGroups($"{UnknownNames.DominatorAudioMixer}{i}");

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
        /// Note : this is used by audio player as extension method
        public IEffectPlayer AsDominator(AudioPlayer audioPlayer) {
            if(dominatorsQueue.Count <= 0)
                return effectPlayer;

            AudioMixerGroup dominatorGrp = dominatorsQueue.Dequeue();

            audioPlayer.GetAudioSource().outputAudioMixerGroup = dominatorGrp;

            audioPlayer.onAudioEnded += () => {
                ResetEffects();
                audioPlayer.GetAudioSource().outputAudioMixerGroup = null;
                dominatorsQueue.Enqueue(dominatorGrp);
            };

            return effectPlayer;
        }

        /// <summary>
        /// Low pass effect
        /// </summary>
        /// Note : This us used by effect player object
        public void LowPassEffects(float lowPassValue) {
            audioMixer.SetFloat(UnknownNames.MainAudioMixerVolume, -80f);
            audioMixer.SetFloat(LOW_PASS_EFFCT, lowPassValue);
        }

        /// <summary>
        /// Highpass effect
        /// </summary>
        /// Note : This us used by effect player object
        public void HighpassEffects(float highPassValue) {
            audioMixer.SetFloat(UnknownNames.MainAudioMixerVolume, -80f);
            audioMixer.SetFloat(HIGH_PASS_EFFCT, highPassValue);
        }

        /// <summary>
        /// Takes value from 0 - 1
        /// </summary>
        /// Note : This us used by effect player object
        public void SetEffectsVolume(float volume) {
            float volumeDB = volume <= 0f ? MinVolumeDB : Mathf.Log10(volume) * 20f;

            audioMixer.SetFloat(UnknownNames.MainAudioMixerVolume, -80f);
            audioMixer.SetFloat(EFFECTS_VOL, volumeDB);
        }

        /// <summary>
        /// Resets the effects applied in effects
        /// </summary>
        /// Automatically reset after the audio is compelted which was playing as dominator
        public void ResetEffects() {
            audioMixer.SetFloat(HIGH_PASS_EFFCT, UnknownConstants.DefaultHighpass);
            audioMixer.SetFloat(LOW_PASS_EFFCT, UnknownConstants.DefaultLowpass);
            audioMixer.SetFloat(EFFECTS_VOL, UnknownConstants.FullDecibelVolume);
            audioMixer.SetFloat(UnknownNames.MainAudioMixerVolume, 0f);
        }

        #endregion
#endif


    }
}