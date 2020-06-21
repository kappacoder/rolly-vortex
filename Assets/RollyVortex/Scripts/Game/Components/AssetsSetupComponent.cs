using Adic;
using UnityEngine;
using UnityEngine.Scripting;
using RollyVortex.Scripts.Interfaces.Services;

namespace RollyVortex.Scripts.Game.Components
{
    public class AssetsSetupComponent : MonoBehaviour
    {
        public Transform GameWrapper;

        public GameObject CharacterPrefab;

        [Inject]
        private IGameService gameService;
        [Inject]
        private IEntitiesService entitiesService;
    
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
            entitiesService.Init(CharacterPrefab);
            gameService.Init(GameWrapper);
        }
    }
}
