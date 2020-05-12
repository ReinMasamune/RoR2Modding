namespace Sniper.Effects
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ReinCore;
    using RoR2;
    using UnityEngine;
    using Sniper.Modules;

    internal static partial class EffectCreator
    {
        internal static GameObject CreatePlasmaAmmoTracer()
        {
            GameObject obj = CreateBaseAmmoTracer( MaterialModule.GetPlasmaTracerMaterial().material, MaterialModule.GetPlasmaTracerTrailMaterial().material );
            return obj;
        }
    }
}
