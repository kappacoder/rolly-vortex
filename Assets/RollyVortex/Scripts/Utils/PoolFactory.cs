using System;
using UnityEngine;
using System.Linq;
using SubjectNerd.Utilities;
using System.Collections.Generic;

namespace RollyVortex.Scripts.Utils
{
    public enum ObstacleType
    {
        Easy,
        Medium,
        Hard,
        Gem,
        BoostPad,
        ObstacleExplosion,
        CharacterExplosion
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
        /// Get a random obstacle by obstacleType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public GameObject GetObstacle(ObstacleType type)
        {
            try
            {
                var obstaclesData = ObstaclesPoolConfig.Where(x => x.obstacleType == type)
                    .ToArray();

                string id = obstaclesData[UnityEngine.Random.Range(0, obstaclesData.Length)].Id;

                return pools[id].GetObject();
            }
            catch
            {
                Debug.LogWarning("An object with the obstacleType " + type +
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
        public ObstacleType obstacleType;
    }
}