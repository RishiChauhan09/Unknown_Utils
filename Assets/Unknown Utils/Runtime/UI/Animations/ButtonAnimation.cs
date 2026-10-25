using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Unknown.UI {

    [RequireComponent(typeof(Button))]
    public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler {

        [SerializeField] private ButtonAnimationSettings buttonAnimationSettings;
        
        protected Button button;
        protected Graphic buttonGraphic;
        protected Color startColor;

        private void Awake() {
            if(button == null)
                button = GetComponent<Button>();

            if(buttonGraphic == null)
                buttonGraphic = button.targetGraphic;

            startColor = buttonGraphic.color;
        }

        private void OnEnable() {
            if(buttonGraphic != null)
                startColor = buttonGraphic.color;
        }

        public void OnPointerEnter(PointerEventData eventData) {
            if(!buttonAnimationSettings.isHoverColorChangeOn) return;
            buttonGraphic.DOColor(buttonAnimationSettings.hoverColor, buttonAnimationSettings.hoverAnimationTime).SetUpdate(true);
        }

        public void OnPointerExit(PointerEventData eventData) {
            if(!buttonAnimationSettings.isHoverColorChangeOn) return;
            buttonGraphic.DOColor(startColor, buttonAnimationSettings.hoverAnimationTime).SetUpdate(true);
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
            buttonGraphic.DOColor(startColor, buttonAnimationSettings.hoverAnimationTime).SetUpdate(true);
        }

        public void SetSelected() {
            if(!button.enabled)
                return;
            buttonGraphic.DOColor(buttonAnimationSettings.selectedColor, buttonAnimationSettings.hoverAnimationTime).SetUpdate(true);
        }

        public void SetUnSelected() {
            if(!button.enabled)
                return;
            buttonGraphic.DOColor(startColor, buttonAnimationSettings.hoverAnimationTime).SetUpdate(true);
        }
    }

    [System.Serializable]
    public class ButtonAnimationSettings {
        public Color hoverColor = Color.red;
        public Color selectedColor = Color.red;

        public bool isHoverColorChangeOn = false;
        public bool onClickScaleAnimation = true;

        public float hoverAnimationTime = .15f;
        public float buttonScaleOnClick = .9f;
        public float buttonClickAnimationTime = .1f;
    }
}