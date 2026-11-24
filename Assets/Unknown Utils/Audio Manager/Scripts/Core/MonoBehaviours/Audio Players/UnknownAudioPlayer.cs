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
        private Transform followObject;
        private Transform t;
        private Transform defaultParent;

        // private values
        private float delay;

        #region unity methods 

        private void Awake() {
            t = transform;

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
                return null;

            return audioConfig.GetAudioType();
        }

        /// <inheritdoc/>
        public override void Restart() {
            Play();
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

        public override void SetupAudio3DSettings(Audio3DSettings audioSettings) {
            if(audioSettings == null) {
                DefaultAudio3DSettings();
                return;
            }

            audioSource.spatialBlend = audioSettings.spartialBlend;
            audioSource.reverbZoneMix = audioSettings.reverbZoneMix;

            audioSource.dopplerLevel = audioSettings.dopperLevel;
            audioSource.spread = audioSettings.spread;
            audioSource.rolloffMode = audioSettings.volumeRolloff;
            audioSource.minDistance = audioSettings.minDistance;
            audioSource.maxDistance = audioSettings.maxDistance;
        }

        private void DefaultAudio3DSettings() {
            audioSource.spatialBlend = UnknownConstants.DefaultSpartialBlend;
            audioSource.reverbZoneMix = UnknownConstants.DefaultReverbZoneMix;

            audioSource.dopplerLevel = UnknownConstants.DefaultDopperLevel;
            audioSource.spread = UnknownConstants.DefaultSpread;
            audioSource.rolloffMode = UnknownConstants.DefaultAudioRolloffMode;
            audioSource.minDistance = UnknownConstants.DefaultMinDistance;
            audioSource.maxDistance = UnknownConstants.DefaultMaxDistance;
        }

        #endregion

        #region private methods 

        private void Reset() {
            delay = 0;
            audioConfig = null;
            isPaused = false;
            DefaultAudio3DSettings();
            ResetEvents();
            t.parent = defaultParent;
            followObject = null;
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

            StartCoroutine(PlayRange(audioSource, clip, uAudioClip.startPosition, uAudioClip.endPosition));

            return this;
        }

        /// <inheritdoc/>
        public override AudioPlayer Play(AudioClip audioClip, float startPos = 0, float endPos = 1) {
            if(audioClip == null)
                return this;

            StopAllCoroutines();

            if(audioSource.isPlaying)
                audioSource.Stop();

            StartCoroutine(PlayRange(audioSource, audioClip, startPos, endPos));

            return this;
        }

        /// <inheritdoc/>
        public override AudioPlayer SetDelay(float delay) {
            this.delay = delay;
            return this;
        }

        /// <inheritdoc/>
        public override AudioPlayer AttachTo(Transform parentObject) {
            t.parent = parentObject;
            return this;
        }

        /// <inheritdoc/>
        public override AudioPlayer Follow(Transform followObject) {
            this.followObject = followObject;
            return this;
        }

        public override AudioPlayer SetDefaultParent(Transform parent) {
            defaultParent = parent;
            return this;
        }

        #endregion

        #region event methods 

        /// <inheritdoc/>
        public override AudioPlayer OnAudioEnded(Action callbackMethod) {
            onAudioEnded += callbackMethod;
            return this;
        }

        /// <inheritdoc/>
        public override AudioPlayer OnLoopStep(Action callbackMethod) {
            onLoopStep += callbackMethod;
            return this;
        }

        #endregion

        #region coroutines 

        private IEnumerator PlayRange(AudioSource source, AudioClip clip, float startPosNormalized, float endPosNormalized) {
            yield return null;

            if(delay != 0)
                yield return CommonUtils.GetWaitForSeconds(delay);

            source.clip = clip;

            int startSample = Mathf.RoundToInt(clip.samples * startPosNormalized);
            int endSample = Mathf.RoundToInt(clip.samples * endPosNormalized);

            source.timeSamples = startSample;
            source.Play();

            while(source.isPlaying && source.timeSamples < endSample) {
                if(followObject != null)
                    t.position = followObject.position;

                yield return null;
            }

            source.Stop();

            if(audioConfig != null && audioConfig.IsLooped()) {
                RaiseOnLoopStep();
            } else {
                RaiseOnAudioEnded();
            }

            Reset();
        }

        #endregion

    }
}