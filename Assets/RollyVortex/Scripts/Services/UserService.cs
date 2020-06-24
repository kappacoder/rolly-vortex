using UniRx;
using UnityEngine.Scripting;
using RollyVortex.Scripts.Interfaces.Services;

namespace RollyVortex.Scripts.Services
{
    [Preserve]
    public class UserService : IUserService
    {
        public IReactiveProperty<int> SelectedCharacterSkinRX { get; private set; }
        
        public IReactiveProperty<int> SelectedTunnelSkinRX { get; private set; }

        public IReactiveProperty<int> GemsRX { get; private set; }

        public UserService()
        {
            SelectedCharacterSkinRX = new ReactiveProperty<int>(StorageService.GetInt(PlayerPrefInts.SelectedCharacterSkin, 0));
            SelectedTunnelSkinRX = new ReactiveProperty<int>(StorageService.GetInt(PlayerPrefInts.SelectedTunnelSkin, 0));
            GemsRX = new ReactiveProperty<int>(StorageService.GetInt(PlayerPrefInts.Gems, 0));

            Subscribe();
        }

        private void Subscribe()
        {
            SelectedCharacterSkinRX
                .Subscribe(x =>
                {
                    StorageService.SetInt(PlayerPrefInts.SelectedCharacterSkin, x);
                });
            
            SelectedTunnelSkinRX
                .Subscribe(x =>
                {
                    StorageService.SetInt(PlayerPrefInts.SelectedTunnelSkin, x);
                });
            
            GemsRX
                .Subscribe(x =>
                {
                    StorageService.SetInt(PlayerPrefInts.Gems, x);
                });
        }
    }
}
