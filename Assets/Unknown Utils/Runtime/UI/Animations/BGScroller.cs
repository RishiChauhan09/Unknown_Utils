using UnityEngine;
using UnityEngine.UI;

namespace Unknown.UI {
    public class BGScroller : MonoBehaviour {

        [SerializeField] private RawImage rawImage;
        [SerializeField] private float xSpeed, ySpeed;

        private void Update() {
            Rect uv = rawImage.uvRect;
            uv.position += new Vector2(xSpeed, ySpeed) * Time.deltaTime;
            rawImage.uvRect = uv;
        }

    }
}