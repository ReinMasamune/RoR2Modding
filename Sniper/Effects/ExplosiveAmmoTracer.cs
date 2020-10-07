namespace Rein.Sniper.Effects
{
    using Rein.Sniper.Modules;

    using UnityEngine;

    internal static partial class EffectCreator
    {
        internal static GameObject CreateExplosiveAmmoTracer()
        {
            GameObject obj = CreateBaseAmmoTracer( MaterialModule.GetExplosiveTracerMaterial().material, MaterialModule.GetExplosiveTracerTrailMaterial().material );
            return obj;
        }
    }
}
