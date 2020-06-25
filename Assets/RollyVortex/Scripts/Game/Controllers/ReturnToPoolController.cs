using DG.Tweening;
using UniRx;
using UnityEngine;
using UnityEngine.Scripting;
using RollyVortex.Scripts.Interfaces.Game.Controllers;

namespace RollyVortex.Scripts.Game.Controllers
{
    [Preserve]
    public class ReturnToPoolController : IReturnToPoolController
    {
        private Transform transform;

        private float returnToPoolPositionZ;

        private bool isObstacle;
        
        public void Init(Transform transform, float returnToPoolPositionZ, bool isObstacle)
        {
            this.transform = transform;
            this.returnToPoolPositionZ = returnToPoolPositionZ;
            this.isObstacle = isObstacle;
            
            Subscribe();
        }

        private void Subscribe()
        {
            Observable.EveryFixedUpdate()
                .TakeUntilDisable(transform.gameObject)
                .Subscribe(x => OnFixedUpdate())
                .AddTo(transform.gameObject);
        }

        private void OnFixedUpdate()
        {
            if (transform.position.z >= returnToPoolPositionZ)
                return;

            if (isObstacle)
            {
                foreach (Transform child in transform)
                {
                    child.DOKill(true);

                    child.localScale = Vector3.one;
                    child.gameObject.SetActive(true);
                }
            }
            
            transform.gameObject.SetActive(false);
        }
    }
}
