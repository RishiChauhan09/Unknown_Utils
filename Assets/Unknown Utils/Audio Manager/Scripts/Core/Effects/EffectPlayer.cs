using UnityEngine;

namespace Unknown.Manager.AudioManager {

    public class EffectPlayer : IEffectPlayer {

        private SoundManager soundManager;

        public EffectPlayer(SoundManager soundManager) {
            this.soundManager = soundManager;
        }

        public IEffectPlayer HighpassOthers(float highPassValue) {
            soundManager.HighpassEffects(highPassValue);
            return this;
        }

        public IEffectPlayer LowPassOthers(float lowpassValue) {
            soundManager.LowPassEffects(lowpassValue);
            return this;
        }

        public void ResetEffects() {
            soundManager.ResetEffects();
        }

        public IEffectPlayer SilentOthers() {
            soundManager.SetEffectsVolume(0);
            return this;
        }

        public IEffectPlayer VolumeOthers(float volume) {
            soundManager.SetEffectsVolume(volume);
            return this;
        }
    }
}