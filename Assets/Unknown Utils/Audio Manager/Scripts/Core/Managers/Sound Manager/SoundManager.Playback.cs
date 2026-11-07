using System.Collections.Generic;
using UnityEngine;

namespace Unknown.Manager.AudioManager {

    public partial class SoundManager : MonoBehaviour {

        [Header("Playback Settings")]
        private ObjectPool<AudioPlayer> audioPlayerPool;

        // non serialized private variables 
        private Dictionary<AudioType, Dictionary<string, UnknownAudioConfig>> audioConfigDict;

        private void Awake() {
            // initialzing
            audioConfigDict = new();

            foreach(AudioType type in System.Enum.GetValues(typeof(AudioType))) {
                audioConfigDict[type] = new();
            }

            foreach(UnknownAudioConfig audioConfig in audioDatabase.audioConfigs) {
                //audioConfigDict[audioConfig.GetAudioType()][audioConfig.audioConfigName] = audioConfig;
            }
        }

        #region playing methods 

        public void Play(string soundID, float delay = 0) {

        }

        public void Play(string soundID, Vector3 pos) {

        }

        #endregion

    }
}