namespace Rein.Sniper.Effects
{
    using Rein.Sniper.Modules;

    using UnityEngine;

    internal static partial class EffectCreator
    {
        internal static GameObject CreateSporeAmmoTracer()
        {
            GameObject obj = CreateBaseAmmoTracer( MaterialModule.GetSporeTracerMaterial().material, MaterialModule.GetSporeTracerTrailMaterial().material );
            return obj;
        }
    }
}
