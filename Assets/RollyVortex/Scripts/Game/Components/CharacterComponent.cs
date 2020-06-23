using Adic;
using DG.Tweening;
using RollyVortex.Scripts.Interfaces.Services;
using System;
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
        public GameObject Shield;
        public SphereCollider Collider;
        
        public IReactiveProperty<CharacterState> StateRX { get; private set; }

        [Inject] 
        private IGameService gameService;
        
        private Vector3 defaultCharacterRotation;
        
        public void Reset()
        {
            Body.gameObject.SetActive(true);
            
            transform.eulerAngles = defaultCharacterRotation;

            StateRX.Value = CharacterState.Unknown;
        }
        
        private void Awake()
        {
            StateRX = new ReactiveProperty<CharacterState>();

            defaultCharacterRotation = transform.eulerAngles;

            Subscribe();
        }

        private void Start()
        {
            this.Inject();;
        }

        private void Subscribe()
        {
            StateRX.Where(x => x == CharacterState.Invincible)
                .Subscribe(x =>
                {
                    Shield.SetActive(true);
                    
                    // it should automatically play the clip
                    
                    Observable.Timer(TimeSpan.FromSeconds(2))
                        .Where(_ => gameService.IsRunningRX.Value)
                        .Subscribe(_ =>
                        {
                            Shield.SetActive(false);
                        })
                        .AddTo(this);
                })
                .AddTo(this);
        }

        private void StartBlinking()
        {
            var material = Shield.GetComponent<MeshRenderer>().material;

            Color currentColor = material.GetColor("_BaseColor");
            // currentColor
            
            // VERY UGLY AND HACKY WAY TO BLINK COLOR >>>
            
            DOTween.To(() => 1f, x =>
            {
                currentColor.a = x;
                material.SetColor("_BaseColor", currentColor);
            }, 0.5f, 0.2f).Play();
            
            DOTween.To(() => 0.5f, x =>
            {
                currentColor.a = x;
                material.SetColor("_BaseColor", currentColor);
            }, 1f, 0.2f)
                .SetDelay(0.2f).Play();
            
            DOTween.To(() => 1f, x =>
                {
                    currentColor.a = x;
                    material.SetColor("_BaseColor", currentColor);
                }, 0.5f, 0.2f)
                .SetDelay(0.4f).Play();
            
            DOTween.To(() => 0.5f, x =>
                {
                    currentColor.a = x;
                    material.SetColor("_BaseColor", currentColor);
                }, 1f, 0.2f)
                .SetDelay(0.6f).Play();
            
            DOTween.To(() => 1f, x =>
                {
                    currentColor.a = x;
                    material.SetColor("_BaseColor", currentColor);
                }, 0f, 0.2f)
                .SetDelay(0.8f).Play();
        }
    }
}
