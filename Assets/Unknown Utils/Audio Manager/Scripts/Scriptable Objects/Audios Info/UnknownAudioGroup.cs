using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.STP;

namespace Unknown.Manager.AudioManager {

    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Unknown/Audio Manager/Audio Group")]
    public class UnknownAudioGroup : ScriptableObject {

        public string groupName;

        public List<UnknownAudioConfig> audioConfigs;

        private void OnValidate() {
            groupName = name;
        }

        /// <summary>
        /// Checks if there is duplicate audio config in this group
        /// </summary>
        public void CheckDuplicate() {
            HashSet<AudioID> uniqueIDs = new();

            for(int i = audioConfigs.Count - 1; i >= 0; i--) {
                if(!uniqueIDs.Add(audioConfigs[i].GetAudioID())) {
                    audioConfigs.RemoveAt(i);
                }
            }
        }


        public void RemoveConfig(string configName) {
            for(int i = 0; i < audioConfigs.Count; i++) {
                if(audioConfigs[i].GetAudioID().id == configName) {
                    audioConfigs.RemoveAt(i);
                    return;
                }
            }
        }

        public void AddConfig(UnknownAudioConfig config) {
            audioConfigs.Add(config);
            CheckDuplicate();
        }

    }
}