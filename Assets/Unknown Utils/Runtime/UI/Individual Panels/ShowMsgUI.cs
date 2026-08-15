using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Unknown.UI {
    public class ShowMsgUI : MonoBehaviour {

        [Header("References")]
        [SerializeField] private TMP_Text text;

        [SerializeField] private RectTransform startingPos;
        [SerializeField] private RectTransform endPos;

        [SerializeField] private float animationDuration;

        /// <summary>
        /// spawn screen msg but, reuses the single text
        /// </summary>
        public void SpawnScreenMsg(string msg) {
            CanvasGroup cg = text.GetComponent<CanvasGroup>();
            cg.alpha = 1f;

            // setting text
            text.text = msg;

            RectTransform textRect = text.GetComponent<RectTransform>();
            textRect.DOKill();
            cg.DOKill();
            textRect.anchoredPosition = startingPos.anchoredPosition;

            text.gameObject.SetActive(true);

            textRect.DOAnchorPos(endPos.anchoredPosition, animationDuration);
            cg.DOFade(0, animationDuration).SetEase(Ease.InExpo).OnComplete(() => {
                textRect.gameObject.SetActive(false);
                cg.alpha = 1f;
            });
        }
    }
}