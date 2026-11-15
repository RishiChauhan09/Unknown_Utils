using UnityEditor;
using UnityEngine;

namespace Unknown.Manager.AudioManager {
    public class AudioCategoryEditorWindow : EditorWindow {

        [MenuItem(UnknownNames.ToolbarPath + "/Audio Category Settings")]
        public static void Open() {
            Selection.activeObject = Resources.Load(nameof(AudioCategory));
        }
    }
}