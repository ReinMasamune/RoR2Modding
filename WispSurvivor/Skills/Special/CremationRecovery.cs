/*
using EntityStates;

namespace WispSurvivor.Skills.Special
{
    public class CremationRecovery : BaseState
    {
        public System.UInt32 skin;

        private System.Single duration;


        private System.Boolean fired = false;
        private System.Boolean rotated = false;

        private System.Single timer = 0f;

        private Components.WispPassiveController passive;

        public override void OnEnter()
        {
            base.OnEnter();
            this.skin = this.characterBody.skinIndex;

            this.duration = Cremation.baseRecoveryDuration / this.attackSpeedStat;

            this.PlayAnimation( "Body", "SpecialFire", "SpecialFire.playbackRate", this.duration );


            this.GetComponent<Components.WispAimAnimationController>().EndCannonMode( this.duration );
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if( this.fixedAge > this.duration )
            {
                this.PlayAnimation( "Body", "Idle" );
                RoR2.Util.PlaySound( "Stop_item_use_BFG_loop", this.gameObject );
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
*/