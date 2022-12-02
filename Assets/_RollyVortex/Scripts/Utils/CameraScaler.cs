using UnityEngine;

namespace _RollyVortex.Utils
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class CameraScaler : MonoBehaviour
    {
        public float HorizontalFOV = 45.0f;
        private Camera camera;

        void Awake ()
        {
            camera = GetComponent<Camera>();
            
            UpdateCrop();
        }
    
        void UpdateCrop()
        {
            float halfWidth = Mathf.Tan(0.5f * HorizontalFOV * Mathf.Deg2Rad);
            float halfHeight = halfWidth * Screen.height / Screen.width;

            float verticalFOV = 2.0f * Mathf.Atan(halfHeight) * Mathf.Rad2Deg;

            camera.fieldOfView = verticalFOV;
        }
    }
}