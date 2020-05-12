namespace ReinCore
{
    using System;
    using System.Collections.Generic;

    using RoR2;

    using UnityEngine;

    // TODO: Docs for EffectsCore
    /// <summary>
    /// 
    /// </summary>
    public static class EffectsCore
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Boolean loaded { get; internal set; } = false;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static void AddEffect( GameObject effect )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static void AddEffect( EffectDef effect )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
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
            HooksCore.RoR2.EffectCatalog.GetDefaultEffectDefs.On += GetDefaultEffectDefs_On;

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
