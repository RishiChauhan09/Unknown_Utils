using UnityEngine;

namespace Unknown.Manager {
    public class PoolObject : MonoBehaviour {

        [Header("Debugging")]
        [SerializeField] private ObjectPool currentPool;

        public void SetPool(ObjectPool pool) {
            currentPool = pool;
        }

        public void Release() {
            if(currentPool != null) {
                currentPool.Release(this);
            } else {
                Debug.LogError("There is no object pool assigned to " + gameObject.name);
            }
        }

    }
}