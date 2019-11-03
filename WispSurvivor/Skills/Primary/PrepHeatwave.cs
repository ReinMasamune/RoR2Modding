using RoR2;
using EntityStates;
using UnityEngine;
using System;
using RoR2.Orbs;
using UnityEngine.Networking;
using System.Linq;

namespace WispSurvivor.Skills.Primary
{
    public class PrepHeatwave : BaseState
    {
        public static float basePrepDuration = 0.2f;
        public static float baseFireDuration = 0.425f;
        public static float baseFireDelay = 0.02125f;
        public static float baseDamage = 2.5f;
        public static float explosionRadius = 6f;
        public static float maxRange = 40f;
        public static float maxAngle = 7.5f;
        public static float chargeDamageScaler = 0.5f;
        public static float noStockDamageMult = 1.0f;
        public static float noStockSpeedMult = 0.5f;
        public static float animReturnPercent = 0.55f;

        public static double baseChargeAdded = 10.0;
        public static double noStockChargeMult = 0.5;

        private float prepDuration;
        private float fireDuration;
        private float fireDelay;
        private float damageValue;

        private double chargeAdded;

        private bool crit;

        private uint skin = 0;

        private Components.WispPassiveController passive;
        private ChildLocator childLoc;
        private Animator anim;
        
        public override void OnEnter()
        {
            base.OnEnter();
            passive = gameObject.GetComponent<Components.WispPassiveController>();

            bool hasStock = skillLocator.primary.stock > 0;
            skillLocator.primary.stock = hasStock ? skillLocator.primary.stock - 1 : 0;

            float durationMult = hasStock ? 1f : 1f / noStockSpeedMult;
            prepDuration = basePrepDuration * durationMult / attackSpeedStat;
            fireDuration = baseFireDuration * durationMult / attackSpeedStat;
            float totalDuration = prepDuration + fireDuration;
            fireDelay = baseFireDelay * durationMult / attackSpeedStat;

            chargeAdded = baseChargeAdded * (hasStock ? 1.0 : noStockChargeMult);

            damageValue = damageStat;
            damageValue *= baseDamage;
            damageValue *= (1f - chargeDamageScaler) + chargeDamageScaler * (float)((passive.ReadCharge() - 100.0) / 100.0);
            damageValue *= hasStock ? 1f : noStockDamageMult;

            passive.AddCharge(chargeAdded);

            crit = RollCrit();

            if( isAuthority )
            {
                skin = characterBody.skinIndex;
            }

            Transform modelTrans = base.GetModelTransform();
            anim = GetModelAnimator();
            modelTrans = GetModelTransform();
            childLoc = modelTrans.GetComponent<ChildLocator>();
            string animStr = anim.GetCurrentAnimatorStateInfo(anim.GetLayerIndex("Gesture")).IsName("Throw1") ? "Throw2" : "Throw1";
            PlayCrossfade("Gesture", animStr, "Throw.playbackRate", totalDuration / (1f - animReturnPercent), 0.2f);

            if( characterBody )
            {
                characterBody.SetAimTimer(totalDuration + 1f);
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            characterBody.isSprinting = false;

            if( fixedAge >= prepDuration && isAuthority )
            {
                this.outer.SetNextState(new FireHeatwave
                {
                    fireDuration = fireDuration,
                    fireDelay = fireDelay,
                    damageValue = damageValue,
                    explosionRadius = explosionRadius,
                    maxRange = maxRange,
                    maxAngle = maxAngle,
                    crit = crit,
                    skin = skin
                });
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
