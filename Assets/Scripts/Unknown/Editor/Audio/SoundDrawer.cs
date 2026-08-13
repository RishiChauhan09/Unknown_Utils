using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unknown.Audio {

    [CustomPropertyDrawer(typeof(Sound))]
    public class SoundDrawer : PropertyDrawer {

        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            var root = new VisualElement();

            // -----------------------------------------
            // Properties
            // -----------------------------------------

            // name
            var name = property.FindPropertyRelative(nameof(Sound.name));

            // volume
            var useRandomVolume = property.FindPropertyRelative(nameof(Sound.useRandomVolume));

            var randomVolumeRange = property.FindPropertyRelative(nameof(Sound.randomVolumeRange));

            var volume = property.FindPropertyRelative(nameof(Sound.volume));

            // pitch
            var useRandomPitch = property.FindPropertyRelative(nameof(Sound.useRandomPitch));

            var randomPitchRange = property.FindPropertyRelative(nameof(Sound.randomPitchRange));

            var pitch = property.FindPropertyRelative(nameof(Sound.pitch));

            // other settings
            var cooldownTime = property.FindPropertyRelative(nameof(Sound.cooldownTime));

            var selectionMode = property.FindPropertyRelative(nameof(Sound.defaultSelectionMode));

            var sequenceResetTimer = property.FindPropertyRelative(nameof(Sound.sequenceResetTimer));

            var clips = property.FindPropertyRelative(nameof(Sound.clips));

            // -----------------------------------------
            // Starting of drawing fields 
            // -----------------------------------------
            int index = GetArrayIndex(property);

            if(index % 2 == 0) {
                root.style.backgroundColor = StyleKeyword.Null;
            } else {
                root.style.backgroundColor = new Color(0, 0, 0, 0.05f);
            }

            var soundFoldout = new Foldout {
                text = name.stringValue,
                value = false,
                viewDataKey = $"{property.propertyPath}_SoundMain"
            };

            // -----------------------------------------
            // Name
            // -----------------------------------------

            var nameField = new PropertyField(name, "Name");

            soundFoldout.Add(nameField);

            // -----------------------------------------
            // Volume
            // -----------------------------------------

            var volumeFoldout = new Foldout {
                text = "Volume",
                value = false,
                viewDataKey = $"{property.propertyPath}_Volume"
            };

            var useRandomVolumeField = new PropertyField(useRandomVolume, "Use Random Volume");

            var randomVolumeRangeField = new PropertyField(randomVolumeRange, "Random Range");

            var volumeField = new PropertyField(volume, "Volume");

            volumeFoldout.Add(useRandomVolumeField);
            volumeFoldout.Add(randomVolumeRangeField);
            volumeFoldout.Add(volumeField);

            soundFoldout.Add(volumeFoldout);

            // -----------------------------------------
            // Pitch
            // -----------------------------------------

            var pitchFoldout = new Foldout {
                text = "Pitch",
                value = false,
                viewDataKey = $"{property.propertyPath}_Pitch"
            };

            var useRandomPitchField = new PropertyField(useRandomPitch, "Use Random Pitch");

            var randomPitchRangeField = new PropertyField(randomPitchRange, "Random Range");

            var pitchField = new PropertyField(pitch, "Pitch");

            pitchFoldout.Add(useRandomPitchField);
            pitchFoldout.Add(randomPitchRangeField);
            pitchFoldout.Add(pitchField);

            soundFoldout.Add(pitchFoldout);

            // -----------------------------------------
            // Other Settings
            // -----------------------------------------

            var otherFoldout = new Foldout {
                text = "Other Settings",
                value = false,
                viewDataKey = $"{property.propertyPath}_Other"
            };

            otherFoldout.Add(new PropertyField(cooldownTime, "Cooldown"));

            soundFoldout.Add(otherFoldout);


            // -----------------------------------------
            // Clip Selection
            // -----------------------------------------

            var selectionFoldout = new Foldout {
                text = "Clip Selection",
                value = false
            };

            var selectionModeField = new PropertyField(selectionMode, "Selection Mode");

            var sequenceResetTimerField = new PropertyField(sequenceResetTimer, "Reset Timer");

            selectionFoldout.Add(selectionModeField);
            selectionFoldout.Add(sequenceResetTimerField);

            soundFoldout.Add(selectionFoldout);


            // -----------------------------------------
            // Clips
            // -----------------------------------------

            var clipsFoldout = new Foldout {
                text = "Clips",
                value = false,
                viewDataKey = $"{property.propertyPath}_Clips"
            };

            clipsFoldout.Add(new PropertyField(clips, "Audio Clips"));

            soundFoldout.Add(clipsFoldout);

            root.Add(soundFoldout);

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

            nameField.RegisterValueChangeCallback(_ => {
                UpdateFoldoutName();
            });


            // -----------------------------------------
            // Local functions
            // -----------------------------------------

            void UpdateVolumeVisibility() {
                randomVolumeRangeField.style.display = useRandomVolume.boolValue ? DisplayStyle.Flex : DisplayStyle.None;

                volumeField.style.display = useRandomVolume.boolValue ? DisplayStyle.None : DisplayStyle.Flex;
            }

            void UpdatePitchVisibility() {
                randomPitchRangeField.style.display = useRandomPitch.boolValue ? DisplayStyle.Flex : DisplayStyle.None;

                pitchField.style.display = useRandomPitch.boolValue ? DisplayStyle.None : DisplayStyle.Flex;
            }

            void UpdateSequenceVisibility() {
                var mode = (Sound.ClipSelectionMode)selectionMode.enumValueIndex;

                bool showResetTimer = mode == Sound.ClipSelectionMode.Sequence || mode == Sound.ClipSelectionMode.PingPong;

                sequenceResetTimerField.style.display = showResetTimer ? DisplayStyle.Flex : DisplayStyle.None;
            }

            void UpdateFoldoutName() {
                soundFoldout.text = string.IsNullOrEmpty(name.stringValue) ? "-----" : name.stringValue;
            }

            return root;
        }

        private int GetArrayIndex(SerializedProperty property) {
            string path = property.propertyPath;

            int start = path.LastIndexOf('[');
            int end = path.LastIndexOf(']');

            if(start == -1 || end == -1)
                return -1;

            return int.Parse(path.Substring(start + 1, end - start - 1));
        }

    }
}