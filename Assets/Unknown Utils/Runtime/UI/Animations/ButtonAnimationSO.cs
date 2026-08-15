using UnityEngine;

namespace Unknown.UI {

    [CreateAssetMenu(fileName = "ButtonAnimationSO", menuName = "Scriptable Objects/ButtonAnimationSO")]
    public class ButtonAnimationSO : ScriptableObject {

        public Color hoverColor = Color.red;
        public Color selectedColor = Color.red;

        public bool isHoverColorChangeOn = false;
        public bool onClickScaleAnimation = true;

        [Header("Button Animation Parameters")]
        public float hoverAnimationTime = .15f;
        public float buttonScaleOnClick = .9f;
        public float buttonClickAnimationTime = .1f;

    }
}