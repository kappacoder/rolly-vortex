using UnityEngine;

namespace RollyVortex.Scripts.Utils
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class CameraScaler : MonoBehaviour {

        Camera _camera;
        public float horizontalFoV = 45.0f;

        void Awake () {
            _camera = GetComponent<Camera>();
            UpdateCrop();
        }

        // Call this method if your window size or target aspect change.
        // public void UpdateCrop() {
        //     // Determine ratios of screen/window & target, respectively.
        //     float screenRatio = Screen.width / (float)Screen.height;
        //     float targetRatio = targetAspect.x / targetAspect.y;
        //
        //     if(Mathf.Approximately(screenRatio, targetRatio)) {
        //         // Screen or window is the target aspect ratio: use the whole area.
        //         _camera.rect = new Rect(0, 0, 1, 1);
        //     }
        //     else if(screenRatio > targetRatio) {
        //         // Screen or window is wider than the target: pillarbox.
        //         float normalizedWidth = targetRatio / screenRatio;
        //         float barThickness = (1f - normalizedWidth)/2f;
        //         _camera.rect = new Rect(barThickness, 0, normalizedWidth, 1);
        //     }
        //     else {
        //         // Screen or window is narrower than the target: letterbox.
        //         float normalizedHeight = screenRatio / targetRatio;
        //         float barThickness = (1f - normalizedHeight) / 2f;
        //         _camera.rect = new Rect(0, barThickness, 1, normalizedHeight);
        //     }
        // }
    
        void UpdateCrop() {
            float halfWidth = Mathf.Tan(0.5f * horizontalFoV * Mathf.Deg2Rad);

            float halfHeight = halfWidth * Screen.height / Screen.width;

            float verticalFoV = 2.0f * Mathf.Atan(halfHeight) * Mathf.Rad2Deg;

            _camera.fieldOfView = verticalFoV;
        }
    }
}