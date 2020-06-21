using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace RollyVortex.Scripts
{
    public class CollisionDetection : MonoBehaviour
    {
        private SphereCollider collider;
    
        // Start is called before the first frame update
        void Start()
        {
            collider = GetComponent<SphereCollider>();
        
            // collider.OnCollisionEnterAsObservable()
            //     .Subscribe(collision =>
            //     {
            //         Debug.Log("Colliding with tag: " + collision.collider.tag);
            //         Debug.Log("Colliding with name: " + collision.collider.name);
            //     })
            //     .AddTo(this);
        
            collider.OnCollisionExitAsObservable()
                .Subscribe(collision =>
                {
                    Debug.Log("EXIT ---------------------------------");
                    Debug.Log("Colliding with name: " + collision.collider.name);
                    Debug.Log("-------------------------------------");
                
                })
                .AddTo(this);
        }
    }
}
