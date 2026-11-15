using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Unknown.Manager.AudioManager {

    [CustomEditor(typeof(AudioCategory))]
    public class AudioCategoryDatabaseEditor : Editor {

        public VisualTreeAsset mainUxml;

        public override VisualElement CreateInspectorGUI() {
            var root = new VisualElement();

            mainUxml.CloneTree(root);

            ListView listView = root.Q<ListView>("AudioTypesList");
            SerializedProperty serializedProperty = serializedObject.FindProperty(AudioCategory.NameOf.CATEGORIES_LIST);

            listView.BindProperty(serializedProperty);

            return root;

        }
    }
}