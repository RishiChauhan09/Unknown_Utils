using DG.Tweening;
using TMPro;

namespace Unknown.Extensions {
    public static class TMPExtensions {

        /// <summary>
        /// shows the number animation like for 1-10 animation doing animation from 1 - 10
        /// </summary>
        public static void NumberAnimation(this TMP_Text text, float startValue, float endValue, float animationTime = 3, string extraString = "") {
            float currentValue = startValue;

            DOTween.To(() => currentValue,
                (x) => {
                    text.text = x.ToString() + extraString;
                },
                endValue,
                animationTime);
        }

        public static void VisibilityAnimation(this TMP_Text text, float animationTime) {
            text.maxVisibleCharacters = 0;
            text.ForceMeshUpdate();

            DOTween.To(() => text.maxVisibleCharacters,
                (x) => {
                    text.maxVisibleCharacters = x;
                },
                text.textInfo.characterCount,
                animationTime);
        }

    }
}