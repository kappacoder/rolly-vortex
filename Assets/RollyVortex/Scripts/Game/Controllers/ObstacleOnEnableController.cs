using UniRx;
using DG.Tweening;
using UnityEngine;
using UniRx.Triggers;
using UnityEngine.Scripting;
using RollyVortex.Scripts.Interfaces.Game.Controllers;

namespace RollyVortex.Scripts.Game.Controllers
{
    [Preserve]
    public class ObstacleOnEnableController : IObstacleOnEnableController
    {
        private Transform transform;

        private bool rotate;
        
        public void Init(Transform transform, bool rotate)
        {
            this.transform = transform;
            this.rotate = rotate;

            if (transform.gameObject.activeSelf)
                OnEnable();
            else
                Subscribe();
        }

        private void Subscribe()
        {
            transform.gameObject.OnEnableAsObservable()
                .TakeUntilDisable(transform.gameObject)
                .Subscribe(x => OnEnable());
        }

        void OnEnable()
        {
            foreach (Transform child in transform)
            {
                child.DOScale(0f, 0.3f)
                    .From()
                    .SetEase(Ease.InBack)
                    .Play();
            }

            if (rotate)
            {
                transform.DOLocalRotate(new Vector3(0f, 0f, transform.rotation.z + Random.Range(-90f, 90f)), 0.3f, RotateMode.LocalAxisAdd)
                    .From()
                    .SetDelay(0.2f)
                    .SetEase(Ease.OutSine)
                    .Play();
            }
        }
    }
}
