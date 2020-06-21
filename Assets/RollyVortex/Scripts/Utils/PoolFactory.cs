using System;
using UnityEngine;
using System.Linq;
using SubjectNerd.Utilities;
using System.Collections.Generic;

namespace RollyVortex.Scripts.Utils
{
    public enum ObstacleDifficulty
    {
        None,
        Easy,
        Medium,
        Hard
    }

    public class PoolFactory : MonoBehaviour
    {
        public Transform ObstaclesTransform;
        
        [Reorderable("", true, true)] public List<ObstaclePoolData> ObstaclesPoolConfig;

        private Dictionary<object, ObjectPool> pools;

        /// <summary>
        /// Get a specific obstacle by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GameObject GetObstacle(string id)
        {
            try
            {
                return pools[id].GetObject();
            }
            catch
            {
                Debug.LogWarning("An obstacle with the ID " + id +
                    " has not been added to the pool. Add it from SharedComponent -> PoolFactory -> ObstaclesPoolConfig");

                return null;
            }
        }
        
        /// <summary>
        /// Get a random obstacle by difficulty
        /// </summary>
        /// <param name="difficulty"></param>
        /// <returns></returns>
        public GameObject GetObstacle(ObstacleDifficulty difficulty)
        {
            try
            {
                var obstaclesData = ObstaclesPoolConfig.Where(x => x.Difficulty == difficulty)
                    .ToArray();

                string id = obstaclesData[UnityEngine.Random.Range(0, obstaclesData.Length)].Id;

                return pools[id].GetObject();
            }
            catch
            {
                Debug.LogWarning("An object with the difficulty " + difficulty +
                    " has not been added to the pool. Add it from SharedComponent -> PoolFactory -> ObstaclesPoolConfig");

                return null;
            }
        }

        protected void Awake()
        {
            pools = new Dictionary<object, ObjectPool>();

            PreloadObstacles();
        }

        private void PreloadObstacles()
        {
            foreach (var objData in ObstaclesPoolConfig)
            {
                objData.Id = objData.Prefab.name;
                
                var wrapper = new GameObject().transform;

                wrapper.name = objData.Id;
                wrapper.SetParent(ObstaclesTransform, false);

                pools[objData.Id] = new ObjectPool(objData.Prefab, objData.Count, wrapper);
            }
        }
    }

    [Serializable]
    public class ObjectPoolData
    {
        public int Count;
        public GameObject Prefab;
        [HideInInspector] public string Id;
    }

    [Serializable]
    public class ObstaclePoolData : ObjectPoolData
    {
        public ObstacleDifficulty Difficulty;
    }
}