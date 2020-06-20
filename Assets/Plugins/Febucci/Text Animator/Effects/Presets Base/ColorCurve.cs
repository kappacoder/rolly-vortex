using UnityEngine;


namespace Febucci.UI.Core
{
    [System.Serializable]
    internal class ColorCurve
    {

        [SerializeField] public bool enabled;

        [SerializeField] Gradient gradient;
        [SerializeField, MinValue(0.1f)] float duration;
        [SerializeField, Range(0, 100)] float charsTimeOffset; //clamping to 100 because it repeates the behavior after it

        public float GetDuration()
        {
            return duration;
        }

        bool isAppearance;

        public ColorCurve(bool enabled, bool isAppearance)
        {
            this.gradient = new Gradient();
            this.isAppearance = isAppearance;

            if (isAppearance)
            {

                gradient.colorKeys = new GradientColorKey[] {
                    new GradientColorKey(Color.cyan, 0),
                    new GradientColorKey(Color.black, 1)
                };

                gradient.alphaKeys = new GradientAlphaKey[] {
                    new GradientAlphaKey(0, 0),
                    new GradientAlphaKey(1, 1)
                };

                this.duration = .3f;
            }
            else
            {
                gradient.colorKeys = new GradientColorKey[] {
                    new GradientColorKey(Color.black, 0),
                    new GradientColorKey(Color.red, .5f),
                    new GradientColorKey(Color.black, 1)
                };

                gradient.alphaKeys = new GradientAlphaKey[] {
                    new GradientAlphaKey(1, 0),
                    new GradientAlphaKey(1, 1)
                };

                this.duration = 1;
            }

            this.enabled = enabled;
            this.charsTimeOffset = 0;
        }

        public void Initialize()
        {
            if (duration < .1f)
            {
                duration = .1f;
            }
        }

        public Color32 GetColor(float time, int characterIndex)
        {
            if (isAppearance)
                return gradient.Evaluate(Mathf.Clamp01(time / duration));

            return gradient.Evaluate(((time / duration) % 1f + characterIndex * (charsTimeOffset / 100f)) % 1f);
        }
    }
}