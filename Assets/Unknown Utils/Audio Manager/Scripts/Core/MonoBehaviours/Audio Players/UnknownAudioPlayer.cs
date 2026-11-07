using System;
using System.Collections;
using UnityEngine;

namespace Unknown.Manager.AudioManager {

    [RequireComponent(typeof(AudioSource))]
    public class UnknownAudioPlayer : AudioPlayer {

        [SerializeField] private AudioSource audioSource;

        // private variables
        private UnknownAudioConfig audioConfig;

        // private values
        private float externalDelay;

        #region unity methods 

        private void Awake() {
            if(audioSource == null)
                audioSource = GetComponent<AudioSource>();
        }

        #endregion

        #region abstract methods 

        public override UnknownAudioConfig GetAudioConfig() {
            return audioConfig;
        }

        public override AudioSource GetAudioSource() {
            return audioSource;
        }

        public override void Stop() {
            if(audioSource.isPlaying)
                audioSource.Stop();
        }

        public override AudioType GetAudioType() {
            if(audioConfig == null)
                return AudioType.None;

            return audioConfig.GetAudioType();
        }

        #endregion

        #region private methods 

        private void Reset() {
            externalDelay = 0;
            audioConfig = null;
            ResetEvents();
        }

        #endregion

        #region builder methods

        public override AudioPlayer SetAudioConfig(UnknownAudioConfig audioConfig) {
            Reset();
            this.audioConfig = audioConfig;
            return this;
        }

        public override AudioPlayer Play() {
            if(audioConfig == null)
                return this;

            UnknownAudioClip uAudioClip = audioConfig.PickNewClip();
            AudioClip clip = uAudioClip.audioClip;

            float finalVolume = uAudioClip.volume * audioConfig.GetMasterVolume();

            audioSource.volume = finalVolume;
            audioSource.pitch = audioConfig.GetPitch();

            StartCoroutine(PlayRange(audioSource, clip, uAudioClip));

            return this;
        }

        public override AudioPlayer OnAudioEnded(Action callbackMethod) {
            onAudioEnded += callbackMethod;
            return this;
        }

        public override AudioPlayer SetDelay(float delay) {
            externalDelay = delay;
            return this;
        }

        #endregion

        #region coroutines 

        private IEnumerator PlayRange(AudioSource source, AudioClip clip, UnknownAudioClip uAudioClip) {
            if(externalDelay != 0)
                yield return CommonUtils.GetWaitForSeconds(externalDelay);

            if(uAudioClip.delay != 0)
                yield return CommonUtils.GetWaitForSeconds(uAudioClip.delay);

            source.clip = clip;

            int startSample = Mathf.RoundToInt(clip.samples * uAudioClip.startPosition);
            int endSample = Mathf.RoundToInt(clip.samples * uAudioClip.endPosition);

            source.timeSamples = startSample;
            source.Play();

            while(source.isPlaying && source.timeSamples < endSample) {
                yield return null;
            }

            source.Stop();
            RaiseOnAudioEnded();
        }

        #endregion

    }
}