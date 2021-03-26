namespace ReinCore
{
    using System;
    using System.Collections.Generic;

    using MonoMod.Cil;

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
            //Log.Warning( "EffectsCore loaded" );
            //HooksCore.RoR2.EffectCatalog.GetDefaultEffectDefs.On += GetDefaultEffectDefs_On;
            HooksCore.RoR2.EffectCatalog.Init.Il += Init_Il;
            HooksCore.RoR2.EffectCatalog.SetEntries.Il += SetEntries_Il;

            //Log.Warning( "EffectsCore loaded" );
            loaded = true;
        }

        private static void SetEntries_Il(ILContext il)
        {
            var c = new ILCursor(il);
            if(c.TryGotoNext(MoveType.After, x => x.MatchCallOrCallvirt<Array>("Sort")))
            {
                c
                    .Mark(out var lab)
                    .Move(-11)
                    .MoveAfterLabels()
                    .Br_(lab);

            } else
            {
                Log.Warning("No sort call in EffectCatalog.SetEntries");
            }

            //c.LogFull();
        }



        private static void Init_Il(ILContext il) => new ILCursor(il)
            .GotoNext(MoveType.AfterLabel, x => x.MatchCallOrCallvirt(typeof(EffectCatalog), nameof(EffectCatalog.SetEntries)))
            .CallDel_(ArrayHelper.AppendDel(addedEffects));

        private static readonly HashSet<EffectDef> addedEffectSet = new HashSet<EffectDef>();
        private static readonly List<EffectDef> addedEffects = new List<EffectDef>();
    }
}
