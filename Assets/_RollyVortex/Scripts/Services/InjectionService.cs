using _RollyVortex.Interfaces.Services;
using Adic.Container;
using UnityEngine.Scripting;

namespace _RollyVortex.Services
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
