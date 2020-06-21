using Adic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace RollyVortex.Scripts.Components
{
    public class SharedComponent : MonoBehaviour
    {
        [Header("General")]
        public Camera SharedCamera;

        [Header("Rewards Service")]
        public Image RewardsBackground;
        public GameObject RewardWrapper;
        public GameObject HeroWrapper;
        
        [Header("Notifications Service")]
        public Canvas NotificationsCanvas;
        public GameObject OptionPopupWrapper;

        [Header("Hero Card Assets")]
        public GameObject HeroCardPrefab;
        public GameObject HeroRoundResultOverlayPrefab;

        [Header("Audio")]
        public GameObject AudioWrapper;
        public AudioMixer AudioMixer;

#pragma warning disable 0649
        // [Inject]
        // private IEntitiesUIService entitiesUIService;
        // [Inject]
        // private IConfigurationService configurationService;
        // [Inject]
        // private IChilliConnectService chilliConnectService;
        // [Inject]
        // private IRewardsUIService rewardsUIService;
        // [Inject]
        // private INotificationsUIService notificationsUIService;
        // [Inject]
        // private ISceneService sceneService;
#pragma warning restore 0649

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        public void Start()
        {
            DontDestroyOnLoad(this);

            this.Inject();
        }

        [Inject]
        [UsedImplicitly]
        private void OnPostConstruct()
        {
            InitServices();
        }

        private void InitServices()
        {
            Debug.Log("init services");
            // configurationService.Init();
            // chilliConnectService.Init();
            // entitiesUIService.Init(HeroCardPrefab, HeroRoundResultOverlayPrefab);
            // rewardsUIService.Init(SharedCamera, RewardsBackground, RewardWrapper, HeroWrapper);
            // notificationsUIService.Init(NotificationsCanvas, OptionPopupWrapper);
        }

        // private void SetupAudioService()
        // {
        //     if (this.AudioWrapper != null && !this.audioService.Inited)
        //     {
        //         this.audioService.Init(this.AudioWrapper, this.AudioMixer);
        //     }
        // }
    }
}
