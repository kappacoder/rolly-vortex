using System.Collections.Generic;
using _RollyVortex.Models;

namespace _RollyVortex.Interfaces.Services
{
    public interface IObstacleGenerationService
    {
        void Init(List<ObstacleSectionData> obstacleSections);
        void Reset();
        void CheckForNextObstacle(int obstaclesPassed, float zDistancePassed);
    }
}
