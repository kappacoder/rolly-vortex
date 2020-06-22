﻿using Adic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using RollyVortex.Scripts.Services;
using RollyVortex.Scripts.Interfaces.Services;

namespace RollyVortex.Scripts.UI
{
    public class MenuUIComponent : MonoBehaviour
    {
        public Button PlayButton;

        [Inject]
        private IGameService gameService;

        private Canvas canvas;
    
        private void Start()
        {
            canvas = GetComponent<Canvas>();
            
            this.Inject();
        }

        [Inject]
        private void PostConstruct()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            PlayButton.OnClickAsObservable()
                .Where(x => !gameService.IsRunningRX.Value)
                .Subscribe(x =>
                {
                    gameService.StartRunning(GameMode.Endless);
                })
                .AddTo(this);
            
            gameService.IsRunningRX
                .Subscribe(isRunning =>
                {
                    Debug.Log("game service " + isRunning);
                    canvas.enabled = !isRunning;
                })
                .AddTo(this);
        }
    }
}
