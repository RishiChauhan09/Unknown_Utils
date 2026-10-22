using DG.Tweening;
using UnityEngine;

namespace Unknown.Extensions {
    public static class AnimationExtensions {

        public const float FAST = .15F;
        public const float NORMAL = .25f;
        public const float SLOW = .4f;
        public const float VERYSLOW = .65f;

        #region Squash Animation

        /// <summary>
        /// Squash animation for object
        /// </summary>
        public static Tween DOSquash(this Transform t, float height = .8f, float length = 1.2f, float duration = .25f) {

            DOTween.Kill(t, "Squash");

            Vector3 squashScale = new Vector3(length, height, length);

            Sequence seq = DOTween.Sequence().SetId("Squash");

            seq.Append(
                t.DOScale(squashScale, duration * 0.5f)
                .SetEase(Ease.InQuad)
            );

            seq.Append(
                t.DOScale(Vector3.one, duration * 0.5f)
                .SetEase(Ease.OutBack)
            );

            return seq;
        }

        /// <summary>
        /// Blended squash for more good feel.
        /// Starts scaling before going to final state. 
        /// Does not use append in sequence
        /// </summary>
        public static Tween DOBlendSquash(this Transform t, float height = 0.8f, float width = 1.2f, float duration = 0.25f, float overlap = 0.25f) {
            DOTween.Kill(t, "Squash");

            Vector3 squashScale = new Vector3(width, height, 1f);

            float squashTime = duration * 0.5f;
            float returnTime = duration * 0.5f;

            Sequence seq = DOTween.Sequence()
                .SetTarget(t)
                .SetId("Squash");

            seq.Insert(0f,
                t.DOScale(squashScale, squashTime)
                 .SetEase(Ease.InQuad));

            seq.Insert(squashTime * (1f - overlap),
                t.DOScale(1, returnTime)
                 .SetEase(Ease.OutBack));

            return seq;
        }

        #endregion

        #region Squash And Stretch Animation 

        public static Tween DOSquashStretch(this Transform t, Vector3? squashScale = null, Vector3? stretchScale = null, float duration = 0.3f) {

            DOTween.Kill(t, "SquashStretch");

            Vector3 squash = squashScale ?? new Vector3(1.2f, 0.8f, 1f);
            Vector3 stretch = stretchScale ?? new Vector3(0.9f, 1.1f, 1f);

            Sequence seq = DOTween.Sequence()
                .SetId("SquashStretch");

            seq.Append(t.DOScale(squash, duration * 0.35f).SetEase(Ease.InQuad));
            seq.Append(t.DOScale(stretch, duration * 0.30f).SetEase(Ease.OutQuad));
            seq.Append(t.DOScale(Vector3.one, duration * 0.35f).SetEase(Ease.OutBack));

            return seq;
        }

        /// <summary>
        /// Blended squash and stretch
        /// </summary>
        public static Tween DOBlendSquashStretch(this Transform t, Vector3? squashScale = null, Vector3? stretchScale = null, float duration = 0.3f, float blend = 0.2f) {
            DOTween.Kill(t, "SquashStretch");

            Vector3 squash = squashScale ?? new Vector3(1.2f, 0.8f, 1f);
            Vector3 stretch = stretchScale ?? new Vector3(0.9f, 1.1f, 1f);

            float part = duration / 3f;
            float overlap = part * blend;

            Sequence seq = DOTween.Sequence()
                .SetTarget(t)
                .SetId("SquashStretch");

            seq.Insert(0f,
                t.DOScale(squash, part)
                 .SetEase(Ease.InQuad));

            seq.Insert(part - overlap,
                t.DOScale(stretch, part)
                 .SetEase(Ease.OutQuad));

            seq.Insert(part * 2f - overlap,
                t.DOScale(1, part)
                 .SetEase(Ease.OutBack));

            return seq;
        }


        #endregion
    }
}