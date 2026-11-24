using System.Collections.Generic;
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

        // audio config things
        private const string AUDIO_GROUP_NAME = "GroupName";
        private const string PLAY_BUTTON = "PlayButton";
        private const string LOOP_TOGGLE = "LoopToggle";
        private const string AUDIO_TYPE_VISUAL_ELEMENT = "AudioTypeRoot";

        // const fields audio clips 
        private const string START_POS = "clipStartPos";
        private const string END_POS = "clipEndPos";
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

        // 3d settings
        private const string SETTINGS_3D_OBJECT_FIELD = "3DSettingsField";
        private const string NEW_3D_SETTINGS_BUTTON = "New3DSettingsButton";

        // ui fields 
        private DropdownField groupNameFieldDD;
        private VisualElement audioTypeVisualElement;
        private ListView clipsListView;
        private FloatField startPosField;
        private FloatField endPosField;
        private Image clipImage;
        private Image startPosImage;
        private Image endPosImage;
        private MinMaxSlider trimSlider;
        private Button playTestingAudioButton;
        private ToolbarToggle loopToggle;

        // 3d settings
        private ObjectField settings3DObjectField;
        private Button new3DSettingsButton;

        // private fields 
        private SerializedProperty currentClip;
        private EditorAudioPlayer audioPlayer;

        // toggle bar 
        private ToolbarToggle volumeRndToggle;
        private Slider volumeSlider;
        private VisualElement volumeMinMaxSlider;

        private ToolbarToggle pitchRndToggle;
        private Slider pitchSlider;
        private VisualElement pitchMinMaxSlider;

        private Dictionary<int, UnknownAudioClipDrawer> clipDrawersByIndex;

        // fields
        private bool IsTestingLooped => loopToggle.value;

        public override VisualElement CreateInspectorGUI() {
            var root = new VisualElement();

            mainUxml.CloneTree(root);

            UnknownAudioConfig audioConfig = serializedObject.targetObject as UnknownAudioConfig;

            audioClipsListProperty = serializedObject.FindProperty(UnknownAudioConfig.NameOf.AudioClips);

            // ============== getting all fields ==============
            groupNameFieldDD = root.Q<DropdownField>(AUDIO_GROUP_NAME);
            audioTypeVisualElement = root.Q<VisualElement>(AUDIO_TYPE_VISUAL_ELEMENT);
            startPosField = root.Q<FloatField>(START_POS);
            endPosField = root.Q<FloatField>(END_POS);
            clipsListView = root.Q<ListView>(CLIPS_LIST);
            clipImage = root.Q<Image>(CLIP_IMAGE);
            startPosImage = root.Q<Image>(START_POS_IMAGE);
            endPosImage = root.Q<Image>(END_POS_IMAGE);
            trimSlider = root.Q<MinMaxSlider>(TRIM_SLIDER);
            playTestingAudioButton = root.Q<Button>(PLAY_BUTTON);
            loopToggle = root.Q<ToolbarToggle>(LOOP_TOGGLE);

            // 3d settings
            settings3DObjectField = root.Q<ObjectField>(SETTINGS_3D_OBJECT_FIELD);
            new3DSettingsButton = root.Q<Button>(NEW_3D_SETTINGS_BUTTON);

            volumeRndToggle = root.Q<ToolbarToggle>(RND_TOGGLE_VOLUME);
            volumeSlider = root.Q<Slider>(MASTER_VOLUME_SLIDER);
            volumeMinMaxSlider = root.Q<VisualElement>(MASTER_VOLUME_SLIDER_MINMAX);

            pitchRndToggle = root.Q<ToolbarToggle>(RND_TOGGLE_PITCH);
            pitchSlider = root.Q<Slider>(PITCH_SLIDER);
            pitchMinMaxSlider = root.Q<VisualElement>(PITCH_SLIDER_MINMAX);

            // ============== List initialization ==============
            clipDrawersByIndex = new Dictionary<int, UnknownAudioClipDrawer>();
            clipsListView.makeItem = MakeClipItem;
            clipsListView.bindItem = BindClipItem;

            // ============== Updating Things ==============
            UpdateGroupDropDown();
            Update3DSettings();
            UpdateToggleBar();
            UpdateAudioTypeProperty();

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

            playTestingAudioButton.clicked += PlayAudio;

            RefreshFields();

            return root;
        }

        #region List methods 

        private void RefreshFields() {
            int index = clipsListView.selectedIndex;
            bool hasSelection = index >= 0 && index < audioClipsListProperty.arraySize;

            startPosField.SetEnabled(hasSelection);
            endPosField.SetEnabled(hasSelection);
            trimSlider.SetEnabled(hasSelection);

            startPosField.Unbind();
            endPosField.Unbind();
            trimSlider.Unbind();

            currentClip = null;

            if(!hasSelection)
                return;

            SerializedProperty selectedClip = audioClipsListProperty.GetArrayElementAtIndex(index);
            SerializedProperty startPosProp = selectedClip.FindPropertyRelative(UnknownAudioClip.NameOf.StartPosition);
            SerializedProperty endPosProp = selectedClip.FindPropertyRelative(UnknownAudioClip.NameOf.EndPosition);

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

        private VisualElement MakeClipItem() {
            return new VisualElement();
        }

        private void BindClipItem(VisualElement element, int index) {
            element.Clear();

            SerializedProperty property = audioClipsListProperty.GetArrayElementAtIndex(index);

            UnknownAudioClipDrawer drawer = new UnknownAudioClipDrawer();

            clipDrawersByIndex[index] = drawer;

            element.Add(drawer.CreateGUI(property));
        }

        private void InitializeAudioClip() {

            serializedObject.Update();

            int index = audioClipsListProperty.arraySize;
            audioClipsListProperty.arraySize++;

            SerializedProperty addedClip = audioClipsListProperty.GetArrayElementAtIndex(index);

            if(addedClip == null) {
                return;
            }

            addedClip.FindPropertyRelative(UnknownAudioClip.NameOf.StartPosition).floatValue = 0f;
            addedClip.FindPropertyRelative(UnknownAudioClip.NameOf.EndPosition).floatValue = 1f;
            addedClip.FindPropertyRelative(UnknownAudioClip.NameOf.Volume).floatValue = 1f;
            addedClip.FindPropertyRelative(UnknownAudioClip.NameOf.AudioClip).objectReferenceValue = null;

            serializedObject.ApplyModifiedProperties();

        }

        #endregion

        #region Updating methods 

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

        private void UpdateGroupDropDown() {
            AudioDatabase database = Resources.Load(nameof(AudioDatabase)) as AudioDatabase;

            List<string> groupsName = database.GetGroupsName();
            groupNameFieldDD.choices = groupsName;

            SerializedProperty groupNameProperty = serializedObject.FindProperty(UnknownAudioConfig.NameOf.AudioGroupName);
            string currentGroupID = groupNameProperty.stringValue;
            UnknownAudioConfig audioConfig = serializedObject.targetObject as UnknownAudioConfig;

            if(string.IsNullOrEmpty(currentGroupID) && groupNameFieldDD.choices.Count > 0) {
                groupNameProperty.stringValue = groupsName[0];
                serializedObject.ApplyModifiedProperties();
            }

            groupNameFieldDD.RegisterValueChangedCallback((evt) => {
                UnknownAudioGroup group = database.GetGroup(currentGroupID);

                if(group != null)
                    group.RemoveConfig(audioConfig.GetAudioID().id);

                string newGroupName = evt.newValue;
                UnknownAudioGroup newGroup = database.GetGroup(newGroupName);

                if(newGroup != null)
                    newGroup.AddConfig(audioConfig);

                AssetDatabase.SaveAssets();
            });
        }

        private void Update3DSettings() {
            SerializedProperty settings3DProperty = serializedObject.FindProperty(UnknownAudioConfig.NameOf.Audio3DSettings);
            bool hasSettings = settings3DProperty.objectReferenceValue != null;

            new3DSettingsButton.style.display = hasSettings ? DisplayStyle.None : DisplayStyle.Flex;

            new3DSettingsButton.RegisterCallback<ClickEvent>(evt => {
                Create3DSettings();
            });

            settings3DObjectField.RegisterValueChangedCallback(evt => {
                bool hasSettings = settings3DProperty.objectReferenceValue != null;
                new3DSettingsButton.style.display = hasSettings ? DisplayStyle.None : DisplayStyle.Flex;
            });
        }

        private void Create3DSettings() {
            string path = EditorUtility.SaveFilePanelInProject(
                "Create 3D Settings",
                "Audio 3D Settings",
                "asset",
                "Select where to save the 3D Settings asset."
            );

            if(string.IsNullOrEmpty(path))
                return;

            Audio3DSettings settings = CreateInstance<Audio3DSettings>();

            AssetDatabase.CreateAsset(settings, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            SerializedProperty settings3DProperty = serializedObject.FindProperty(UnknownAudioConfig.NameOf.Audio3DSettings);
            settings3DProperty.objectReferenceValue = settings;
            serializedObject.ApplyModifiedProperties();
        }

        private void UpdateToggleBar() {
            SerializedProperty randomFlagsProperty = serializedObject.FindProperty(UnknownAudioConfig.NameOf.RandomFlags);

            RandomFlags randomFlags = (RandomFlags)randomFlagsProperty.intValue;

            volumeRndToggle.value = (randomFlags & RandomFlags.Volume) > 0;
            pitchRndToggle.value = (randomFlags & RandomFlags.Pitch) > 0;

            UpdateGUIForRandom(volumeRndToggle, volumeSlider, volumeMinMaxSlider);
            UpdateGUIForRandom(pitchRndToggle, pitchSlider, pitchMinMaxSlider);
        }

        private void UpdateAudioTypeProperty() {
            SerializedProperty audioTypeProperty = serializedObject.FindProperty(UnknownAudioConfig.NameOf.AudioType);

            VisualElement audioTypeEle = AudioTypePropertyDrawer.CreateEditorGUI(audioTypeProperty);

            audioTypeVisualElement.Add(audioTypeEle);
        }

        #endregion

        #region button methods 

        private void PlayAudio() {
            EditorApplication.update -= PlayAudio;

            if(this.audioPlayer != null && this.audioPlayer.IsPlaying()) {
                this.audioPlayer.Stop();
                this.audioPlayer.Destroy();
                this.audioPlayer = null;
                playTestingAudioButton.text = "▶";

                return;
            }

            UnknownAudioConfig audioConfig = serializedObject.targetObject as UnknownAudioConfig;

            UnknownAudioClip audioClipInfo = audioConfig.PickNewClip(out int index);

            if(!clipDrawersByIndex.ContainsKey(index)) {
                Debug.LogWarning("Error in finding the index to play audio");
            }

            EditorAudioPlayer audioPlayer = clipDrawersByIndex[index].PlayOrStopUsingAudioConfig(audioClipInfo.volume * audioConfig.GetMasterVolume(),
                audioConfig.GetPitch());

            this.audioPlayer = audioPlayer;

            if(audioPlayer.IsPlaying()) {
                playTestingAudioButton.text = "■";

                audioPlayer.OnComplete(() => {
                    if(IsTestingLooped) {
                        double delay = EditorApplication.timeSinceStartup + 0.1f;

                        EditorApplication.update += PlayAudio;
                    } else {
                        playTestingAudioButton.text = "▶";
                    }
                });

            } else {
                playTestingAudioButton.text = "▶";
            }
        }

        #endregion

    }
}