using BepInEx;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Navigation;
using System;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using System.Collections;
using ReinCore;
using MonoMod.Cil;
using Mono.Cecil.Cil;

namespace ReinGeneralFixes
{
    internal partial class Main
    {
        partial void BalanceDeathMark()
        {
            this.Enable += this.Main_Enable5;
            this.Disable += this.Main_Disable5;
            this.FirstFrame += this.Main_FirstFrame;
        }

        private void Main_FirstFrame()
        {
            var bleed = BuffCatalog.GetBuffDef( BuffIndex.Bleeding );
            bleed.isDebuff = true;

            var fire1 = BuffCatalog.GetBuffDef( BuffIndex.OnFire );
            fire1.isDebuff = true;

            var poison = BuffCatalog.GetBuffDef( BuffIndex.Poisoned );
            poison.isDebuff = true;

            var blight = BuffCatalog.GetBuffDef( BuffIndex.Blight );
            blight.isDebuff = true;

            var mark = BuffCatalog.GetBuffDef( BuffIndex.DeathMark );
            mark.canStack = true;
        }

        private void Main_Disable5()
        {
            HooksCore.RoR2.GlobalEventManager.OnHitEnemy.Il -= this.OnHitEnemy_Il;
            try
            {
                var bleed = BuffCatalog.GetBuffDef( BuffIndex.Bleeding );
                bleed.isDebuff = false;

                var fire1 = BuffCatalog.GetBuffDef( BuffIndex.OnFire );
                fire1.isDebuff = false;

                var poison = BuffCatalog.GetBuffDef( BuffIndex.Poisoned );
                poison.isDebuff = false;

                var blight = BuffCatalog.GetBuffDef( BuffIndex.Blight );
                blight.isDebuff = false;
            } catch { }
        }
        private void Main_Enable5()
        {
            HooksCore.RoR2.GlobalEventManager.OnHitEnemy.Il += this.OnHitEnemy_Il;
        }

        private void OnHitEnemy_Il( ILContext il )
        {
            ILCursor c = new ILCursor( il );

            c.GotoNext( MoveType.Before, x => x.MatchLdloc( 17 ), x => x.MatchLdcI4( 1 ), x => x.MatchAdd(), x => x.MatchStloc( 17 ) );
            c.Index++;
            c.Remove();
            c.Emit( OpCodes.Ldloc_1 );
            c.Emit( OpCodes.Ldloc, 64 );
            c.EmitDelegate<Func<CharacterBody, BuffIndex, Int32>>( ( body, index ) =>
            {
                if( body.HasBuff( index ) )
                {
                    var def = BuffCatalog.GetBuffDef(index);
                    if( def.canStack )
                    {
                        return body.GetBuffCount( index );
                    } else
                    {
                        return 10;
                    }
                }
                return 0;
            } );

            var c2 = c.Clone();
            c2.GotoPrev( MoveType.AfterLabel, x => x.MatchLdcI4( 1 ), x => x.MatchStloc( 18 ), x => x.MatchBr( out _ ) );
            c2.Remove();
            c2.Emit( OpCodes.Ldloc_1 );
            c2.EmitDelegate<Func<CharacterBody, Int32>>( ( body ) => body.GetBuffCount( BuffIndex.DeathMark ) );
            c2.Index++;
            c2.Remove();


            c.GotoNext( MoveType.Before, x => x.MatchLdloc( 17 ), x => x.MatchLdcI4( 1 ), x => x.MatchAdd(), x => x.MatchStloc( 17 ) );
            c.Index++;
            c.Remove();
            c.Emit( OpCodes.Ldc_I4_0 );

            c.GotoNext( MoveType.After, x => x.MatchLdloc( 18 ) );
            c.Emit( OpCodes.Ldloc, 16 );
            c.EmitDelegate<Func<Int32, Int32, Int32>>( ( count, itemCount ) =>
            {
                return itemCount > count ? 0 : 1;
            } );

            c.GotoNext( MoveType.After, x => x.MatchLdloc( 17 ), x => x.MatchLdcI4( 4 ) );
            c.Index--;
            c.Remove();
            c.Emit( OpCodes.Ldc_I4, 35 );

            c.GotoNext( MoveType.AfterLabel, x => x.MatchLdcI4( 0 ), x => x.MatchStloc( 67 ) );
            c.Emit( OpCodes.Ldloc_1 );
            c.Emit( OpCodes.Ldloc, 18 );
            c.EmitDelegate<Action<CharacterBody, Int32>>( ( body, count ) =>
            {
                for( Int32 i = 0; i < count; ++i )
                {
                    body.RemoveBuff( BuffIndex.DeathMark );
                }
            });
        }
    }
}
