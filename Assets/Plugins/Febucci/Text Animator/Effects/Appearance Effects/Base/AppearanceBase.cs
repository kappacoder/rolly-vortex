namespace Febucci.UI.Core
{
    /// <summary>
    /// Base class for all appearance effects
    /// </summary>
    internal abstract class AppearanceBase : EffectsBase
    {
        internal float showDuration = .3f;

        public abstract void SetDefaultValues(AppearanceDefaultValues data);

        public virtual bool CanShowAppearanceOn(float timePassed)
        {
            return timePassed <= showDuration;
        }
    }
}