using System.Collections.Generic;
using RollyVortex.Scripts.Game.Models;

namespace RollyVortex.Scripts.Interfaces.Services
{
    public interface IObstacleGenerationService
    {
        void Init(List<ObstacleSectionData> obstacleSections);

        void Reset();

        void CheckForNextObstacle(int obstaclesPassed, float zDistancePassed);
    }
}
