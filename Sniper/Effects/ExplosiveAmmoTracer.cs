using System;
using System.Collections.Generic;
using System.Text;
using Sniper.Modules;
using UnityEngine;

namespace Sniper.Effects
{
    internal static partial class EffectCreator
    {
        internal static GameObject CreateExplosiveAmmoTracer()
        {
            var obj = CreateBaseAmmoTracer( MaterialModule.GetExplosiveTracerMaterial().material, MaterialModule.GetExplosiveTracerTrailMaterial().material );
            return obj;
        }
    }
}
