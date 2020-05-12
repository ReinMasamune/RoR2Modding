namespace Sniper.Effects
{
    using Sniper.Modules;

    using UnityEngine;

    internal static partial class EffectCreator
    {
        internal static GameObject CreateScatterAmmoTracer()
        {
            GameObject obj = CreateSmallAmmoTracer( MaterialModule.GetScatterTracerMaterial().material, MaterialModule.GetScatterTracerTrailMaterial().material );
            return obj;
        }
    }
}
