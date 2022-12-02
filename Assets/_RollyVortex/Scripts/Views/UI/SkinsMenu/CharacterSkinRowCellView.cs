using _RollyVortex.Models;
using _RollyVortex.Interfaces.Services;
using Adic;
using UnityEngine;
using UnityEngine.UI;

namespace _RollyVortex.Views.UI.SkinsMenu
{
    public class CharacterSkinRowCellView : MonoBehaviour
    {
        public GameObject container;
        public Image image;
        public Image selectionPanel;

        public Color selectedColor;
        public Color unSelectedColor;

        public int DataIndex { get; private set; }

        /// <summary>
        /// The handler to call when this cell's button traps a click event
        /// </summary>
        public SelectedDelegate Selected;

        [Inject]
        private IUserService userService;
        
        private CharacterSkinData data;

        void Start()
        {
            this.Inject();
        }
        
        void OnDestroy()
        {
            if (data != null)
            {
                data.SelectedChanged -= SelectedChanged;
            }
        }

        public void SetData(int dataIndex, CharacterSkinData data, SelectedDelegate selected)
        {
            // Set the Selected delegate
            this.Selected = selected;

            // This cell was outside the range of the data, so we disable the container
            container.SetActive(data != null);

            // Set the image of the character skin
            if (data != null)
                image.sprite = data.Sprite;

            // If there was previous data assigned to this cell view,
            // remove the handler for the selection change
            if (this.data != null)
                this.data.SelectedChanged -= SelectedChanged;

            // link the data to the cell view
            DataIndex = dataIndex;
            this.data = data;

            if (data != null)
            {
                // Set up a handler so that when the data changes
                // the cell view will update accordingly. We only
                // want a single handler for this cell view, so 
                // first we remove any previous handlers before
                // adding the new one
                this.data.SelectedChanged -= SelectedChanged;
                this.data.SelectedChanged += SelectedChanged;

                // update the selection state UI
                SelectedChanged(data.Selected);
            }
        }
        
        private void SelectedChanged(bool selected)
        {
            selectionPanel.color = (selected ? selectedColor : unSelectedColor);
        }

        /// <summary>
        /// This function is called by the cell's button click event
        /// </summary>
        public void OnSelected()
        {
            userService.SelectedCharacterSkinRX.Value = DataIndex;
            
            if (Selected != null) Selected(this);
        }
    }
}