using _RollyVortex.Controllers;
using _RollyVortex.Interfaces.Controllers;
using _RollyVortex.Interfaces.Services;
using _RollyVortex.Services;
using _RollyVortex.Utils;
using Adic;
using Adic.Container;

namespace _RollyVortex.Installers
{
    public class GameInstaller : ContextRoot
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
