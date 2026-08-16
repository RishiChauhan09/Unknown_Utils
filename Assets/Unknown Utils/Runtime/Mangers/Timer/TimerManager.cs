using System;
using System.Collections.Generic;
using UnityEngine;
using Unknown.Utils;

namespace Unknown.Manager {
    public class TimerManager : Singleton<TimerManager> {

        private List<Timer> timers = new();

        private void Update() {
            float scaledDeltaTime = Time.deltaTime;
            float unScaledDeltaTime = Time.deltaTime;

            for(int i = timers.Count - 1; i >= 0; i--) {
                Timer timer = timers[i];

                bool finished = timer.Update(timer.IsUnscaledTime ? unScaledDeltaTime : scaledDeltaTime);

                if(finished || timer.IsCancelled) {
                    timers.RemoveAt(i);
                }
            }
        }

        #region Static Methods

        public static Timer Create(float duration, Action callback, bool isUnscaledTime = false) {
            Timer timer = new Timer(duration, callback: callback, isUnscaledTime: isUnscaledTime);

            Instance.timers.Add(timer);

            return timer;
        }

        public static Timer Delay(float duration, Action callback, bool isUnscaledTime = false) {
            return Create(duration, callback, isUnscaledTime: isUnscaledTime);
        }

        public static Timer RepeatingTimer(float step, Action callback, bool isUnscaledTime = false) {
            Timer timer = new Timer(step, callback: callback, true, isUnscaledTime: isUnscaledTime);

            Instance.timers.Add(timer);

            return timer;
        }

        #endregion

    }
}