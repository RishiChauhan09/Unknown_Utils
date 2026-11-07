using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Unknown.Manager.AudioManager {
    public static class AudioDatabaseUpdater {

        [MenuItem("Tools/Unknown Utils/Audio Manager/Refresh Database")]
        public static void RefreshDatabase() {
            AudioDatabase database = Resources.Load(nameof(AudioDatabase)) as AudioDatabase;

            if(database == null) {
                Debug.LogError("AudioDatabase not found!");
                return;
            }

            string[] guids = AssetDatabase.FindAssets("t:UnknownAudioConfig");

            var configs = guids.Select(guid => AssetDatabase.LoadAssetAtPath<UnknownAudioConfig>(AssetDatabase.GUIDToAssetPath(guid)))
                .Where(config => config != null)
                .ToArray();

            database.audioConfigs = configs;
            database.CheckDuplicate();

            EditorUtility.SetDirty(database);
            AssetDatabase.SaveAssets();
        }

    }
}