using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Unknown.Manager.AudioManager {

    public class AudioDatabasePostprocessor : AssetPostprocessor {

        private static bool refreshScheduled;

        private static readonly HashSet<string> promptedConfigs = new();

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {

            if(refreshScheduled || EditorApplication.isPlayingOrWillChangePlaymode)
                return;

            foreach(string path in importedAssets) {
                if(!path.EndsWith(".asset"))
                    continue;

                UnknownAudioConfig audioConfig =
                    AssetDatabase.LoadAssetAtPath<UnknownAudioConfig>(path);

                if(audioConfig == null)
                    continue;

                string guid = AssetDatabase.AssetPathToGUID(path);

                // Already handled this config.
                if(!promptedConfigs.Add(guid))
                    continue;

                EditorApplication.delayCall += () => {
                    if(audioConfig == null)
                        return;

                    if(CheckAudioGroups()) {
                        SerializedObject configSerializedObject = new SerializedObject(audioConfig);

                        audioConfig.InitialzeAudioConfig();
                        configSerializedObject.ApplyModifiedProperties();

                        Selection.activeObject = audioConfig;

                        return;
                    }

                    string currentPath = AssetDatabase.GetAssetPath(audioConfig);

                    if(!string.IsNullOrEmpty(currentPath)) {
                        promptedConfigs.Remove(guid);
                        AssetDatabase.DeleteAsset(currentPath);
                    }
                };

                return;
            }

            // updating audio database
            bool relevant = HasAudioAsset(importedAssets) || HasAudioAsset(movedAssets);

            if(!relevant)
                relevant = HasAnyAsset(deletedAssets) || HasAnyAsset(movedFromAssetPaths);

            if(!relevant)
                return;

            refreshScheduled = true;

            EditorApplication.delayCall += () => {
                refreshScheduled = false;
                AudioDatabaseUpdater.RefreshDatabase();
            };
        }

        #region helper methods

        // Checking if audio asset has been change in audio group or audio config
        private static bool HasAudioAsset(string[] paths) {
            foreach(string path in paths) {
                if(!path.EndsWith(".asset"))
                    continue;

                System.Type type = AssetDatabase.GetMainAssetTypeAtPath(path);

                if(type == null)
                    continue;

                if(typeof(UnknownAudioConfig).IsAssignableFrom(type) || typeof(UnknownAudioGroup).IsAssignableFrom(type))
                    return true;
            }

            return false;
        }

        // Checking if any asset is changed
        private static bool HasAnyAsset(string[] paths) {
            foreach(string path in paths) {
                if(path.EndsWith(".asset"))
                    return true;
            }

            return false;
        }

        #endregion

        #region Audio Group Checking Method

        private static bool CheckAudioGroups() {

            if(HasAudioGroup())
                return true;

            bool create = EditorUtility.DisplayDialog("No Audio Group Found",
                "You have created audio config, but no Audio Groups exist. Would you like to create one ?",
                "Create Audio Group",
                "Cancel");

            if(create) {
                if(GiveAudioGroupCreatePrompt()) {
                    return true;
                }

                return false;
            } else {
                return false;
            }
        }

        private static bool HasAudioGroup() {
            AudioDatabase database = Resources.Load<AudioDatabase>(nameof(AudioDatabase));

            List<string> groupNames = database.GetGroupsName();

            if(groupNames.Count > 0)
                return true;
            return false;
        }

        private static bool GiveAudioGroupCreatePrompt() {
            string path = EditorUtility.SaveFilePanelInProject(
                "Create Audio Group",
                "Audio Group",
                "asset",
                "Select path where you want to create new Audio Group"
            );

            if(string.IsNullOrEmpty(path))
                return false;

            UnknownAudioGroup audioGroup = ScriptableObject.CreateInstance<UnknownAudioGroup>();

            AssetDatabase.CreateAsset(audioGroup, path);
            audioGroup.UpdateGroupName();            // for updating the group name
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return true;
        }

        #endregion
    }
}