using Adic;
using UniRx;
using UnityEngine;
using UniRx.Triggers;
using UnityEngine.Scripting;
using RollyVortex.Scripts.Game.Components;
using RollyVortex.Scripts.Interfaces.Services;
using RollyVortex.Scripts.Interfaces.Game.Controllers;

namespace RollyVortex.Scripts.Game.Controllers
{
    public enum CollisionLayer
    {
        Obstacle = 10,
        Hole = 11,
        Gem = 12,
        FinishLine = 13,
        BoostPlatform = 14
    }
    
    [Preserve]
    public class CharacterCollisionController : ICharacterCollisionController
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
            Debug.Log("subscribing for collision");
            character.Collider
                .OnTriggerEnterAsObservable()
                .Subscribe(collider =>
                {
                    switch (collider.gameObject.layer)
                    {
                        case (int)CollisionLayer.Obstacle:
                            OnCollisionWithObstacle();
                            break;
                    }
                })
                .AddTo(character);
        }

        private void OnCollisionWithObstacle()
        {
            // Die
            Debug.Log("dying?");
            character.StateRX.Value = CharacterState.Dead;
        }
    }
}
