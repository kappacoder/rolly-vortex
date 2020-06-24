using Adic;
using UnityEngine;
using UnityEngine.UI;
using RollyVortex.Scripts.Game.Models;
using RollyVortex.Scripts.Interfaces.Services;

namespace RollyVortex.Scripts.UI.SkinsMenu
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

        /// <summary>
        /// This function just takes the Demo data and displays it
        /// </summary>
        /// <param name="data"></param>
        public void SetData(int dataIndex, CharacterSkinData data, SelectedDelegate selected)
        {
            // set the Selected delegate
            this.Selected = selected;

            // this cell was outside the range of the data, so we disable the container.
            // Note: We could have disable the cell gameobject instead of a child container,
            // but that can cause problems if you are trying to get components (disabled objects are ignored).
            container.SetActive(data != null);

            if (data != null)
            {
                // set the image of the skin
                image.sprite = data.Sprite;
            }

            // if there was previous data assigned to this cell view,
            // we need to remove the handler for the selection change
            if (this.data != null)
            {
                this.data.SelectedChanged -= SelectedChanged;
            }

            // link the data to the cell view
            DataIndex = dataIndex;
            this.data = data;

            if (data != null)
            {
                // set up a handler so that when the data changes
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