using Adic;
using Adic.Binding;
using Adic.Container;
using Adic.Injection;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting; // using CityGangs.Scripts.Interfaces.Models.Configuration;
// using CityGangs.Scripts.Interfaces.Services.Configuration;
// using CityGangs.Scripts.Interfaces.Services.Core;
// using CityGangs.Scripts.Interfaces.Services.UI;
// using CityGangs.Scripts.Models.Configuration;
// using CityGangs.Scripts.Services.Core;
// using CityGangs.Scripts.Services.Configuration;
// using CityGangs.Scripts.Services.Test;
// using CityGangs.Scripts.Services.UI;

namespace RollyVortex.Scripts.Utils
{
    [Preserve]
    public class AdicSharedContainerExtension : IContainerExtension
    {
        private static IInjectionContainer sharedContainer;

        public void Init(IInjectionContainer container)
        {
        }

        public void OnRegister(IInjectionContainer container)
        {
            container.beforeResolve += BeforeResolve;
        }

        public void OnUnregister(IInjectionContainer container)
        {
            container.beforeResolve -= BeforeResolve;
        }

        private static IInjectionContainer CreateSharedContainer()
        {
            //string configDebugFile = Path.Combine(Application.dataPath, "CityGangs/JSON/Game.Config.json");
            // IGameConfiguration configuration = new Configuration.GameConfiguration();
            // IConfigurationService configurationService = new ConfigurationService(configuration);

            var container = new InjectionContainer()
                .RegisterExtension<UnityBindingContainerExtension>();

            //string serverEndpoint = @"http://localhost:8080/";

            GameObject poolFactory = GameObject.Find("SharedComponent/PoolFactory");
            //UnityEngine.Object.DontDestroyOnLoad(poolFactory);

            // container
            //         //.Bind<IBundleManager>().ToSingleton<BundleManager>()
            //         .Bind<IConfigurationService>().To(configurationService)
            //         .Bind<IChilliConnectService>().ToSingleton<ChilliConnectServiceTest>()
            //         .Bind<ISceneService>().ToSingleton<SceneService>()
            //         .Bind<IEntitiesUIService>().ToSingleton<EntitiesUIService>()
            //         .Bind<IRewardsUIService>().ToSingleton<RewardsUIService>()
            //         .Bind<INotificationsUIService>().ToSingleton<NotificationsUIService>()
            //         .Bind<PoolFactory>().ToGameObject(poolFactory);

            return container;
        }

        private bool BeforeResolve(IInjector source, Type type, InjectionMember member, object parentInstance, object identifier, ref object resolutionInstance)
        {
            if (sharedContainer == null)
            {
                sharedContainer = CreateSharedContainer();
            }

            IList<BindingInfo> bindings;

            if (type.IsInterface)
            {
                bindings = sharedContainer.GetBindingsFor(type);
            }
            else
            {
                bindings = sharedContainer.GetBindingsTo(type);
            }

            if (bindings == null || !bindings.Any())
            {
                return true;
            }

            resolutionInstance = sharedContainer.Resolve(type);

            return false;
        }
    }
}
