namespace Febucci.UI.Core
{
    /// <summary>
    /// Base class for all the effects
    /// </summary>
    internal abstract class EffectsBase
    {
        public float effectIntensity = 1;
        public string effectTag;
        public bool closed = false;
        public int charStartIndex;
        public int charEndIndex;

        public bool IsCharInsideRegion(int charIndex)
        {
            return charIndex >= charStartIndex && charIndex < charEndIndex;
        }

        public override string ToString()
        {
            return $"{GetType().ToString()}\nStart index: {charStartIndex} - End index: {charEndIndex}";
        }

        public abstract void ApplyEffect(ref CharacterData data, int charIndex);


        internal void ApplyModifierTo(ref float value, string modifierValue)
        { 
            if (FormatUtils.ParseFloat(modifierValue, out float multiplier))
            {
                value *= multiplier;
            }
        }

        /// <summary>
        /// Invoked upon effect creation
        /// </summary>
        /// <param name="charactersCount"></param>
        public virtual void Initialize(int charactersCount)
        {

        }

        /// <summary>
        /// Used to calculate effect values (once each frame) before applying it to all the letters
        /// </summary>
        public virtual void Calculate()
        {

        }

    }
}