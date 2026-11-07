using UnityEngine;

namespace Unknown.Manager.AudioManager {
    public interface IClipSelectionStrategy {

        public IAudioClip PickAudio(IAudioClip[] audioClips, out int index);

        public void ResetSelectionStrategy();

    }
}