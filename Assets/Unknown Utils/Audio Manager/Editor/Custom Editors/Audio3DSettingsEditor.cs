using UnityEditor;
using UnityEngine.UIElements;

namespace Unknown.Manager.AudioManager {


    [CustomEditor(typeof(Audio3DSettings))]
    public class Audio3DSettingsEditor : Editor {

        public VisualTreeAsset mainUxml;

        public override VisualElement CreateInspectorGUI() {
            var rootVisualElement = new VisualElement();

            mainUxml.CloneTree(rootVisualElement);

            return rootVisualElement;
        }

    }
}