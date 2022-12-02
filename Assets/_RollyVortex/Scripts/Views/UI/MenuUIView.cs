using Adic;
using UniRx;
using System;
using _RollyVortex.Interfaces.Services;
using _RollyVortex.Services;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Triggers;

namespace _RollyVortex.Views.UI
{
    public class MenuUIView : MonoBehaviour
    {
        public Camera MainCamera;

        [Header("Main View")] 
        public GameObject MainView;
        public Button PlayButton;
        public Button SkinsButton;
        public GameObject Tutorial;

        [Header("SkinsButton View")]
        public GameObject SkinsView;
        public Button SkinsBackButton;

        [Header("Currency View")] 
        public GameObject CurrencyView;
        public TMP_Text GemsText;
        
        [Inject]
        private IGameService gameService;
        [Inject]
        private IUserService userService;

        private void Awake()
        {
            Hide();
        }
        
        private void Start()
        {
            this.Inject();
        }

        [Inject]
        private void PostConstruct()
        {
            GemsText.text = userService.GemsRX.Value.ToString();
            
            Subscribe();

            ShowCanvasWithDelay(1f);
        }

        private void Subscribe()
        {
            PlayButton.OnClickAsObservable()
                .Where(x => !gameService.IsRunningRX.Value)
                .Where(x => MainView.activeInHierarchy)
                .Subscribe(x =>
                {
                    Hide();
                    CurrencyView.SetActive(true);
                    gameService.StartRunning(GameMode.Endless);
                })
                .AddTo(this);
            
            SkinsButton.OnClickAsObservable()
                .Where(x => !gameService.IsRunningRX.Value)
                .Subscribe(x =>
                {
                    ToggleMainView(false);
                    
                    MainCamera.transform.DOKill();
                    MainCamera.transform.DOLocalMoveY(-2f, 0.3f)
                        .OnComplete(() =>
                        {
                            ToggleSkinsView(true);
                        })
                        .Play();
                })
                .AddTo(this);
            
            SkinsBackButton.OnPointerDownAsObservable()
                .Where(x => !gameService.IsRunningRX.Value)
                .Subscribe(x =>
                {
                    ToggleSkinsView(false);

                    MainCamera.transform.DOKill();
                    MainCamera.transform.DOLocalMoveY(0f, 0.3f)
                        .OnComplete(() =>
                        {
                            ToggleMainView(true);
                        })
                        .Play();
                })
                .AddTo(this);

            userService.GemsRX
                .Subscribe(x =>
                {
                    GemsText.text = x.ToString();
                })
                .AddTo(this);

            gameService.IsRunningRX
                .Skip(1)
                .Where(isRunning => !isRunning)
                .Delay(TimeSpan.FromSeconds(1.5f))
                .Subscribe(x =>
                {
                    gameService.Reset();
                    ToggleMainView(true);
                })
                .AddTo(this);
        }

        private void ToggleMainView(bool active)
        {
            CurrencyView.SetActive(active);
            MainView.SetActive(active);
            Tutorial.SetActive(active);
        }

        private void ToggleSkinsView(bool active)
        {
            CurrencyView.SetActive(active);
            SkinsView.SetActive(active);
        }

        private void Hide()
        {
            ToggleMainView(false);
            ToggleSkinsView(false);
        }
        
        private void ShowCanvasWithDelay(float delay)
        {
            Observable.Timer(TimeSpan.FromSeconds(delay))
                .Subscribe(x =>
                {
                    ToggleMainView(true);
                })
                .AddTo(this);
        }
    }
}
