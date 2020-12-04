namespace Rein.Sniper.States.Utility
{
    using System;

    using EntityStates;

    using ReinCore;

    using RoR2;

    using Rein.Sniper.Components;

    using UnityEngine;
    using UnityEngine.Networking;
    using Rein.Sniper.Modules;

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

        private const Single maxVSpeed = 10f;

        private const Single radius = 5f;

        private Single prevVSpeed = Single.NaN;

        private Single duration;

        private Vector3 direction;

        private Boolean launch = false;

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
                    position = base.transform.position + new Vector3(0f, 1f, 0f),
                    procChainMask = default,
                    procCoefficient = 1.0f,
                    radius = radius,
                    teamIndex = base.teamComponent.teamIndex,
                }.Fire();

                if(base.characterBody is SniperCharacterBody body)
                {
                    body.SendBonusReload(Enums.ReloadTier.Perfect);
                }

                var rot = new Quaternion(0f, 0.7071f, 0f, 0.7071f);

                var data = new EffectData()
                {
                    origin = base.transform.position + new Vector3(0f, 1f, 0f),
                    rotation = Util.QuaternionSafeLookRotation(this.direction, Vector3.up) * rot,
                    scale = radius,
                };

                EffectManager.SpawnEffect(VFXModule.GetKnifePickupSlash(base.characterBody.skinIndex), data, true);

                //EffectManager.SimpleMuzzleFlash(VFXModule.GetBackflipSlash(), base.gameObject, "Base", true);
            }

            base.PlayAnimation("Gesture, Override", "Backflip", "rateBackflip", this.duration);
            Util.PlaySound("Play_merc_m1_hard_swing", base.gameObject);

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

            if(!Single.IsNaN(this.prevVSpeed))
            {
                var delta = y - this.prevVSpeed;
                if(delta > maxVSpeed)
                {
                    this.launch = true;
                    if(base.isAuthority) base.outer.SetNextStateToMain();
                }
            }
            this.prevVSpeed = y;

            if(this.launch)
            {
                base.characterMotor.velocity = Vector3.zero;
            } else
            {
                Vector3 boost = speed * this.direction;
                boost.y = y;
                base.characterMotor.velocity = boost;
            }
            if(base.isAuthority && base.fixedAge > this.duration)
            {
                base.outer.SetNextStateToMain();
            }
        }
        public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.PrioritySkill;
    }
}
