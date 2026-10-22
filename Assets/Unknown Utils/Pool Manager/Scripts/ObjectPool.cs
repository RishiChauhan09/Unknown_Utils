using System.Collections.Generic;
using UnityEngine;

namespace Unknown.Manager {

    [System.Serializable]
    public class ObjectPool {

        public PoolObject prefab;
        public Transform parent;

        private List<PoolObject> avaiableObjects = new();

        private List<PoolObject> allPoolObject = new();


        public PoolObject Get() {
            PoolObject instance = null;

            if(avaiableObjects.Count == 0) {
                instance = CreateObject();
            } else {
                instance = avaiableObjects[0];
                avaiableObjects.RemoveAt(0);
            }

            instance.gameObject.SetActive(true);
            return instance;
        }

        public void ReleaseAll() {
            foreach(PoolObject poolObj in allPoolObject) {
                avaiableObjects.Add(poolObj);
                poolObj.gameObject.SetActive(false);
            }
        }

        public void Release(PoolObject obj) {
            avaiableObjects.Add(obj);
            obj.gameObject.SetActive(false);
        }

        public void Clear() {
            while(allPoolObject.Count > 0) {
                PoolObject comp = allPoolObject[0];
                allPoolObject.RemoveAt(0);

                if(comp != null)
                    Object.Destroy(comp.gameObject);
            }
        }

        public void Initialize(int count) {
            for(int i = 0; i < count; i++) {
                PoolObject obj = CreateObject();
                avaiableObjects.Add(obj);
                obj.gameObject.SetActive(false);
            }
        }

        #region private method 

        private PoolObject CreateObject() {
            PoolObject newObj = Object.Instantiate(prefab, parent);
            allPoolObject.Add(newObj);
            newObj.SetPool(this);
            return newObj;
        }

        #endregion

    }
}