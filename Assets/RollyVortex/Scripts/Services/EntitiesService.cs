﻿using UnityEngine;
using System.Linq;
using UnityEngine.Scripting;
using System.Collections.Generic;
using RollyVortex.Scripts.Game.Models;
using RollyVortex.Scripts.Game.Components;
using RollyVortex.Scripts.Interfaces.Services;

namespace RollyVortex.Scripts.Services
{
    [Preserve]
    public class EntitiesService : IEntitiesService
    {
        public static List<CharacterSkinData> CharacterSkins { get; set; }
        public static List<TunnelSkinData> TunnelSkins { get; set; }

        private GameObject characterPrefab;
        private Material tunnelMaterial;
    
        public void Init(GameObject characterPrefab, Material tunnelMaterial)
        {
            this.characterPrefab = characterPrefab;

            this.tunnelMaterial = tunnelMaterial;
        }

        public CharacterComponent GenerateCharacter(Transform parent, Vector3 position, int skinId = -1)
        {
            var obj = Object.Instantiate(characterPrefab, parent);
            obj.transform.localPosition = position;
            obj.name = "Character";

            var character = obj.GetComponent<CharacterComponent>();
            
            if (skinId != -1)
                character.SetSkin(skinId);

            return character;
        }

        public CharacterSkinData GetCharacterSkinData(int id)
        {
            return CharacterSkins.FirstOrDefault(x => x.Id == id);
        }

        public void SetTunnelSkin(int id)
        {
            var skinData = TunnelSkins.FirstOrDefault(x => x.Id == id);

            if (skinData == null)
                return;
            
            tunnelMaterial.SetTexture("_BaseMap", skinData.Texture);
        }
    }
}
