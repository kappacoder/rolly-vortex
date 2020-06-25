using Adic;
using UniRx;
using System;
using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Triggers;
using RollyVortex.Scripts.Services;
using RollyVortex.Scripts.Interfaces.Services;

namespace RollyVortex.Scripts.UI
{
    public class MenuUIComponent : MonoBehaviour
    {
        public Camera MainCamera;
        
        [Header("Main View")] 
        public Canvas MainViewCanvas;
        public Image PlayButton;
        public Button SkinsButton;
        public GameObject Tutorial;
        
        [Header("SkinsButton View")]
        public Canvas SkinsCanvas;
        public Button SkinsBackButton;

        [Header("Currency View")] 
        public Canvas CurrencyCanvas;
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
            PlayButton.OnPointerDownAsObservable()
                .Where(x => !gameService.IsRunningRX.Value)
                .Where(x => !SkinsCanvas.enabled)
                .Subscribe(x =>
                {
                    Hide();
                    CurrencyCanvas.enabled = true;
                    gameService.StartRunning(GameMode.Endless);
                })
                .AddTo(this);
            
            SkinsButton.OnPointerDownAsObservable()
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

        private void ToggleMainView(bool enabled)
        {
            CurrencyCanvas.enabled = !enabled;
            MainViewCanvas.enabled = enabled;
            
            Tutorial.SetActive(enabled);
        }

        private void ToggleSkinsView(bool enabled)
        {
            CurrencyCanvas.enabled = enabled;
            SkinsCanvas.enabled = enabled;
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
