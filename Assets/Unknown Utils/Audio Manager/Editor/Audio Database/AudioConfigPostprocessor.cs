using System.Linq;
using UnityEditor;

namespace Unknown.Manager.AudioManager {
    public class AudioConfigPostprocessor : AssetPostprocessor {
        private static bool refreshScheduled;

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
            bool audioConfigChanged = importedAssets.Concat(deletedAssets)
                    .Concat(movedAssets)
                    .Concat(movedFromAssetPaths)
                    .Any(path => path.EndsWith(".asset") && AssetDatabase.GetMainAssetTypeAtPath(path) == typeof(UnknownAudioConfig));

            if(!audioConfigChanged || refreshScheduled)
                return;

            refreshScheduled = true;

            EditorApplication.delayCall += () => {
                refreshScheduled = false;
                AudioDatabaseUpdater.RefreshDatabase();
            };
        }
    }
}