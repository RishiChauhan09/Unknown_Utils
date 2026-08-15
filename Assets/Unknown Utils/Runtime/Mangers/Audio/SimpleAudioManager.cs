// version : v0.5
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unknown.Audio {

    public class SimpleAudioManager : MonoBehaviour {

        [SerializeField] private Sound[] sfx, music;
        [SerializeField] public AudioSource sfxAudioSource, musicAudioSource;

        private Dictionary<string, Sound> sfxLookup, musicLookup;            // making dictionary so that there is no need for running foreach everytime for sound

        #region Unity Methods

        private void Awake() {
            sfxLookup = new Dictionary<string, Sound>();
            musicLookup = new Dictionary<string, Sound>();

            foreach(Sound s in sfx) {
                if(sfxLookup.ContainsKey(s.id)) {
                    Debug.Log("Duplicated sfx name : " + s.id);
                    continue;
                }

                sfxLookup[s.id] = s;
            }

            foreach(Sound s in music) {
                if(musicLookup.ContainsKey(s.id)) {
                    Debug.Log("Duplicate music name : " + s.id);
                    return;
                }

                musicLookup[s.id] = s;
            }
        }

        #endregion

        #region Public Methods

        #region SFX

        /// <summary>
        /// Plays sfx using sfx audio source
        /// </summary>
        public void PlaySFX(string name, float delay = 0) {
            if(!TryGetSound(name, out Sound sound, out AudioClip clip)) {
                return;
            }

            sfxAudioSource.volume = sound.GetVolume();
            sfxAudioSource.clip = clip;

            if(delay > 0) {
                StartCoroutine(WaitAndRun(delay, () => {
                    sfxAudioSource.Play();
                }));
            } else {
                sfxAudioSource.Play();
            }

        }

        /// <summary>
        /// Plays one shot from the string name provided
        /// </summary>
        public void PlayOneShot(string name, float delay = 0) {
            if(!TryGetSound(name, out Sound sound, out AudioClip clip)) {
                return;
            }

            if(delay > 0) {
                StartCoroutine(WaitAndRun(delay, () => {
                    sfxAudioSource.PlayOneShot(clip, sound.GetVolume());
                }));
            } else {
                sfxAudioSource.PlayOneShot(clip, sound.GetVolume());
            }

        }

        /// <summary>
        /// Plays one shot from the clip provided
        /// </summary>
        public void PlayOneShot(AudioClip clip, float delay = 0, AudioClipParameters audioClipParameters = null) {
            if(audioClipParameters == null) {
                audioClipParameters = new AudioClipParameters();
            }

            if(delay > 0) {
                StartCoroutine(WaitAndRun(delay, () => {
                    sfxAudioSource.pitch = audioClipParameters.pitch;
                    sfxAudioSource.PlayOneShot(clip, audioClipParameters.volume);
                }));
            } else {
                sfxAudioSource.pitch = audioClipParameters.pitch;
                sfxAudioSource.PlayOneShot(clip, audioClipParameters.volume);
            }
        }

        /// <summary>
        /// Pause the sfx currently being played exluding one shot
        /// </summary>
        public void PauseSFX() {
            sfxAudioSource.Pause();
        }

        /// <summary>
        /// resumes the sfx of sfx audio source
        /// </summary>
        public void ResumeSFX() {
            sfxAudioSource.UnPause();
        }

        /// <summary>
        /// stop the sfx of sfx audio source
        /// </summary>
        public void StopSFX() {
            sfxAudioSource.Stop();
        }

        /// <summary>
        /// sets the sfx audio source to mute
        /// </summary>
        public void SetSFXMute(bool value) {
            sfxAudioSource.mute = value;
        }

        /// <summary>
        /// toggle the sfx audio source of mute
        /// </summary>
        public void ToggleSFX() {
            sfxAudioSource.mute = !sfxAudioSource.mute;
        }

        /// <summary>
        /// Changes the pitch of sfx audio source
        /// </summary>
        public void ChangePitchSFX(int pitch) {
            sfxAudioSource.pitch = pitch;
        }

        #endregion

        #region Music

        /// <summary>
        /// Plays music using music audio source provided
        /// </summary>
        public void PlayMusic(string name) {
            Sound sound = null;
            if(!musicLookup.TryGetValue(name, out sound))
                return;

            AudioClip clip = sound.GetClip();

            if(clip == null)
                return;

            musicAudioSource.clip = clip;
            musicAudioSource.Play();
        }

        /// <summary>
        /// pause the music audio source
        /// </summary>
        public void PauseMusic() {
            musicAudioSource.Pause();
        }

        /// <summary>
        /// resumes the music audio source
        /// </summary>
        public void ResumeMusic() {
            musicAudioSource.UnPause();
        }

        /// <summary>
        /// stops the music audio source
        /// </summary>
        public void StopMusic() {
            musicAudioSource.Stop();
        }

        /// <summary>
        /// toggle the music audio source
        /// </summary>
        public void ToggleMusic() {
            musicAudioSource.mute = !musicAudioSource.mute;
        }

        /// <summary>
        /// set the music audio source mute value
        /// </summary>
        public void SetMusicMute(bool value) {
            musicAudioSource.mute = value;
        }

        /// <summary>
        /// Changes the pitch of music audio source
        /// </summary>
        public void ChangePitchMusic(int pitch) {
            musicAudioSource.pitch = pitch;
        }

        #endregion

        #endregion

        #region Private Methods

        private bool TryGetSound(string name, out Sound sound, out AudioClip clip) {
            sound = null;
            clip = null;

            if(!sfxLookup.TryGetValue(name, out sound))
                return false;

            clip = sound.GetClip();

            if(clip == null)
                return false;

            sfxAudioSource.pitch = sound.GetPitch();

            return true;
        }

        #endregion

        #region Utils 

        private IEnumerator WaitAndRun(float time, Action method) {
            yield return new WaitForSeconds(time);
            method?.Invoke();
        }

        #endregion
    }

    public class AudioClipParameters {
        public float volume = 1;
        public float pitch = 1;
    }
}