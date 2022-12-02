using UnityEngine;

namespace _RollyVortex.Interfaces.Controllers
{
    public interface IReturnToPoolController
    {
        void Init(Transform transform, float returnToPoolPositionZ, bool isObstacle);
    }
}
