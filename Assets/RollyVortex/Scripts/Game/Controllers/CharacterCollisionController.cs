using Adic;
using UniRx;
using DG.Tweening;
using UnityEngine;
using UniRx.Triggers;
using UnityEngine.Scripting;
using RollyVortex.Scripts.Game.Components;
using RollyVortex.Scripts.Interfaces.Services;
using RollyVortex.Scripts.Interfaces.Game.Controllers;
using RollyVortex.Scripts.Utils;
using System;

namespace RollyVortex.Scripts.Game.Controllers
{
    public enum CollisionLayer
    {
        Obstacle = 10,
        Score = 11,
        Gem = 12,
        Boostpad = 13,
        BoostPlatform = 14
    }
    
    [Preserve]
    public class CharacterCollisionController : ICharacterCollisionController
    {
        [Inject]
        private IGameService gameService;

        [Inject]
        private PoolFactory poolFactory;

        private CharacterComponent character;
        
        public void Init(CharacterComponent character)
        {
            this.character = character;
            
            Subscribe();
        }

        private void Subscribe()
        {
            SubscribeForCollision();

            // Every time the skin is changed, re-subscribe for the new collider
            character.OnSkinChangedRX
                .Subscribe(x => SubscribeForCollision())
                .AddTo(character);
        }

        private void SubscribeForCollision()
        {
            character.Collider
                .OnTriggerEnterAsObservable()
                .TakeUntil(character.OnSkinChangedRX)
                .Subscribe(collider =>
                {
                    switch (collider.gameObject.layer)
                    {
                        case (int)CollisionLayer.Obstacle:
                            OnCollisionWithObstacle(collider);
                            break;
                        case (int)CollisionLayer.Score:
                            OnCollisionWithScore(collider);
                            break;
                        case (int)CollisionLayer.Gem:
                            // collect gems
                            collider.transform.parent.gameObject.SetActive(false);
                            break;
                        case (int)CollisionLayer.Boostpad:
                            // make invincible, increase speed
                            character.StateRX.Value = CharacterState.Invincible;
                            gameService.TriggerSpeedBoost();
                            break;
                    }
                })
                .AddTo(character);
        }

        private void OnCollisionWithObstacle(Collider collider)
        {
            Camera.main.DOKill(true);

            GameObject explosion;
            
            if (character.StateRX.Value == CharacterState.Invincible)
            {
                // destroy game object
                collider.gameObject.SetActive(false);

                explosion = poolFactory.GetObstacle("ObstacleExplosionVFX");
                explosion.transform.position = collider.transform.position;
                
                explosion.SetActive(true);

                Observable.Timer(TimeSpan.FromSeconds(2))
                    .Subscribe(x =>
                    {
                        explosion.SetActive(false);
                    })
                    .AddTo(explosion);

                Camera.main.DOShakePosition(0.2f, 0.3f, 25)
                    .From()
                    .Play();
                
                return;
            }
            
            // Die
            character.StateRX.Value = CharacterState.Dead;
            character.Body.gameObject.SetActive(false);
            
            explosion = poolFactory.GetObstacle("CharacterDeathVFX");
            explosion.transform.position = character.Body.position;
            
            explosion.SetActive(true);

            Observable.Timer(TimeSpan.FromSeconds(2))
                .Subscribe(x =>
                {
                    explosion.SetActive(false);
                })
                .AddTo(explosion);

            Camera.main.DOShakePosition(0.3f, 1.2f, 25)
                .From()
                .Play();
        }

        private void OnCollisionWithScore(Collider collider)
        {
            gameService.CurrentScoreRX.Value++;
            
            foreach (Transform child in collider.transform)
            {
                child.DOScale(0f, 0.4f)
                    .SetEase(Ease.OutSine)
                    .Play();
                // part.DOMove(new Vector3(pos.x + 1f, pos.y + 1f, 0f), 0.3f)
                //     .From()
                //     .SetEase(Ease.OutBounce)
                //     .Play();
            }
        }
    }
}
