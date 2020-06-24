using UniRx;

namespace RollyVortex.Scripts.Interfaces.Services
{
    public interface IUserService
    {
        IReactiveProperty<int> SelectedCharacterSkinRX { get; }
        
        IReactiveProperty<int> SelectedTunnelSkinRX { get; }

        IReactiveProperty<int> GemsRX { get; }
    }
}
