using UnityEngine;
using UnityEngine.InputSystem;
using Unknown.Manager.AudioManager;

public class Testing : MonoBehaviour {

    [SerializeField] private AudioID loopAudioID;
    [SerializeField] private AudioID effectAudioID;
    [SerializeField] private Unknown.Manager.AudioManager.AudioType audioType;
    [SerializeField] private AudioTypeMask audioTypeMask;
    [SerializeField] private Color colorTesting;


    private void Update() {
        if(Keyboard.current.tKey.wasPressedThisFrame) {
            AudioManager.Play(loopAudioID);
        }

        if(Keyboard.current.sKey.wasPressedThisFrame) {
            AudioManager.SetVolume(audioType, 0);
        }

        if(Keyboard.current.aKey.wasPressedThisFrame) {
            AudioManager.SetVolume(audioType, 1);
        }

        if(Keyboard.current.kKey.wasPressedThisFrame) {
            AudioManager.Play(effectAudioID).AsDominator().HighpassOthers(UnknownAdvice.HighPassFrequency);
        }

    }

}