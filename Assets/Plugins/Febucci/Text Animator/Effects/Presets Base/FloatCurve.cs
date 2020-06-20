using UnityEngine;

namespace Febucci.UI.Core
{
    [System.Serializable]
    internal class FloatCurve : EffectEvaluator
    {
        public bool enabled;

        [SerializeField] float amplitude;
        [SerializeField] AnimationCurve curve;

        [SerializeField, HideInInspector] float defaultReturn;

        [SerializeField, Range(0, 100)] float charsTimeOffset; //clamping to 100 because it repeates the behavior after it

        [System.NonSerialized] float calculatedDuration;

        public bool isEnabled => enabled;

        public float GetDuration()
        {
            return calculatedDuration;
        }

        bool isAppearance;

        public FloatCurve(int type, bool enabled)
        {
            this.enabled = enabled;

            isAppearance = type >= 3;

            switch (type)
            {
                //mov
                default:
                case 0:

                    defaultReturn = 0;
                    amplitude = 1;

                    curve = new AnimationCurve(
                        new Keyframe(0, 0, -5.4f, -5.4f),
                        new Keyframe(.25f, -1, -1, -1),
                        new Keyframe(.75f, 1, -1, -1),
                        new Keyframe(1, 0, -5.4f, -5.4f)
                    );

                    break;

                //scale
                case 1:
                    defaultReturn = 1;
                    amplitude = 2;

                    curve = new AnimationCurve(
                        new Keyframe(0, .5f),
                        new Keyframe(.5f, 1f),
                        new Keyframe(1, .5f)
                    );

                    break;

                //rot
                case 2:
                    defaultReturn = 0;
                    amplitude = 35; //angle of 35

                    curve = new AnimationCurve(
                        new Keyframe(0, 0, -5.4f, -5.4f),
                        new Keyframe(.25f, -1, -1, -1),
                        new Keyframe(.75f, 1, -1, -1),
                        new Keyframe(1, 0, -5.4f, -5.4f)
                    );
                    break;

                //app mov
                case 3:

                    defaultReturn = 0;
                    amplitude = 1;

                    curve = new AnimationCurve(
                        new Keyframe(0, 0),
                        new Keyframe(1, 1)
                    );

                    break;

                //app scale
                case 4:
                    defaultReturn = 1;
                    amplitude = 2;

                    curve = new AnimationCurve(
                        new Keyframe(0, 0),
                        new Keyframe(1, 1)
                    );

                    break;

                //app rot
                case 5:
                    defaultReturn = 0;
                    amplitude = 35; //angle of 35

                    curve = new AnimationCurve(
                        new Keyframe(0, 0),
                        new Keyframe(1, 1)
                    );
                    break;

            }

            //duration = isAppearance ? .3f : 1;
            curve.postWrapMode = isAppearance ? WrapMode.Once : WrapMode.Loop; //loops if behavior
            charsTimeOffset = 0;
        }

        public void Initialize()
        {
            calculatedDuration = curve.GetDuration();
        }

        public float Evaluate(float time, int characterIndex)
        {
            if (!enabled)
                return defaultReturn;

            if (isAppearance)
                return curve.Evaluate(calculatedDuration - time) * amplitude * Mathf.Cos(Mathf.Deg2Rad * (characterIndex * charsTimeOffset / 2f));

            //behavior
            return curve.Evaluate(time + characterIndex * (charsTimeOffset / 100f)) * amplitude;
        }
    }
}