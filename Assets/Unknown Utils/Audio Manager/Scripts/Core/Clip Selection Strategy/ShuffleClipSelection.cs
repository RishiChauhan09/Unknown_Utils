using UnityEngine;

namespace Unknown.Manager.AudioManager {
    public class ShuffleClipSelection : IClipSelectionStrategy {

        private int lastIndex = -1;

        public IAudioClip PickAudio(IAudioClip[] audioClips, out int index) {
            index = -1;
            if(audioClips.Length == 0)
                return null;

            if(audioClips.Length == 1)
                return audioClips[0];

            while(lastIndex == index) {
                index = Random.Range(0, audioClips.Length);
            }

            return audioClips[index];
        }

        public void ResetSelectionStrategy() {
            lastIndex = -1;
        }
    }

}