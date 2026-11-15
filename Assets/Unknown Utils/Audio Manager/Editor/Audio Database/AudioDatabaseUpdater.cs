using System;
using UnityEditor;
using UnityEngine;

namespace Unknown.Manager.AudioManager {

    public static class AudioDatabaseUpdater {

        [MenuItem("Tools/Unknown Utils/Audio Manager/Refresh Database")]
        public static void RefreshDatabase() {

            AudioDatabase database = FindDatabase();

            if(database == null) {
                Debug.LogError("AudioDatabase not found!");
                return;
            }

            UnknownAudioGroup[] groups = LoadAssets<UnknownAudioGroup>();
            UnknownAudioConfig[] configs = LoadAssets<UnknownAudioConfig>();

            Undo.RecordObject(database, "Refresh Audio Database");
            Undo.RecordObjects(groups, "Refresh Audio Database");

            bool changed = database.Rebuild(groups, configs);
            if(!changed)
                return;

            EditorUtility.SetDirty(database);

            foreach(UnknownAudioGroup group in groups) {
                if(group != null)
                    EditorUtility.SetDirty(group);
            }

            AssetDatabase.SaveAssets();
        }


        // Returns the AudioDatabase asset from Resources folder
        private static AudioDatabase FindDatabase() {
            return Resources.Load<AudioDatabase>(nameof(AudioDatabase));
        }


        // Loads Assets of that type
        private static T[] LoadAssets<T>() where T : UnityEngine.Object {

            string[] guids = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            string[] paths = new string[guids.Length];

            for(int i = 0; i < guids.Length; i++)
                paths[i] = AssetDatabase.GUIDToAssetPath(guids[i]);

            Array.Sort(paths, StringComparer.Ordinal);

            T[] assets = new T[paths.Length];

            for(int i = 0; i < paths.Length; i++)
                assets[i] = AssetDatabase.LoadAssetAtPath<T>(paths[i]);

            return assets;
        }

    }
}