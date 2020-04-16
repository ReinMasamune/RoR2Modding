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
using EntityStates.Huntress.HuntressWeapon;
using UnityEngine.Networking;

namespace ReinGeneralFixes
{
    internal partial class Main
    {
        partial void FixHuntressFlurry()
        {
            this.Enable += this.Main_Enable1;
            this.Disable += this.Main_Disable1;
        }

        private void Main_Disable1()
        {

        }
        private void Main_Enable1()
        {
            //HooksCore.EntityStates.Huntress.HuntressWeapon.FireSeekingArrow.FireOrbArrow.Il += this.FireOrbArrow_Il;
        }

        private void FireOrbArrow_Il( ILContext il )
        {
            var type = typeof(FireSeekingArrow);
            var allFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic;
            var arrowReloadDuration = type.GetField( "arrowReloadDuration", allFlags );
            var arrowReloadTimer = type.GetField( "arrowReloadTimer", allFlags );


            ILCursor c = new ILCursor( il );
            ILLabel label = null;
            c.GotoNext( x => x.MatchCallOrCallvirt( typeof( NetworkServer ), "get_active" ), x => x.MatchBrtrue( out label ) );

            c.GotoLabel( label );
            c.GotoNext( MoveType.Before, x => x.MatchLdarg( 0 ), x => x.MatchLdarg( 0 ), x => x.MatchLdfld( typeof( FireSeekingArrow ), "arrowReloadDuration" ) );
            c.Emit( OpCodes.Ldarg_0 );
            c.Index += 2;

        }
    }
}
