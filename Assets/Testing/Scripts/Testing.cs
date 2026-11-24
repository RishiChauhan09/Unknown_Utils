using UnityEngine;
using UnityEngine.InputSystem;
using Unknown.Manager.AudioManager;

public class Testing : MonoBehaviour {

    [SerializeField] private AudioID audioId;
    [SerializeField] private Unknown.Manager.AudioManager.AudioType audioType;
    [SerializeField] private AudioTypeMask audioTypeMask;
    [SerializeField] private Color colorTesting;


    private void Update() {
        if(Keyboard.current.tKey.wasPressedThisFrame) {
            AudioManager.Play(audioId);
            //AudioManager.SetVolume(audioType, 0);
        }
    }

}