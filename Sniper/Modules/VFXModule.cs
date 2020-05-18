namespace Sniper.Modules
{
    using ReinCore;

    using RoR2;

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



        private static GameObject knifeBlinkPrefab;
        internal static GameObject GetKnifeBlinkPrefab()
        {
            if( knifeBlinkPrefab == null )
            {
                knifeBlinkPrefab = Resources.Load<GameObject>( "Prefabs/Effects/HuntressBlinkEffect" );
            }

            return knifeBlinkPrefab;
        }

        private static GameObject knifeSlashPrefab;
        internal static GameObject GetKnifeSlashPrefab()
        {
            if( knifeSlashPrefab == null )
            {
                knifeSlashPrefab = Resources.Load<GameObject>( "Prefabs/Effects/MageLightningBombExplosion" );
            }

            return knifeSlashPrefab;
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
