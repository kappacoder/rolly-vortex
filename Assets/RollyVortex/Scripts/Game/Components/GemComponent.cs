using UnityEngine;

namespace RollyVortex.Scripts.Game.Components
{
    // TODO: This could be done as a controller
    public class GemComponent : MonoBehaviour
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
