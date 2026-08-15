using TMPro;
using UnityEngine;
using Unknown.Extensions;

public class Testing : MonoBehaviour {

    [SerializeField] private TMP_Text text;
    [SerializeField] private float animationTime = 2f;

    [ContextMenu("Show text animation")]
    public void ShowTextAnimation() {
        text.VisibilityAnimation(animationTime);
    }

}