using System;
using System.Collections;
using UnityEngine;

namespace Unknown.Manager.AudioManager {

    [RequireComponent(typeof(AudioSource))]
    public class UnknownAudioPlayer : AudioPlayer {

        [SerializeField] private AudioSource audioSource;

        // private variables
        private UnknownAudioConfig audioConfig;
        private bool isPaused;

        // private values
        private float externalDelay;

        #region unity methods 

        private void Awake() {
            if(audioSource == null)
                audioSource = GetComponent<AudioSource>();
        }

        #endregion

        #region abstract methods 

        /// <inheritdoc/>
        public override UnknownAudioConfig GetAudioConfig() {
            return audioConfig;
        }

        /// <inheritdoc/>
        public override AudioSource GetAudioSource() {
            return audioSource;
        }

        /// <inheritdoc/>
        public override void Stop() {
            if(!audioSource.isPlaying)
                return;

            RaiseOnAudioEnded();
            Reset();
            audioSource.Stop();
        }

        /// <inheritdoc/>
        public override AudioType GetAudioType() {
            if(audioConfig == null)
                return AudioType.None;

            return audioConfig.GetAudioType();
        }

        /// <inheritdoc/>
        public override bool IsPlaying() {
            return audioSource.isPlaying;
        }

        /// <inheritdoc/>
        public override void Pause() {
            if(!audioSource.isPlaying)
                return;

            isPaused = true;
            audioSource.Pause();
        }

        /// <inheritdoc/>
        public override void Resume() {
            audioSource.Play();
            isPaused = false;
        }

        public override bool IsPaused() {
            return isPaused;
        }

        #endregion

        #region private methods 

        private void Reset() {
            externalDelay = 0;
            audioConfig = null;
            isPaused = false;
            ResetEvents();
        }

        #endregion

        #region builder methods

        /// <inheritdoc/>
        public override AudioPlayer SetAudioConfig(UnknownAudioConfig audioConfig) {
            Reset();
            this.audioConfig = audioConfig;
            return this;
        }

        /// <inheritdoc/>
        public override AudioPlayer Play() {
            if(audioConfig == null)
                return this;

            StopAllCoroutines();

            if(audioSource.isPlaying) {
                audioSource.Stop();
            }

            UnknownAudioClip uAudioClip = audioConfig.PickNewClip();
            AudioClip clip = uAudioClip.audioClip;

            float finalVolume = uAudioClip.volume * audioConfig.GetMasterVolume();

            audioSource.volume = finalVolume;
            audioSource.pitch = audioConfig.GetPitch();

            StartCoroutine(PlayRange(audioSource, clip, uAudioClip));

            return this;
        }

        /// <inheritdoc/>
        public override AudioPlayer OnAudioEnded(Action callbackMethod) {
            onAudioEnded += callbackMethod;
            return this;
        }

        /// <inheritdoc/>
        public override AudioPlayer SetDelay(float delay) {
            externalDelay = delay;
            return this;
        }

        #endregion

        #region coroutines 

        private IEnumerator PlayRange(AudioSource source, AudioClip clip, UnknownAudioClip uAudioClip) {
            yield return null;

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