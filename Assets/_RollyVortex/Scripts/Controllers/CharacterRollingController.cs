using _RollyVortex.Views;
using _RollyVortex.Interfaces.Controllers;
using _RollyVortex.Interfaces.Services;
using _RollyVortex.Utils;
using Adic;
using UniRx;
using UnityEngine;
using UnityEngine.Scripting;

namespace _RollyVortex.Controllers
{
    [Preserve]
    public class CharacterRollingController : ICharacterRollingController
    {
        [Inject]
        private IGameService gameService;
    
        private CharacterView character;

        public void Init(CharacterView character)
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
