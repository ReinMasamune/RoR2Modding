using System;
using System.Collections.Generic;
using System.Text;
using ReinCore;
using RoR2;
using UnityEngine;
using Sniper.Modules;

namespace Sniper.Effects
{
    internal static partial class EffectCreator
    {
        internal static GameObject CreateScatterAmmoTracer()
        {
            var obj = CreateSmallAmmoTracer( MaterialModule.GetScatterTracerMaterial().material, MaterialModule.GetScatterTracerTrailMaterial().material );
            return obj;
        }
    }
}
