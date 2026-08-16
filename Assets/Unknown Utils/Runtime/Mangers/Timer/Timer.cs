using System;

namespace Unknown.Manager {

    public class Timer {

        public float TotalDuration { get; private set; }
        public float TimeRemaining { get; private set; }

        public bool IsRunning { get; private set; }
        public bool IsCancelled { get; private set; } = false;
        public bool IsCompleted { get; private set; } = false;
        public bool IsUnscaledTime { get; private set; }

        /// <summary>
        /// Gives normalized progress of timer.
        /// 1 when timer is completed
        /// </summary>
        public float Progress => TotalDuration <= 0f ? 1f : 1f - (TimeRemaining / TotalDuration);

        // events
        public event Action onComplete;
        public event Action onStart;
        public event Action onPause;
        public event Action onCancel;
        public event Action onResume;

        private Action callback;
        private bool isRepeating;

        public Timer(float duration, Action callback = null, bool repeat = false, bool isUnscaledTime = false) {
            if(duration < 0) {
                throw new ArgumentOutOfRangeException(nameof(duration));
            }

            TotalDuration = duration;
            TimeRemaining = duration;
            isRepeating = repeat;
            IsUnscaledTime = isUnscaledTime;
            onStart?.Invoke();
        }

        public bool Update(float timeDelta) {
            if(!IsRunning)
                return false;

            TimeRemaining -= timeDelta;

            if(TimeRemaining > 0)
                return false;

            onComplete?.Invoke();
            callback?.Invoke();

            if(!isRepeating) {
                Complete();
                return true;
            }

            TimeRemaining += TotalDuration;

            return false;
        }

        #region public methods 

        public Timer Restart() {
            TimeRemaining = TotalDuration;
            IsRunning = true;
            onStart?.Invoke();
            return this;
        }

        public Timer Pause() {
            IsRunning = false;
            onPause?.Invoke();
            return this;
        }

        public Timer Cancel() {
            IsRunning = false;
            IsCancelled = true;
            onCancel?.Invoke();
            return this;
        }

        public Timer Resume() {
            IsRunning = true;
            onResume?.Invoke();
            return this;
        }

        public Timer OnStart(Action callback) {
            onStart += callback;
            return this;
        }

        public Timer OnPause(Action callback) {
            onPause += callback;
            return this;
        }

        public Timer OnResume(Action callback) {
            onResume += callback;
            return this;
        }

        public Timer OnCancel(Action callback) {
            onCancel += callback;
            return this;
        }

        public Timer OnComplete(Action callback) {
            onComplete += callback;
            return this;
        }

        #endregion

        private void Complete() {
            IsRunning = false;
            TimeRemaining = 0;
            IsCompleted = true;
        }

    }
}