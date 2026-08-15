using UnityEngine;

namespace Unknown.Utils {
    public abstract class PersistentSingleton<T> : MonoBehaviour where T : MonoBehaviour {
        private static T instance;

        public static T Instance {
            get {
                if(instance == null) {
                    instance = FindFirstObjectByType<T>();

                    if(instance == null) {
                        GameObject go = new GameObject(typeof(T).Name);
                        instance = go.AddComponent<T>();
                    }
                }

                return instance;
            }
        }

        protected virtual void Awake() {
            if(instance == null) {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
            } else if(instance != this) {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy() {
            if(instance == this) {
                instance = null;
            }
        }
    }
}