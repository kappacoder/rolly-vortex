﻿using Adic;
using UniRx;
using System;
using UnityEngine;
using RollyVortex.Scripts.Interfaces.Services;

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
        public ISubject<bool> OnSkinChangedRX { get; private set; }
        
        public Transform Body;
        public GameObject Shield;
        public Collider Collider;
        
        public IReactiveProperty<CharacterState> StateRX { get; private set; }

        [Inject] 
        private IGameService gameService;
        [Inject]
        private IEntitiesService entitiesService;
        [Inject]
        private IUserService userService;
        
        private Vector3 defaultCharacterRotation;

        public void SetSkin(int id)
        {
            var skinData = entitiesService.GetCharacterSkinData(id);

            if (skinData == null)
                return;
            
            GameObject newBody = Instantiate(skinData.Body, transform);
            DestroyImmediate(Body.gameObject);

            Body = newBody.transform;
            Collider = Body.GetComponent<Collider>();
            
            OnSkinChangedRX.OnNext(true);
        }
        
        public void Reset()
        {
            Body.gameObject.SetActive(true);
            
            transform.eulerAngles = defaultCharacterRotation;

            StateRX.Value = CharacterState.Unknown;
        }
        
        private void Awake()
        {
            StateRX = new ReactiveProperty<CharacterState>();
            OnSkinChangedRX = new Subject<bool>();

            defaultCharacterRotation = transform.eulerAngles;
        }

        private void Start()
        {
            this.Inject();;
        }

        [Inject]
        private void PostConstruct()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            StateRX.Where(x => x == CharacterState.Invincible)
                .Subscribe(x =>
                {
                    Shield.SetActive(true);

                    Observable.Timer(TimeSpan.FromSeconds(2))
                        .Where(_ => gameService.IsRunningRX.Value)
                        .Subscribe(_ =>
                        {
                            Shield.SetActive(false);
                        })
                        .AddTo(this);
                })
                .AddTo(this);

            userService.SelectedCharacterSkinRX
                .Subscribe(x => SetSkin(x))
                .AddTo(this);
        }
    }
}
