#if ANCIENTWISP
using System;

using EntityStates;

using RoR2;

using UnityEngine;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        internal class AWChargeUtility : BaseState
        {
            const Single baseChargeTime = 5.0f;

            private Single duration;

            private GameObject effectInstance1;
            private GameObject effectInstance2;


            public override void OnEnter()
            {
                base.OnEnter();

                this.duration = baseChargeTime / base.attackSpeedStat;

                //var par1 = base.FindModelChild( "MuzzleLeft" );
                //this.effectInstance1 = UnityEngine.Object.Instantiate<GameObject>( Main.AW_utilityChargeEffect, Vector3.zero, Quaternion.identity, par1 );
                //this.effectInstance1.GetComponent<BitSkinController>().Apply( WispBitSkin.GetWispSkin( base.characterBody.skinIndex ) );
                //var par2 = base.FindModelChild( "MuzzleRight" );
                //this.effectInstance2 = UnityEngine.Object.Instantiate<GameObject>( Main.AW_utilityChargeEffect, Vector3.zero, Quaternion.identity, par2 );
                //this.effectInstance2.GetComponent<BitSkinController>().Apply( WispBitSkin.GetWispSkin( base.characterBody.skinIndex ) );

                base.PlayCrossfade( "Gesture", "ChargeBomb", "ChargeBomb.playbackRate", this.duration, 0.2f );

                Util.PlayScaledSound( "Play_gravekeeper_attack2_charge", base.gameObject, 0.35f );
                Util.PlayScaledSound( "Play_gravekeeper_attack2_charge", base.gameObject, 0.35f );
                Util.PlayScaledSound( "Play_gravekeeper_attack2_charge", base.gameObject, 0.35f );

            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();

                if( base.fixedAge >= this.duration && base.isAuthority )
                {
                    base.outer.SetNextState( new AWFireUtility() );
                    return;
                }
            }

            public override void OnExit()
            {
                //Destroy effects
                if( this.effectInstance1 != null ) UnityEngine.Object.Destroy( this.effectInstance1 );
                if( this.effectInstance2 != null ) UnityEngine.Object.Destroy( this.effectInstance2 );
                base.OnExit();
            }

            public override InterruptPriority GetMinimumInterruptPriority()
            {
                return InterruptPriority.PrioritySkill;
            }
        }
    }

}
#endif
