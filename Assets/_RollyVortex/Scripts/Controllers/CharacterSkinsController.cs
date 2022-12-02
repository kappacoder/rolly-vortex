using Adic;
using UniRx;
using UnityEngine;
using System.Collections.Generic;
using _RollyVortex.Models;
using _RollyVortex.Interfaces.Services;
using _RollyVortex.Views.UI.SkinsMenu;
using EnhancedUI.EnhancedScroller;

namespace _RollyVortex.Controllers
{
    public class CharacterSkinsController : MonoBehaviour, IEnhancedScrollerDelegate
    {
        public EnhancedScroller Scroller;
        public EnhancedScrollerCellView CellViewPrefab;
        public int NumberOfCellsPerRow = 3;

        public float CellSize = 200f;

        [Inject]
        private IUserService userService;
        [Inject]
        private IEntitiesService entitiesService;
        [Inject]
        private IGameService gameService;
        
        private List<CharacterSkinData> data;

        void Start()
        {
            this.Inject();
            
            Subscribe();
        }

        private void Subscribe()
        {
            gameService.IsReadyRX
                .Where(ready => ready)
                .First()
                .Subscribe(x =>
                {
                    Scroller.Delegate = this;
            
                    data = entitiesService.CharacterSkins;
                    data[userService.SelectedCharacterSkinRX.Value].Selected = true;
            
                    Scroller.ReloadData();
                })
                .AddTo(this);
        }
        
        private void CellViewSelected(CharacterSkinRowCellView cellView)
        {
            if (cellView == null)
                return;
            
            var selectedDataIndex = cellView.DataIndex;

            for (var i = 0; i < data.Count; i++)
                data[i].Selected = (selectedDataIndex == i);
        }

#region EnhancedScroller Handlers
public int GetNumberOfCells(EnhancedScroller scroller)
        {
            return Mathf.CeilToInt((float)data.Count / (float)NumberOfCellsPerRow);
        }

        public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
        {
            return CellSize;
        }

        public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
        {
            CharacterSkinCellView cellView = scroller.GetCellView(CellViewPrefab) as CharacterSkinCellView;

            var di = dataIndex * NumberOfCellsPerRow;

            cellView.name = "SkinsButton " + (di).ToString() + " to " + ((di) + NumberOfCellsPerRow - 1).ToString();

            cellView.SetData(ref data, di, CellViewSelected);

            return cellView;
        }
#endregion
    }
}
