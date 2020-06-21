using RollyVortex.Scripts.Game.Components;
using UnityEngine;
using UnityEngine.Scripting;
using RollyVortex.Scripts.Interfaces.Services;

namespace RollyVortex.Scripts.Services
{
    [Preserve]
    public class EntitiesService : IEntitiesService
    {
        private GameObject characterPrefab;
    
        public void Init(GameObject characterPrefab)
        {
            this.characterPrefab = characterPrefab;
        }

        public CharacterComponent GenerateCharacter(Transform parent, Vector3 position)
        {
            var character = Object.Instantiate(characterPrefab, parent);
            character.transform.localPosition = position;
            character.name = "Character";

            return character.GetComponent<CharacterComponent>();
        }

        // public ObstacleComponent GenerateObstacle()
        // {
        //     
        // }
    }
}
