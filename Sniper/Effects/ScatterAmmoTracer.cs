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
        internal static GameObject CreateScatterAmmoTracer()
        {
            GameObject obj = CreateSmallAmmoTracer( MaterialModule.GetScatterTracerMaterial().material, MaterialModule.GetScatterTracerTrailMaterial().material );
            return obj;
        }
    }
}
