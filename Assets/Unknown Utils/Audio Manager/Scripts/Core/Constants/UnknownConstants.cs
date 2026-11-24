using UnityEngine;

namespace Unknown.Manager.AudioManager {

    public static class UnknownConstants {

        /// <summary>
        /// Normalized Min value audio mixer supports
        /// </summary>
        public const float MinVolume = 0.0001f;

        /// <summary>
        /// Normalized full volume (or default) for Audio Mixer
        /// </summary>
        public const float FullVolume = 1f;

        /// <summary>
        /// Normalized Max volume Audio Mixer can support
        /// </summary>
        public const float MaxVolume = 10f;

        /// <summary>
        /// The minimum volume that Unity AudioMixer can reach in dB. 
        /// </summary>
        public const float MinDecibelVolume = -80f;

        /// <summary>
        /// The full (or default) volume in dB.
        /// </summary>
        public const float FullDecibelVolume = 0f;

        /// <summary>
        /// The maximum volume that Unity AudioMixer can reach in dB.
        /// </summary>
        public const float MaxDecibelVolume = 20f;

        /// <summary>
        /// The maximum sound frequency in Hz. (base on Unity's audio mixer effect like Lowpass/Highpass)
        /// </summary>
        public const float MaxFrequency = 22000f;

        /// <summary>
        /// The minimum sound frequency in Hz. (base on Unity's audio mixer effect like Lowpass/Highpass)
        /// </summary>
        public const float MinFrequency = 10f;


        // =================== 3d settings ===================

        /// <summary>
        /// Default spartial blend for audio source
        /// </summary>
        public const float DefaultSpartialBlend = 0f;

        /// <summary>
        /// Default value for reverb zone mix for auido source
        /// </summary>
        public const float DefaultReverbZoneMix = 1f;

        /// <summary>
        /// Default value of dopper level for audio mixer
        /// </summary>
        public const float DefaultDopperLevel = 1f;

        /// <summary>
        /// Default value of spread for audio source
        /// </summary>
        public const float DefaultSpread = 0f;

        /// <summary>
        /// Default audio roll off mode for audio mixer
        /// </summary>
        public const AudioRolloffMode DefaultAudioRolloffMode = AudioRolloffMode.Logarithmic;

        /// <summary>
        /// Default value of min distance for auido source
        /// </summary>
        public const float DefaultMinDistance = 1f;

        /// <summary>
        /// Default value of max distance for auido source
        /// </summary>
        public const float DefaultMaxDistance = 500f;


        // =================== effects ===================

        /// <summary>
        /// Default value of highpass in effects
        /// </summary>
        public const float DefaultHighpass = 10f;

        /// <summary>
        /// Default value of lowpass in effects 
        /// </summary>
        public const float DefaultLowpass = 22000f;

    }
}