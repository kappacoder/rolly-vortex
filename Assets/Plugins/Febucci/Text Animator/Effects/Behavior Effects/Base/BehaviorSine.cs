using UnityEngine;

namespace Febucci.UI.Core
{
    internal abstract class BehaviorSine : BehaviorBase
    {
        internal float amplitude = 1;
        internal float frequency = 1;
        internal float waveSize = .08f;

        public override void SetModifier(string modifierName, string modifierValue)
        {
            switch (modifierName)
            {
                //amplitude
                case "a": ApplyModifierTo(ref amplitude, modifierValue); break;
                //frequency
                case "f": ApplyModifierTo(ref frequency, modifierValue); break;
                //wave size
                case "w": ApplyModifierTo(ref waveSize, modifierValue); break;
            }
        }

        public override string ToString()
        {
            return $"freq: {frequency}\n" +
                $"ampl: {amplitude}\n" +
                $"waveSize: {waveSize}" +
                $"\n{base.ToString()}";
        }
    }
}