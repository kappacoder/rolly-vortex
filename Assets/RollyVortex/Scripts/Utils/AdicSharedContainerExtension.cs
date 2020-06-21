using Adic;
using System;
using System.Linq;
using UnityEngine;
using Adic.Binding;
using Adic.Container;
using Adic.Injection;
using UnityEngine.Scripting;
using System.Collections.Generic;
using RollyVortex.Scripts.Services;
using RollyVortex.Scripts.Interfaces.Services;

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
            // UnityEngine.Object.DontDestroyOnLoad(poolFactory);

            container
                .Bind<PoolFactory>().ToGameObject(poolFactory)
                .Bind<IEntitiesService>().ToSingleton<EntitiesService>();
            //         .Bind<IConfigurationService>().To(configurationService)
            //         .Bind<ISceneService>().ToSingleton<SceneService>()
            //         .Bind<IEntitiesUIService>().ToSingleton<EntitiesUIService>()
            //         //.Bind<IBundleManager>().ToSingleton<BundleManager>()
            //         .Bind<IRewardsUIService>().ToSingleton<RewardsUIService>()
            //         .Bind<INotificationsUIService>().ToSingleton<NotificationsUIService>()

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
