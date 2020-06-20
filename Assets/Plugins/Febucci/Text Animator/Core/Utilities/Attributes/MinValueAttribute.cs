using UnityEngine;

namespace Febucci.UI.Core
{
    public class MinValueAttribute : PropertyAttribute
    {
        public float min = 0;
        public MinValueAttribute(float min)
        {
            this.min = min;
        }
    }

}