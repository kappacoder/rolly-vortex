using UnityEngine;

namespace Febucci.UI.Core
{
    [System.Serializable]
    internal struct EmissionControl
    {
        [SerializeField, MinValue(0)] int cycles;

        [SerializeField] AnimationCurve attackCurve;
        [SerializeField, MinValue(0)] float overrideDuration;
        [SerializeField] bool continueForever;
        [SerializeField] AnimationCurve decayCurve;


        [System.NonSerialized] float maxDuration;
        [System.NonSerialized] AnimationCurve intensityOverDuration;
        [System.NonSerialized] float passedTime;
        [System.NonSerialized] float cycleDuration;

        [System.NonSerialized] public float effectWeigth;

        public EmissionControl(bool something)
        {
            this.effectWeigth = 0;
            this.continueForever = true;
            this.cycles = 0; //<0 = infinite

            this.passedTime = 0;
            this.maxDuration = 0;
            this.cycleDuration = 0;
            this.overrideDuration = 0;

            this.intensityOverDuration = new AnimationCurve();

            this.attackCurve = new AnimationCurve(
                new Keyframe(0, 0),
                new Keyframe(1, 1));

            this.decayCurve = new AnimationCurve(
                new Keyframe(0, 1),
                new Keyframe(1, 0));
        }

        public void Initialize(float effectsMaxDuration)
        {
            passedTime = 0;

            Keyframe[] totalKeys = new Keyframe[
                attackCurve.length + (continueForever ? 0 : decayCurve.length)
                ];

            for (int i = 0; i < attackCurve.length; i++)
            {
                totalKeys[i] = attackCurve[i];
            }

            if (!continueForever)
            {
                if (overrideDuration > 0)
                {
                    effectsMaxDuration = overrideDuration;
                }

                float attackDuration = attackCurve.GetDuration();

                for (int i = attackCurve.length; i < totalKeys.Length; i++)
                {
                    totalKeys[i] = decayCurve[i - attackCurve.length];
                    totalKeys[i].time += effectsMaxDuration + attackDuration;
                }
            }

            intensityOverDuration = new AnimationCurve(totalKeys);
            intensityOverDuration.preWrapMode = WrapMode.Loop;
            intensityOverDuration.postWrapMode = WrapMode.Loop;

            this.cycleDuration = intensityOverDuration.GetDuration();
            maxDuration = cycleDuration * cycles;
        }

        public float IncreaseEffectTime(float deltaTime)
        {
            if (deltaTime == 0)
                return passedTime;

            passedTime += deltaTime;

            if (passedTime < 0)
                passedTime = 0;

            //inside duration
            if (passedTime > cycleDuration) //increases cycle
            {
                if (continueForever)
                {
                    effectWeigth = 1;
                    return passedTime;
                }
            }

            //outside cycles
            if (cycles > 0 && passedTime >= maxDuration)
            {
                effectWeigth = 0;
                return 0;
            }

            effectWeigth = intensityOverDuration.Evaluate(passedTime);

            return passedTime;
        }
    }
}