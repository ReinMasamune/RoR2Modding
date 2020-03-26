#if ROGUEWISP
using EntityStates;
using UnityEngine;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        public class IncinerationRecovery : BaseState
        {
            public static System.Single baseDuration = 0.75f;
            public System.UInt32 skin;

            public Vector3 camPos1;
            public Vector3 camPos2;

            private System.Single duration;


            private readonly System.Boolean fired = false;
            private readonly System.Boolean rotated = false;

            private readonly System.Single timer = 0f;

            private readonly WispPassiveController passive;

            public override void OnEnter()
            {
                base.OnEnter();
                this.skin = this.characterBody.skinIndex;

                this.duration = baseDuration / this.attackSpeedStat;

                this.PlayAnimation( "Body", "SpecialFire", "SpecialFire.playbackRate", this.duration * 1.2f );


                this.GetComponent<WispAimAnimationController>().EndCannonMode( this.duration * 0.5f );
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();

                this.cameraTargetParams.idealLocalCameraPos = Vector3.Lerp( this.camPos1, this.camPos2, this.fixedAge / this.duration );

                if( this.fixedAge > this.duration )
                {
                    this.PlayAnimation( "Body", "Idle" );
                    if( this.isAuthority )
                    {
                        this.outer.SetNextStateToMain();
                    }
                }
            }

            public override void OnExit() => base.OnExit();

            public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Death;
        }
    }

}
#endif