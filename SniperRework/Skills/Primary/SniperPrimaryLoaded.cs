using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using System;
using UnityEngine.Networking;

namespace ReinSniperRework
{
    internal partial class Main
    {
        internal class SniperLoaded : BaseState
        {
            internal SniperShoot.ReloadQuality quality;

            public override void OnEnter()
            {
                base.OnEnter();
                if( this.quality == SniperShoot.ReloadQuality.None ) this.quality = SniperShoot.ReloadQuality.Normal;

                if( this.quality == SniperShoot.ReloadQuality.Perfect ) Util.PlaySound( "Play_MULT_m1_snipe_charge_end", base.gameObject );
            }
            public override void OnSerialize( NetworkWriter writer )
            {
                writer.Write( (Int16)this.quality );
            }
            public override void OnDeserialize( NetworkReader reader )
            {
                this.quality = (SniperShoot.ReloadQuality)reader.ReadInt16();
            }
            public override InterruptPriority GetMinimumInterruptPriority()
            {
                base.outer.SetNextState( new SniperShoot { quality = this.quality } );
                return InterruptPriority.Death;
            }
        }
    }
}


