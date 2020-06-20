
namespace Febucci.UI.Core
{
    public enum CustomFeature
    {
        NotImplemented, //Do not remove this one

        //Add from here
    }

    /// <summary>
    /// 'Top' class that tells the script which tag is a custom feature
    /// </summary>
    public static class CustomFunctionalities
    {
        public struct FeaturePair
        {
            public readonly string tag;
            public readonly CustomFeature feature;

            public FeaturePair(string tag, CustomFeature feature)
            {
                this.tag = tag;
                this.feature = feature;
            }
        }

        public static readonly FeaturePair[] customFeatures = new FeaturePair[] {
            //new FeaturePair("exampleID", CustomFeature.Something),
        };
    }

}