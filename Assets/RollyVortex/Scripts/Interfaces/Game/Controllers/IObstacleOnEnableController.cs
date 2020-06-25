using UnityEngine;

namespace RollyVortex.Scripts.Interfaces.Game.Controllers
{
    public interface IObstacleOnEnableController
    {
        void Init(Transform transform, bool rotate);
    }
}
