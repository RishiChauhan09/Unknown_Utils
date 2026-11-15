using UnityEditor;

namespace Unknown.Manager.AudioManager {

    public class AudioDatabasePostprocessor : AssetPostprocessor {

        private static bool refreshScheduled;

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {

            if(refreshScheduled || EditorApplication.isPlayingOrWillChangePlaymode)
                return;

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

        // Checking if audio asset has been ch anges
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
    }
}