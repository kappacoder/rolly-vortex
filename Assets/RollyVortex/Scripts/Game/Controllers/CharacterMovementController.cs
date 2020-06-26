using Adic;
using UniRx;
using UnityEngine;
using UnityEngine.Scripting;
using RollyVortex.Scripts.Game.Components;
using RollyVortex.Scripts.Interfaces.Services;
using RollyVortex.Scripts.Interfaces.Game.Controllers;

namespace RollyVortex.Scripts.Game.Controllers
{
    [Preserve]
    public class CharacterMovementController : ICharacterMovementController
    {
        [Inject]
        private IGameService gameService;
    
        private Transform characterTransform;

        public void Init(CharacterComponent character)
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
        }

        private void OnUserTouch()
        {
            Vector2 touchDelta = Input.GetTouch(0).deltaPosition;

            characterTransform.Rotate(Vector3.forward, touchDelta.x * Constants.Game.CharacterControlSpeedMultiplier);
        }
    }
}
