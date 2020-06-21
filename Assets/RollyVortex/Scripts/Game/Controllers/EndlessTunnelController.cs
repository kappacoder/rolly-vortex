using Adic;
using UniRx;
using UnityEngine;
using System.Linq;
using UnityEngine.Scripting;
using System.Collections.Generic;
using RollyVortex.Scripts.Interfaces.Services;
using RollyVortex.Scripts.Interfaces.Game.Controllers;

namespace RollyVortex.Scripts.Game.Controllers
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
    }
}
