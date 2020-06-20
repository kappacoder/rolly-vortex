using UnityEngine;

namespace Febucci.UI.Core
{
    [System.Serializable]
    public class PresetBaseValues
    {
        public string effectTag;

        [SerializeField] internal FloatCurve movementX;
        [SerializeField] internal FloatCurve movementY;
        [SerializeField] internal FloatCurve movementZ;

        [SerializeField] internal FloatCurve scaleX;
        [SerializeField] internal FloatCurve scaleY;

        [SerializeField] internal FloatCurve rotX;
        [SerializeField] internal FloatCurve rotY;
        [SerializeField] internal FloatCurve rotZ;

        [SerializeField] internal ColorCurve color;


        public PresetBaseValues(bool isAppearance)
        {
            int baseIdentifier = isAppearance ? 3 : 0;
            this.effectTag = string.Empty;

            movementX = new FloatCurve(baseIdentifier + 0, false);
            movementY = new FloatCurve(baseIdentifier + 0, false);
            movementZ = new FloatCurve(baseIdentifier + 0, false);

            scaleX = new FloatCurve(baseIdentifier + 1, false);
            scaleY = new FloatCurve(baseIdentifier + 1, false);

            rotX = new FloatCurve(baseIdentifier + 2, false);
            rotY = new FloatCurve(baseIdentifier + 2, false);
            rotZ = new FloatCurve(baseIdentifier + 2, false);

            color = new ColorCurve(false, isAppearance);
        }

        public float GetMaxDuration()
        {
            float GetEffectEvaluatorDuration(EffectEvaluator effect)
            {
                if (effect.isEnabled)
                    return effect.GetDuration();
                return 0;
            }

            return Mathf.Max
                (
                    GetEffectEvaluatorDuration(movementX),
                    GetEffectEvaluatorDuration(movementY),
                    GetEffectEvaluatorDuration(movementZ),
                    GetEffectEvaluatorDuration(scaleX),
                    GetEffectEvaluatorDuration(scaleY),
                    color.enabled ? color.GetDuration() : 0
                );
        }

        public virtual void Initialize()
        {
            movementX.Initialize();
            movementY.Initialize();
            movementZ.Initialize();

            scaleX.Initialize();
            scaleY.Initialize();

            rotX.Initialize();
            rotY.Initialize();
            rotZ.Initialize();

            color.Initialize();
        }
    }

}