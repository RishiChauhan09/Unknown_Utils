using UnityEngine;

namespace Unknown.Manager.AudioManager {

    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Unknown/Audio Manager/AudioConfig")]
    public class UnknownAudioConfig : ScriptableObject, IAudioConfig {

        [SerializeField] private AudioID audioID;

        // global settings variables
        [SerializeField] private Vector2 masterVolume;
        [SerializeField] private Vector2 pitch;
        [SerializeField] private bool isLoop;
        [SerializeField] private RandomFlags randomFlags;

        // clip selection settings variables
        [SerializeField] private ClipSelectionStrategy clipSelectionStrategyType;

        // audio clips settings variables
        [SerializeField] private UnknownAudioClip[] audioClips;

        // private variables
        private IAudioClip[] cachedAudioClips;
        private IClipSelectionStrategy clipSelectionStrategy;

        private void OnValidate() {
            audioID.id = name;

            if(audioClips == null || audioClips.Length == 0)
                return;

            foreach(UnknownAudioClip ac in audioClips) {
                if(ac == null)
                    continue;

                ac.delay = Mathf.Max(0f, ac.delay);
                ac.volume = Mathf.Clamp01(ac.volume);

                ac.startPosition = Mathf.Clamp01(ac.startPosition);
                ac.endPosition = Mathf.Clamp01(ac.endPosition);

                if(ac.startPosition > ac.endPosition)
                    ac.startPosition = ac.endPosition;
            }
        }

        #region interface methods 

        public float GetMasterVolume() {
            if(randomFlags == RandomFlags.Volume) {
                return UnityEngine.Random.Range(masterVolume.x, masterVolume.y);
            } else {
                return masterVolume.y;
            }
        }

        public float GetPitch() {
            if(randomFlags == RandomFlags.Pitch) {
                return UnityEngine.Random.Range(pitch.x, pitch.y);
            } else {
                return pitch.y;
            }
        }

        public bool IsLooped() => isLoop;

        public UnknownAudioClip PickNewClip() {
            switch(clipSelectionStrategyType) {
                case ClipSelectionStrategy.Single: EnsureClipSelectionStrategy<SingleClipSelection>(); break;
                case ClipSelectionStrategy.Random: EnsureClipSelectionStrategy<RandomClipSelection>(); break;
                case ClipSelectionStrategy.Shuffle: EnsureClipSelectionStrategy<ShuffleClipSelection>(); break;
                case ClipSelectionStrategy.Sequence: EnsureClipSelectionStrategy<SequenceClipSelection>(); break;
                case ClipSelectionStrategy.PingPong: EnsureClipSelectionStrategy<PingPongClipSelection>(); break;
            }

            clipSelectionStrategy.PickAudio(GetAudioClips(), out int index);
            return index >= 0 && index < audioClips.Length ? audioClips[index] : null;
        }

        public void ResetClipSelectionStrategy() {
            clipSelectionStrategy.ResetSelectionStrategy();
        }

        public AudioType GetAudioType() => audioID.audioType;

        public IAudioClip[] GetAudioClips() {
            if(cachedAudioClips == null) {
                cachedAudioClips = new IAudioClip[audioClips.Length];

                for(int i = 0; i < audioClips.Length; i++) {
                    cachedAudioClips[i] = audioClips[i];
                }
            }

            return cachedAudioClips;
        }

        public AudioID GetAudioID() {
            return audioID;
        }

        #endregion

        #region private methods 

        private void EnsureClipSelectionStrategy<T>() where T : IClipSelectionStrategy, new() {
            if(clipSelectionStrategy == null || clipSelectionStrategy.GetType() != typeof(T)) {
                clipSelectionStrategy = new T();
            }
        }

        #endregion

        #region public methods 

        public void SetRandomFlag(RandomFlags flag) {
            randomFlags |= flag;
        }

        public void UnSetRandomFlag(RandomFlags flag) {
            randomFlags &= ~flag;
        }

        #endregion

        public class NameOf {
            public static string AudioConfigName = nameof(audioID) + "." + nameof(audioID.id);
            public static string MasterVolume = nameof(masterVolume);
            public static string Pitch = nameof(pitch);
            public static string IsLoop = nameof(isLoop);
            public static string RandomFlags = nameof(randomFlags);
            public static string AudioType = nameof(audioID) + "." + nameof(audioID.audioType);
            public static string ClipSelectionStrategyType = nameof(clipSelectionStrategyType);
            public static string AudioClips = nameof(audioClips);
        }

    }
}