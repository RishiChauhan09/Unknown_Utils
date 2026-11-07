using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unknown.Manager.AudioManager {

    [CustomPropertyDrawer(typeof(UnknownAudioClip))]
    public class UnknownAudioClipDrawer : PropertyDrawer {

        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            SerializedProperty clipProperty = property.FindPropertyRelative(UnknownAudioClip.NameOf.AudioClip);
            SerializedProperty volumeProperty = property.FindPropertyRelative(UnknownAudioClip.NameOf.Volume);

            SerializedProperty startPos = property.FindPropertyRelative(UnknownAudioClip.NameOf.StartPosition);
            SerializedProperty endPos = property.FindPropertyRelative(UnknownAudioClip.NameOf.EndPosition);

            // ============== root ==============
            var root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;
            root.style.alignItems = Align.Center;
            root.style.flexGrow = 1;
            root.style.minWidth = 0;                 // lets children shrink instead of overflowing

            // ============== play / stop button ==============
            bool isPlayingThis = false;
            double stopAtTime = 0;

            var playButton = new Button { text = "▶", tooltip = "Play / stop preview" };
            playButton.style.width = 24;
            playButton.style.height = 20;
            playButton.style.flexShrink = 0;
            playButton.style.marginLeft = 0;
            playButton.style.marginRight = 4;
            playButton.style.paddingLeft = playButton.style.paddingRight = 0;

            playButton.clicked += () => {
                if(isPlayingThis) {
                    AudioPreview.Stop();
                    isPlayingThis = false;

                } else if(clipProperty.objectReferenceValue is AudioClip clip) {
                    AudioPreview.Play(clip, Mathf.RoundToInt(clip.samples * startPos.floatValue));
                    float length = (endPos.floatValue - startPos.floatValue) * clip.length;
                    stopAtTime = EditorApplication.timeSinceStartup + length;
                    isPlayingThis = true;
                }
                playButton.text = isPlayingThis ? "■" : "▶";
            };
            root.Add(playButton);

            // ============== clip field ==============
            var clipField = new ObjectField {
                objectType = typeof(AudioClip),
                allowSceneObjects = false,
                label = string.Empty
            };
            clipField.BindProperty(clipProperty);
            clipField.style.flexGrow = 1;
            clipField.style.flexShrink = 1;          // shrink first when space runs out
            clipField.style.flexBasis = 0;
            clipField.style.minWidth = 40;
            clipField.style.marginLeft = 0;
            clipField.style.marginRight = 0;
            root.Add(clipField);

            // ============== volume slider ==============
            var volumeSlider = new Slider(0f, 1f) {
                showInputField = true,
                tooltip = "Volume"
            };
            volumeSlider.BindProperty(volumeProperty);
            volumeSlider.style.flexGrow = 0;
            volumeSlider.style.flexShrink = 1;
            volumeSlider.style.width = 140;
            volumeSlider.style.minWidth = 60;
            volumeSlider.style.marginLeft = 6;
            volumeSlider.style.marginRight = 0;
            volumeSlider.fill = true;
            root.Add(volumeSlider);

            // ============== enable/disable play button based on clip ==============
            void RefreshPlayEnabled() => playButton.SetEnabled(clipProperty.objectReferenceValue != null);
            clipField.RegisterValueChangedCallback(_ => RefreshPlayEnabled());
            RefreshPlayEnabled();

            // ============== reset the button when the preview ends on its own ==============
            playButton.schedule.Execute(() => {
                if(!isPlayingThis) return;

                if(EditorApplication.timeSinceStartup >= stopAtTime) {
                    AudioPreview.Stop();
                    isPlayingThis = false;
                    playButton.text = "▶";
                } else if(!AudioPreview.IsPlaying()) {
                    isPlayingThis = false;
                    playButton.text = "▶";
                }
            }).Every(20);

            // ============== responsiveness ==============
            root.RegisterCallback<GeometryChangedEvent>(evt => {
                float w = evt.newRect.width;

                // very narrow: drop the slider entirely
                volumeSlider.style.display = w < 180 ? DisplayStyle.None : DisplayStyle.Flex;

                // narrow: keep the slider but hide its numeric input
                volumeSlider.showInputField = w >= 280;

                // wide: give the slider more room
                volumeSlider.style.width = w >= 420 ? 180 : 120;
            });

            // ============== cleanup ==============
            root.RegisterCallback<DetachFromPanelEvent>(_ => {
                if(isPlayingThis) {
                    AudioPreview.Stop();
                    isPlayingThis = false;
                }
            });

            return root;
        }

    }
}