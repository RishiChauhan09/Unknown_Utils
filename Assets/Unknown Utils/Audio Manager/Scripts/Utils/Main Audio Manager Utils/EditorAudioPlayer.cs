
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class EditorAudioPlayer {

    private AudioSource audioSource;
    private Action onComplete;
    private Action onStop;

    public AudioSource AudioSource => audioSource;

    public EditorAudioPlayer Create() {
        if(audioSource != null)
            return this;

        GameObject playerObject = new GameObject("[Audio Player Editor]") {
            hideFlags = HideFlags.HideAndDontSave
        };

        audioSource = playerObject.AddComponent<AudioSource>();

        return this;
    }

    // General Settings

    public EditorAudioPlayer SetClip(AudioClip value) {
        audioSource.clip = value;
        return this;
    }

    public EditorAudioPlayer SetOutput(AudioMixerGroup value) {
        audioSource.outputAudioMixerGroup = value;
        return this;
    }

    public EditorAudioPlayer SetMute(bool value) {
        audioSource.mute = value;
        return this;
    }

    public EditorAudioPlayer SetBypassEffects(bool value) {
        audioSource.bypassEffects = value;
        return this;
    }

    public EditorAudioPlayer SetBypassListenerEffects(bool value) {
        audioSource.bypassListenerEffects = value;
        return this;
    }

    public EditorAudioPlayer SetBypassReverbZones(bool value) {
        audioSource.bypassReverbZones = value;
        return this;
    }

    public EditorAudioPlayer SetPlayOnAwake(bool value) {
        audioSource.playOnAwake = value;
        return this;
    }

    public EditorAudioPlayer SetLoop(bool value) {
        audioSource.loop = value;
        return this;
    }

    // Playback Settings

    public EditorAudioPlayer SetPriority(int value) {
        audioSource.priority = value;
        return this;
    }

    public EditorAudioPlayer SetVolume(float value) {
        audioSource.volume = value;
        return this;
    }

    public EditorAudioPlayer SetPitch(float value) {
        audioSource.pitch = value;
        return this;
    }

    public EditorAudioPlayer SetStereoPan(float value) {
        audioSource.panStereo = value;
        return this;
    }

    // 3D Settings

    public EditorAudioPlayer SetSpatialBlend(float value) {
        audioSource.spatialBlend = value;
        return this;
    }

    public EditorAudioPlayer SetReverbZoneMix(float value) {
        audioSource.reverbZoneMix = value;
        return this;
    }

    public EditorAudioPlayer SetDopplerLevel(float value) {
        audioSource.dopplerLevel = value;
        return this;
    }

    public EditorAudioPlayer SetSpread(float value) {
        audioSource.spread = value;
        return this;
    }

    public EditorAudioPlayer SetRolloffMode(AudioRolloffMode value) {
        audioSource.rolloffMode = value;
        return this;
    }

    public EditorAudioPlayer SetMinDistance(float value) {
        audioSource.minDistance = value;
        return this;
    }

    public EditorAudioPlayer SetMaxDistance(float value) {
        audioSource.maxDistance = value;
        return this;
    }

    // Curves

    public EditorAudioPlayer SetCustomCurve(AudioSourceCurveType type, AnimationCurve curve) {
        audioSource.SetCustomCurve(type, curve);
        return this;
    }

    public EditorAudioPlayer SetVolumeCurve(AnimationCurve value) {
        return SetCustomCurve(AudioSourceCurveType.CustomRolloff, value);
    }

    public EditorAudioPlayer SetSpatialBlendCurve(AnimationCurve value) {
        return SetCustomCurve(AudioSourceCurveType.SpatialBlend, value);
    }

    public EditorAudioPlayer SetSpreadCurve(AnimationCurve value) {
        return SetCustomCurve(AudioSourceCurveType.Spread, value);
    }

    public EditorAudioPlayer SetReverbZoneMixCurve(AnimationCurve value) {
        return SetCustomCurve(AudioSourceCurveType.ReverbZoneMix, value);
    }

    // Playback

    public EditorAudioPlayer Play(float normalizedStart = 0, float normalizedEnd = 1) {
        if(!CanPlay())
            return this;

        normalizedStart = Mathf.Clamp01(normalizedStart);
        normalizedEnd = Mathf.Clamp01(normalizedEnd);

        if(normalizedEnd <= normalizedStart)
            return this;

        audioSource.Stop();

        float startTime = normalizedStart * audioSource.clip.length;
        float endTime = normalizedEnd * audioSource.clip.length;

        audioSource.time = startTime;
        audioSource.Play();

        float duration = (endTime - startTime) / audioSource.pitch;

        audioSource.SetScheduledEndTime(
            AudioSettings.dspTime + duration
        );

        StartMonitoring(duration);

        return this;
    }


    // Main

    public AudioSource GetAudioSource() {
        return audioSource;
    }

    // Events 

    public EditorAudioPlayer OnComplete(Action callback) {
        onComplete += callback;
        return this;
    }

    public EditorAudioPlayer RemoveOnComplete(Action callback) {
        onComplete -= callback;
        return this;
    }

    public EditorAudioPlayer OnStop(Action callback) {
        onStop += callback;
        return this;
    }

    public EditorAudioPlayer RemoveOnStop(Action callback) {
        onStop -= callback;
        return this;
    }

    #region Clean Up Methods

    private void Complete() {
        StopMonitoring();

        Action callback = onComplete;
        onComplete = null;
        onStop = null;

        try {
            callback?.Invoke();
        } finally {
            Destroy();
        }
    }

    public EditorAudioPlayer Stop() {
        StopMonitoring();

        if(audioSource != null) {
            audioSource.Stop();
            onStop?.Invoke();
        }

        return this;
    }

    public void Destroy() {
        StopMonitoring();

        if(audioSource == null)
            return;

        if(audioSource.isPlaying)
            audioSource.Stop();

        GameObject playerObject = audioSource.gameObject;

        audioSource = null;

        UnityEngine.Object.DestroyImmediate(playerObject);
    }

    #endregion

    #region Getter Methods

    public bool IsPlaying() {
        if(audioSource == null)
            return false;

        return audioSource.isPlaying;
    }

    #endregion

    #region Monitoring 

    double completionTime;
    bool isMonitoring;

    private void StartMonitoring(float duration) {
        StopMonitoring();

        completionTime = AudioSettings.dspTime + duration;
        isMonitoring = true;

        EditorApplication.update += UpdatePlayback;
    }

    private void UpdatePlayback() {
        if(AudioSettings.dspTime < completionTime)
            return;

        Complete();
    }

    private void StopMonitoring() {
        if(!isMonitoring)
            return;

        EditorApplication.update -= UpdatePlayback;
        isMonitoring = false;
    }


    #endregion

    #region Helper methods

    private bool CanPlay() {
        if(audioSource == null) {
            Debug.LogWarning("Audio player has not been created.");
            return false;
        }

        if(audioSource.clip == null) {
            Debug.LogWarning("No AudioClip has been assigned.");
            return false;
        }

        return true;
    }

    #endregion
}
