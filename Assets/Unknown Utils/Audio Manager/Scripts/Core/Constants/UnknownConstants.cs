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

    }
}