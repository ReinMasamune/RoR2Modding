namespace Sniper.Effects
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Sniper.Modules;
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
