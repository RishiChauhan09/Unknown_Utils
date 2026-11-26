using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Unknown.Manager.AudioManager {

    //[CreateAssetMenu(fileName = "AudioDatabase", menuName = "Unknown/Audio Manager/AudioDatabase")]
    public class AudioDatabase : ScriptableObject {

        public UnknownAudioGroup[] audioGroups;

        /// <summary>
        /// Rebuilds the whole database using arrays of groups and configs.
        /// Returns true only if something actually changed.
        /// </summary>
        public bool Rebuild(UnknownAudioGroup[] groups, UnknownAudioConfig[] configs) {

            bool changed = false;

            Dictionary<string, UnknownAudioGroup> groupLookup = new();
            Dictionary<UnknownAudioGroup, List<UnknownAudioConfig>> newConfigs = new();
            List<UnknownAudioGroup> validGroups = new();

            if(groups != null) {
                foreach(UnknownAudioGroup group in groups) {

                    if(group == null)
                        continue;

                    if(string.IsNullOrEmpty(group.groupName))
                        continue;

                    if(!groupLookup.TryAdd(group.groupName, group)) {
                        Debug.LogError($"Duplicate audio group: {group.groupName}", group);
                        continue;
                    }

                    validGroups.Add(group);
                    newConfigs[group] = new List<UnknownAudioConfig>();
                }
            }

            UnknownAudioGroup[] newGroupArray = validGroups.ToArray();

            if(audioGroups == null || !audioGroups.SequenceEqual(newGroupArray)) {
                audioGroups = newGroupArray;
                changed = true;
            }

            if(configs != null) {
                foreach(UnknownAudioConfig config in configs) {

                    if(config == null)
                        continue;

                    string groupName = config.GetAudioID().groupName;

                    if(string.IsNullOrEmpty(groupName))
                        continue;

                    if(groupLookup.TryGetValue(groupName, out UnknownAudioGroup group)) {
                        newConfigs[group].Add(config);
                    } else {
                        Debug.LogWarning($"No audio group found for config: {config.name} (Group: {groupName})", config);
                    }
                }
            }

            foreach(UnknownAudioGroup group in validGroups) {

                List<UnknownAudioConfig> updated = newConfigs[group];

                if(group.audioConfigs == null) {
                    group.audioConfigs = updated;
                    changed = true;
                } else if(!group.audioConfigs.SequenceEqual(updated)) {
                    group.audioConfigs.Clear();
                    group.audioConfigs.AddRange(updated);
                    changed = true;
                }
            }

            foreach(UnknownAudioGroup group in validGroups)
                group.CheckDuplicate();

            return changed;
        }

        /// <summary>
        /// Returns all configs from database
        /// </summary>
        public List<UnknownAudioConfig> GetAudioConfigs() {
            List<UnknownAudioConfig> configs = new();

            if(audioGroups == null)
                return configs;

            foreach(UnknownAudioGroup group in audioGroups) {
                if(group != null && group.audioConfigs != null)
                    configs.AddRange(group.audioConfigs);
            }

            return configs;
        }

        /// <summary>
        /// Returns all group names
        /// </summary>
        public List<string> GetGroupsName() {
            List<string> names = new();

            if(audioGroups == null)
                return names;

            foreach(UnknownAudioGroup group in audioGroups) {
                if(group != null)
                    names.Add(group.groupName);
            }

            return names;
        }

        /// <summary>
        /// Returns UnknownAudioGroup of that name
        /// </summary>
        /// <returns></returns>
        public UnknownAudioGroup GetGroup(string groupName) {
            foreach(UnknownAudioGroup group in audioGroups) {
                if(group.groupName == groupName)
                    return group;
            }

            return null;
        }

        #region Static Methods

        /// <summary>
        /// Returns if AudioID is valid or not
        /// </summary>
        public static bool IsAudioIDValid(AudioID audioID) {
            if(!IsAudioGroupValid(audioID.groupName, out UnknownAudioGroup audioGroup))
                return false;

            UnknownAudioConfig audioConfig = audioGroup.GetAudioConfig(audioID.id);

            if(audioConfig == null)
                return false;
            return true;
        }

        /// <summary>
        /// Returns if AudioID is valid or not
        /// </summary>
        public static bool IsAudioIDValid(string audioGroupName, string audioIDName) {
            if(!IsAudioGroupValid(audioGroupName, out UnknownAudioGroup audioGroup))
                return false;

            UnknownAudioConfig audioConfig = audioGroup.GetAudioConfig(audioIDName);

            if(audioConfig == null)
                return false;
            return true;
        }

        /// <summary>
        /// Returns if audio group is valid or not
        /// </summary>
        public static bool IsAudioGroupValid(string audioGroupName, out UnknownAudioGroup audioGroup) {
            AudioDatabase audioDatabase = Resources.Load<AudioDatabase>(nameof(AudioDatabase));

            audioGroup = audioDatabase.GetGroup(audioGroupName);

            if(audioGroup == null)
                return false;

            return true;
        }

        #endregion
    }
}