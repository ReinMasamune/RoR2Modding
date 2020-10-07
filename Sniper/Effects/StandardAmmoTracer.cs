namespace Rein.Sniper.Effects
{
    using Rein.Sniper.Modules;

    using UnityEngine;

    internal static partial class EffectCreator
    {
        internal static GameObject CreateStandardAmmoTracer()
        {
            GameObject obj = CreateBaseAmmoTracer( MaterialModule.GetStandardTracerMaterial().material, MaterialModule.GetStandardTracerTrailMaterial().material );
            return obj;
        }
    }
}
