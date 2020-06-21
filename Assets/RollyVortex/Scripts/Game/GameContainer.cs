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
                .Bind<IGameService>().ToSingleton<GameService>();
                    // .Bind<ITouchService>().ToSingleton<TouchService>()
                    // .Bind<IGameService>().ToSingleton<GameServiceTest>()
                    // .Bind<IGameboardUIService>().ToSingleton<GameboardUIService>()
                    // .Bind<IAnimationService>().ToSingleton<AnimationService>()
                    // .Bind<IInitAnimations>().ToSingleton<InitAnimations>();

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
                .Bind<IEndlessTunnelController>().To<EndlessTunnelController>();

            //     .Bind<ICardPlayedIndicationController>().To<CardPlayedIndicationController>();
        }
    }
}
