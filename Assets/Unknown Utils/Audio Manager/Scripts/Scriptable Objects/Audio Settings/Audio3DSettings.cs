using UnityEngine;

namespace Unknown.Manager.AudioManager {

    [CreateAssetMenu(fileName = "Audio 3D Settings", menuName = "Unknown/Audio Manager/Audio 3D Settings")]
    public class Audio3DSettings : ScriptableObject {

        [Range(0f, 1f)] public float spartialBlend = UnknownConstants.DefaultSpartialBlend;
        [Range(0, 1.1f)] public float reverbZoneMix = UnknownConstants.DefaultReverbZoneMix;

        [Header("3D Sound Settings")]
        [Range(0f, 5f)] public float dopperLevel = UnknownConstants.DefaultDopperLevel;
        [Range(0f, 360f)] public float spread = UnknownConstants.DefaultSpread;
        public AudioRolloffMode volumeRolloff = UnknownConstants.DefaultAudioRolloffMode;
        public float minDistance = UnknownConstants.DefaultMinDistance;
        public float maxDistance = UnknownConstants.DefaultMaxDistance;

        public AnimationCurve volumeCurve;
        public AnimationCurve spartialBlendCurve;
        public AnimationCurve spreadCurve;
        public AnimationCurve reverbZoneMixCurve;

        private void OnValidate() {
            spartialBlend = Mathf.Clamp01(spartialBlend);
            reverbZoneMix = Mathf.Clamp(reverbZoneMix, 0f, 1.1f);

            dopperLevel = Mathf.Clamp(dopperLevel, 0f, 5f);
            spread = Mathf.Clamp(spread, 0f, 360f);
        }

    }
}