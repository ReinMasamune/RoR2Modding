namespace Rein.Sniper.Effects
{
    using Rein.Sniper.Modules;

    using UnityEngine;

    internal static partial class EffectCreator
    {
        internal static GameObject CreateShockAmmoTracer()
        {
            GameObject obj = CreateBaseAmmoTracer( MaterialModule.GetShockTracerMaterial().material, MaterialModule.GetShockTracerTrailMaterial().material );
            return obj;
        }
    }
}
