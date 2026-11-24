using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unknown.Manager.AudioManager {

    [CustomPropertyDrawer(typeof(AudioType))]
    public class AudioTypePropertyDrawer : PropertyDrawer {

        public override VisualElement CreatePropertyGUI(SerializedProperty property) {

            VisualElement root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;
            root.style.justifyContent = Justify.SpaceBetween;
            root.style.alignItems = Align.Center;

            root.style.paddingLeft = 3f;

            string audioName = property.FindPropertyRelative(AudioType.NameOf.AudioTypeName).stringValue;
            int index = property.FindPropertyRelative(AudioType.NameOf.Index).intValue;

            // ================== label ==================
            Label nameLabel = new Label();
            nameLabel.text = property.displayName;
            nameLabel.style.flexGrow = 0f;
            nameLabel.style.flexShrink = 0f;
            nameLabel.style.width = Length.Percent(35);

            root.Add(nameLabel);

            // ================== dropdown ==================
            List<string> audioTypeChoices = AudioCategory.GetAllTypes();

            DropdownField audioTypeDropdown = new DropdownField();
            audioTypeDropdown.choices = audioTypeChoices;

            string ADD_TYPE = "Add Type...";

            audioTypeDropdown.choices.Add(ADD_TYPE);

            if(string.IsNullOrEmpty(audioName) || AudioTypeMask.NameToAudioType(audioName) == null) {
                audioTypeDropdown.index = 0;
                property.serializedObject.ApplyModifiedProperties();
            } else {
                int currentIndex = audioTypeDropdown.choices.IndexOf(audioName);
                audioTypeDropdown.index = currentIndex;
            }

            property.serializedObject.Update();

            audioTypeDropdown.style.width = Length.Percent(62.5f);

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

                AudioType audioType = AudioTypeMask.NameToAudioType(name);

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

        /// <summary>
        /// This is used when we don't want display name
        /// </summary>
        public static VisualElement CreateEditorGUI(SerializedProperty property) {

            VisualElement root = new VisualElement();

            string audioName = property.FindPropertyRelative(AudioType.NameOf.AudioTypeName).stringValue;
            int index = property.FindPropertyRelative(AudioType.NameOf.Index).intValue;

            List<string> audioTypeChoices = AudioCategory.GetAllTypes();

            DropdownField audioTypeDropdown = new DropdownField();
            audioTypeDropdown.choices = audioTypeChoices;

            string ADD_TYPE = "Add Type...";

            audioTypeDropdown.choices.Add(ADD_TYPE);

            if(string.IsNullOrEmpty(audioName) || AudioTypeMask.NameToAudioType(audioName) == null) {
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

                AudioType audioType = AudioTypeMask.NameToAudioType(name);

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