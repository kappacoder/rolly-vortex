using UnityEngine;

namespace RollyVortex.Scripts
{
    public class SpinLogic : MonoBehaviour {
 
        // float f_lastX = 0.0f;
        // float f_difX = 0.5f;
        // float f_steps = 0.0f;
        // int i_direction = 1;

        // Convert to constant
        public float Speed = 1f;
     
        // Use this for initialization
        void Start () 
        {
         
        }
     
        // Update is called once per frame
        // void Update () 
        // {
        //     if (Input.GetMouseButtonDown(0))
        //     {
        //         f_difX = 0.0f;
        //     }
        //     else if (Input.GetMouseButton(0))
        //     {
        //         f_difX = Mathf.Abs(f_lastX - Input.GetAxis ("Mouse X")) * speed;
        //
        //         if (f_lastX < Input.GetAxis ("Mouse X"))
        //         {
        //             // i_direction = -1;
        //             transform.Rotate(Vector3.back, -f_difX);
        //         }
        //
        //         if (f_lastX > Input.GetAxis ("Mouse X"))
        //         {
        //             // i_direction = 1;
        //             transform.Rotate(Vector3.back, f_difX);
        //         }
        //
        //         f_lastX = -Input.GetAxis ("Mouse X");
        //     }
        // }
     
        void Update ()
        {
            if (Input.touchCount == 0 || Input.GetTouch(0).phase != TouchPhase.Moved)
                return;

            // Get movement of the finger since last frame
            Vector2 touchDelta = Input.GetTouch(0).deltaPosition;

            transform.Rotate(Vector3.forward, touchDelta.x * Speed);
        }
    }
}