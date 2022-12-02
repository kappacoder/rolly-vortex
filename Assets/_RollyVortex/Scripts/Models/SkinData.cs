using System;
using UnityEngine;

namespace _RollyVortex.Models 
{
    public delegate void SelectedChangedDelegate(bool val);

    [Serializable]
    public class SkinData
    {
        public int Id;
        public int Price;
        public Sprite Sprite;
        public SelectedChangedDelegate SelectedChanged;

        public bool Selected
        {
            get { return _selected; }
            set
            {
                if (_selected == value)
                    return;

                _selected = value;

                if (SelectedChanged != null)
                    SelectedChanged(_selected);
            }
        }

        private bool _selected;
    }

    [Serializable]
    public class TunnelSkinData : SkinData
    {
        public Texture Texture;
    }
    
    [Serializable]
    public class CharacterSkinData : SkinData
    {
        public Material Material;
        public GameObject Body;
    }
}
