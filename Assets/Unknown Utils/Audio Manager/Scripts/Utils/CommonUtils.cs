using System.Collections.Generic;
using UnityEngine;

namespace Unknown.Manager.AudioManager {
    public static class CommonUtils {

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Reset() {
            waitForSecDict = new();
        }

        // wait for seconds 
        private static Dictionary<float, WaitForSeconds> waitForSecDict;
        public static WaitForSeconds GetWaitForSeconds(float value) {
            if(waitForSecDict.ContainsKey(value)) {
                return waitForSecDict[value];
            }

            waitForSecDict[value] = new WaitForSeconds(value);
            return waitForSecDict[value];
        }

    }
}