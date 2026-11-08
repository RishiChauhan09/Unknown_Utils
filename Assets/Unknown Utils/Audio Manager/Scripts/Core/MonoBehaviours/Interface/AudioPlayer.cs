using System;
using UnityEngine;

namespace Unknown.Manager.AudioManager {

    public abstract class AudioPlayer : MonoBehaviour {

        #region abstract methods 

        /// <summary>
        /// Returns the audio source on which it's playing
        /// </summary>
        public abstract AudioSource GetAudioSource();

        /// <summary>
        /// Returns the Audio Config
        /// </summary>
        public abstract UnknownAudioConfig GetAudioConfig();

        /// <summary>
        /// Stops the audio being played
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Returns the audio type being played
        /// </summary>
        /// <returns></returns>
        public abstract AudioType GetAudioType();

        /// <summary>
        /// Returns if audio player is playing audio or not
        /// </summary>
        public abstract bool IsPlaying();

        /// <summary>
        /// Returns if audio player is paused or not
        /// </summary>
        public abstract bool IsPaused();

        /// <summary>
        /// pauses the audio being played
        /// </summary>
        public abstract void Pause();

        /// <summary>
        /// Resumes the audio being played
        /// </summary>
        public abstract void Resume();

        #region builder methods 

        /// <summary>
        /// Sets the audio config 
        /// </summary>
        public abstract AudioPlayer SetAudioConfig(UnknownAudioConfig audioConfig);

        /// <summary>
        /// Plays the audio from the curretn audio config
        /// </summary>
        public abstract AudioPlayer Play();

        /// <summary>
        /// Subs to on audio ended event
        /// </summary>
        public abstract AudioPlayer OnAudioEnded(Action callbackMethod);

        /// <summary>
        /// Sets delay before playing audio
        /// </summary>
        public abstract AudioPlayer SetDelay(float delay);

        #endregion

        #endregion


        #region events 

        /// <summary>
        /// Raised when playing audio is ended.
        /// </summary>
        public event Action onAudioEnded;

        #endregion


        #region protected methods

        protected void RaiseOnAudioEnded() {
            onAudioEnded?.Invoke();
        }

        protected void ResetEvents() {
            onAudioEnded = null;
        }

        #endregion

    }
}