using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unknown.Manager.AudioManager {

    [CustomPropertyDrawer(typeof(AudioType))]
    public class AudioTypePropertyDrawer : PropertyDrawer {

        public override VisualElement CreatePropertyGUI(SerializedProperty property) {

            VisualElement root = new VisualElement();

            string audioName = property.FindPropertyRelative(AudioType.NameOf.AudioTypeName).stringValue;
            int index = property.FindPropertyRelative(AudioType.NameOf.Index).intValue;

            List<string> audioTypeChoices = AudioCategory.GetAllTypes();

            DropdownField audioTypeDropdown = new DropdownField();
            audioTypeDropdown.choices = audioTypeChoices;

            string ADD_TYPE = "Add Type...";

            audioTypeDropdown.choices.Add(ADD_TYPE);

            if(string.IsNullOrEmpty(audioName) || AudioType.NameToAudioType(audioName) == null) {
                audioTypeDropdown.index = 0;
                property.serializedObject.ApplyModifiedProperties();
            } else {
                int currentIndex = audioTypeDropdown.choices.IndexOf(audioName);
                audioTypeDropdown.index = currentIndex;
            }

            property.serializedObject.Update();

            root.Add(audioTypeDropdown);

            // ================== events ==================
            audioTypeDropdown.RegisterValueChangedCallback(evt => {

                if(evt.newValue == ADD_TYPE) {
                    Selection.activeObject = Resources.Load(nameof(AudioCategory));

                    UpdateAudioTypeValues(evt.previousValue);
                    return;
                }

                UpdateAudioTypeValues(evt.newValue);

            });

            // ================== private methods ================== 
            void UpdateAudioTypeValues(string name) {

                AudioType audioType = AudioType.NameToAudioType(name);

                if(audioType == null) {
                    audioTypeDropdown.index = 0;
                    property.serializedObject.Update();
                    return;
                }

                property.FindPropertyRelative(AudioType.NameOf.AudioTypeName).stringValue = name;

                property.FindPropertyRelative(AudioType.NameOf.Index).intValue = audioType.index;

                audioTypeDropdown.index = audioTypeDropdown.choices.IndexOf(name);

                property.serializedObject.ApplyModifiedProperties();
                property.serializedObject.Update();
            }

            return root;
        }

    }
}