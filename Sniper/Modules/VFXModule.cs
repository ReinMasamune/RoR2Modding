using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using Sniper.Components;
using UnityEngine;

namespace Sniper.Modules
{
    internal static class VFXModule
    {
        private static GameObject standardAmmoTracerPrefab;
        internal static GameObject GetStandardAmmoTracer()
        {
            if( standardAmmoTracerPrefab == null )
            {
                standardAmmoTracerPrefab = Effects.EffectCreator.CreateStandardAmmoTracer();
            }

            return standardAmmoTracerPrefab;
        }

        private static GameObject explosiveAmmoTracerPrefab;
        internal static GameObject GetExplosiveAmmoTracer()
        {
            if( explosiveAmmoTracerPrefab == null )
            {
                explosiveAmmoTracerPrefab = Effects.EffectCreator.CreateExplosiveAmmoTracer();
            }

            return explosiveAmmoTracerPrefab;
        }

        private static GameObject explosiveAmmoExplosionPrefab;
        private static EffectIndex explosiveAmmoExplosionEffectIndex = EffectIndex.Invalid;
        internal static GameObject GetExplosiveAmmoExplosionPrefab()
        {
            if( explosiveAmmoExplosionPrefab == null )
            {
                explosiveAmmoExplosionPrefab = Effects.EffectCreator.CreateExplosiveAmmoExplosion();
            }

            return explosiveAmmoExplosionPrefab;
        }
        internal static EffectIndex GetExplosiveAmmoExplosionIndex()
        {
            if( explosiveAmmoExplosionEffectIndex == EffectIndex.Invalid )
            {
                explosiveAmmoExplosionEffectIndex = EffectCatalog.FindEffectIndexFromPrefab( GetExplosiveAmmoExplosionPrefab() );
            }

            return explosiveAmmoExplosionEffectIndex;
        }
    }

}
