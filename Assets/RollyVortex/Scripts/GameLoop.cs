using UnityEngine;

namespace RollyVortex.Scripts
{
    public class GameLoop : MonoBehaviour
    {
        public Transform ObstaclesContainer;
        public Transform Character;

        public bool Run = false;
        public float Speed = 5f;

        private float rotationSpeed = 100f;

        private void Start()
        {
            Application.targetFrameRate = 60;
        }
    
        void FixedUpdate()
        {
            if (!Run)
                return;
        
            ObstaclesContainer.position -= new Vector3(0f, 0f, Speed * Time.fixedDeltaTime);
        
            Character.Rotate(Vector3.right, Speed * rotationSpeed * Time.fixedDeltaTime);
        }
    }
}
