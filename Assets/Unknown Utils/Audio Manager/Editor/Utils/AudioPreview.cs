#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Reflection;

public static class AudioPreview {

    private static readonly System.Type audioUtilType = typeof(Editor).Assembly.GetType("UnityEditor.AudioUtil");

    private static readonly MethodInfo playAudioMethod = audioUtilType?.GetMethod(
            "PlayPreviewClip",
            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
            null,
            new[] { typeof(AudioClip), typeof(int), typeof(bool) },
            null
        );

    private static readonly MethodInfo stopAudioMethod =
        audioUtilType?.GetMethod(
            "StopAllPreviewClips",
            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
        );

    private static readonly MethodInfo isPlayingMethod =
        audioUtilType?.GetMethod(
            "IsPreviewClipPlaying",
            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic
        );

    public static void Play(AudioClip clip, int startSample) {
        if(clip == null || playAudioMethod == null)
            return;

        Stop();

        playAudioMethod.Invoke(null, new object[] { clip, startSample, false });
    }

    public static void Stop() {
        stopAudioMethod?.Invoke(null, null);
    }

    public static bool IsPlaying() {
        if(isPlayingMethod == null)
            return false;

        return (bool)isPlayingMethod.Invoke(null, null);
    }

    #region preview image generation

    public static float[] GetWaveform(AudioClip audio, int size, float sat) {
        int channels = audio.channels;
        float[] samples = new float[audio.samples * channels];
        audio.GetData(samples, 0);

        float[] waveform = new float[size];
        int packSize = Mathf.Max(1, audio.samples / size);

        // Sum absolute amplitude per bucket (all channels combined)
        for(int i = 0; i < samples.Length; i++) {
            int frame = i / channels;
            int bucket = Mathf.Min(frame / packSize, size - 1);
            waveform[bucket] += Mathf.Abs(samples[i]);
        }

        // Find the peak bucket
        float max = 0f;
        for(int i = 0; i < size; i++)
            if(waveform[i] > max) max = waveform[i];

        // Normalize and clamp
        if(max > 0f) {
            float scale = 1f / (max * sat);
            for(int i = 0; i < size; i++)
                waveform[i] = Mathf.Min(waveform[i] * scale, 1f);
        }

        return waveform;
    }

    public static Texture2D PaintWaveformSpectrum(float[] waveform, int height, Color c) {
        int width = waveform.Length;
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);

        // Start with a fully transparent texture
        Color[] pixels = new Color[width * height];
        for(int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.clear;

        int mid = height / 2;
        for(int x = 0; x < width; x++) {
            int half = Mathf.RoundToInt(waveform[x] * mid);
            for(int y = 0; y <= half; y++) {
                int up = Mathf.Min(mid + y, height - 1);
                int down = Mathf.Max(mid - y, 0);
                pixels[up * width + x] = c;
                pixels[down * width + x] = c;
            }
        }

        tex.SetPixels(pixels);
        tex.Apply();
        return tex;
    }

    #endregion

}
#endif