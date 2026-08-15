using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Unknown.Extensions;

namespace Unknown.Utils { 
    public static class CommonAnimationUtilsUI {

        /// <summary>
        /// shows the panel with bg bg fades in and rect scales
        /// </summary>
        public static void ShowPanelWithBG(Image alphaBgImage, RectTransform rect) {
            float startingAlpha = alphaBgImage.color.a;
            alphaBgImage.color = CommonUtils.ChangeColorAlpha(alphaBgImage.color, 0);
            alphaBgImage.gameObject.SetActive(true);
            alphaBgImage.DOFade(startingAlpha, AnimationExtensions.NORMAL);

            rect.ShowPanel(AnimationExtensions.NORMAL);
        }

        /// <summary>
        /// hides the panel with bg bg faces out and rect scales
        /// </summary>
        public static void HidePanelWithBG(Image alphaBgImage, RectTransform rect) {
            float panelAlpha = alphaBgImage.color.a;

            alphaBgImage.DOFade(0f, AnimationExtensions.NORMAL).OnComplete(() => {
                alphaBgImage.gameObject.SetActive(false);
                alphaBgImage.color = CommonUtils.ChangeColorAlpha(alphaBgImage.color, panelAlpha);
            });

            rect.HidePanel(AnimationExtensions.NORMAL);
        }

    }
}