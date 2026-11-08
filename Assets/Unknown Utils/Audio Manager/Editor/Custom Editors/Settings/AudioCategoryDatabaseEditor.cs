using UnityEditor;
using UnityEngine.UIElements;

namespace Unknown.Manager.AudioManager {

    [CustomEditor(typeof(AudioCategory))]
    public class AudioCategoryDatabaseEditor : Editor {

        public VisualTreeAsset mainUxml;

        public override VisualElement CreateInspectorGUI() {
            var root = new VisualElement();

            mainUxml.CloneTree(root);

            return root;

        }
    }
}