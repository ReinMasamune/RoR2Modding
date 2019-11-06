using RoR2;
using EntityStates;
using UnityEngine;
using RoR2.Projectile;
using UnityEngine.Networking;

namespace WispSurvivor.Skills.Special
{
    public class CremationRecovery : BaseState
    {
        public uint skin;

        private float duration;


        private bool fired = false;
        private bool rotated = false;

        private float timer = 0f;

        private Components.WispPassiveController passive;

        public override void OnEnter()
        {
            base.OnEnter();
            skin = characterBody.skinIndex;

            duration = Cremation.baseRecoveryDuration / attackSpeedStat;

            PlayAnimation("Body", "SpecialFire", "SpecialFire.playbackRate", duration);
            

            GetComponent<Components.WispAimAnimationController>().cannonMode = false;
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if( fixedAge > duration )
            {
                PlayAnimation("Body", "Idle");
                RoR2.Util.PlaySound("Stop_item_use_BFG_loop", gameObject);
                if ( isAuthority )
                {
                    outer.SetNextStateToMain();
                }
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Death;
        }
    }
}
