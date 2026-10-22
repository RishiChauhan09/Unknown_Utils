using UnityEngine;

public class BillboardCamera : MonoBehaviour {

    private Camera mainCamera;

    private void Awake() {
        mainCamera = Camera.main;
    }

    private void LateUpdate() {
        if(mainCamera == null)
            return;

        transform.forward = mainCamera.transform.forward;
    }

}