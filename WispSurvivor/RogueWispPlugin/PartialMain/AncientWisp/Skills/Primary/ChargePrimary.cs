#if ANCIENTWISP
using System;

using EntityStates;

using Rein.RogueWispPlugin.Helpers;

using RoR2;

using UnityEngine;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        internal class AWChargePrimary : BaseState
        {
            const Single baseDuration = 1.5f;


            private Single duration;

            private GameObject chargeInstance;

            public override void OnEnter()
            {
                base.OnEnter();

                this.duration = baseDuration / base.attackSpeedStat;

                base.PlayCrossfade( "Gesture", "ChargeRHCannon", "ChargeRHCannon.playbackRate", this.duration, 0.2f );

                var muzzleTransform = base.GetModelTransform().GetComponent<ChildLocator>().FindChild( "MuzzleRight" );

                this.chargeInstance = UnityEngine.Object.Instantiate<GameObject>( Main.AW_primaryChargeEffect, muzzleTransform.position, muzzleTransform.rotation, muzzleTransform );
                this.chargeInstance.GetComponent<BitSkinController>().Apply( WispBitSkin.GetWispSkin( base.characterBody.skinIndex ) );

                base.characterBody.SetAimTimer( this.duration );

                Util.PlayScaledSound( "Play_greater_wisp_attack", base.gameObject, base.attackSpeedStat * ( 2f / baseDuration ) );
                Util.PlayScaledSound( "Play_greater_wisp_attack", base.gameObject, base.attackSpeedStat * ( 2f / baseDuration ) );
                Util.PlayScaledSound( "Play_greater_wisp_attack", base.gameObject, base.attackSpeedStat * ( 2f / baseDuration ) );
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();

                if( base.fixedAge >= this.duration && base.isAuthority )
                {
                    base.outer.SetNextState( new AWFirePrimary() );
                    return;
                }
            }

            public override void OnExit()
            {
                base.OnExit();

                UnityEngine.Object.Destroy( this.chargeInstance );
            }

            public override InterruptPriority GetMinimumInterruptPriority()
            {
                return InterruptPriority.PrioritySkill;
            }
        }
    }

}
#endif