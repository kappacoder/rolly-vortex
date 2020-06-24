using Adic;
using RollyVortex.Scripts.Game.Models;
using UnityEngine;
using UnityEngine.Scripting;
using RollyVortex.Scripts.Interfaces.Services;
using RollyVortex.Scripts.Services;
using SubjectNerd.Utilities;
using System.Collections.Generic;

namespace RollyVortex.Scripts.Game.Components
{
    public class AssetsSetupComponent : MonoBehaviour
    {
        [Header("General")]
        public Transform GameWrapper;
        public GameObject CharacterPrefab;
        public Material TunnelMaterial;

        // [Header("Skins")]
        [Reorderable("", true, true)] public List<CharacterSkinData> CharacterSkins;
        [Reorderable("", true, true)] public List<TunnelSkinData> TunnelSkins;
        
        // game configuration
        
        [Inject]
        private IGameService gameService;
        [Inject]
        private IEntitiesService entitiesService;

        void Awake()
        {
            // Hacky way to have the skins loaded as static members
            EntitiesService.CharacterSkins = CharacterSkins;
            EntitiesService.TunnelSkins = TunnelSkins;
        }
        
        void Start()
        {
            this.Inject();
        }

        [Inject, Preserve]
        void PostConstruct()
        {
            InitServices();
        }

        private void InitServices()
        {
            entitiesService.Init(CharacterPrefab, TunnelMaterial);
            gameService.Init(GameWrapper);
        }
    }
}
