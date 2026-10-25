using System.Collections;
using UnityEngine;

namespace Unknown.Utils {
    public static class PolishingUtils {
        private static CoroutineRunner runner;

        #region freeze time

        public static void FreezeGame(float duration) {
            if(duration <= 0f)
                return;

            runner ??= CoroutineRunner.Create();

            Time.timeScale = 0f;
            runner.StartCoroutine(UnfreezeAfter(duration));
        }

        private static IEnumerator UnfreezeAfter(float duration) {
            yield return new WaitForSecondsRealtime(duration);

            Time.timeScale = 1f;
        }

        #endregion

        #region slow mo 

        public static void SlowMotion(float timeScale, float duration) {
            if(duration <= 0f || timeScale <= 0f)
                return;
            runner ??= CoroutineRunner.Create();
            Time.timeScale = timeScale;
            runner.StartCoroutine(ResetTimeScaleAfter(duration));
        }

        private static IEnumerator ResetTimeScaleAfter(float duration) {
            yield return new WaitForSecondsRealtime(duration);

            Time.timeScale = 1f;
        }

        #endregion

        private class CoroutineRunner : MonoBehaviour {
            private static CoroutineRunner instance;

            public static CoroutineRunner Create() {
                if(instance != null)
                    return instance;

                GameObject obj = new GameObject("[PolishingUtils]");
                DontDestroyOnLoad(obj);

                instance = obj.AddComponent<CoroutineRunner>();

                return instance;
            }

            private void OnDestroy() {
                if(instance == this)
                    instance = null;
            }
        }
    }
}