using System.Collections.Generic;
using UnityEngine;

namespace _RollyVortex.Utils
{
    public class ObjectPool
    {
        private readonly GameObject prefab;
        private readonly Transform parent;
        private readonly List<GameObject> pool;

        public ObjectPool(GameObject prefab, int poolCount, Transform parent)
        {
            this.prefab = prefab;
            this.parent = parent;

            pool = new List<GameObject>();

            for (var i = 0; i < poolCount; i++)
            {
                var obj = Object.Instantiate(this.prefab, this.parent, false);
                obj.SetActive(false);
                
                pool.Add(obj);
            }
        }

        public GameObject GetObject()
        {
            foreach (var obj in pool)
                if (!obj.activeInHierarchy)
                    return obj;

            var newObj = Object.Instantiate(prefab, parent);
            pool.Add(newObj);

            return newObj;
        }
    }
}
