using Adic;
using UniRx;
using UnityEngine;
using System.Linq;
using UnityEngine.Scripting;
using System.Collections.Generic;
using System;
using _RollyVortex.Interfaces.Controllers;
using _RollyVortex.Interfaces.Services;

namespace _RollyVortex.Controllers
{
    [Preserve]
    public class EndlessTunnelController : IEndlessTunnelController
    {
        [Inject]
        private IGameService gameService;
        
        private IList<Transform> tunnels;

        private float thresholdPositionZ = -35f;
        private float distanceBetweenTunnels = 30f;
        
        public void Init(Transform tunnelsWrapper)
        {
            tunnels = new List<Transform>();

            foreach (Transform tunnel in tunnelsWrapper)
                tunnels.Add(tunnel);
            
            Subscribe();
        }

        private void Subscribe()
        {
            Observable.EveryFixedUpdate()
                .Where(x => gameService.IsRunningRX.Value)
                .Subscribe(x => OnFixedUpdate())
                .AddTo(tunnels[0].parent.gameObject);
            
            gameService.IsRunningRX
                .Where(isRunning => isRunning)
                .Subscribe(x => Reset())
                .AddTo(tunnels[0].parent.gameObject);
            
            // Temporary - after game ends, reset the tunnels with a 1.5 second delay
            // since we don't have an end-of-run UI
            gameService.IsRunningRX
                .Where(isRunning => !isRunning)
                .Delay(TimeSpan.FromSeconds(1.5f))
                .Subscribe(x => Reset())
                .AddTo(tunnels[0].parent.gameObject);
        }

        private void OnFixedUpdate()
        {
            foreach (Transform tunnel in tunnels)
            {
                if (!(tunnel.position.z <= thresholdPositionZ))
                    continue;

                float maxPositionZ = tunnels.Max(x => x.position.z);
                tunnel.position = new Vector3(0f, 0f, maxPositionZ + distanceBetweenTunnels);
            }
        }

        private void Reset()
        {
            int index = 0;
            foreach (Transform tunnel in tunnels)
            {
                tunnel.localPosition = new Vector3(0f, 0f, distanceBetweenTunnels * index);

                index++;
            }
        }
    }
}
