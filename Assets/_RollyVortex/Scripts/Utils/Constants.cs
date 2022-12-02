using UnityEngine;

namespace _RollyVortex.Utils
{
    public static class Constants
    {
        public static class Game
        {
            // The default position for the character when starting the game
            public static readonly Vector3 DefaultCharacterPosition = new Vector3(-2.5f, 0f, 1f);
            
            // The default game speed
            public static readonly float DefaultGameSpeed = 15f;
            
            // The max game speed
            public static readonly float MaxGameSpeed = 27f;
            
            // Character rolling speed multiplier
            public static readonly float RollingSpeedMultiplier = 1.2f;
            
            // Character control ( movement left/right ) speed multiplier
            public static readonly float CharacterControlSpeedMultiplier = 0.5f;
            
            // Speed increase per obstacle passed
            public static readonly float SpeedIncreasePerObstaclePassed = 0.1f;

            // After what z-distance will obstacles start spawning
            public static readonly float FirstObstacleDistance = 5f;
            
            // Default Z-distance between obstaclesc
            public static readonly float DefaultDistanceBetweenObstacles = 10f;
            
            // Default Z-position to spawn new obstacles
            public static readonly float DefaultObstacleSpawnPositionZ = 20f;

            // How much the game speed will increase when moving over a Booster Pad
            public static readonly float BoosterPadSpeedMultiplier = 1.5f;

            // When the obstacle ( or gem ) passes this position ( outside of camera view ), it will be returned to the pool
            public static readonly float ReturnToPoolPositionZ = -8f;

            // Every X player score a new predefined section of obstacles will be spawned
            // ( between sections single random obstacles are spawned to vary the gameplay )
            public static readonly int SpawnObstacleSectionsEveryXObstacles = 8;
            
            // Every X obstacles their color will be changed
            public static readonly int ChangeObstaclesColorEveryXObstacles = 10;
            
            // Shield duration in seconds
            public static readonly int ShieldDuration = 2;
        }
    }
}
