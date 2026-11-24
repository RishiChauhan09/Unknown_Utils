using UnityEngine;

namespace Unknown.Manager.AudioManager {

    public static partial class AudioManager {

        /// <summary>
        /// Used to initialize manually Audio Manager
        /// </summary>
        public static void Initialize() {
            SoundManager.Init();
        }

    }
}