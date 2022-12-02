using UnityEngine;
using System.Collections.Generic;
using _RollyVortex.Views;
using _RollyVortex.Models;

namespace _RollyVortex.Interfaces.Services
{
    public interface IEntitiesService
    {
        List<CharacterSkinData> CharacterSkins { get; }
        List<TunnelSkinData> TunnelSkins { get; }

        void Init(GameObject characterPrefab, Material tunnelMaterial, Material obstaclesMaterial,
            List<Color> obstaclesColors, List<CharacterSkinData> characterSkins, List<TunnelSkinData> tunnelSkins);

        CharacterView GenerateCharacter(Transform parent, Vector3 position, int skinId = -1);

        CharacterSkinData GetCharacterSkinData(int id);

        void SetTunnelSkin(int id);

        void ChangeObstaclesColor();
    }
}
