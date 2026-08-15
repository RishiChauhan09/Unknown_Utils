using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;
using Unknown.Extensions;

namespace Unknown.UI {

    public class TabButton : MonoBehaviour {

        [SerializeField] private Button tabButton;
        [SerializeField] private RectTransform iconRect;
        [SerializeField] private RectTransform textRect;
        [SerializeField] private RectTransform hightlightImage;
        [SerializeField] private bool isLocked;

        [Header("Info Box References")]
        [SerializeField] private RectTransform infoImageRect;

        public event Action<TabButton> Clicked;

        private void Awake() {
            tabButton.onClick.AddListener(() => {
                if(!isLocked)
                    Clicked?.Invoke(this);
                else
                    ShowIsLockedInfo();
            });
        }

        public void SetSelected() {
            hightlightImage.DOAnchorPos(Vector2.zero, AnimationExtensions.NORMAL).SetEase(Ease.OutCirc);

            float iconFinalY = iconRect.rect.height * .5f;
            iconRect.DOAnchorPosY(iconFinalY, AnimationExtensions.NORMAL);

            textRect.localScale = Vector2.zero;
            textRect.gameObject.SetActive(true);
            textRect.DOScale(Vector2.one, AnimationExtensions.NORMAL).SetEase(Ease.OutBack);
        }

        public void SetUnSelected() {
            float highlightFinalY = -hightlightImage.rect.height;
            hightlightImage.DOAnchorPosY(highlightFinalY, AnimationExtensions.NORMAL).SetEase(Ease.OutCirc);

            iconRect.DOAnchorPosY(0f, AnimationExtensions.NORMAL);

            textRect.DOScale(Vector2.zero, AnimationExtensions.NORMAL).SetEase(Ease.OutCirc);
        }

        private void ShowIsLockedInfo() {
            infoImageRect.localScale = Vector2.zero;
            infoImageRect.gameObject.SetActive(true);
            infoImageRect.DOScale(Vector2.one, AnimationExtensions.NORMAL).SetEase(Ease.OutBack);

            // make sure sub to this when is locked is shown 
            //PlayerInputManager.OnPointerOverUI += OnPointerDown;
        }

        private void OnPointerDown(Vector2 pos) {
            // make sure you get player input manager and then check if it's clicked on it or not

            //PlayerInputManager playerInputManager = ServiceLocator.Get<PlayerInputManager>();
            //if(!playerInputManager.IsPointerOver(gameObject, pos)) {
            //HideIsLockedInfo();
            //}
        }

        private void HideIsLockedInfo() {
            //PlayerInputManager.OnPointerOverUI -= OnPointerDown;

            infoImageRect.DOScale(Vector2.zero, AnimationExtensions.NORMAL).SetEase(Ease.InBack);
        }

    }
}