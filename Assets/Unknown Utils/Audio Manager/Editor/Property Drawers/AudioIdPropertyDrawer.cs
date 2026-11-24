using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unknown.Manager.AudioManager {

    /// <summary>
    /// Shared look for the audio picker. Colors adapt to the editor's light/dark skin.
    /// </summary>
    internal static class AudioPickerStyle {

        private static bool Dark => EditorGUIUtility.isProSkin;

        public static Color Accent => new Color(0.24f, 0.49f, 0.91f);
        public static Color FieldBackground => Dark ? new Color(0.16f, 0.16f, 0.16f) : new Color(0.93f, 0.93f, 0.93f);
        public static Color FieldHover => Dark ? new Color(0.21f, 0.21f, 0.21f) : new Color(0.87f, 0.87f, 0.87f);
        public static Color RowHover => new Color(Accent.r, Accent.g, Accent.b, Dark ? 0.35f : 0.22f);
        public static Color Border => Dark ? new Color(0.09f, 0.09f, 0.09f) : new Color(0.65f, 0.65f, 0.65f);
        public static Color Divider => Dark ? new Color(1f, 1f, 1f, 0.08f) : new Color(0f, 0f, 0f, 0.12f);
        public static Color TextPrimary => Dark ? new Color(0.88f, 0.88f, 0.88f) : new Color(0.1f, 0.1f, 0.1f);
        public static Color TextMuted => Dark ? new Color(0.6f, 0.6f, 0.6f) : new Color(0.4f, 0.4f, 0.4f);

        public static void SetBorder(VisualElement e, float width, Color color) {
            e.style.borderTopWidth = width;
            e.style.borderBottomWidth = width;
            e.style.borderLeftWidth = width;
            e.style.borderRightWidth = width;
            e.style.borderTopColor = color;
            e.style.borderBottomColor = color;
            e.style.borderLeftColor = color;
            e.style.borderRightColor = color;
        }

        public static void SetBorderColor(VisualElement e, Color color) {
            e.style.borderTopColor = color;
            e.style.borderBottomColor = color;
            e.style.borderLeftColor = color;
            e.style.borderRightColor = color;
        }

        public static void SetRadius(VisualElement e, float radius) {
            e.style.borderTopLeftRadius = radius;
            e.style.borderTopRightRadius = radius;
            e.style.borderBottomLeftRadius = radius;
            e.style.borderBottomRightRadius = radius;
        }

        public static void SetPadding(VisualElement e, float vertical, float horizontal) {
            e.style.paddingTop = vertical;
            e.style.paddingBottom = vertical;
            e.style.paddingLeft = horizontal;
            e.style.paddingRight = horizontal;
        }

        public static void SetMargin(VisualElement e, float top, float bottom, float left, float right) {
            e.style.marginTop = top;
            e.style.marginBottom = bottom;
            e.style.marginLeft = left;
            e.style.marginRight = right;
        }

        /// <summary>
        /// Flat button with a hover highlight, used as a base for every row and field
        /// </summary>
        public static void MakeFlat(Button b, Color normal, Color hover, bool accentBorderOnHover = false, Color? borderNormal = null) {
            b.text = string.Empty;
            b.style.backgroundColor = normal;
            b.style.flexDirection = FlexDirection.Row;
            b.style.alignItems = Align.Center;
            SetBorder(b, 0, Color.clear);
            SetRadius(b, 4);
            SetMargin(b, 0, 0, 0, 0);

            b.RegisterCallback<MouseEnterEvent>(_ => {
                b.style.backgroundColor = hover;

                if(accentBorderOnHover)
                    SetBorderColor(b, Accent);
            });

            b.RegisterCallback<MouseLeaveEvent>(_ => {
                b.style.backgroundColor = normal;

                if(accentBorderOnHover)
                    SetBorderColor(b, borderNormal ?? Border);
            });
        }

        public static Label MakeLabel(string text, Color color, bool bold = false, int size = 12) {
            Label label = new Label(text);
            label.pickingMode = PickingMode.Ignore;
            label.style.color = color;
            label.style.fontSize = size;
            label.style.unityFontStyleAndWeight = bold ? FontStyle.Bold : FontStyle.Normal;
            label.style.overflow = Overflow.Hidden;
            label.style.textOverflow = TextOverflow.Ellipsis;
            label.style.whiteSpace = WhiteSpace.NoWrap;
            SetMargin(label, 0, 0, 0, 0);
            return label;
        }
    }

    [CustomPropertyDrawer(typeof(AudioID))]
    public class AudioIdPropertyDrawer : PropertyDrawer {

        public override VisualElement CreatePropertyGUI(SerializedProperty property) {

            VisualElement root = new VisualElement();

            root.style.flexDirection = FlexDirection.Row;
            root.style.justifyContent = Justify.SpaceBetween;
            root.style.alignItems = Align.Center;
            root.style.minHeight = 22;
            root.style.marginLeft = 3f;

            SerializedProperty idProperty = property.FindPropertyRelative(AudioID.NameOf.ID);

            SerializedProperty groupProperty = property.FindPropertyRelative(AudioID.NameOf.GroupName);

            Label label = new Label(property.displayName);

            label.tooltip = property.propertyPath;
            label.style.width = Length.Percent(35);
            label.style.unityTextAlign = TextAnchor.MiddleLeft;
            label.style.overflow = Overflow.Hidden;
            label.style.textOverflow = TextOverflow.Ellipsis;

            root.Add(label);

            // Field-style button
            Button selectButton = new Button();
            AudioPickerStyle.MakeFlat(selectButton, AudioPickerStyle.FieldBackground, AudioPickerStyle.FieldHover, true, AudioPickerStyle.Border);
            AudioPickerStyle.SetBorder(selectButton, 1, AudioPickerStyle.Border);
            AudioPickerStyle.SetPadding(selectButton, 2, 8);
            selectButton.style.width = Length.Percent(62.5f);
            selectButton.style.height = 20;
            selectButton.style.justifyContent = Justify.SpaceBetween;

            Label valueLabel = AudioPickerStyle.MakeLabel(string.Empty, AudioPickerStyle.TextPrimary, true);
            valueLabel.style.flexGrow = 1;
            valueLabel.style.flexShrink = 1;
            valueLabel.style.unityTextAlign = TextAnchor.MiddleLeft;

            Label chevron = AudioPickerStyle.MakeLabel("\u25BE", AudioPickerStyle.TextMuted);
            chevron.style.marginLeft = 6;

            selectButton.Add(valueLabel);
            selectButton.Add(chevron);

            RefreshButton();

            selectButton.clicked += () => {

                Rect buttonRect = selectButton.worldBound;

                Vector2 screenPosition = GUIUtility.GUIToScreenPoint(
                    new Vector2(buttonRect.xMin, buttonRect.yMax)
                );

                Rect screenRect = new Rect(
                    screenPosition.x,
                    screenPosition.y,
                    buttonRect.width,
                    buttonRect.height
                );

                AudioPickerPopup window = AudioPickerPopup.Show(screenRect, property.serializedObject.targetObjects, property.propertyPath);

                window.OnNewAudioIdSelected += ShowUpdateButtonValue;
            };

            root.Add(selectButton);

            return root;

            void RefreshButton() {
                string groupName = groupProperty.stringValue;
                string id = idProperty.stringValue;

                valueLabel.text = $"{groupName}/{id}";
                selectButton.tooltip = valueLabel.text;
            }

            void ShowUpdateButtonValue(string audioGroup, string audioID) {
                string groupName = audioGroup;
                string id = audioID;

                valueLabel.text = $"{groupName}/{id}";
                selectButton.tooltip = valueLabel.text;
            }
        }
    }


    /// <summary>
    /// ============================================================================
    /// </summary>

    public class AudioPickerPopup : EditorWindow {

        private class AudioEntry {
            public string groupName;
            public string audioID;
        }

        private UnityEngine.Object[] targetObjects;
        private string propertyPath;

        private readonly List<AudioEntry> audioEntries = new();

        private ToolbarSearchField searchField;
        private VisualElement menuContainer;

        private string currentGroup;

        public event Action<string, string> OnNewAudioIdSelected;

        // Open popup
        public static AudioPickerPopup Show(Rect position, UnityEngine.Object[] targetObjects, string propertyPath) {

            AudioPickerPopup window = CreateInstance<AudioPickerPopup>();

            window.targetObjects = targetObjects;
            window.propertyPath = propertyPath;

            window.titleContent = new GUIContent("Select Audio");

            window.ShowAsDropDown(position, new Vector2(200, 250));

            return window;
        }

        // Create UI
        public void CreateGUI() {

            LoadAudioEntries();

            VisualElement root = rootVisualElement;

            root.style.flexDirection = FlexDirection.Column;
            AudioPickerStyle.SetPadding(root, 8, 8);
            AudioPickerStyle.SetBorder(root, 1, AudioPickerStyle.Border);

            // Search
            searchField = new ToolbarSearchField();
            searchField.style.width = Length.Auto();
            searchField.style.flexShrink = 0;
            AudioPickerStyle.SetMargin(searchField, 0, 8, 0, 0);
            AudioPickerStyle.SetRadius(searchField, 4);

            searchField.RegisterValueChangedCallback(evt => {
                currentGroup = null;

                RefreshMenu(evt.newValue);
            });

            root.Add(searchField);

            // Scroll view
            ScrollView scrollView = new ScrollView();

            scrollView.style.flexGrow = 1;

            menuContainer = scrollView.contentContainer;
            menuContainer.style.paddingRight = 2;

            root.Add(scrollView);

            RefreshMenu(string.Empty);

            searchField.Focus();
        }

        // Just fils the audio entry list using audio configs
        private void LoadAudioEntries() {

            AudioDatabase database = Resources.Load<AudioDatabase>(nameof(AudioDatabase));

            if(database == null) {
                Debug.LogError("AudioDatabase not found in Resources.");
                return;
            }

            if(database.audioGroups == null)
                return;

            foreach(UnknownAudioGroup group in database.audioGroups) {

                if(group == null || group.audioConfigs == null)
                    continue;

                foreach(UnknownAudioConfig config in group.audioConfigs) {

                    if(config == null)
                        continue;

                    AudioEntry entry = new AudioEntry { groupName = group.groupName, audioID = config.GetAudioID().id };

                    audioEntries.Add(entry);
                }
            }
        }

        // Refresh menu
        private void RefreshMenu(string search) {
            menuContainer.Clear();

            search = search.Trim();

            bool searching = !string.IsNullOrEmpty(search);

            // Show selected group submenu
            if(!searching && !string.IsNullOrEmpty(currentGroup)) {

                ShowSubMenu(currentGroup);
                return;
            }

            // Search results
            if(searching) {

                var results = audioEntries
                    .Where(x => x.groupName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 || x.audioID.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();

                foreach(AudioEntry entry in results) {

                    CreateEntryButton(entry, true);
                }

                if(results.Count == 0)
                    CreateEmptyLabel();

                return;
            }

            // Main category menu
            var groups = audioEntries
                .Select(x => x.groupName)
                .Distinct()
                .ToList();

            foreach(string group in groups) {

                CreateGroupButton(group);
            }

            if(groups.Count == 0)
                CreateEmptyLabel();
        }

        // Category button
        private void CreateGroupButton(string groupName) {

            Button button = new Button();
            AudioPickerStyle.MakeFlat(button, Color.clear, AudioPickerStyle.RowHover);
            AudioPickerStyle.SetPadding(button, 0, 8);
            AudioPickerStyle.SetMargin(button, 0, 1, 0, 0);
            button.style.height = 28;
            button.style.justifyContent = Justify.SpaceBetween;

            Label name = AudioPickerStyle.MakeLabel(groupName, AudioPickerStyle.TextPrimary, true, 12);
            name.style.flexGrow = 1;
            name.style.unityTextAlign = TextAnchor.MiddleLeft;

            int count = audioEntries.Count(x => x.groupName == groupName);
            Label countLabel = AudioPickerStyle.MakeLabel(count.ToString(), AudioPickerStyle.TextMuted, false, 11);
            countLabel.style.marginRight = 8;

            Label arrow = AudioPickerStyle.MakeLabel("\u203A", AudioPickerStyle.TextMuted, true, 14);

            button.Add(name);
            button.Add(countLabel);
            button.Add(arrow);

            button.clicked += () => {

                currentGroup = groupName;

                RefreshMenu(searchField.value);
            };

            menuContainer.Add(button);
        }

        // Submenu
        private void ShowSubMenu(string groupName) {

            menuContainer.Clear();

            // Back button
            Button backButton = new Button();
            AudioPickerStyle.MakeFlat(backButton, Color.clear, AudioPickerStyle.RowHover);
            AudioPickerStyle.SetPadding(backButton, 0, 8);
            backButton.style.height = 28;

            Label backArrow = AudioPickerStyle.MakeLabel("\u2039", AudioPickerStyle.Accent, true, 16);
            backArrow.style.marginRight = 8;

            Label backLabel = AudioPickerStyle.MakeLabel(groupName, AudioPickerStyle.TextPrimary, true, 12);
            backLabel.style.unityTextAlign = TextAnchor.MiddleLeft;

            backButton.Add(backArrow);
            backButton.Add(backLabel);

            backButton.clicked += () => {

                currentGroup = null;

                RefreshMenu(string.Empty);
            };

            menuContainer.Add(backButton);

            // Separator
            VisualElement separator = new VisualElement();

            separator.style.height = 1;
            AudioPickerStyle.SetMargin(separator, 4, 4, 8, 8);
            separator.style.backgroundColor = AudioPickerStyle.Divider;

            menuContainer.Add(separator);

            // Audio entries
            var entries = audioEntries
                .Where(x => x.groupName == groupName)
                .ToList();

            foreach(AudioEntry entry in entries) {

                CreateEntryButton(entry, false);
            }
        }

        // Audio entry button
        private void CreateEntryButton(AudioEntry entry, bool showGroup) {

            Button button = new Button();
            AudioPickerStyle.MakeFlat(button, Color.clear, AudioPickerStyle.RowHover);
            AudioPickerStyle.SetPadding(button, 0, 8);
            AudioPickerStyle.SetMargin(button, 0, 1, 0, 0);
            button.style.height = 26;
            button.style.justifyContent = Justify.SpaceBetween;
            button.tooltip = $"{entry.groupName}/{entry.audioID}";

            Label id = AudioPickerStyle.MakeLabel(entry.audioID, AudioPickerStyle.TextPrimary, false, 12);
            id.style.flexGrow = 1;
            id.style.flexShrink = 1;
            id.style.unityTextAlign = TextAnchor.MiddleLeft;
            button.Add(id);

            // In search results, show which group each entry belongs to
            if(showGroup) {
                Label group = AudioPickerStyle.MakeLabel(entry.groupName, AudioPickerStyle.TextMuted, false, 11);
                group.style.marginLeft = 8;
                group.style.flexShrink = 0;
                button.Add(group);
            }

            button.clicked += () => {

                SelectAudio(entry.groupName, entry.audioID);
            };

            menuContainer.Add(button);
        }

        // Empty results
        private void CreateEmptyLabel() {

            Label label = new Label("No audio entries found.");

            label.style.paddingTop = 24;
            label.style.paddingBottom = 24;
            label.style.unityTextAlign = TextAnchor.MiddleCenter;
            label.style.color = AudioPickerStyle.TextMuted;
            label.style.unityFontStyleAndWeight = FontStyle.Italic;

            menuContainer.Add(label);
        }

        // Select audio
        private void SelectAudio(string groupName, string audioID) {

            if(targetObjects == null || targetObjects.Length == 0)
                return;

            SerializedObject serializedObject = new SerializedObject(targetObjects);

            serializedObject.Update();

            SerializedProperty property = serializedObject.FindProperty(propertyPath);

            if(property == null) {
                Debug.LogError("AudioID property not found: " + propertyPath);
                return;
            }

            SerializedProperty groupProperty = property.FindPropertyRelative(AudioID.NameOf.GroupName);

            SerializedProperty idProperty = property.FindPropertyRelative(AudioID.NameOf.ID);

            if(groupProperty == null || idProperty == null) {
                Debug.LogError("AudioID fields not found.");
                return;
            }

            groupProperty.stringValue = groupName;
            idProperty.stringValue = audioID;

            serializedObject.ApplyModifiedProperties();

            OnNewAudioIdSelected?.Invoke(groupName, audioID);

            Close();
        }
    }

}