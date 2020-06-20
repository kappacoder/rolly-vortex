namespace Febucci.UI.Core
{
    internal interface EffectEvaluator
    {
        void Initialize();

        bool isEnabled { get; }
        float Evaluate(float time, int characterIndex);
        float GetDuration();
    }

}