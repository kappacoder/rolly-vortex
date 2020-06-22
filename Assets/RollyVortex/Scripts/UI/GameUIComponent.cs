using Adic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using RollyVortex.Scripts.Services;
using RollyVortex.Scripts.Interfaces.Services;
using TMPro;

namespace RollyVortex.Scripts.UI
{
    public class GameUIComponent : MonoBehaviour
    {
        public TMP_Text ScoreText;

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
            gameService.CurrentScoreRX
                .Where(x => gameService.IsRunningRX.Value)
                .Subscribe(x =>
                {
                    ScoreText.text = x.ToString();
                })
                .AddTo(this);
            
            gameService.IsRunningRX
                .Subscribe(isRunning =>
                {
                    canvas.enabled = isRunning;
                    
                    ScoreText.text = "0";
                })
                .AddTo(this);
        }
    }
}
