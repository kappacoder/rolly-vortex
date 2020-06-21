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

        private void Awake()
        {
            StateRX = new ReactiveProperty<CharacterState>();
        }
    }
}
