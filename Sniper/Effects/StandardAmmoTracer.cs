using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Sniper.Effects
{
    internal static partial class EffectCreator
    {
        internal static GameObject CreateStandardAmmoTracer()
        {
            // TODO: Standard ammo tracer
            return Resources.Load<GameObject>( "Prefabs/Effects/Tracers/TracerCommandoDefault" );
        }
    }
}
