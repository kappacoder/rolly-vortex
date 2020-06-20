﻿using UnityEngine;

namespace Febucci.UI.Core
{
    internal class FadeBehavior : BehaviorBase
    {
        float delay = .3f;

        float[] charPCTs;

        public override void SetDefaultValues(BehaviorDefaultValues data)
        {
            delay = data.defaults.fadeDelay;
        }

        public override void Initialize(int charactersCount)
        {
            base.Initialize(charactersCount);
            charPCTs = new float[charactersCount];
        }

        public override void SetModifier(string modifierName, string modifierValue)
        {
            switch (modifierName)
            {
                //delay
                case "d": ApplyModifierTo(ref delay, modifierValue); break;
            }
        }

        public override void ApplyEffect(ref CharacterData data, int charIndex)
        {
            charPCTs[charIndex] += animatorDeltaTime;
            //Lerps
            if (charPCTs[charIndex] <= 1 && charPCTs[charIndex]>=0)
            {
                data.colors.LerpUnclamped(Color.clear, Tween.EaseInOut(Mathf.Clamp01(charPCTs[charIndex])));
            }
            else //Keeps them hidden
            {
                data.colors.SetColor(Color.clear);
            }

        }


        public override string ToString()
        {
            return $"delay: {delay}\n" +
                $"\n{ base.ToString()}";
        }

    }
}