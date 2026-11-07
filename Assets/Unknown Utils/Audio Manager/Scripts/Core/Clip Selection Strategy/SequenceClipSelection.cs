using UnityEngine;

namespace Unknown.Manager.AudioManager {

    public class SequenceClipSelection : IClipSelectionStrategy {

        private int currentAudioIndex = 0;

        public IAudioClip PickAudio(IAudioClip[] audioClips, out int index) {
            index = -1;
            if(audioClips.Length == 0)
                return null;

            if(currentAudioIndex >= audioClips.Length)
                currentAudioIndex = currentAudioIndex % audioClips.Length;

            IAudioClip clipData = audioClips[currentAudioIndex];
            index = currentAudioIndex;

            currentAudioIndex = (currentAudioIndex + 1) % audioClips.Length;
            return clipData;
        }

        public void ResetSelectionStrategy() {
            currentAudioIndex = 0;
        }
    }
}