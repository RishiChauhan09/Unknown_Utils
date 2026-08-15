using UnityEngine;

namespace Unknown.Extensions {

    public static class TransformExtensions {

        /// <summary>
        /// does a specific method for each child in transform 
        /// </summary>
        public static void ForEveryChild(this Transform tranform, System.Action<Transform> method) {
            foreach(Transform t in tranform) {
                method?.Invoke(t);
            }
        }

        /// <summary>
        /// disable all children of this transform 
        /// </summary>
        public static void DisableChildren(this Transform transform) {
            transform.ForEveryChild((child) => child.gameObject.SetActive(false));
        }

        /// <summary>
        /// enable all trasnform of this transform 
        /// </summary>
        public static void EnableChildren(this Transform transform) {
            transform.ForEveryChild((child) => child.gameObject.SetActive(true));
        }

        /// <summary>
        /// destroy immediate children of this transform 
        /// </summary>
        public static void DestroyChildrenImmediate(this Transform parent) {
            parent.ForEveryChild(child => Object.DestroyImmediate(child.gameObject));
        }

        /// <summary>
        /// destroys all children of this transform
        /// </summary>
        public static void DestroyChildren(this Transform parent) {
            parent.ForEveryChild(child => Object.Destroy(child.gameObject));
        }

        /// <summary>
        /// resets this trasnform all position, rotation and scale
        /// </summary>
        public static void Reset(this Transform transform) {
            transform.position = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }

    }

}