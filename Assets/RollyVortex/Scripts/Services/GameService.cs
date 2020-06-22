using Adic;
using UniRx;
using System;
using UnityEngine;
using UnityEngine.Scripting;
using RollyVortex.Scripts.Game.Components;
using RollyVortex.Scripts.Interfaces.Services;
using RollyVortex.Scripts.Interfaces.Game.Controllers;
using RollyVortex.Scripts.Utils;
using System.Collections.Generic;
using Random = UnityEngine.Random;

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
        public IReactiveProperty<bool> IsRunningRX { get; } = new ReactiveProperty<bool>();

        public IReactiveProperty<int> CurrentScoreRX { get; } = new ReactiveProperty<int>();
        
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

        private Vector3 characterDefaultPosition = new Vector3(-2.5f, 0f, 1f);
        private float gameSpeed = 20f;

        private IList<GameObject> obstacles;
        private IList<GameObject> gems;

        private float firstObstacleDistance = 10f;
        private float distanceBetweenObstacles = 8f;
        
        private int obstaclesPassed = 0;
        
        public void Init(Transform gameWrapper)
        {
            disposables = new CompositeDisposable();
            obstacles = new List<GameObject>();
            gems = new List<GameObject>();
            
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
            Reset();
    
            switch (gameMode)
            {
                case GameMode.Endless:
                    
                    IsRunningRX.Value = true;
                    break;
                case GameMode.Finish:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(gameMode), gameMode, null);
            }
        }

        public void TriggerSpeedBoost()
        {
            gameSpeed *= 2;

            Observable.Timer(TimeSpan.FromSeconds(1))
                .Where(x => IsRunningRX.Value)
                .Subscribe(x =>
                {
                    gameSpeed = 20f;
                })
                .AddTo(mainCharacter.gameObject);
            
            Observable.Timer(TimeSpan.FromSeconds(2))
                .Where(x => IsRunningRX.Value)
                .Subscribe(x =>
                {
                    mainCharacter.StateRX.Value = CharacterState.Alive;
                })
                .AddTo(mainCharacter.gameObject);
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

            if (((movingObjectsWrapper.position.z + firstObstacleDistance) / distanceBetweenObstacles) * -1 >
                obstacles.Count + obstaclesPassed)
            {
                SpawnObstacle();
                
                if (Random.Range(0, 3) == 0)
                    SpawnGem();
                
                if (Random.Range(0, 3) == 0)
                    SpawnBoostpad();
            }
            
            // check if obstacles need to be returned to pool
            foreach (GameObject o in obstacles)
            {
                if (o.transform.position.z < -5f)
                {
                    o.SetActive(false);
                    obstacles.Remove(o);
                    obstaclesPassed++;
                    
                    break;
                }
            }
            
            // check if gems need to be returned to pool
            foreach (GameObject gem in gems)
            {
                if (gem.transform.position.z < -5f)
                {
                    gem.SetActive(false);
                    gems.Remove(gem);
                    
                    break;
                }
            }
        }

        private void SpawnObstacle()
        {
            var obstacle = poolFactory.GetObstacle(ObstacleDifficulty.Easy);

            var pos = obstacle.transform.position;
            obstacle.transform.position = new Vector3(pos.x, pos.y, 25f);
            obstacle.transform.Rotate(0f, 0f, Random.Range(0, 360));
            
            obstacle.SetActive(true);
            
            obstacles.Add(obstacle);
        }

        private void SpawnGem()
        {
            var gem = poolFactory.GetObstacle("Gem");

            var pos = gem.transform.position;
            gem.transform.position = new Vector3(pos.x, pos.y, 25f + distanceBetweenObstacles/2f);
            gem.transform.Rotate(0f, 0f, Random.Range(0, 360));
            
            gem.SetActive(true);
            
            gems.Add(gem);
        }
        
        private void SpawnBoostpad()
        {
            var pad = poolFactory.GetObstacle("Boostpad");

            var pos = pad.transform.position;
            // 1.5f is half the depth of boostpad
            pad.transform.position = new Vector3(pos.x, pos.y, 25f + distanceBetweenObstacles/2f - 1.5f);
            pad.transform.Rotate(0f, 0f, Random.Range(0, 360));
            
            pad.SetActive(true);
            
            gems.Add(pad);
        }

        private void Reset()
        {
            foreach (GameObject o in obstacles)
            {
                o.SetActive(false);
            }
            
            obstacles.Clear();
            obstaclesPassed = 0;
            
            mainCharacter.Reset();
            
            movingObjectsWrapper.position = Vector3.zero;

            gameSpeed = 20f;
            
            CurrentScoreRX.Value = 0;
        }
    }
}
