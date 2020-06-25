using Adic;
using UniRx;
using UnityEngine;
using System.Linq;
using UnityEngine.Scripting;
using RollyVortex.Scripts.Utils;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using RollyVortex.Scripts.Game.Models;
using RollyVortex.Scripts.Interfaces.Services;
using RollyVortex.Scripts.Interfaces.Game.Controllers;

namespace RollyVortex.Scripts.Services
{
    [Preserve]
    public class ObstacleGenerationService : IObstacleGenerationService
    {
        [Inject] 
        private IInjectionService injectionService;
        [Inject]
        private PoolFactory poolFactory;
        
        private List<ObstacleSectionData> obstacleSections;

        private ObstacleSectionData currentSection;
        private int currentSectionProgress = 0;
        
        private IList<GameObject> entities;
        
        private CompositeDisposable disposables;

        private int generatedObstacles = 0;
        
        public void Init(List<ObstacleSectionData> obstacleSections)
        {
            this.obstacleSections = obstacleSections;
            
            disposables = new CompositeDisposable();
            entities = new List<GameObject>();
        }

        public void Dispose()
        {
            disposables.Dispose();
            disposables.Clear();
        }

        public void Reset()
        {
            foreach (GameObject o in entities)
                o.SetActive(false);

            currentSection = null;
            currentSectionProgress = 0;
            generatedObstacles = 0;
            
            entities.Clear();
        }

        public void CheckForNextObstacle(int gameScore, float zDistancePassed)
        {
            // Check if we can spawn obstacles
            if (zDistancePassed - Constants.Game.FirstObstacleDistance < 0)
                return;
            
            // Check if enough obstacles are spawn
            if ((zDistancePassed - Constants.Game.FirstObstacleDistance) /
                Constants.Game.DefaultDistanceBetweenObstacles <= generatedObstacles)
                return;
            
            // Spawn a gem between obstacles with a small chance
            if (Random.Range(0, 10) == 0)
                SpawnEntity(ObstacleType.Gem, Constants.Game.DefaultObstacleSpawnPositionZ + Constants.Game.DefaultDistanceBetweenObstacles * 0.5f, false);
            
            // Spawn a boostpad with a small chance
            if (Random.Range(0, 15) == 0)
                SpawnEntity(ObstacleType.BoostPad, Constants.Game.DefaultObstacleSpawnPositionZ + Constants.Game.DefaultDistanceBetweenObstacles * 0.5f - 2.5f, false);
            
            // Check if a new section of obstacles needs to be created
            if (currentSection == null)
            {
                if (gameScore % Constants.Game.SpawnObstacleSectionsEveryXObstacles == 0)
                {
                    StartNewObstacleSection();

                    return;
                }
            }
            else
            {
                if (currentSectionProgress < currentSection.Obstacles.Count)
                {
                    SpawNextObstacleFromCurrentSection();
                    
                    return;
                }
            }

            // There isn't a predefined section of obstacles, spawn a random obstacle
            SpawnEntity(ObstacleType.Easy, Constants.Game.DefaultObstacleSpawnPositionZ, true);
            generatedObstacles++;
        }

        private void StartNewObstacleSection()
        {
            var easySections = obstacleSections.Where(section => section.Difficulty == ObstacleSectionDifficulty.Easy)
                .ToArray();

            currentSection = easySections[Random.Range(0, easySections.Length)];
            currentSectionProgress = 0;

            SpawNextObstacleFromCurrentSection();
        }

        private void SpawNextObstacleFromCurrentSection()
        {
            var obstacleData = currentSection.Obstacles[currentSectionProgress];
            
            SpawnEntity(obstacleData.Id, obstacleData.Rotation, true,
                obstacleData.RotatesOnEnable, obstacleData.IsRotating);

            generatedObstacles++;
            currentSectionProgress++;

            // End the current section
            if (currentSectionProgress >= currentSection.Obstacles.Count)
                currentSection = null;
        }

        private void SpawnEntity(string id, Vector3 rotation, bool isObstacle, bool rotatesOnEnable = false, bool isRotating = false)
        {
            var entity = poolFactory.GetObstacle(id);
            
            var pos = entity.transform.position;
            entity.transform.position = new Vector3(pos.x, pos.y, Constants.Game.DefaultObstacleSpawnPositionZ);
            entity.transform.localEulerAngles = rotation;
            
            BindControllersToEntity(entity.transform, rotatesOnEnable, isObstacle);
            
            entities.Add(entity);
            entity.SetActive(true);
        }

        private void SpawnEntity(ObstacleType obstacleType, float positionZ, bool isObstacle)
        {
            var entity = poolFactory.GetObstacle(obstacleType);
            
            var pos = entity.transform.position;
            entity.transform.position = new Vector3(pos.x, pos.y, positionZ);
            entity.transform.Rotate(0f, 0f, Random.Range(0, 360));

            BindControllersToEntity(entity.transform, false, isObstacle);
            
            entities.Add(entity);
            entity.SetActive(true);
        }

        private void BindControllersToEntity(Transform entity, bool rotatesOnEnable, bool isObstacle)
        {
            injectionService.Container.Resolve<IReturnToPoolController>().Init(entity, Constants.Game.ReturnToPoolPositionZ, isObstacle);
            injectionService.Container.Resolve<IObstacleOnEnableController>().Init(entity, rotatesOnEnable);
        }
    }
}
