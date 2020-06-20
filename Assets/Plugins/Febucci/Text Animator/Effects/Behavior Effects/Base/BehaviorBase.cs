namespace Febucci.UI.Core
{
    internal abstract class BehaviorBase : EffectsBase
    {
        public abstract void SetDefaultValues(BehaviorDefaultValues data);

        public abstract void SetModifier(string modifierName, string modifierValue);


        internal float animatorTime { get; private set; }
        internal float animatorDeltaTime { get; private set; }

        public void SetTimeValues(float time, float deltaTime)
        {
            this.animatorTime = time;
            this.animatorDeltaTime = deltaTime;
        }


#if UNITY_EDITOR
        //Used only in the editor to set again modifiers if we change values in the inspector

        public System.Collections.Generic.List<Modifier> modifiers { get; set; } = new System.Collections.Generic.List<Modifier>();

        public void EDITOR_RecordModifier(string name, string value)
        {
            modifiers.Add(new Modifier
            {
                name = name,
                value = value,
            });
        }

        public void EDITOR_ApplyModifiers()
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                SetModifier(modifiers[i].name, modifiers[i].value);
            }
        }
#endif
    }
}