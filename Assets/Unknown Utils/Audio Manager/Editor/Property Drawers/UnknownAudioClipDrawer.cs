using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Unknown.Manager.AudioManager {

    public class UnknownAudioClipDrawer {

        EditorAudioPlayer audioPlayer;
        SerializedProperty clipProperty;
        SerializedProperty volumeProperty;
        SerializedProperty startPos;
        SerializedProperty endPos;
        Button playButton;

        private bool isAudioConfigPlaying = false;

        public VisualElement CreateGUI(SerializedProperty property) {
            clipProperty = property.FindPropertyRelative(UnknownAudioClip.NameOf.AudioClip);
            volumeProperty = property.FindPropertyRelative(UnknownAudioClip.NameOf.Volume);

            startPos = property.FindPropertyRelative(UnknownAudioClip.NameOf.StartPosition);
            endPos = property.FindPropertyRelative(UnknownAudioClip.NameOf.EndPosition);

            // ============== root ==============
            var root = new VisualElement();
            root.style.flexDirection = FlexDirection.Row;
            root.style.alignItems = Align.Center;
            root.style.flexGrow = 1;
            root.style.minWidth = 0;                 // lets children shrink instead of overflowing

            // ============== play / stop button ==============
            playButton = new Button { text = "▶", tooltip = "Play / stop preview" };
            playButton.style.width = 24;
            playButton.style.height = 20;
            playButton.style.flexShrink = 0;
            playButton.style.marginLeft = 0;
            playButton.style.marginRight = 4;
            playButton.style.paddingLeft = playButton.style.paddingRight = 0;

            root.Add(playButton);

            // ============== Initializations ==============
            isAudioConfigPlaying = false;

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

            // button methods 
            playButton.clicked += PlayAudio;

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

            return root;
        }

        public void PlayAudio() {
            if(audioPlayer == null)
                audioPlayer = new EditorAudioPlayer();

            if(audioPlayer.IsPlaying()) {
                audioPlayer.Destroy();
                playButton.text = "▶";
            } else {

                audioPlayer.Create()
                    .SetVolume(volumeProperty.floatValue)
                    .SetClip(clipProperty.objectReferenceValue as AudioClip)
                    .Play(startPos.floatValue, endPos.floatValue)
                    .OnComplete(() => {
                        playButton.text = "▶";
                    });
                playButton.text = "■";
            }
        }

        public EditorAudioPlayer PlayOrStopUsingAudioConfig(float volume, float pitch) {
            if(isAudioConfigPlaying) {
                audioPlayer?.Destroy();
                audioPlayer = null;
                isAudioConfigPlaying = false;
                return null;
            }

            if(audioPlayer == null)
                audioPlayer = new EditorAudioPlayer();

            if(audioPlayer.IsPlaying()) {
                audioPlayer.Stop();
                audioPlayer.Destroy();
                audioPlayer = null;
                playButton.text = "▶";

            } else {
                isAudioConfigPlaying = true;
                audioPlayer.Create()
                    .SetClip(clipProperty.objectReferenceValue as AudioClip)
                    .SetVolume(volume)
                    .SetPitch(pitch)
                    .Play(startPos.floatValue, endPos.floatValue)
                    .OnComplete(() => {
                        playButton.text = "▶";
                        isAudioConfigPlaying = false;
                    })
                    .OnStop(() => {
                        playButton.text = "▶";
                        isAudioConfigPlaying = false;
                    });
                playButton.text = "■";
            }

            return audioPlayer;
        }

    }
}