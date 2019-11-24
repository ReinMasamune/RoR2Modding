using EntityStates;
using UnityEngine;

namespace WispSurvivor.Skills.Special
{
    public class IncinerationWindup : BaseState
    {
        public static System.Single baseDuration = 0.5f;
        public static System.Single rotatePoint = 0.6f;
        public static System.Single camYOff = 0.75f;
        public static System.Single camZMult = 0.5f;

        private System.Single duration;
        private Vector3 camStart;
        private Vector3 camEnd;

        public override void OnEnter()
        {
            base.OnEnter();

            this.PlayCrossfade( "Gesture", "Idle", this.duration / 2f );
            this.duration = baseDuration / this.attackSpeedStat;

            this.characterBody.SetAimTimer( this.duration * 1.5f );
            this.PlayAnimation( "Body", "SpecialTransform", "SpecialTransform.playbackRate", this.duration );

            camStart = cameraTargetParams.idealLocalCameraPos;
            camEnd = new Vector3( camStart.x, camStart.y - camYOff, camStart.z * camZMult );
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            cameraTargetParams.idealLocalCameraPos = Vector3.Lerp( camStart, camEnd, this.fixedAge / this.duration );

            //Gradual slowdown of movement

            if( this.fixedAge >= this.duration * rotatePoint )
            {
                this.GetComponent<Components.WispAimAnimationController>().StartCannonMode( this.duration * 0.5f, 60.0f );
            }

            if( this.fixedAge >= this.duration )
            {
                if( this.isAuthority )
                {
                    this.outer.SetNextState( new Incineration
                    {
                        camPos1 = camEnd,
                        camPos2 = camStart
                    } );
                }
            }
        }

        public override void OnExit() => base.OnExit();

        public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Death;
    }
}
