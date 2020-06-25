using Adic;
using UnityEngine;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using RollyVortex.Scripts.Game.Models;
using RollyVortex.Scripts.Interfaces.Services;
using RollyVortex.Scripts.Services;

namespace RollyVortex.Scripts.UI.SkinsMenu
{
    public class CharacterSkinsController : MonoBehaviour, IEnhancedScrollerDelegate
    {
        public EnhancedScroller Scroller;
        public EnhancedScrollerCellView CellViewPrefab;
        public int NumberOfCellsPerRow = 3;

        public float CellSize = 200f;

        [Inject]
        private IUserService userService;
        
        private List<CharacterSkinData> data;

        void Start()
        {
            this.Inject();
        }

        [Inject]
        private void PostConstruct()
        {
            Scroller.Delegate = this;
            
            data = EntitiesService.CharacterSkins;
            data[userService.SelectedCharacterSkinRX.Value].Selected = true;
            
            Scroller.ReloadData();
        }
        
        private void CellViewSelected(CharacterSkinRowCellView cellView)
        {
            if (cellView == null)
            {
                // nothing was Selected
            }
            else
            {
                // get the Selected data index of the cell view
                var selectedDataIndex = cellView.DataIndex;

                // loop through each item in the data list and turn
                // on or off the selection state.
                for (var i = 0; i < data.Count; i++)
                {
                    data[i].Selected = (selectedDataIndex == i);
                }
            }
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

            // data index of the first sub cell
            var di = dataIndex * NumberOfCellsPerRow;

            cellView.name = "SkinsButton " + (di).ToString() + " to " + ((di) + NumberOfCellsPerRow - 1).ToString();

            // pass in a reference to our data set with the offset for this cell
            cellView.SetData(ref data, di, CellViewSelected);

            // return the cell to the Scroller
            return cellView;
        }

        #endregion
    }
}
