using DG.Tweening;
using RollyVortex.Scripts.Utils;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace RollyVortex.Scripts.Game.Components
{
    public class ObstacleComponent : MonoBehaviour
    {
        public List<Color> Colors;
        
        private List<Transform> colliders;

        private List<Pair<MeshRenderer, MaterialPropertyBlock>> renderPairs;
        
        private bool initialized;
        
        void Awake()
        {
            if (!initialized)
                Init();
        }
        
        void OnEnable()
        {
            if (!initialized)
                Init();
            
            foreach (Transform child in transform)
            {
                child.localScale = Vector3.one;
                child.gameObject.SetActive(true);
                
                child.DOScale(0f, 0.3f)
                    .From()
                    .SetEase(Ease.InBack)
                    .Play();
                // part.DOMove(new Vector3(pos.x + 1f, pos.y + 1f, 0f), 0.3f)
                //     .From()
                //     .SetEase(Ease.OutBounce)
                //     .Play();
            }
            
            // Rotate some obstacles when spawned
            transform.DORotate(new Vector3(0f, 0f, transform.position.z + Random.Range(-30f, 30f)), 0.3f, RotateMode.Fast)
                .From()
                .SetDelay(0.2f)
                .SetEase(Ease.OutSine)
                .Play();

            // Color newColor = GetRandomColor();
            //         
            // foreach (var pair in renderPairs)
            // {
            //     var renderer = pair.First;
            //     var propertyBlock = pair.Second;
            //     
            //     renderer.GetPropertyBlock(propertyBlock);
            //     propertyBlock.SetColor("_BaseColor", Color.blue);
            //     renderer.SetPropertyBlock(propertyBlock);
            // }
        }

        void OnDisable()
        {
            foreach (Transform child in transform)
                child.DOKill(true);
        }

        private void Init()
        {
            colliders = new List<Transform>();
            renderPairs = new List<Pair<MeshRenderer, MaterialPropertyBlock>>();

            
            
            foreach (Transform child in transform)
            {
                colliders.Add(child);
                
                child.GetComponent<MeshRenderer>().sharedMaterial.SetColor("_BaseColor", Color.magenta);
                
                renderPairs.Add(new Pair<MeshRenderer, MaterialPropertyBlock>(
                    child.GetComponent<MeshRenderer>(),
                    new MaterialPropertyBlock())
                );
            }

            initialized = true;
        }

        private Color GetRandomColor()
        {
            return Colors[Random.Range(0, Colors.Count)];
        }
    }
}
