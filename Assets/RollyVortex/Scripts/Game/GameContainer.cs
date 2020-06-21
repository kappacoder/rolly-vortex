using Adic;
using Adic.Container;
using RollyVortex.Scripts.Utils;

// using CityGangs.Scripts.Services.Core;
// using CityGangs.Scripts.Services.Game;
// using CityGangs.Scripts.Services.Configuration;
// using CityGangs.Scripts.Controllers.Entities.Cards;
// using CityGangs.Scripts.Game.Animations;
// using CityGangs.Scripts.Interfaces.Controllers.Entities.Cards;
// using CityGangs.Scripts.Interfaces.Services.Animation;
// using CityGangs.Scripts.Interfaces.Services.Configuration;
// using CityGangs.Scripts.Interfaces.Services.Core;
// using CityGangs.Scripts.Interfaces.Services.Game;
// using CityGangs.Scripts.Interfaces.Services.UI;
// using CityGangs.Scripts.Services.Animation;
// using CityGangs.Scripts.Services.Test;
// using CityGangs.Scripts.Services.UI;

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

            // IInjectionService injectionService = new InjectionService(container);

            // container
                    // .Bind<IInjectionService>().To(injectionService)
                    // .Bind<IColyseusService>().ToSingleton<ColyseusService>()
                    // .Bind<ITouchService>().ToSingleton<TouchService>()
                    // .Bind<IGameService>().ToSingleton<GameServiceTest>()
                    // .Bind<IGameboardUIService>().ToSingleton<GameboardUIService>()
                    // .Bind<IAnimationService>().ToSingleton<AnimationService>()
                    // .Bind<IInitAnimations>().ToSingleton<InitAnimations>();

            BindHeroCardControllers(container);

            AddContainer(container);
        }

        public override void Init() { }

        private void BindHeroCardControllers(IInjectionContainer container)
        {
            // container
            //     .Bind<ICardBoardViewTouchController>().To<CardBoardViewTouchController>()
            //     .Bind<ICardPlayedIndicationController>().To<CardPlayedIndicationController>();
        }
    }
}
