using UnityEngine;

namespace Unknown.Manager.AudioManager {

    public class PingPongClipSelection : IClipSelectionStrategy {

        private bool sequenceForward;
        private int currentIndex;

        public IAudioClip PickAudio(IAudioClip[] audioClips, out int index) {
            index = -1;
            if(audioClips.Length == 0)
                return null;

            index = currentIndex;
            IAudioClip audioClip = audioClips[index];

            if(sequenceForward) {
                if(currentIndex >= audioClips.Length - 1) {
                    sequenceForward = false;
                    currentIndex--;
                } else {
                    currentIndex++;
                }
            } else {
                if(currentIndex <= 0) {
                    sequenceForward = true;
                    currentIndex++;
                } else {
                    currentIndex--;
                }
            }

            return audioClip;
        }

        public void ResetSelectionStrategy() {
            sequenceForward = true;
            currentIndex = 0;
        }
    }
}