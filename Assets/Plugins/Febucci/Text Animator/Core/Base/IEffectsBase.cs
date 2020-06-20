namespace Febucci.UI.Core
{

    internal interface IEffectsBase
    {
        float effectIntensity { get; set; }
        bool closed { get; set; }
        string effectTag { get; set; }
        int charStartIndex { get; set; }
        int charEndIndex { get; set; }

        void Initialize(int charactersCount);

        void Calculate();

        void ApplyEffect(ref CharacterData data, int charIndex);

        bool IsCharInsideRegion(int charIndex);
    }
}