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
        /// stops the audio being played
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// returns the audio type being played
        /// </summary>
        /// <returns></returns>
        public abstract AudioType GetAudioType();

        #region builder methods 

        /// <summary>
        /// sets the audio config 
        /// </summary>
        public abstract AudioPlayer SetAudioConfig(UnknownAudioConfig audioConfig);

        /// <summary>
        /// plays the audio 
        /// </summary>
        public abstract AudioPlayer Play();

        /// <summary>
        /// subs to on audio ended event
        /// </summary>
        public abstract AudioPlayer OnAudioEnded(Action callbackMethod);

        /// <summary>
        /// sets delay before playing audio
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