using UnityEngine;

namespace Febucci.UI.Core
{
    [System.Serializable]
    public class PresetBehaviorValues : PresetBaseValues
    {

        [SerializeField] internal EmissionControl emission;


        public PresetBehaviorValues() : base(false)
        {
            emission = new EmissionControl(false);
        }

        public override void Initialize()
        {
            base.Initialize();
            emission.Initialize(GetMaxDuration());

        }
    }

}