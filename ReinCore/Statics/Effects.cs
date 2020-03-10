using System;
using System.Collections.Generic;
using BepInEx;
using RoR2;
using UnityEngine;

namespace ReinCore
{
    // TODO: Docs for EffectsCore
    /// <summary>
    /// 
    /// </summary>
    public static class EffectsCore
    {
        public static Boolean loaded { get; internal set; } = false;

        public static void AddEffect( GameObject effect )
        {
            if( !loaded ) throw new CoreNotLoadedException( nameof( EffectsCore ) );
            if( effect == null ) throw new ArgumentNullException( "effect" );
            var effectComp = effect.GetComponent<EffectComponent>();
            if( effectComp == null ) throw new ArgumentException( "must have an EffectComponent", "effect" );
            var vfx = effect.GetComponent<VFXAttributes>();
            if( vfx == null ) throw new ArgumentException( "must have a VFXAttributesComponent", "effect" );

            AddEffect( new EffectDef
            {
                prefab = effect,
                prefabEffectComponent = effectComp,
                prefabVfxAttributes = vfx,
                prefabName = effect.name,
                spawnSoundEventName = effectComp.soundName,
            } );
        }

        public static void AddEffect( EffectDef effect )
        {
            if( !loaded ) throw new CoreNotLoadedException( nameof( EffectsCore ) );
            if( effect == null ) throw new ArgumentNullException( "effect" );
            if( addedEffectSet.Contains( effect ) ) return;
            addedEffectSet.Add( effect );
            addedEffects.Add( effect );
        }


        static EffectsCore()
        {
            HooksCore.on_RoR2_EffectCatalog_GetDefaultEffectDefs += HooksCore_on_RoR2_EffectCatalog_GetDefaultEffectDefs;

            loaded = true;
        }

        private static EffectDef[] HooksCore_on_RoR2_EffectCatalog_GetDefaultEffectDefs( HooksCore.orig_RoR2_EffectCatalog_GetDefaultEffectDefs orig )
        {
            var effects = orig();

            var ind = effects.Length;
            var newCount = addedEffects.Count;
            if( newCount > 0 )
            {
                newCount += ind;
                Array.Resize<EffectDef>( ref effects, newCount );
                for( Int32 i = ind; i < newCount; ++i )
                {
                    effects[i] = addedEffects[i - ind];
                }
            }
            return effects;
        }

        private static HashSet<EffectDef> addedEffectSet = new HashSet<EffectDef>();
        private static List<EffectDef> addedEffects = new List<EffectDef>();
    }
}
