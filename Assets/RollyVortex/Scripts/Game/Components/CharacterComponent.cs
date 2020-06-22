using UniRx;
using UnityEngine;

namespace RollyVortex.Scripts.Game.Components
{
    public enum CharacterState
    {
        Unknown,
        Alive,
        Invincible,
        Dead
    }
    
    public class CharacterComponent : MonoBehaviour
    {
        public Transform Body;
        public SphereCollider Collider;
        
        public IReactiveProperty<CharacterState> StateRX { get; private set; }

        private Vector3 defaultCharacterRotation;
        
        public void Reset()
        {
            transform.eulerAngles = defaultCharacterRotation;

            StateRX.Value = CharacterState.Unknown;
        }
        
        private void Awake()
        {
            StateRX = new ReactiveProperty<CharacterState>();

            defaultCharacterRotation = transform.eulerAngles;
        }
    }
}
