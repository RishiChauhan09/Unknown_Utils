using UnityEngine;
using Unknown.Manager.AudioManager;

namespace Unknown.Manager.AudioManager {
    public class RandomClipSelection : IClipSelectionStrategy {

        public IAudioClip PickAudio(IAudioClip[] audioClips, out int index) {
            index = -1;
            if(audioClips.Length == 0) {
                return null;
            }

            index = Random.Range(0, audioClips.Length);
            return audioClips[index];
        }

        public void ResetSelectionStrategy() {

        }
    }
}