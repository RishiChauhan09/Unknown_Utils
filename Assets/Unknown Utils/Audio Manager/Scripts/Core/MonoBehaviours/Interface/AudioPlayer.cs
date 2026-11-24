
using System;
using UnityEngine;

namespace Unknown.Manager.AudioManager {

    public abstract class AudioPlayer : MonoBehaviour {

        public bool IsLooped { get; protected set; }

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

        /// <summary>
        /// Sets up audio 3d settings in audio source
        /// </summary>
        public abstract void SetupAudio3DSettings(Audio3DSettings audioSettings);

        /// <summary>
        /// Restart the audio currently playing
        /// </summary>
        public abstract void Restart();

        #region builder methods 

        /// <summary>
        /// Sets the audio config 
        /// </summary>
        public abstract AudioPlayer SetAudioConfig(UnknownAudioConfig audioConfig);

        /// <summary>
        /// Plays the audio from the current audio config
        /// </summary>
        public abstract AudioPlayer Play();

        /// <summary>
        /// Sets delay before playing audio
        /// </summary>
        public abstract AudioPlayer SetDelay(float delay);

        /// <summary>
        /// Plays audio just using audio cilp
        /// </summary>
        public abstract AudioPlayer Play(AudioClip audioClip, float startPos = 0f, float endPos = 1f);

        /// <summary>
        /// To attack to other object
        /// </summary>
        public abstract AudioPlayer AttachTo(Transform parentObject);

        /// <summary>
        /// To make this audio player follow a follow Object | 
        /// Note if the followObject will not be destroyed make sure you use AttackTo()
        /// </summary>
        public abstract AudioPlayer Follow(Transform followObject);


        /// <summary>
        /// Sets default parent after audio ends it returns to this parent
        /// </summary>
        public abstract AudioPlayer SetDefaultParent(Transform parent);

        #endregion

        #region event methods

        /// <summary>
        /// Subs to on audio ended event
        /// </summary>
        public abstract AudioPlayer OnAudioEnded(Action callbackMethod);

        /// <summary>
        /// Subs to on loop step event
        /// </summary>
        public abstract AudioPlayer OnLoopStep(Action callbackMethod);

        #endregion

        #endregion


        #region events 

        /// <summary>
        /// Raised when playing audio is ended.
        /// </summary>
        public event Action onAudioEnded;

        /// <summary>
        /// Raised when audio is looped and single audio is ended
        /// </summary>
        public event Action onLoopStep;

        #endregion


        #region protected methods

        protected void ResetEvents() {
            onAudioEnded = null;
            onLoopStep = null;
        }

        protected void RaiseOnAudioEnded() {
            onAudioEnded?.Invoke();
        }

        protected void RaiseOnLoopStep() {
            onLoopStep?.Invoke();
        }

        #endregion

    }
}