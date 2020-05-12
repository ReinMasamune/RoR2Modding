namespace Sniper.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using BepInEx.Logging;
    using ReinCore;
    using RoR2;
    using Sniper.Components;
    using UnityEngine;

    internal static class VFXModule
    {
        private static GameObject standardAmmoTracerPrefab;
        internal static GameObject GetStandardAmmoTracer()
        {
            if( standardAmmoTracerPrefab == null )
            {
                standardAmmoTracerPrefab = Effects.EffectCreator.CreateStandardAmmoTracer();
                EffectsCore.AddEffect( standardAmmoTracerPrefab );
            }

            return standardAmmoTracerPrefab;
        }

        private static GameObject explosiveAmmoTracerPrefab;
        internal static GameObject GetExplosiveAmmoTracer()
        {
            if( explosiveAmmoTracerPrefab == null )
            {
                explosiveAmmoTracerPrefab = Effects.EffectCreator.CreateExplosiveAmmoTracer();
                EffectsCore.AddEffect( explosiveAmmoTracerPrefab );
            }

            return explosiveAmmoTracerPrefab;
        }

        private static GameObject plasmaAmmoTracerPrefab;
        internal static GameObject GetPlasmaAmmoTracer()
        {
            if( plasmaAmmoTracerPrefab == null )
            {
                plasmaAmmoTracerPrefab = Effects.EffectCreator.CreatePlasmaAmmoTracer();
                EffectsCore.AddEffect( plasmaAmmoTracerPrefab );
            }
            return plasmaAmmoTracerPrefab;
        }

        private static GameObject scatterAmmoTracerPrefab;
        internal static GameObject GetScatterAmmoTracer()
        {
            if( scatterAmmoTracerPrefab == null )
            {
                scatterAmmoTracerPrefab = Effects.EffectCreator.CreateScatterAmmoTracer();
                EffectsCore.AddEffect( scatterAmmoTracerPrefab );
            }
            return scatterAmmoTracerPrefab;
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
