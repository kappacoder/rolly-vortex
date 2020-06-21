using System;
using UnityEngine;
using SubjectNerd.Utilities;
using System.Collections.Generic;

namespace RollyVortex.Scripts.Utils
{
    public enum ParticleTypes
    {
        // Per faction
        StreetKidsSkateboard,
        StreetKidsSprayCan,
        MafiaKnife,
        BrainiacsDefault,
        MysticsDefault,
        EliteDefault,
        GuardiansDefault,
        ArmyDefault,
        CelebritiesDefault,
        SportsDefault
    }

    public class PoolFactory : MonoBehaviour
    {
        [Reorderable("", true, true)] public List<ParticlePoolData> ParticlesPoolConfig;

        private Dictionary<object, ObjectPool> pools;

        public GameObject GetObject(ParticleTypes id)
        {
            try
            {
                return pools[id].GetObject();
            }
            catch
            {
                Debug.LogWarning("An object with the ID " + id +
                    " has not been added to the pool. Add it from Splash -> SharedComponent -> PoolFactory -> objectsToPool");

                return null;
            }
        }

        protected void Awake()
        {
            pools = new Dictionary<object, ObjectPool>();

            PreloadParticles();
        }

        private void PreloadParticles()
        {
            var particlesWrapper = new GameObject().transform;

            particlesWrapper.name = "Particles";
            particlesWrapper.SetParent(transform, false);

            foreach (var objData in ParticlesPoolConfig)
            {
                var wrapper = new GameObject().transform;

                wrapper.name = objData.Id.ToString();
                wrapper.SetParent(particlesWrapper, false);

                pools[objData.Id] = new ObjectPool(objData.Prefab, objData.Count, wrapper);
            }
        }
    }

    [Serializable]
    public class ObjectPoolData
    {
        public int Count;
        public GameObject Prefab;
    }

    [Serializable]
    public class ParticlePoolData : ObjectPoolData
    {
        public ParticleTypes Id;
    }
}