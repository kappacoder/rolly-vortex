using Adic;
using UniRx;
using System;
using _RollyVortex.Views;
using _RollyVortex.Interfaces.Controllers;
using _RollyVortex.Interfaces.Services;
using _RollyVortex.Utils;
using UnityEngine;
using DG.Tweening;
using UniRx.Triggers;
using UnityEngine.Scripting;

namespace _RollyVortex.Controllers
{
    public enum CollisionLayer
    {
        Obstacle = 10,
        Score = 11,
        Gem = 12,
        BoostPad = 13
    }

    [Preserve]
    public class CharacterCollisionController : ICharacterCollisionController
    {
        [Inject]
        private IGameService gameService;
        [Inject]
        private IEntitiesService entitiesService;
        [Inject]
        private IUserService userService;
        [Inject]
        private PoolFactory poolFactory;

        private CharacterView character;
        
        public void Init(CharacterView character)
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
                            OnCollisionWithGem(collider);
                            break;
                        case (int)CollisionLayer.BoostPad:
                            OnCollisionWithBoostPad();
                            break;
                    }
                })
                .AddTo(character);
        }

        private void OnCollisionWithObstacle(Collider collider)
        {
            // Stop all camera tweens
            Camera.main.DOKill(true);

            GameObject explosion;

            // Trigger obstacle destruction if invincible
            if (character.StateRX.Value == CharacterState.Invincible)
            {
                collider.gameObject.SetActive(false);

                explosion = poolFactory.GetObstacle(ObstacleType.ObstacleExplosion);
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
            
            // Trigger character death
            character.StateRX.Value = CharacterState.Dead;
            character.Body.gameObject.SetActive(false);
            
            explosion = poolFactory.GetObstacle(ObstacleType.CharacterExplosion);
            explosion.transform.position = character.Body.position;
            
            // Update the material of the death particle to match the selected skin
            explosion.GetComponent<ParticleSystemRenderer>().material = 
                entitiesService.GetCharacterSkinData(userService.SelectedCharacterSkinRX.Value).Material;
            
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
                child.DOScale(0.01f, 0.4f)
                    .SetEase(Ease.OutSine)
                    .Play();
            }
        }

        private void OnCollisionWithGem(Collider collider)
        {
            userService.GemsRX.Value++;
            
            collider.transform.parent.gameObject.SetActive(false);
        }

        private void OnCollisionWithBoostPad()
        {
            // If the state is already invincible reset it to trigger the reactive property event
            if (character.StateRX.Value == CharacterState.Invincible)
                character.StateRX.Value = CharacterState.Unknown;
                            
            character.StateRX.Value = CharacterState.Invincible;
            gameService.TriggerSpeedBoost();
        }
    }
}
