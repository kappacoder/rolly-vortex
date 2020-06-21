using RollyVortex.Scripts.Game.Components;
using UnityEngine;

namespace RollyVortex.Scripts.Interfaces.Services
{
    public interface IEntitiesService
    {
        void Init(GameObject characterPrefab);

        CharacterComponent GenerateCharacter(Transform parent, Vector3 position);
    }
}
