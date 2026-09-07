using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unknown.Manager {
    public class PoolObject : MonoBehaviour {

        [SerializeField] private ObjectPool currentPool;
        [SerializeField] private Component[] references;

        private Dictionary<Type, Component> referenceLookup;

        private void Awake() {
            referenceLookup = new Dictionary<Type, Component>(references.Length);

            foreach(Component comp in references) {
                if(comp != null) {
                    if(referenceLookup.ContainsKey(comp.GetType())) {
                        Debug.LogWarning("Duplicate componenet in Pool Object.");
                        continue;
                    }
                    referenceLookup[comp.GetType()] = comp;
                }
            }
        }

        public void SetPool(ObjectPool pool) {
            currentPool = pool;
        }

        #region Public Methods 

        /// <summary>
        /// Use this method to release this object to pool.
        /// </summary>
        public void Release() {
            if(currentPool != null) {
                currentPool.Release(this);
            } else {
                Debug.LogError($"There is no object pool assigned to {gameObject.name}");
            }
        }

        /// <summary>
        /// Use this method to get references of object you sent in inspector
        /// </summary>
        public T GetReference<T>() where T : Component {
            referenceLookup.TryGetValue(typeof(T), out Component component);
            return component as T;
        }

        #endregion

    }
}