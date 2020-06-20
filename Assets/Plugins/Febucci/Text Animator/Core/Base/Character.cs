using UnityEngine;

namespace Febucci.UI.Core
{
    /// <summary>
    /// Represents visible characters in the text (including sprites and excluding bars)
    /// </summary>
    public struct Character
    {
        public bool hasBehaviorEffects;
        public int[] indexBehaviorEffects;

        public bool hasAppearanceEffects;
        public int[] indexAppearanceEffects;

        public CharacterSourceData sources;
        public CharacterData data;

        public void ResetVertices()
        {
            for (byte i = 0; i < sources.vertices.Length; i++)
            {
                data.vertices[i] = sources.vertices[i];
            }
        }

        public void ResetColors()
        {
            for (byte i = 0; i < sources.colors.Length; i++)
            {
                data.colors[i] = sources.colors[i];
            }
        }

        //TODO ToString
    }

    //Original character data
    public struct CharacterSourceData
    {
        public Color32[] colors;
        public Vector3[] vertices;
    }

    //Character's pasted and modified data
    public struct CharacterData
    {
        public float passedTime;

        public Color32[] colors;
        public Vector3[] vertices;
    }

}