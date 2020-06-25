using Adic;
using Adic.Container;
using RollyVortex.Scripts.Game.Controllers;
using RollyVortex.Scripts.Interfaces.Game.Controllers;
using RollyVortex.Scripts.Utils;
using RollyVortex.Scripts.Services;
using RollyVortex.Scripts.Interfaces.Services;

namespace RollyVortex.Scripts.Game
{
    public class GameContainer : ContextRoot
    {
        public override void SetupContainers()
        {
            IInjectionContainer container = new InjectionContainer()
                .RegisterExtension<UnityBindingContainerExtension>()
                .RegisterExtension<EventCallerContainerExtension>()
                .RegisterExtension<AdicSharedContainerExtension>();

            IInjectionService injectionService = new InjectionService(container);

            container
                .Bind<IInjectionService>().To(injectionService)
                .Bind<IObstacleGenerationService>().ToSingleton<ObstacleGenerationService>()
                .Bind<IGameService>().ToSingleton<GameService>();

            BindControllers(container);

            AddContainer(container);
        }

        public override void Init() { }

        private void BindControllers(IInjectionContainer container)
        {
            container
                .Bind<ICharacterMovementController>().To<CharacterMovementController>()
                .Bind<ICharacterCollisionController>().To<CharacterCollisionController>()
                .Bind<ICharacterRollingController>().To<CharacterRollingController>()
                .Bind<IReturnToPoolController>().To<ReturnToPoolController>()
                .Bind<IObstacleOnEnableController>().To<ObstacleOnEnableController>()
                .Bind<IEndlessTunnelController>().To<EndlessTunnelController>();
        }
    }
}
