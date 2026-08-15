using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unknown.Utils {

    public static class CommonUtils {

        #region Coroutine Methods
        public static IEnumerator WaitAndRun(float time, Action method) {
            yield return new WaitForSeconds(time);
            method.Invoke();
        }

        public static IEnumerator WaitForFrames(float frames, Action method) {
            for(int i = 0; i < frames; i++) {
                yield return new WaitForEndOfFrame();
            }

            method.Invoke();
        }

        public static IEnumerator WaitAndRunReal(float time, Action method) {
            yield return new WaitForSecondsRealtime(time);
            method.Invoke();
        }

        #endregion

        #region UI 

        public static Color ChangeColorAlpha(Color c, float alpha) {
            return new Color(c.r, c.g, c.b, alpha);
        }

        public static Vector2 WorldToCanvasPosition(Canvas canvas, RectTransform canvasRect, Vector3 worldPosition) {
            Camera cam = canvas.renderMode == RenderMode.ScreenSpaceOverlay
                ? null
                : canvas.worldCamera;

            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, worldPosition);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                screenPoint,
                cam,
                out Vector2 localPoint);

            return localPoint;
        }

        public static Vector2 Get01ScreenPoint(Vector2 screenPoint) {
            return new Vector2(
                screenPoint.x / Screen.width,
                screenPoint.y / Screen.height
            );
        }

        #endregion

        public static void Shuffle<T>(this List<T> list) {
            for(int i = list.Count - 1; i > 0; i--) {
                int j = UnityEngine.Random.Range(0, i + 1);

                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}