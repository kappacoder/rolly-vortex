using RollyVortex.Scripts.Services;
using UniRx;
using UnityEngine;

namespace RollyVortex.Scripts.Interfaces.Services
{
    public interface IGameService
    {
        IReactiveProperty<bool> IsRunningRX { get; }
        
        IReactiveProperty<int> CurrentScoreRX { get; }
        
        float GameSpeed { get; }
        
        void Init(Transform gameWrapper);

        void StartRunning(GameMode gameMode);

        void Dispose();

        void TriggerSpeedBoost();
    }
}
