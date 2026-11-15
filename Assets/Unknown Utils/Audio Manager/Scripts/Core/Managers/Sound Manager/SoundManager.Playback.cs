using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Unknown.Manager.AudioManager {

    public partial class SoundManager : MonoBehaviour {

        [Header("Playback Settings")]
        [SerializeField] private ObjectPool<AudioPlayer> audioPlayerPool;
        [SerializeField] private AudioMixer audioMixer;

        // non serialized private variables 
        private Dictionary<AudioID, UnknownAudioConfig> audioConfigDict;

        // runtime audio playing info
        private Dictionary<AudioID, HashSet<AudioPlayer>> runtimeAudioPlayingAccID = new();
        private Dictionary<string, HashSet<AudioPlayer>> currentAudioPlayingAccType = new();

        private void InitializePlayback() {
            // initialzing
            audioConfigDict = new();

            foreach(UnknownAudioConfig audioConfig in audioDatabase.GetAudioConfigs()) {
                if(audioConfigDict.ContainsKey(audioConfig.GetAudioID())) {
                    Debug.LogError("Same audio config with type and id");
                    continue;
                }

                audioConfigDict.Add(audioConfig.GetAudioID(), audioConfig);
            }
        }

        #region playing methods 

        public void Play(AudioID audioID, float delay = 0) {
            if(!audioConfigDict.TryGetValue(audioID, out UnknownAudioConfig audioConfig)) {
                Debug.Log("Error finding audio config for : " + audioID + 
                    "\nTry referesing database : Tools > Unknown Utils > Audio Manager > Refresh database");
                return;
            }

            // getting audio from pool
            AudioPlayer audioPlayer = audioPlayerPool.Get();

            // adding to current playing audioplayers
            AddToRuntime(audioID, audioPlayer);

            audioPlayer.SetAudioConfig(audioConfig).SetDelay(delay).Play().OnAudioEnded(() => {
                audioPlayerPool.Release(audioPlayer);

                RemoveFromRuntime(audioID, audioPlayer);
            });
        }

        #endregion

        #region controlling methods 

        #region Stop

        public void Stop(AudioID audioID) {
            if(!runtimeAudioPlayingAccID.ContainsKey(audioID))
                return;

            foreach(AudioPlayer player in runtimeAudioPlayingAccID[audioID])
                player.Stop();
        }

        public void Stop(AudioType audioType) {
            if(!currentAudioPlayingAccType.ContainsKey(audioType.audioTypeName))
                return;

            foreach(AudioPlayer player in currentAudioPlayingAccType[audioType.audioTypeName])
                player.Stop();
        }

        public void StopAll() {
            foreach(var kvp in currentAudioPlayingAccType) {
                foreach(AudioPlayer player in kvp.Value) {
                    player.Stop();
                }
            }
        }

        #endregion

        #region Pause 

        public void Pause(AudioID audioID) {
            if(!runtimeAudioPlayingAccID.ContainsKey(audioID))
                return;

            foreach(AudioPlayer player in runtimeAudioPlayingAccID[audioID])
                player.Pause();
        }

        public void Pause(AudioType audioType) {
            if(!currentAudioPlayingAccType.ContainsKey(audioType.audioTypeName))
                return;

            foreach(AudioPlayer player in currentAudioPlayingAccType[audioType.audioTypeName])
                player.Pause();
        }

        public void PasueAll() {
            foreach(var kvp in currentAudioPlayingAccType) {
                foreach(AudioPlayer player in kvp.Value) {
                    player.Pause();
                }
            }
        }

        #endregion

        #region Resume 

        public void Resume(AudioID audioID) {
            if(!runtimeAudioPlayingAccID.ContainsKey(audioID))
                return;

            foreach(AudioPlayer player in runtimeAudioPlayingAccID[audioID])
                player.Resume();
        }

        public void Resume(AudioType audioType) {
            if(!currentAudioPlayingAccType.ContainsKey(audioType.audioTypeName))
                return;

            foreach(AudioPlayer player in currentAudioPlayingAccType[audioType.audioTypeName])
                player.Resume();
        }

        public void ResumeAll() {
            foreach(var kvp in currentAudioPlayingAccType) {
                foreach(AudioPlayer player in kvp.Value) {
                    player.Resume();
                }
            }
        }

        #endregion

        #endregion

        #region private methods 

        private void AddToRuntime(AudioID audioID, AudioPlayer audioPlayer) {
            // according to audio id
            if(!runtimeAudioPlayingAccID.ContainsKey(audioID)) {
                runtimeAudioPlayingAccID[audioID] = new();
            }
            runtimeAudioPlayingAccID[audioID].Add(audioPlayer);

            // according to audio type
            if(!currentAudioPlayingAccType.ContainsKey(audioPlayer.GetAudioConfig().GetAudioType().audioTypeName)) {
                currentAudioPlayingAccType[audioPlayer.GetAudioConfig().GetAudioType().audioTypeName] = new();
            }
            currentAudioPlayingAccType[audioPlayer.GetAudioConfig().GetAudioType().audioTypeName].Add(audioPlayer);
        }

        private void RemoveFromRuntime(AudioID audioID, AudioPlayer audioPlayer) {
            runtimeAudioPlayingAccID[audioID].Remove(audioPlayer);
            currentAudioPlayingAccType[audioPlayer.GetAudioConfig().GetAudioType().audioTypeName].Remove(audioPlayer);
        }

        #endregion

    }
}