using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unknown.UI {

    public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

        [SerializeField] private ButtonAnimationSO buttonAnimationSettings;

        [SerializeField] protected Image buttonImage;
        [SerializeField] protected Button button;

        protected Color startColor;

        private void Start() {
            if(buttonImage == null)
                buttonImage = GetComponent<Image>();
            if(button == null)
                button = GetComponent<Button>();
            startColor = buttonImage.color;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            if(!buttonAnimationSettings.isHoverColorChangeOn) return;
            buttonImage.DOColor(buttonAnimationSettings.hoverColor, buttonAnimationSettings.hoverAnimationTime).SetUpdate(true);
        }

        public void OnPointerExit(PointerEventData eventData) {
            if(!buttonAnimationSettings.isHoverColorChangeOn) return;
            buttonImage.DOColor(startColor, buttonAnimationSettings.hoverAnimationTime).SetUpdate(true);
        }

        public void OnPointerDown(PointerEventData eventData) {
            if(!buttonAnimationSettings.onClickScaleAnimation) return;
            if(button.enabled) {
                transform.DOScale(Vector3.one * buttonAnimationSettings.buttonScaleOnClick, buttonAnimationSettings.buttonClickAnimationTime).SetUpdate(true);
            }
        }

        public void OnPointerUp(PointerEventData eventData) {
            if(!buttonAnimationSettings.onClickScaleAnimation) return;
            if(button.enabled)
                transform.DOScale(Vector3.one, buttonAnimationSettings.buttonClickAnimationTime).SetUpdate(true);
        }

        private void OnDisable() {
            if(!buttonAnimationSettings.isHoverColorChangeOn) return;
            buttonImage.DOColor(startColor, buttonAnimationSettings.hoverAnimationTime).SetUpdate(true);
        }

        public void SetSelected() {
            if(!button.enabled)
                return;
            buttonImage.DOColor(buttonAnimationSettings.selectedColor, buttonAnimationSettings.hoverAnimationTime).SetUpdate(true);
        }

        public void SetUnSelected() {
            if(!button.enabled)
                return;
            buttonImage.DOColor(startColor, buttonAnimationSettings.hoverAnimationTime).SetUpdate(true);
        }
    }
}