using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Unknown.EditorExtensions;

namespace Unknown.Manager {

    [CustomEditor(typeof(Sound))]
    public class SoundDrawer : Editor {

        public override VisualElement CreateInspectorGUI() {
            var root = new VisualElement();

            // -----------------------------------------
            // Properties
            // -----------------------------------------

            // id
            var id = serializedObject.FindProperty(nameof(Sound.id));

            // volume
            var useRandomVolume = serializedObject.FindProperty(nameof(Sound.useRandomVolume));

            var randomVolumeRange = serializedObject.FindProperty(nameof(Sound.randomVolumeRange));

            var volume = serializedObject.FindProperty(nameof(Sound.volume));

            // pitch
            var useRandomPitch = serializedObject.FindProperty(nameof(Sound.useRandomPitch));

            var randomPitchRange = serializedObject.FindProperty(nameof(Sound.randomPitchRange));

            var pitch = serializedObject.FindProperty(nameof(Sound.pitch));

            // other settings
            var cooldownTime = serializedObject.FindProperty(nameof(Sound.cooldownTime));

            var selectionMode = serializedObject.FindProperty(nameof(Sound.defaultSelectionMode));

            var sequenceResetTimer = serializedObject.FindProperty(nameof(Sound.sequenceResetTimer));

            var clips = serializedObject.FindProperty(nameof(Sound.clips));

            // -----------------------------------------
            // Name
            // -----------------------------------------

            var nameField = new PropertyField(id, "Name");

            root.Add(nameField);

            // -----------------------------------------
            // Volume
            // -----------------------------------------

            root.AddHeader("Volume");

            var useRandomVolumeField = new PropertyField(useRandomVolume, "Use Random Volume");

            root.Add(useRandomVolumeField);
            var randomVolumeRangeField = root.AddMinMaxSlider("Random Volume", randomVolumeRange, 0f, 1f);
            var volumeSlider = root.AddFloatSlider("Volume", volume, 0, 1);

            // -----------------------------------------
            // Pitch
            // -----------------------------------------

            root.AddHeader("Pitch");

            var useRandomPitchField = new PropertyField(useRandomPitch, "Use Random Pitch");

            root.Add(useRandomPitchField);
            var randomPitchRangeField = root.AddMinMaxSlider("Random Pitch", randomPitchRange, -3f, 3f);
            var pitchSlider = root.AddFloatSlider("Pitch", pitch, -3, 3);

            // -----------------------------------------
            // Other Settings
            // -----------------------------------------

            root.AddHeader("Other Settings");

            root.Add(new PropertyField(cooldownTime, "Cooldown"));

            // -----------------------------------------
            // Clip Selection
            // -----------------------------------------

            root.AddHeader("Clip Selection");

            var selectionModeField = new PropertyField(selectionMode, "Selection Mode");

            var sequenceResetTimerField = new PropertyField(sequenceResetTimer, "Reset Timer");

            root.Add(selectionModeField);
            root.Add(sequenceResetTimerField);

            // -----------------------------------------
            // Clips
            // -----------------------------------------

            root.AddHeader("Clip");

            root.Add(new PropertyField(clips, "Audio Clips"));

            // -----------------------------------------
            // Initial visibility
            // -----------------------------------------

            UpdateVolumeVisibility();
            UpdatePitchVisibility();
            UpdateSequenceVisibility();


            // -----------------------------------------
            // Callbacks
            // -----------------------------------------

            useRandomVolumeField.RegisterValueChangeCallback(_ => {
                UpdateVolumeVisibility();
            });

            useRandomPitchField.RegisterValueChangeCallback(_ => {
                UpdatePitchVisibility();
            });

            selectionModeField.RegisterValueChangeCallback(_ => {
                UpdateSequenceVisibility();
            });


            // -----------------------------------------
            // Local functions
            // -----------------------------------------

            void UpdateVolumeVisibility() {
                randomVolumeRangeField.style.display = useRandomVolume.boolValue ? DisplayStyle.Flex : DisplayStyle.None;

                volumeSlider.style.display = useRandomVolume.boolValue ? DisplayStyle.None : DisplayStyle.Flex;
            }

            void UpdatePitchVisibility() {
                randomPitchRangeField.style.display = useRandomPitch.boolValue ? DisplayStyle.Flex : DisplayStyle.None;

                pitchSlider.style.display = useRandomPitch.boolValue ? DisplayStyle.None : DisplayStyle.Flex;
            }

            void UpdateSequenceVisibility() {
                var mode = (Sound.ClipSelectionMode)selectionMode.enumValueIndex;

                bool showResetTimer = mode == Sound.ClipSelectionMode.Sequence || mode == Sound.ClipSelectionMode.PingPong;

                sequenceResetTimerField.style.display = showResetTimer ? DisplayStyle.Flex : DisplayStyle.None;
            }

            return root;
        }

    }
}