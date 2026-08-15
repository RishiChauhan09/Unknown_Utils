using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Unknown.UI {

    public class SlidingToggleUI : MonoBehaviour {

        [SerializeField] private RectTransform selfRect;
        [SerializeField] private Image bgImage;
        [SerializeField] private TMP_Text onOffText;

        [SerializeField] private Button toggleButton;
        [SerializeField] private RectTransform movingImage;
        [SerializeField] private float padding = 15f;
        [SerializeField] private bool playAnimation;

        [SerializeField] private Color activeBgColor;
        [SerializeField] private Color inActiveBgColor = Color.gray;


        /// <summary>
        /// Right side is on
        /// </summary>
        public bool IsRightSide { get; private set; }
        private Action<bool> MethodToRun;

        private void Awake() {
            toggleButton.onClick.AddListener(Toggle);
        }

        private void OnEnable() {
            UpdateUI();
        }

        /// <summary>
        /// For initializing the sliding toggle.
        /// This method to run method is called when sliding is changed
        /// </summary>
        public void Init(bool isRight, Action<bool> MethodToRun, bool playAnimation = true) {
            IsRightSide = isRight;
            this.playAnimation = playAnimation;
            this.MethodToRun = MethodToRun;
            UpdateUI();
        }

        /// <summary>
        /// Updating toggle UI
        /// </summary>
        private void UpdateUI() {
            float x = (selfRect.rect.width - movingImage.rect.width) * 0.5f - padding;
            Vector2 finalPos = new Vector2(
                IsRightSide ? x : -x,
                0f);

            if(IsRightSide && movingImage.anchoredPosition.x < 0) {

                if(playAnimation) {
                    movingImage.DOAnchorPos(finalPos, .15f);
                } else {
                    movingImage.anchoredPosition = finalPos;
                }
                bgImage.color = activeBgColor;
                onOffText.text = "ON";

            } else if(!IsRightSide && movingImage.anchoredPosition.x >= 0) {

                if(playAnimation) {
                    movingImage.DOAnchorPos(finalPos, .15f);
                } else {
                    movingImage.anchoredPosition = finalPos;
                }
                bgImage.color = inActiveBgColor;
                onOffText.text = "OFF";

            }

            MethodToRun?.Invoke(IsRightSide);
        }

        /// <summary>
        /// Main Button Function
        /// </summary>
        private void Toggle() {
            IsRightSide = !IsRightSide;
            UpdateUI();
        }

    }
}