using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unknown.Manager {

    [System.Serializable]
    public class ObjectPool<T> where T : Component {

        public T prefab;
        public Transform parent;

        private List<T> avaiableObjects = new();

        private List<T> allPoolObject = new();

        private Func<T> createObjectFunc;

        public ObjectPool(Func<T> createObjectFunc = null, int initializeObjectsNumber = 0) {
            this.createObjectFunc = createObjectFunc;
            Initialize(initializeObjectsNumber);
        }

        public ObjectPool() {

        }

        public T Get() {
            T instance = null;

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
            foreach(T poolObj in allPoolObject) {
                avaiableObjects.Add(poolObj);
                poolObj.gameObject.SetActive(false);
            }
        }

        public void Release(T obj) {
            avaiableObjects.Add(obj);
            obj.gameObject.SetActive(false);
        }

        public void Clear() {
            while(allPoolObject.Count > 0) {
                T comp = allPoolObject[0];
                allPoolObject.RemoveAt(0);

                if(comp != null)
                    UnityEngine.Object.Destroy(comp.gameObject);
            }
        }

        public void Initialize(int count) {
            for(int i = 0; i < count; i++) {
                T obj = CreateObject();
                avaiableObjects.Add(obj);
                obj.gameObject.SetActive(false);
            }
        }

        #region private method 

        private T CreateObject() {
            if(createObjectFunc != null) {
                return createObjectFunc?.Invoke();
            }

            T newObj = UnityEngine.Object.Instantiate(prefab, parent);
            allPoolObject.Add(newObj);
            return newObj;
        }

        #endregion

    }
}