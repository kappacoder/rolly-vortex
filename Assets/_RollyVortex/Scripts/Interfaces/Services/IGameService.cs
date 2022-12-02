using _RollyVortex.Services;
using UniRx;
using UnityEngine;

namespace _RollyVortex.Interfaces.Services
{
    public interface IGameService
    {
        IReactiveProperty<bool> IsReadyRX { get; }
        IReactiveProperty<bool> IsRunningRX { get; }
        IReactiveProperty<int> CurrentScoreRX { get; }
        
        float GameSpeed { get; }
        
        void Init(Transform gameWrapper);
        void StartRunning(GameMode gameMode);
        void Reset();
        void Dispose();
        void TriggerSpeedBoost();
    }
}
