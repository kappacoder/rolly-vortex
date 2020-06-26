using Adic;
using UniRx;
using System;
using UnityEngine;
using UnityEngine.Scripting;
using RollyVortex.Scripts.Utils;
using RollyVortex.Scripts.Game.Components;
using RollyVortex.Scripts.Interfaces.Services;
using RollyVortex.Scripts.Interfaces.Game.Controllers;

namespace RollyVortex.Scripts.Services
{
    public enum GameMode
    {
        Endless,
        Challenge
    }
    
    [Preserve]
    public class GameService : IGameService
    {
        public IReactiveProperty<bool> IsReadyRX { get; } = new ReactiveProperty<bool>();
        
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
        private IObstacleGenerationService obstacleGenerationService;
        [Inject]
        private IInjectionService injectionService;
        [Inject]
        private PoolFactory poolFactory;
        
        private Transform movingObjectsWrapper;
        private CharacterComponent mainCharacter;

        private float gameSpeed = Constants.Game.DefaultGameSpeed;

        private CompositeDisposable disposables;

        public void Init(Transform gameWrapper)
        {
            disposables = new CompositeDisposable();
            
            movingObjectsWrapper = gameWrapper.Find("MovingObjects");

            GenerateMainCharacter(gameWrapper);
            BindGameControllers();
             
            Subscribe();

            IsReadyRX.Value = true;
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
                case GameMode.Challenge:
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void Reset()
        {
            obstacleGenerationService.Reset();
            mainCharacter.Reset();
            
            movingObjectsWrapper.position = Vector3.zero;

            gameSpeed = Constants.Game.DefaultGameSpeed;
            
            CurrentScoreRX.Value = 0;
        }
        
        private IDisposable speedBoostDisposable1;
        private IDisposable speedBoostDisposable2;
        
        public void TriggerSpeedBoost()
        {
            // If you trigger 2 speed boosts one after another,
            // dispose of previous observables
            speedBoostDisposable1?.Dispose();
            speedBoostDisposable2?.Dispose();

            gameSpeed *= Constants.Game.BoosterPadSpeedMultiplier;

            // return to normal speed after X-1 seconds
            speedBoostDisposable1 = Observable.Timer(TimeSpan.FromSeconds(Constants.Game.ShieldDuration - 1f))
                .Where(x => IsRunningRX.Value)
                .Subscribe(x => UpdateSpeed())
                .AddTo(disposables);
            
            // disable shield after X seconds
            speedBoostDisposable2 = Observable.Timer(TimeSpan.FromSeconds(Constants.Game.ShieldDuration))
                .Where(x => IsRunningRX.Value)
                .Subscribe(x =>
                {
                    mainCharacter.StateRX.Value = CharacterState.Alive;
                })
                .AddTo(disposables);
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
                })
                .AddTo(disposables);

            CurrentScoreRX
                .Subscribe(x =>
                {
                    if (x % Constants.Game.ChangeObstaclesColorEveryXObstacles == 0)
                        entitiesService.ChangeObstaclesColor();
                    
                    if (mainCharacter.StateRX.Value != CharacterState.Invincible ||
                        mainCharacter.StateRX.Value != CharacterState.Dead)
                        UpdateSpeed();
                })
                .AddTo(disposables);
        }

        private void OnFixedUpdate()
        {
            // Move obstacles towards the camera
            movingObjectsWrapper.position -= new Vector3(0f, 0f, gameSpeed * Time.fixedDeltaTime);

            // Check if a new obstacles needs to be created
            obstacleGenerationService.CheckForNextObstacle(CurrentScoreRX.Value, movingObjectsWrapper.position.z * -1);
        }

        private void GenerateMainCharacter(Transform parent)
        {
            mainCharacter = entitiesService.GenerateCharacter(parent, Constants.Game.DefaultCharacterPosition);
            
            injectionService.Container.Resolve<ICharacterMovementController>().Init(mainCharacter);
            injectionService.Container.Resolve<ICharacterCollisionController>().Init(mainCharacter);
            injectionService.Container.Resolve<ICharacterRollingController>().Init(mainCharacter);
        }

        private void BindGameControllers()
        {
            injectionService.Container.Resolve<IEndlessTunnelController>().Init(movingObjectsWrapper.Find("Tunnels"));
        }

        private void UpdateSpeed()
        {
            gameSpeed = Math.Min(Constants.Game.DefaultGameSpeed + CurrentScoreRX.Value * Constants.Game.SpeedIncreasePerObstaclePassed, Constants.Game.MaxGameSpeed);
        }
    }
}
