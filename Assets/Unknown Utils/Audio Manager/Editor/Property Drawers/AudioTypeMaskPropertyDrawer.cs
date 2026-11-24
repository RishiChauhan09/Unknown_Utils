using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Unknown.Manager.AudioManager {

    [CustomPropertyDrawer(typeof(AudioTypeMask))]
    public class AudioTypeMaskPropertyDrawer : PropertyDrawer {

        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            SerializedProperty maskValue = property.FindPropertyRelative(AudioTypeMask.NameOf.MaskValue);

            // all auidio types names 
            List<string> audioTypesStrings = AudioCategory.GetAllTypes();
            List<AudioType> audioTypes = new();
            List<string> choices = new();

            foreach(string audioTypeName in audioTypesStrings) {
                AudioType audioType = AudioTypeMask.NameToAudioType(audioTypeName);
                if(audioType != null) {
                    audioTypes.Add(audioType);
                    choices.Add(audioType.audioTypeName);
                }
            }

            // mask field
            MaskField maskField = new MaskField(property.displayName, choices, 0);
            maskField.AddToClassList(BaseField<int>.alignedFieldUssClassName);

            // converting to field mask
            int ToFieldMask(uint actualMask) {
                int fieldMask = 0;
                for(int i = 0; i < audioTypes.Count; i++) {
                    if((actualMask & audioTypes[i].Mask) != 0) {
                        fieldMask |= (1 << i);
                    }
                }
                return fieldMask;
            }

            // converitng to custom uint mask
            uint ToActualMask(int fieldMask) {
                uint actualMask = 0;
                for(int i = 0; i < audioTypes.Count; i++) {
                    if((fieldMask & (1 << i)) != 0) {
                        actualMask |= audioTypes[i].Mask;
                    }
                }
                return actualMask;
            }

            maskField.SetValueWithoutNotify(ToFieldMask(maskValue.uintValue));

            maskField.RegisterValueChangedCallback(evt => {
                maskValue.uintValue = ToActualMask(evt.newValue);
                property.serializedObject.ApplyModifiedProperties();
            });

            maskField.TrackPropertyValue(maskValue, prop => {
                maskField.SetValueWithoutNotify(ToFieldMask(prop.uintValue));
            });

            return maskField;
        }

    }
}