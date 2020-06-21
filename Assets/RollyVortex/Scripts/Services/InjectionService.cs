using Adic.Container;
using UnityEngine.Scripting;
using RollyVortex.Scripts.Interfaces.Services;

namespace RollyVortex.Scripts.Services
{
    [Preserve]
    public class InjectionService : IInjectionService
    {
        public InjectionService(IInjectionContainer container)
        {
            Container = container;
        }

        public IInjectionContainer Container { get; private set; }
    }
}
