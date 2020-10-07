namespace Rein.Sniper.Effects
{
    using Rein.Sniper.Modules;

    using UnityEngine;

    internal static partial class EffectCreator
    {
        internal static GameObject CreatePlasmaAmmoTracer()
        {
            GameObject obj = CreateBaseAmmoTracer( MaterialModule.GetPlasmaTracerMaterial().material, MaterialModule.GetPlasmaTracerTrailMaterial().material );
            return obj;
        }
    }
}
