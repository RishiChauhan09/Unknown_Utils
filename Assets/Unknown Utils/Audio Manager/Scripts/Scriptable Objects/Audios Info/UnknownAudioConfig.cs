using UnityEngine;

namespace Unknown.Manager.AudioManager {

    [CreateAssetMenu(fileName = "Audio Config", menuName = "Unknown/Audio Manager/Audio Config")]
    public class UnknownAudioConfig : ScriptableObject, IAudioConfig {

        [SerializeField] private AudioID audioID;

        // global settings variables
        [SerializeField] private Vector2 masterVolume;
        [SerializeField] private Vector2 pitch;
        [SerializeField] private bool isLoop;
        [SerializeField] private bool loopSameAudio;
        [SerializeField] private RandomFlags randomFlags;
        [SerializeField] private AudioType audioType;

        // clip selection settings variables
        [SerializeField] private ClipSelectionStrategy clipSelectionStrategyType;

        // audio clips settings variables
        [SerializeField] private UnknownAudioClip[] audioClips;
        [SerializeField] private Audio3DSettings audio3dSettings;

        // private variables
        private IAudioClip[] cachedAudioClips;
        private IClipSelectionStrategy clipSelectionStrategy;

        private void OnEnable() {
            cachedAudioClips = null;
            UpdateAudioDetails();
        }

        private void OnValidate() {
            cachedAudioClips = null;
            UpdateAudioDetails();
        }

        #region interface methods 

        public float GetMasterVolume() {
            if((randomFlags & RandomFlags.Volume) > 0) {
                return Random.Range(masterVolume.x, masterVolume.y);
            } else {
                return masterVolume.y;
            }
        }

        public float GetPitch() {
            if((randomFlags & RandomFlags.Pitch) > 0) {
                return Random.Range(pitch.x, pitch.y);
            } else {
                return pitch.y;
            }
        }

        public bool IsLooped() => isLoop;

        public bool IsSameAudioLooped() => loopSameAudio;

        public UnknownAudioClip PickNewClip() {
            switch(clipSelectionStrategyType) {
                case ClipSelectionStrategy.Single: EnsureClipSelectionStrategy<SingleClipSelection>(); break;
                case ClipSelectionStrategy.Random: EnsureClipSelectionStrategy<RandomClipSelection>(); break;
                case ClipSelectionStrategy.Shuffle: EnsureClipSelectionStrategy<ShuffleClipSelection>(); break;
                case ClipSelectionStrategy.Sequence: EnsureClipSelectionStrategy<SequenceClipSelection>(); break;
                case ClipSelectionStrategy.PingPong: EnsureClipSelectionStrategy<PingPongClipSelection>(); break;
            }

            clipSelectionStrategy.PickAudio(GetAudioClips(), out int index);

            UnknownAudioClip audioClip = index >= 0 && index < audioClips.Length ? audioClips[index] : null;
            if(audioClip == null) {
                Debug.LogWarning("Audio clip is null " + index);
            }
            return audioClip;
        }

        public UnknownAudioClip PickNewClip(out int index) {
            switch(clipSelectionStrategyType) {
                case ClipSelectionStrategy.Single: EnsureClipSelectionStrategy<SingleClipSelection>(); break;
                case ClipSelectionStrategy.Random: EnsureClipSelectionStrategy<RandomClipSelection>(); break;
                case ClipSelectionStrategy.Shuffle: EnsureClipSelectionStrategy<ShuffleClipSelection>(); break;
                case ClipSelectionStrategy.Sequence: EnsureClipSelectionStrategy<SequenceClipSelection>(); break;
                case ClipSelectionStrategy.PingPong: EnsureClipSelectionStrategy<PingPongClipSelection>(); break;
            }

            clipSelectionStrategy.PickAudio(GetAudioClips(), out index);

            UnknownAudioClip audioClip = index >= 0 && index < audioClips.Length ? audioClips[index] : null;
            return audioClip;
        }

        public void ResetClipSelectionStrategy() {
            clipSelectionStrategy.ResetSelectionStrategy();
        }

        public AudioType GetAudioType() => audioType;

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


        public Audio3DSettings GetAudio3DSettings() {
            return audio3dSettings;
        }

        #endregion

        #region private methods 

        // Makes new clip selection strategy if it's null
        private void EnsureClipSelectionStrategy<T>() where T : IClipSelectionStrategy, new() {
            if(clipSelectionStrategy == null || clipSelectionStrategy.GetType() != typeof(T)) {
                clipSelectionStrategy = new T();
            }
        }


        // Updates the Audio Config details
        private void UpdateAudioDetails() {
            audioID.id = name;

            if(audioClips == null || audioClips.Length == 0)
                return;

            foreach(UnknownAudioClip ac in audioClips) {
                if(ac == null)
                    continue;

                ac.volume = Mathf.Clamp01(ac.volume);

                ac.startPosition = Mathf.Clamp01(ac.startPosition);
                ac.endPosition = Mathf.Clamp01(ac.endPosition);

                if(ac.startPosition > ac.endPosition)
                    ac.startPosition = ac.endPosition;
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

        public void InitialzeAudioConfig() {
            audioID.id = name;

            masterVolume.x = .9f;
            masterVolume.y = 1;

            pitch.x = .9f;
            pitch.y = 1;
        }

        #endregion

        public class NameOf {
            public static string AudioConfigName = nameof(audioID) + "." + nameof(audioID.id);
            public static string AudioGroupName = nameof(audioID) + "." + nameof(audioID.groupName);
            public static string MasterVolume = nameof(masterVolume);
            public static string Pitch = nameof(pitch);
            public static string IsLoop = nameof(isLoop);
            public static string RandomFlags = nameof(randomFlags);
            public static string AudioType = nameof(audioType);
            public static string ClipSelectionStrategyType = nameof(clipSelectionStrategyType);
            public static string AudioClips = nameof(audioClips);
            public static string Audio3DSettings = nameof(audio3dSettings);
        }

    }
}