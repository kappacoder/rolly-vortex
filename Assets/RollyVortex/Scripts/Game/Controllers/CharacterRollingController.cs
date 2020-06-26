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
    public class CharacterRollingController : ICharacterRollingController
    {
        [Inject]
        private IGameService gameService;
    
        private CharacterComponent character;

        public void Init(CharacterComponent character)
        {
            this.character = character;

            Subscribe();
        }

        private void Subscribe()
        {
            Observable.EveryFixedUpdate()
                .Where(x => gameService.IsRunningRX.Value && character.StateRX.Value != CharacterState.Dead)
                .Subscribe(x => Roll())
                .AddTo(character.gameObject);
        }

        private void Roll()
        {
            character.Body.Rotate(Vector3.left, gameService.GameSpeed * Constants.Game.RollingSpeedMultiplier);
        }
    }
}
