using System.Collections.Generic;
using UnityEngine;

namespace Unknown.Manager.AudioManager {

    [CreateAssetMenu(fileName = "AudioDatabase", menuName = "Unknown/Audio Manager/AudioDatabase")]
    public class AudioDatabase : ScriptableObject {

        public UnknownAudioConfig[] audioConfigs;

        public void CheckDuplicate() {
            HashSet<AudioID> uniqueIDs = new();

            foreach(UnknownAudioConfig config in audioConfigs) {
                if(!uniqueIDs.Add(config.GetAudioID())) {
                    Debug.LogError($"Duplicate AudioID: {config.GetAudioID()}");
                }
            }
        }

    }

}