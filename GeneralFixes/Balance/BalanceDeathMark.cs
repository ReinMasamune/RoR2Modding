using BepInEx;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Navigation;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using System.Collections;
using ReinCore;

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
            c.GotoNext( MoveType.Before, x => x.MatchLdloc( 17 ), x => x.MatchLdcI4( 1 ), x => x.MatchAdd(), x => x.MatchStloc( 17 ) );
            c.Index++;
            c.Remove();
            c.Emit( OpCodes.Ldc_I4_0 );
            c.GotoNext( MoveType.After, x => x.MatchLdloc( 17 ), x => x.MatchLdcI4( 4 ) );
            c.Index--;
            c.Remove();
            c.Emit( OpCodes.Ldc_I4, 30 );
        }
    }
}
