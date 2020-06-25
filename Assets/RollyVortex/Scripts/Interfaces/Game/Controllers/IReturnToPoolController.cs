using UnityEngine;

namespace RollyVortex.Scripts.Interfaces.Game.Controllers
{
    public interface IReturnToPoolController
    {
        void Init(Transform transform, float returnToPoolPositionZ, bool isObstacle);
    }
}
