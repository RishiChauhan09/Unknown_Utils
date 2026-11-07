using UnityEngine;

namespace Unknown.Manager.AudioManager {
    public class SingleClipSelection : IClipSelectionStrategy {

        public IAudioClip PickAudio(IAudioClip[] audioClips, out int index) {
            index = -1;

            if(audioClips.Length > 0) {
                index = 0;
                return audioClips[index];
            }

            return null;
        }

        public void ResetSelectionStrategy() {

        }

    }
}