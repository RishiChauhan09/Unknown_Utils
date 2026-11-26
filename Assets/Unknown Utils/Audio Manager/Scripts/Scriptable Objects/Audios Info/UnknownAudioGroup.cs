using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Unknown.Manager.AudioManager {

    [CreateAssetMenu(fileName = "Audio Group", menuName = "Unknown/Audio Manager/Audio Group")]
    public class UnknownAudioGroup : ScriptableObject {

        public string groupName;

        public List<UnknownAudioConfig> audioConfigs = new();

        private void OnEnable() {
            UpdateGroupName();
        }

        private void OnValidate() {
            UpdateGroupName();
        }

        public void UpdateGroupName() {

            if(groupName == name)
                return;

            groupName = name;

#if UNITY_EDITOR
            foreach(UnknownAudioConfig config in audioConfigs) {

                if(config == null)
                    continue;

                AudioID audioID = config.GetAudioID();

                if(audioID == null || audioID.groupName == name)
                    continue;

                audioID.groupName = name;

                EditorUtility.SetDirty(config);
            }
#endif
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

        /// <summary>
        /// Removes audio config from this group
        /// </summary>
        /// <param name="configName"></param>
        public void RemoveConfig(string configName) {
            for(int i = 0; i < audioConfigs.Count; i++) {
                if(audioConfigs[i].GetAudioID().id == configName) {
                    audioConfigs.RemoveAt(i);
                    return;
                }
            }
        }

        /// <summary>
        /// Adds audio config to this group
        /// </summary>
        public void AddConfig(UnknownAudioConfig config) {
            audioConfigs.Add(config);
            CheckDuplicate();
        }

        /// <summary>
        /// Returns AudioConfig and null if not present
        /// </summary>
        public UnknownAudioConfig GetAudioConfig(string id) {
            for(int i = 0; i < audioConfigs.Count; i++) {
                if(audioConfigs[i].GetAudioID().id == id) {
                    return audioConfigs[i];
                }
            }

            return null;
        }

    }
}