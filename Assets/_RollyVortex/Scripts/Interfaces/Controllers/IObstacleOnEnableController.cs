using UnityEngine;

namespace _RollyVortex.Interfaces.Controllers
{
    public interface IObstacleOnEnableController
    {
        void Init(Transform transform, bool rotate);
    }
}
