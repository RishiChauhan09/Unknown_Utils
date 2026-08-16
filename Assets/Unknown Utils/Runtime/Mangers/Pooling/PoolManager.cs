// version : v0.1
using System.Collections.Generic;
using UnityEngine;

namespace Unknown.Manager {

    public class PoolManager : MonoBehaviour {          // choice is up to you using singleton for pool manager or service locator

        [System.Serializable]
        public class ObjectPoolsInfo {
            public string id;
            public ObjectPool objectPool;
        }

        [SerializeField] private List<ObjectPoolsInfo> allObjectPoolsInfo;
        [SerializeField] private int initialSize = 3;

        // private variables
        private Dictionary<string, ObjectPool> lookupDictionary = new();

        #region Unity Methods

        private void Awake() {
            foreach(ObjectPoolsInfo poolInfo in allObjectPoolsInfo) {
                if(lookupDictionary.ContainsKey(poolInfo.id)) {
                    Debug.LogError("Duplicate ID in object pool, id: " + poolInfo.id);
                    continue;
                }

                lookupDictionary[poolInfo.id] = poolInfo.objectPool;

                if(poolInfo.objectPool.parent == null) {
                    poolInfo.objectPool.parent = transform;
                }

                poolInfo.objectPool.Initialize(initialSize);
            }
        }

        #endregion

        #region Public Methods 

        /// <summary>
        /// used to get pool object by specifying id
        /// </summary>
        public PoolObject GetPoolObject(string id) {
            if(!lookupDictionary.ContainsKey(id)) {
                Debug.Log("There is no object pool with id of : " + id);
                return null;
            }

            return lookupDictionary[id].Get();
        }

        #endregion

    }
}