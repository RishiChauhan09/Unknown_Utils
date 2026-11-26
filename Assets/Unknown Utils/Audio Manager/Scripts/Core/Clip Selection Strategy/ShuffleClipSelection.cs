using UnityEngine;

namespace Unknown.Manager.AudioManager {

    public class ShuffleClipSelection : IClipSelectionStrategy {

        private int lastIndex = -1;

        public IAudioClip PickAudio(IAudioClip[] audioClips, out int index) {

            index = -1;

            if(audioClips == null || audioClips.Length == 0)
                return null;

            if(audioClips.Length == 1) {
                index = 0;
                lastIndex = 0;
                return audioClips[0];
            }

            do {
                index = Random.Range(0, audioClips.Length);
            } while(index == lastIndex);

            lastIndex = index;

            return audioClips[index];
        }

        public void ResetSelectionStrategy() {
            lastIndex = -1;
        }
    }
}