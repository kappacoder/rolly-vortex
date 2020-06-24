using UnityEngine;
using System.Collections.Generic;
using RollyVortex.Scripts.Game.Models;
using RollyVortex.Scripts.Game.Components;

namespace RollyVortex.Scripts.Interfaces.Services
{
    public interface IEntitiesService
    {
        void Init(GameObject characterPrefab, Material tunnelMaterial);

        CharacterComponent GenerateCharacter(Transform parent, Vector3 position, int skinId = -1);

        CharacterSkinData GetCharacterSkinData(int id);

        void SetTunnelSkin(int id);
    }
}
