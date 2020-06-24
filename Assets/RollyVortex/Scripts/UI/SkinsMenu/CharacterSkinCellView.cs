using EnhancedUI.EnhancedScroller;
using RollyVortex.Scripts.Game.Models;
using System.Collections.Generic;

namespace RollyVortex.Scripts.UI.SkinsMenu
{
    public delegate void SelectedDelegate(CharacterSkinRowCellView rowCellView);

    public class CharacterSkinCellView : EnhancedScrollerCellView
    {
        public CharacterSkinRowCellView[] rowCellViews;

        public void SetData(ref List<CharacterSkinData> data, int startingIndex, SelectedDelegate selected)
        {
            for (var i = 0; i < rowCellViews.Length; i++)
            {
                var index = startingIndex + i;

                rowCellViews[i].SetData(index, index < data.Count ? data[index] : null, selected);
            }
        }
    }
}
