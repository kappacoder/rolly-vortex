using _RollyVortex.Interfaces.Services;
using Adic;
using UniRx;
using TMPro;
using UnityEngine;

namespace _RollyVortex.Views.UI
{
    public class GameUIView : MonoBehaviour
    {
        public TMP_Text ScoreText;

        [Inject]
        private IGameService gameService;

        private void Start()
        {
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
                    gameObject.SetActive(isRunning);
                    ScoreText.text = "0";
                })
                .AddTo(this);
        }
    }
}
