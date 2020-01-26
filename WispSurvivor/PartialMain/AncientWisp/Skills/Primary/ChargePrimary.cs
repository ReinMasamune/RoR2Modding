#if ANCIENTWISP
using EntityStates;
using System;
using UnityEngine;

namespace RogueWispPlugin
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

                this.chargeInstance = UnityEngine.Object.Instantiate<GameObject>( EntityStates.AncientWispMonster.ChargeRHCannon.effectPrefab, muzzleTransform.position, muzzleTransform.rotation, muzzleTransform );

                base.characterBody.SetAimTimer( this.duration );
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