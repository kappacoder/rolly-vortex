using Adic;
using UnityEngine;
using UnityEngine.Scripting;
using SubjectNerd.Utilities;
using System.Collections.Generic;
using RollyVortex.Scripts.Services;
using RollyVortex.Scripts.Game.Models;
using RollyVortex.Scripts.Interfaces.Services;

namespace RollyVortex.Scripts.Game.Components
{
    public class AssetsSetupComponent : MonoBehaviour
    {
        [Header("General")]
        public Transform GameWrapper;
        public GameObject CharacterPrefab;
        public Material TunnelMaterial;
        public Material ObstaclesMaterial;

        [Header("Obstacle Sections")]
        public List<ObstacleSectionData> ObstacleSections;

        [Reorderable("", true, true)] public List<CharacterSkinData> CharacterSkins;
        [Reorderable("", true, true)] public List<TunnelSkinData> TunnelSkins;

        public List<Color> ObstaclesColors;
        
        [Inject]
        private IGameService gameService;
        [Inject]
        private IEntitiesService entitiesService;
        [Inject]
        private IObstacleGenerationService obstacleGenerationService;

        void Awake()
        {
            // Hacky way to have the skins loaded as static members
            // FIX TODO:
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
            entitiesService.Init(CharacterPrefab, TunnelMaterial, ObstaclesMaterial, ObstaclesColors);
            obstacleGenerationService.Init(ObstacleSections);
            gameService.Init(GameWrapper);
        }
    }
}
