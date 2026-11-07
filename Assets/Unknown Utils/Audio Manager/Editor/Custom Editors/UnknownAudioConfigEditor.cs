using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unknown.Manager.AudioManager {

    [CustomEditor(typeof(UnknownAudioConfig))]
    public class UnknownAudioConfigEditor : Editor {

        public VisualTreeAsset mainUxml;

        // private variables
        private SerializedProperty audioClipsListProperty;

        // const fields 
        private const string START_POS = "clipStartPos";
        private const string END_POS = "clipEndPos";
        private const string DELAY = "clipDelay";
        private const string CLIPS_LIST = "audioClipsListView";

        // overall settings things 
        private const string MASTER_VOLUME_SLIDER = "master-volume-slider";
        private const string MASTER_VOLUME_SLIDER_MINMAX = "master-volume-slider-minmax";
        private const string RND_TOGGLE_VOLUME = "volume-rnd-toggle";
        private const string PITCH_SLIDER = "pitch-slider";
        private const string PITCH_SLIDER_MINMAX = "pitch-slider-minmax";
        private const string RND_TOGGLE_PITCH = "pitch-rnd-toggle";

        // image things
        private const string TRIM_SLIDER = "trimSlider";
        private const string CLIP_IMAGE = "audioImage";
        private const string START_POS_IMAGE = "startPosImage";
        private const string END_POS_IMAGE = "endPosImage";

        // ui fields 
        private ListView clipsListView;
        private FloatField startPosField;
        private FloatField endPosField;
        private FloatField delayField;
        private Image clipImage;
        private Image startPosImage;
        private Image endPosImage;
        private MinMaxSlider trimSlider;

        // private fields 
        private SerializedProperty currentClip;


        public override VisualElement CreateInspectorGUI() {
            var root = new VisualElement();

            mainUxml.CloneTree(root);

            UnknownAudioConfig audioConfig = serializedObject.targetObject as UnknownAudioConfig;

            audioClipsListProperty = serializedObject.FindProperty(UnknownAudioConfig.NameOf.AudioClips);

            // ============== getting all fields ==============
            startPosField = root.Q<FloatField>(START_POS);
            endPosField = root.Q<FloatField>(END_POS);
            delayField = root.Q<FloatField>(DELAY);
            clipsListView = root.Q<ListView>(CLIPS_LIST);
            clipImage = root.Q<Image>(CLIP_IMAGE);
            startPosImage = root.Q<Image>(START_POS_IMAGE);
            endPosImage = root.Q<Image>(END_POS_IMAGE);
            trimSlider = root.Q<MinMaxSlider>(TRIM_SLIDER);

            ToolbarToggle volumeRndToggle = root.Q<ToolbarToggle>(RND_TOGGLE_VOLUME);
            Slider volumeSlider = root.Q<Slider>(MASTER_VOLUME_SLIDER);
            VisualElement volumeMinMaxSlider = root.Q<VisualElement>(MASTER_VOLUME_SLIDER_MINMAX);

            ToolbarToggle pitchRndToggle = root.Q<ToolbarToggle>(RND_TOGGLE_PITCH);
            Slider pitchSlider = root.Q<Slider>(PITCH_SLIDER);
            VisualElement pitchMinMaxSlider = root.Q<VisualElement>(PITCH_SLIDER_MINMAX);

            // ============== events ==============
            clipsListView.selectionChanged += _ => RefreshFields();
            clipsListView.onAdd += _ => InitializeAudioClip();

            volumeRndToggle.RegisterValueChangedCallback(evt => {
                UpdateGUIForRandom(volumeRndToggle, volumeSlider, volumeMinMaxSlider);

                if(evt.newValue) {
                    audioConfig.SetRandomFlag(RandomFlags.Volume);
                } else {
                    audioConfig.UnSetRandomFlag(RandomFlags.Volume);
                }
            });

            pitchRndToggle.RegisterValueChangedCallback(evt => {
                UpdateGUIForRandom(pitchRndToggle, pitchSlider, pitchMinMaxSlider);

                if(evt.newValue) {
                    audioConfig.SetRandomFlag(RandomFlags.Pitch);
                } else {
                    audioConfig.UnSetRandomFlag(RandomFlags.Pitch);
                }
            });

            RefreshFields();

            return root;
        }

        #region list methods 

        private void RefreshFields() {
            int index = clipsListView.selectedIndex;
            bool hasSelection = index >= 0 && index < audioClipsListProperty.arraySize;

            startPosField.SetEnabled(hasSelection);
            endPosField.SetEnabled(hasSelection);
            delayField.SetEnabled(hasSelection);
            trimSlider.SetEnabled(hasSelection);

            startPosField.Unbind();
            endPosField.Unbind();
            delayField.Unbind();
            trimSlider.Unbind();

            currentClip = null;

            if(!hasSelection)
                return;

            SerializedProperty selectedClip = audioClipsListProperty.GetArrayElementAtIndex(index);
            SerializedProperty startPosProp = selectedClip.FindPropertyRelative(UnknownAudioClip.NameOf.StartPosition);
            SerializedProperty endPosProp = selectedClip.FindPropertyRelative(UnknownAudioClip.NameOf.EndPosition);

            delayField.BindProperty(selectedClip.FindPropertyRelative(UnknownAudioClip.NameOf.Delay));
            startPosField.BindProperty(startPosProp);
            endPosField.BindProperty(endPosProp);

            trimSlider.SetValueWithoutNotify(new Vector2(startPosProp.floatValue, endPosProp.floatValue));
            currentClip = selectedClip;

            AudioClip clip = selectedClip.FindPropertyRelative(UnknownAudioClip.NameOf.AudioClip).objectReferenceValue as AudioClip;

            if(clip == null) {
                clipImage.image = null;
                return;
            }

            int imageWidth = 500;

            Texture2D audioPreviewTexture = AudioPreview.PaintWaveformSpectrum(
                AudioPreview.GetWaveform(clip, imageWidth, 1),
                64,
                Color.orange
            );

            // comented bcoz it provides a square image instead rect image and it's blur mainly used for icons and all
            // Texture2D audioPreviewTexture = AssetPreview.GetAssetPreview(clip);

            clipImage.image = audioPreviewTexture;

            trimSlider.RegisterValueChangedCallback(evt => {
                if(currentClip == null) return;

                currentClip.FindPropertyRelative(UnknownAudioClip.NameOf.StartPosition).floatValue = evt.newValue.x;
                currentClip.FindPropertyRelative(UnknownAudioClip.NameOf.EndPosition).floatValue = evt.newValue.y;
                currentClip.serializedObject.ApplyModifiedProperties();

                UpdateStartEndPosImage(currentClip);
            });

            startPosField.RegisterValueChangedCallback(evt => {
                if(currentClip == null) return;
                trimSlider.SetValueWithoutNotify(new Vector2(startPosField.value, endPosField.value));
                UpdateStartEndPosImage(currentClip);
            });

            endPosField.RegisterValueChangedCallback(evt => {
                if(currentClip == null) return;
                trimSlider.SetValueWithoutNotify(new Vector2(startPosField.value, endPosField.value));
                UpdateStartEndPosImage(currentClip);
            });

            startPosField.RegisterValueChangedCallback(evt => {
                trimSlider.value = new Vector2(evt.newValue, trimSlider.value.y);
            });

            endPosField.RegisterValueChangedCallback(evt => {
                trimSlider.value = new Vector2(trimSlider.value.x, evt.newValue);
            });
        }

        private void InitializeAudioClip() {

            serializedObject.Update();

            int index = audioClipsListProperty.arraySize;
            audioClipsListProperty.arraySize++;

            SerializedProperty addedClip = audioClipsListProperty.GetArrayElementAtIndex(index);

            if(addedClip == null) {
                return;
            }

            addedClip.FindPropertyRelative(UnknownAudioClip.NameOf.Delay).floatValue = 0f;
            addedClip.FindPropertyRelative(UnknownAudioClip.NameOf.StartPosition).floatValue = 0f;
            addedClip.FindPropertyRelative(UnknownAudioClip.NameOf.EndPosition).floatValue = 1f;
            addedClip.FindPropertyRelative(UnknownAudioClip.NameOf.Volume).floatValue = 1f;
            addedClip.FindPropertyRelative(UnknownAudioClip.NameOf.AudioClip).objectReferenceValue = null;

            serializedObject.ApplyModifiedProperties();

        }

        #endregion

        #region updating methods 

        private void UpdateGUIForRandom(ToolbarToggle rndToggle, Slider slider, VisualElement minMaxSlider) {
            if(rndToggle.value) {
                slider.style.display = DisplayStyle.None;
                minMaxSlider.style.display = DisplayStyle.Flex;
            } else {
                slider.style.display = DisplayStyle.Flex;
                minMaxSlider.style.display = DisplayStyle.None;
            }
        }

        private void UpdateStartEndPosImage(SerializedProperty selectedClip) {
            float startPos = selectedClip.FindPropertyRelative(UnknownAudioClip.NameOf.StartPosition).floatValue;

            float endPos = selectedClip.FindPropertyRelative(UnknownAudioClip.NameOf.EndPosition).floatValue;

            startPosImage.style.right = Length.Percent(100f - (startPos * 100f));
            endPosImage.style.left = Length.Percent(endPos * 100f);
        }

        #endregion


    }
}