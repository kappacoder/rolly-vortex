using _RollyVortex.Views;
using _RollyVortex.Interfaces.Controllers;
using _RollyVortex.Interfaces.Services;
using _RollyVortex.Utils;
using Adic;
using UniRx;
using UnityEngine;
using UnityEngine.Scripting;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace _RollyVortex.Controllers
{
    [Preserve]
    public class CharacterMovementController : ICharacterMovementController
    {
        [Inject]
        private IGameService gameService;
    
        private Transform characterTransform;

#if UNITY_EDITOR
        private Vector3? lastMousePosition;
#endif

        public void Init(CharacterView character)
        {

            characterTransform = character.transform;

            Subscribe();
        }

        private void Subscribe()
        {
            Observable.EveryUpdate()
                .Where(x => gameService.IsRunningRX.Value)
                .Where(x => Input.touchCount != 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
                .Subscribe(x => OnUserTouch())
                .AddTo(characterTransform.gameObject);

            // In the Editor we might want to use the Game window which detects mouse buttons instead of finger touches
#if UNITY_EDITOR
            Observable.EveryUpdate()
                .Where(x => gameService.IsRunningRX.Value)
                .Where(x => Input.GetMouseButton(0))
                .Subscribe(x => OnUserTouch())
                .AddTo(characterTransform.gameObject);

            Observable.EveryUpdate()
                .Where(x => gameService.IsRunningRX.Value)
                .Where(x => Input.GetMouseButtonUp(0))
                .Subscribe(x => lastMousePosition = null)
                .AddTo(characterTransform.gameObject);
#endif
        }

        private void OnUserTouch()
        {
#if UNITY_EDITOR
            var currentPosition = Input.mousePosition;

            if (lastMousePosition == null) {
                lastMousePosition = currentPosition;

                return;
            }

            Vector2 touchDelta = (Vector2) (currentPosition - lastMousePosition);
            lastMousePosition = currentPosition;
#else
            Vector2 touchDelta = Input.GetTouch(0).deltaPosition;
#endif

            characterTransform.Rotate(Vector3.forward, touchDelta.x * Constants.Game.CharacterControlSpeedMultiplier);
        }
    }
}
