using Adic;
using UniRx;
using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx.Triggers;
using RollyVortex.Scripts.Services;
using RollyVortex.Scripts.Interfaces.Services;

namespace RollyVortex.Scripts.UI
{
    public class MenuUIComponent : MonoBehaviour
    {
        public GameObject Tutorial;
        public Image Play;

        [Inject]
        private IGameService gameService;

        private Canvas canvas;

        private void Awake()
        {
            canvas = GetComponent<Canvas>();
            
            Hide();
        }
        
        private void Start()
        {
            this.Inject();
        }

        [Inject]
        private void PostConstruct()
        {
            Subscribe();

            ShowCanvasWithDelay(1f);
        }

        private void Subscribe()
        {
            Play.OnPointerDownAsObservable()
                .Where(x => !gameService.IsRunningRX.Value)
                .Subscribe(x =>
                {
                    gameService.StartRunning(GameMode.Endless);
                })
                .AddTo(this);
            
            gameService.IsRunningRX
                .Subscribe(isRunning =>
                {
                    if (isRunning)
                        Hide();
                    else
                        ShowCanvasWithDelay(1f);
                })
                .AddTo(this);
        }

        private void Hide()
        {
            canvas.enabled = false;
            
            Tutorial.SetActive(false);
        }

        private void ShowCanvasWithDelay(float delay)
        {
            Observable.Timer(TimeSpan.FromSeconds(delay))
                .Subscribe(x =>
                {
                    canvas.enabled = true;
                    
                    Tutorial.SetActive(true);
                })
                .AddTo(this);
        }
    }
}
