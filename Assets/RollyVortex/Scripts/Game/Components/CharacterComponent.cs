using Adic;
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
        public GameObject ParticlesVFX;
        public Collider Collider;
        
        public IReactiveProperty<CharacterState> StateRX { get; private set; }

        [Inject] 
        private IGameService gameService;
        [Inject]
        private IEntitiesService entitiesService;
        [Inject]
        private IUserService userService;
        
        private Vector3 defaultCharacterRotation;

        private IDisposable shieldDisposable;

        private MeshRenderer currentBodyRenderer;
        
        public void SetSkin(int id)
        {
            var skinData = entitiesService.GetCharacterSkinData(id);

            if (skinData == null)
                return;

            if (Body.name == skinData.Body.name)
            {
                // No need to create a new body, just change the material
                currentBodyRenderer.material = skinData.Material;
                
                // This will trigger the on enable animation clip
                Body.gameObject.SetActive(false);
                Body.gameObject.SetActive(true);
            }
            else
            {
                GameObject newBody = Instantiate(skinData.Body, transform);
                newBody.name = skinData.Body.name;
                
                DestroyImmediate(Body.gameObject);

                Body = newBody.transform;
                
                Collider = Body.GetComponent<Collider>();
                currentBodyRenderer = Body.GetComponent<MeshRenderer>();
                
                currentBodyRenderer.material = skinData.Material;
            }
            
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
            
            currentBodyRenderer = Body.GetComponent<MeshRenderer>();
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
            StateRX
                .Where(x => x == CharacterState.Invincible)
                .Subscribe(x =>
                {
                    // If you trigger 2 shields one after another,
                    // stop the timer that sets it to false
                    shieldDisposable?.Dispose();
                    
                    // Hacky way to reset the shield animation clip if two shields
                    // are triggered one after another
                    Shield.SetActive(false);
                    Shield.SetActive(true);

                    shieldDisposable = Observable.Timer(TimeSpan.FromSeconds(2))
                        .Subscribe(_ =>
                        {
                            Shield.SetActive(false);
                        })
                        .AddTo(this);
                })
                .AddTo(this);

            gameService.IsRunningRX
                .Subscribe(isRunning =>
                {
                    ParticlesVFX.SetActive(isRunning);
                })
                .AddTo(this);
            
            userService.SelectedCharacterSkinRX
                .Subscribe(x => SetSkin(x))
                .AddTo(this);
        }
    }
}
