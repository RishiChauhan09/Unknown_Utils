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
            audioPlayerPool.SetMethodAfterCreate((audioPlayer) => {
                audioPlayer.SetDefaultParent(transform);
            });

            // dict 
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

        public AudioPlayer Play(AudioID audioID, float delay = 0) {
            if(!audioConfigDict.TryGetValue(audioID, out UnknownAudioConfig audioConfig)) {
                Debug.Log("Error finding audio config for : " + audioID +
                    "\nTry referesing database : Tools > Unknown Utils > Audio Manager > Refresh database");
                return null;
            }

            // getting audio from pool
            AudioPlayer audioPlayer = audioPlayerPool.Get();

            // setting mixer group
            audioPlayer.GetAudioSource().outputAudioMixerGroup = GetMixer(audioConfig.GetAudioType().audioTypeName);

            audioPlayer.SetupAudio3DSettings(audioConfig.GetAudio3DSettings());

            if(audioConfig.IsLooped()) {
                audioPlayer.SetAudioConfig(audioConfig).SetDelay(delay).Play().OnLoopStep(() => {
                    if(audioConfig.IsSameAudioLooped()) {
                        audioPlayer.Restart();

                    } else {
                        Play(audioID);
                    }

                }).OnAudioEnded(() => {                 // this is when audio is stopped manually
                    audioPlayerPool.Release(audioPlayer);

                    RemoveFromRuntime(audioID, audioPlayer);
                });

            } else {
                audioPlayer.SetAudioConfig(audioConfig).SetDelay(delay).Play().OnAudioEnded(() => {
                    audioPlayerPool.Release(audioPlayer);

                    RemoveFromRuntime(audioID, audioPlayer);
                });
            }

            // adding to current playing audioplayers
            AddToRuntime(audioID, audioPlayer);

            return audioPlayer;
        }

        public AudioPlayer Play(AudioID audioID, Vector3 position, float delay = 0) {
            AudioPlayer audioPlayer = Play(audioID, delay);
            audioPlayer.transform.position = position;
            return audioPlayer;
        }

        public AudioPlayer Play(AudioClip audioClip, AudioParameter audioparameter = null) {
            if(audioparameter == null)
                audioparameter = new();

            AudioPlayer audioPlayer = audioPlayerPool.Get();

            AudioSource audioSource = audioPlayer.GetAudioSource();
            audioSource.volume = audioparameter.volume;
            audioSource.pitch = audioparameter.pitch;

            audioPlayer.SetDelay(audioparameter.delay)
                .Play(audioClip, audioparameter.normalizedStartPos, audioparameter.normalizedEndPos)
                .OnAudioEnded(() => {
                    audioPlayerPool.Release(audioPlayer);       // just release bcoz there is no audio id assigned to it 
                });

            // here there is no need to add to runtime bcoz it does not have any type or audio id

            return audioPlayer;
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

        public void PauseAll() {
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