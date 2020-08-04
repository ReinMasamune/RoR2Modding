namespace ReinCore
{
    using System;
    using System.Collections.Generic;

    using RoR2;

    using UnityEngine;

    public static class EffectsCore
    {
        public static Boolean loaded { get; internal set; } = false;

        public static void AddEffect( GameObject effect )
        {
            if( !loaded )
            {
                throw new CoreNotLoadedException( nameof( EffectsCore ) );
            }

            if( effect == null )
            {
                throw new ArgumentNullException( "effect" );
            }

            EffectComponent effectComp = effect.GetComponent<EffectComponent>();
            if( effectComp == null )
            {
                throw new ArgumentException( "must have an EffectComponent", "effect" );
            }

            VFXAttributes vfx = effect.GetComponent<VFXAttributes>();
            if( vfx == null )
            {
                throw new ArgumentException( "must have a VFXAttributesComponent", "effect" );
            }

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
            if( !loaded )
            {
                throw new CoreNotLoadedException( nameof( EffectsCore ) );
            }

            if( effect == null )
            {
                throw new ArgumentNullException( "effect" );
            }

            if( addedEffectSet.Contains( effect ) )
            {
                return;
            }

            _ = addedEffectSet.Add( effect );
            addedEffects.Add( effect );
        }


        static EffectsCore()
        {
            Log.Warning( "EffectsCore loaded" );
            HooksCore.RoR2.EffectCatalog.GetDefaultEffectDefs.On += GetDefaultEffectDefs_On;

            Log.Warning( "EffectsCore loaded" );
            loaded = true;
        }

        private static EffectDef[] GetDefaultEffectDefs_On( HooksCore.RoR2.EffectCatalog.GetDefaultEffectDefs.Orig orig )
        {
            EffectDef[] effects = orig();

            Int32 ind = effects.Length;
            Int32 newCount = addedEffects.Count;
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

        private static readonly HashSet<EffectDef> addedEffectSet = new HashSet<EffectDef>();
        private static readonly List<EffectDef> addedEffects = new List<EffectDef>();
    }
}
