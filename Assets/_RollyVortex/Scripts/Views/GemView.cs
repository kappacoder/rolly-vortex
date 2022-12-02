using UnityEngine;

namespace _RollyVortex.Views
{
    // TODO: This should be done as a controller
    public class GemView : MonoBehaviour
    {
        [SerializeField]
        private Transform body;

        private float speed = 10f;

        void FixedUpdate()
        {
            if (gameObject.activeSelf)
                body.Rotate(Vector3.up, Time.fixedDeltaTime * speed);
        }
    }
}
