using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unknown.EditorExtensions {

    public static class UIToolkitEditorExtensions {

        /// <summary>
        /// Adds header with the text provided
        /// </summary>
        public static VisualElement AddHeader(this VisualElement element, string text) {
            var header = new Label(text);

            header.style.unityFontStyleAndWeight = FontStyle.Bold;
            header.style.fontSize = 13;
            header.style.marginTop = 10;
            header.style.marginBottom = 4;

            element.Add(header);

            return header;
        }

        /// <summary>
        /// Adds the slider of float with binding the property provided
        /// </summary>
        public static VisualElement AddFloatSlider(this VisualElement element, string name, SerializedProperty bindingProperty, float min, float max) {
            var volumeRow = new VisualElement();

            volumeRow.style.flexDirection = FlexDirection.Row;
            volumeRow.style.alignItems = Align.Center;

            var slider = new Slider(name, min, max);
            slider.style.flexGrow = 1;
            slider.BindProperty(bindingProperty);

            var valueField = new FloatField();
            valueField.style.width = 60;
            valueField.style.marginLeft = 8;
            valueField.BindProperty(bindingProperty);

            volumeRow.Add(slider);
            volumeRow.Add(valueField);

            element.Add(volumeRow);

            return volumeRow;
        }

        /// <summary>
        /// Adds the min max slider of float with binding the property provided
        /// </summary>
        public static VisualElement AddMinMaxSlider(this VisualElement element, string name, SerializedProperty property, float min, float max) {
            var pitchRange = new MinMaxSlider(name, min, max, min, max);
            pitchRange.BindProperty(property);

            var minField = new FloatField();
            minField.style.width = 55;
            minField.style.marginLeft = 8;

            var maxField = new FloatField();
            maxField.style.width = 55;
            maxField.style.marginLeft = 8;

            minField.value = property.vector2Value.x;

            minField.RegisterValueChangedCallback(evt =>
            {
                Vector2 value = property.vector2Value;
                value.x = evt.newValue;
                property.vector2Value = value;

                property.serializedObject.ApplyModifiedProperties();
            });

            maxField.value = property.vector2Value.x;

            maxField.RegisterValueChangedCallback(evt => {
                Vector2 value = property.vector2Value;
                value.y = evt.newValue;
                property.vector2Value = value;

                property.serializedObject.ApplyModifiedProperties();
            });

            pitchRange.RegisterValueChangedCallback(evt => {
                minField.value = evt.newValue.x;
                maxField.value = evt.newValue.y;
            });

            var rangeRow = new VisualElement();
            rangeRow.style.flexDirection = FlexDirection.Row;

            pitchRange.style.flexGrow = 1;

            rangeRow.Add(pitchRange);
            rangeRow.Add(minField);
            rangeRow.Add(maxField);

            element.Add(rangeRow);

            return rangeRow;
        }

    }

}