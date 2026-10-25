using DG.Tweening;
using UnityEngine;

namespace Unknown.Extensions {

    public static class UIAnimationExtensions {

        #region slide in methods

        /// <summary>
        /// Slide In animation
        /// </summary>
        public static Tween SlideInUI(this RectTransform rect, Vector2 direction, float duration = .25f, Ease ease = Ease.OutBack) {
            Vector2 startingPos = Vector2.zero;

            if(direction == Vector2.down) {
                startingPos = new Vector2(0f, rect.rect.height);

            } else if(direction == Vector2.up) {
                startingPos = new Vector2(0f, -rect.rect.height);

            } else if(direction == Vector2.left) {
                startingPos = new Vector2(rect.rect.width, 0f);

            } else if(direction == Vector2.right) {
                startingPos = new Vector2(-rect.rect.width, 0f);
            }

            rect.anchoredPosition = startingPos;
            rect.gameObject.SetActive(true);

            return rect.DOAnchorPos(Vector2.zero, duration).SetEase(ease);
        }

        /// <summary>
        /// Slide Out animation
        /// </summary>
        public static Tween SlideOutUI(this RectTransform rect, Vector2 direction, float duration = .25f, Ease ease = Ease.InBack, bool setActive = false) {
            Vector2 finalPos = Vector2.zero;

            if(direction == Vector2.down) {
                finalPos = new Vector2(0f, -rect.rect.height);

            } else if(direction == Vector2.up) {
                finalPos = new Vector2(0f, rect.rect.height);

            } else if(direction == Vector2.left) {
                finalPos = new Vector2(-rect.rect.width, 0f);

            } else if(direction == Vector2.right) {
                finalPos = new Vector2(rect.rect.width, 0f);
            }

            return rect.DOAnchorPos(finalPos, duration).SetEase(ease).OnComplete(() => {
                rect.gameObject.SetActive(setActive);
            });
        }

        /// <summary>
        /// Slide In Screen animation
        /// </summary>
        public static Tween ScreenSlideInUI(this RectTransform rect, Vector2 direction, float duration = .25f, Ease ease = Ease.OutBack) {
            Vector2 startingPos = Vector2.zero;

            if(direction == Vector2.down) {
                startingPos = new Vector2(0f, Screen.height);

            } else if(direction == Vector2.up) {
                startingPos = new Vector2(0f, -Screen.height);

            } else if(direction == Vector2.left) {
                startingPos = new Vector2(Screen.width, 0f);

            } else if(direction == Vector2.right) {
                startingPos = new Vector2(-Screen.width, 0f);
            }

            rect.anchoredPosition = startingPos;
            rect.gameObject.SetActive(true);

            return rect.DOAnchorPos(Vector2.zero, duration).SetEase(ease);
        }

        /// <summary>
        /// Slide Out Screen animation
        /// </summary>
        public static Tween ScreenSlideOutUI(this RectTransform rect, Vector2 direction, float duration = .25f, Ease ease = Ease.InBack, bool setActive = false) {
            Vector2 finalPos = Vector2.zero;

            if(direction == Vector2.down) {
                finalPos = new Vector2(0f, -Screen.height);

            } else if(direction == Vector2.up) {
                finalPos = new Vector2(0f, Screen.height);

            } else if(direction == Vector2.left) {
                finalPos = new Vector2(-Screen.width, 0f);

            } else if(direction == Vector2.right) {
                finalPos = new Vector2(Screen.width, 0f);
            }

            return rect.DOAnchorPos(finalPos, duration).SetEase(ease).OnComplete(() => {
                rect.gameObject.SetActive(setActive);
            });
        }

        /// <summary>
        /// Slide In animation realtive to screen with to final pos
        /// </summary>
        public static Tween SlideInUI(this RectTransform rect, Vector2 finalPos, Vector2 direction, float duration = .25f, Ease ease = Ease.OutBack) {
            Vector2 startingPos = Vector2.zero;

            if(direction == Vector2.down) {
                startingPos = new Vector2(0f, Screen.height);

            } else if(direction == Vector2.up) {
                startingPos = new Vector2(0f, -Screen.height);

            } else if(direction == Vector2.left) {
                startingPos = new Vector2(Screen.width, 0f);

            } else if(direction == Vector2.right) {
                startingPos = new Vector2(-Screen.width, 0f);
            }

            rect.anchoredPosition = startingPos;
            rect.gameObject.SetActive(true);

            return rect.DOAnchorPos(finalPos, duration).SetEase(ease);
        }

        #endregion

        #region Individual Panel

        /// <summary>
        /// This is current game universal show panel animation 
        /// </summary>
        public static Tween ShowPanel(this Transform t, float duration = .25f) {
            t.localScale = Vector3.zero;
            t.gameObject.SetActive(true);
            return t.DOScale(1, duration).SetEase(Ease.OutBack);
        }

        /// <summary>
        /// This is current game universal hide panel animation 
        /// </summary>
        public static Tween HidePanel(this Transform t, float duration = .25f) {
            return t.DOScale(0f, duration).OnComplete(() => {
                t.gameObject.SetActive(false);
            }).SetEase(Ease.InBack);
        }

        #endregion

    }
}