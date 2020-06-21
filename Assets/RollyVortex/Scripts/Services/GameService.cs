using Adic;
using UniRx;
using System;
using UnityEngine;
using UnityEngine.Scripting;
using RollyVortex.Scripts.Game.Components;
using RollyVortex.Scripts.Interfaces.Services;
using RollyVortex.Scripts.Interfaces.Game.Controllers;
using RollyVortex.Scripts.Utils;
using System.Collections;
using System.Collections.Generic;

namespace RollyVortex.Scripts.Services
{
    public enum GameMode
    {
        Endless,
        Finish
    }
    
    [Preserve]
    public class GameService : IGameService
    {
        public IReactiveProperty<bool> IsRunningRX { get; private set; }

        public float GameSpeed
        {
            get
            {
                return gameSpeed;
            }
        }
        
        [Inject]
        private IEntitiesService entitiesService;
        [Inject]
        private IInjectionService injectionService;
        [Inject]
        private PoolFactory poolFactory;
        
        private Transform gameWrapper;
        private Transform movingObjectsWrapper;
        private Transform obstaclesWrapper;
        private CharacterComponent mainCharacter;

        private CompositeDisposable disposables;

        private Vector3 characterDefaultPosition = new Vector3(-3f, 0f, 2f);
        private float gameSpeed = 20f;

        private IList<GameObject> obstacles;

        private float firstObstacleDistance = 15f;
        private float distanceBetweenObstacles = 5f;
        
        public void Init(Transform gameWrapper)
        {
            IsRunningRX = new ReactiveProperty<bool>();
            disposables = new CompositeDisposable();
            obstacles = new List<GameObject>();
            
            this.gameWrapper = gameWrapper;
            movingObjectsWrapper = gameWrapper.Find("MovingObjects");
            obstaclesWrapper = movingObjectsWrapper.Find("Obstacles");
            
            // Generate the main character and bind controllers to it
            mainCharacter = entitiesService.GenerateCharacter(gameWrapper, characterDefaultPosition);
            injectionService.Container.Resolve<ICharacterMovementController>().Init(mainCharacter);
            injectionService.Container.Resolve<ICharacterCollisionController>().Init(mainCharacter);
            injectionService.Container.Resolve<ICharacterRollingController>().Init(mainCharacter);
            
            injectionService.Container.Resolve<IEndlessTunnelController>().Init(movingObjectsWrapper.Find("Tunnels"));
            
            Subscribe();
        }

        public void Dispose()
        {
            disposables.Dispose();
            disposables.Clear();
        }

        public void StartRunning(GameMode gameMode)
        {
            switch (gameMode)
            {
                case GameMode.Endless:
                    // start endless level

                    IsRunningRX.Value = true;
                    break;
                case GameMode.Finish:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameMode), gameMode, null);
            }
        }
        
        private void Subscribe()
        {
            Observable.EveryFixedUpdate()
                .Where(x => IsRunningRX.Value)
                .Subscribe(x => OnFixedUpdate())
                .AddTo(disposables);

            mainCharacter.StateRX
                .Where(state => state == CharacterState.Dead)
                .Subscribe(_ =>
                {
                    IsRunningRX.Value = false;
                });
        }

        private void OnFixedUpdate()
        {
            // Move obstacles towards the camera
            movingObjectsWrapper.position -= new Vector3(0f, 0f, gameSpeed * Time.fixedDeltaTime);

            if (movingObjectsWrapper.position.z + firstObstacleDistance > 0)
                return;
            
            if ((movingObjectsWrapper.position.z / distanceBetweenObstacles)*-1 > obstacles.Count)
                SpawnObstacle(movingObjectsWrapper.position.z + 150f * (obstacles.Count+1));
        }

        private void SpawnObstacle(float posZ)
        {
            var obstacle = poolFactory.GetObstacle(ObstacleDifficulty.Easy);

            var pos = obstacle.transform.position;
            obstacle.transform.position = new Vector3(pos.x, pos.y, posZ);
            
            obstacle.SetActive(true);
            
            obstacles.Add(obstacle);
        }
    }
}
