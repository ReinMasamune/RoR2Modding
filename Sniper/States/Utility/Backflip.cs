namespace Sniper.States.Utility
{
    using System;

    using EntityStates;

    using ReinCore;

    using RoR2;

    using Sniper.Components;

    using UnityEngine;
    using UnityEngine.Networking;

    internal class Backflip : GenericCharacterMain
    {
        private static readonly AnimationCurve backflipSpeedCurve = new AnimationCurve(
            new Keyframe( 0f, 0f ),
            new Keyframe( 0.05f, 0f ),
            new Keyframe( 0.15f, 1f ),
            new Keyframe( 0.3f, 0.9f ),
            new Keyframe( 0.9f, 0.3f ),
            new Keyframe( 1f, 0.1f)
        );
        private const Single baseDuration = 0.5f;
        private const Single speedMultiplier = 8f;
        private const Single upwardsBoost = 5f;
        private const Single damageMultiplier = 1.0f;
        private const Single force = 50f;

        private Single duration;

        private Vector3 direction;

        private Single currentSpeed
        {
            get => speedMultiplier * base.moveSpeedStat / (base.characterBody.isSprinting ? base.characterBody.sprintingSpeedMultiplier : 1.0f) * backflipSpeedCurve.Evaluate(base.fixedAge / this.duration);
            //get => speedMultiplier * backflipSpeedCurve.Evaluate(base.fixedAge / this.duration);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = baseDuration / (base.moveSpeedStat / 7f);
            base.characterBody.isSprinting = true;


            if(base.isAuthority)
            {
                Vector3 dir = base.GetAimRay().direction;
                this.direction = -dir;
                this.direction.y = 0f;
                this.direction = this.direction.normalized;


                BlastAttack.Result res = new BlastAttack
                {
                    attacker = base.gameObject,
                    attackerFiltering = AttackerFiltering.NeverHit,
                    baseDamage = base.damageStat * damageMultiplier,
                    baseForce = 0.0f,
                    bonusForce = dir * force,
                    crit = false,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = DamageType.Stun1s,
                    falloffModel = BlastAttack.FalloffModel.None,
                    impactEffect = EffectIndex.Invalid,
                    inflictor = null,
                    losType = BlastAttack.LoSType.None,
                    position = base.transform.position,
                    procChainMask = default,
                    procCoefficient = 1.0f,
                    radius = 4.0f,
                    teamIndex = base.teamComponent.teamIndex,
                }.Fire();

                if(base.characterBody is SniperCharacterBody body)
                {
                    body.SendBonusReload(Enums.ReloadTier.Perfect);
                }
            }

            base.PlayAnimation("Gesture, Override", "Backflip", "rateBackflip", this.duration);
            // FUTURE: Play Sound
            // FUTURE: VFX

            base.characterMotor.Motor.ForceUnground();
            Single speed = this.currentSpeed;
            Vector3 boost = speed * this.direction;
            boost += new Vector3(0f, upwardsBoost, 0f);
            base.characterMotor.velocity = boost;

            base.StartAimMode(2f);
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(new PackedUnitVector3(this.direction));
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.direction = reader.ReadPackedUnitVector3().Unpack();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            Single speed = this.currentSpeed;
            Single y = base.characterMotor.velocity.y;
            Vector3 boost = speed * this.direction;
            boost.y = y;
            base.characterMotor.velocity = boost;

            if(base.isAuthority && base.fixedAge > this.duration)
            {
                base.outer.SetNextStateToMain();
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.PrioritySkill;
    }
}
