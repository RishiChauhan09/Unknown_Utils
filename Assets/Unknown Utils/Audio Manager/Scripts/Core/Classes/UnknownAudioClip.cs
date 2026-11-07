using UnityEngine;

namespace Unknown.Manager.AudioManager {

    [System.Serializable]
    public class UnknownAudioClip : IAudioClip {

        [field: SerializeField] public AudioClip audioClip { get; set; }

        [field: SerializeField] public float delay { get; set; } = 0;
        [field: SerializeField] public float volume { get; set; } = 1;
        [field: SerializeField] public float startPosition { get; set; } = 0;
        [field: SerializeField] public float endPosition { get; set; } = 1; 

        public AudioClip GetAudioClip() {
            if(IsValid()) {
                return audioClip;
            }
            return null;
        }

        public bool IsValid() {
            return audioClip != null;
        }

        public static class NameOf {
            private const string Prefix = "<";
            private const string Suffix = ">k__BackingField";

            public const string AudioClip = Prefix + nameof(audioClip) + Suffix;
            public const string Delay = Prefix + nameof(delay) + Suffix;
            public const string Volume = Prefix + nameof(volume) + Suffix;
            public const string StartPosition = Prefix + nameof(startPosition) + Suffix;
            public const string EndPosition = Prefix + nameof(endPosition) + Suffix;
        }

    }
}