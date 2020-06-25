using System;
using UnityEngine;
using SubjectNerd.Utilities;
using System.Collections.Generic;

namespace RollyVortex.Scripts.Game.Models
{
    public enum ObstacleSectionDifficulty
    {
        Easy,
        Medium,
        Hard
    }
    
    [Serializable]
    [CreateAssetMenu(menuName = "Rolly Vortex/New Obstacle Section", fileName = "Section")]
    public class ObstacleSectionData : ScriptableObject
    {
        public ObstacleSectionDifficulty Difficulty;
        
        public List<ObstacleData> Obstacles;
    }

    [Serializable]
    public class ObstacleData
    {
        public string Id;

        public Vector3 Rotation;

        public bool RotatesOnEnable;
        public bool IsRotating;
    }
}
