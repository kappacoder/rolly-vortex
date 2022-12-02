using Adic;
using UnityEngine;
using UnityEngine.Scripting;
using System.Collections.Generic;
using _RollyVortex.Interfaces.Services;
using _RollyVortex.Models;

namespace _RollyVortex.Installers
{
    public class AssetsInstaller : MonoBehaviour
    {
        [Header("General")]
        public Transform GameWrapper;
        public GameObject CharacterPrefab;
        public Material TunnelMaterial;
        public Material ObstaclesMaterial;

        [Header("Obstacle Sections")]
        public List<ObstacleSectionData> ObstacleSections;
        public List<CharacterSkinData> CharacterSkins;
        public List<TunnelSkinData> TunnelSkins;
        public List<Color> ObstaclesColors;
        
        [Inject]
        private IGameService gameService;
        [Inject]
        private IEntitiesService entitiesService;
        [Inject]
        private IObstacleGenerationService obstacleGenerationService;

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
            entitiesService.Init(CharacterPrefab, TunnelMaterial, ObstaclesMaterial,
                ObstaclesColors, CharacterSkins, TunnelSkins);
            
            obstacleGenerationService.Init(ObstacleSections);
            
            gameService.Init(GameWrapper);
        }
    }
}
