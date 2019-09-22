using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using ReinArtificerer;

namespace EntityStates.ReinArtificerer.Artificer.Weapon
{
    public class BaseSpecial : BaseState
    {
        // TODO: Special Base EntityState; Needs major work overall
        ReinDataLibrary data;
        ReinElementTracker elements;
        ReinLightningBuffTracker lightning;

        public int fireLevel = 0;
        public int iceLevel = 0;
        public int lightningLevel = 0;


        private float tickDamageCoefficient;
        private float flamethrowerStopwatch;
        private float stopwatch;
        private float entryDuration;
        private float flamethrowerDuration;
        private bool hasBegunFlamethrower;
        private ChildLocator childLocator;
        private Transform leftFlamethrowerTransform;
        private Transform rightFlamethrowerTransform;
        private Transform leftMuzzleTransform;
        private Transform rightMuzzleTransform;
        private bool isCrit;
        private const float flamethrowerEffectBaseDistance = 16f;

        public override void OnEnter()
        {
            base.OnEnter();
            data = base.GetComponent<ReinDataLibrary>();
            elements = data.element;
            lightning = data.lightning;
            Chat.AddMessage("This is the base special");

            elements.AddElement(ReinElementTracker.Element.fire, 2);

            stopwatch = 0f;
            entryDuration = data.r_f_entryDur / attackSpeedStat;
            flamethrowerDuration = data.r_f_totalDur;
            Transform modelTransform = base.GetModelTransform();
            if (base.characterBody)
            {
                base.characterBody.SetAimTimer(entryDuration + flamethrowerDuration + 1f);
            }
            if (modelTransform)
            {
                childLocator = modelTransform.GetComponent<ChildLocator>();
                leftMuzzleTransform = childLocator.FindChild("MuzzleLeft");
                rightMuzzleTransform = childLocator.FindChild("MuzzleRight");
            }
            float num = flamethrowerDuration * data.r_f_tickFreq;
            tickDamageCoefficient = data.r_f_totalDamage / num;
            if (base.isAuthority && base.characterBody)
            {
                isCrit = Util.CheckRoll(critStat, base.characterBody.master);
            }
            base.PlayAnimation("Gesture, Additive", "PrepFlamethrower", "Flamethrower.playbackRate", entryDuration);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
            if (stopwatch >= entryDuration && !hasBegunFlamethrower)
            {
                hasBegunFlamethrower = true;
                Util.PlaySound(data.r_f_startSound, base.gameObject);
                base.PlayAnimation("Gesture, Additive", "Flamethrower", "Flamethrower.playbackRate", flamethrowerDuration);
                if (childLocator)
                {
                    Transform parent = childLocator.FindChild("MuzzleLeft");
                    Transform parent2 = childLocator.FindChild("MuzzleRight");
                    leftFlamethrowerTransform = UnityEngine.Object.Instantiate<GameObject>(data.r_f_mainEffect, parent).transform;
                    rightFlamethrowerTransform = UnityEngine.Object.Instantiate<GameObject>(data.r_f_mainEffect, parent2).transform;
                    if (leftFlamethrowerTransform)
                    {
                        leftFlamethrowerTransform.GetComponent<ScaleParticleSystemDuration>().newDuration = flamethrowerDuration;
                    }
                    if (rightFlamethrowerTransform)
                    {
                        rightFlamethrowerTransform.GetComponent<ScaleParticleSystemDuration>().newDuration = flamethrowerDuration;
                    }
                }
                FireGauntlet("MuzzleCenter");
            }
            if (hasBegunFlamethrower)
            {
                flamethrowerStopwatch += Time.deltaTime;
                if (flamethrowerStopwatch > 1f / data.r_f_tickFreq)
                {
                    flamethrowerStopwatch -= 1f / data.r_f_tickFreq;
                    FireGauntlet("MuzzleCenter");
                }
                UpdateFlamethrowerEffect();
            }
            if (stopwatch >= flamethrowerDuration + entryDuration && base.isAuthority)
            {
                if( lightning.GetBuffed() )
                {
                    LightningBlink blink = new LightningBlink();
                    blink.castValue = data.r_blinkCastValue;
                    data.bodyState.SetNextState(blink);
                }
                outer.SetNextStateToMain();
                return;
            }
        }

        public override void OnExit()
        {
            Util.PlaySound(data.r_f_endSound, base.gameObject);
            base.PlayCrossfade("Gesture, Additive", "ExitFlamethrower", 0.1f);
            if (leftFlamethrowerTransform)
            {
                EntityState.Destroy(leftFlamethrowerTransform.gameObject);
            }
            if (rightFlamethrowerTransform)
            {
                EntityState.Destroy(rightFlamethrowerTransform.gameObject);
            }
            base.OnExit();
        }

        private void FireGauntlet(string muzzleString)
        {
            Ray aimRay = base.GetAimRay();
            if (base.isAuthority)
            {
                new BulletAttack
                {
                    owner = base.gameObject,
                    weapon = base.gameObject,
                    origin = aimRay.origin,
                    aimVector = aimRay.direction,
                    minSpread = 0f,
                    damage = tickDamageCoefficient * damageStat,
                    force = data.r_f_hitForce,
                    muzzleName = muzzleString,
                    hitEffectPrefab = data.r_f_impactEffect,
                    isCrit = isCrit,
                    radius = data.r_f_radius,
                    falloffModel = BulletAttack.FalloffModel.None,
                    stopperMask = LayerIndex.world.mask,
                    procCoefficient = data.r_f_procCoef,
                    maxDistance = data.r_f_maxDistance,
                    smartCollision = true,
                    damageType = (Util.CheckRoll(data.r_f_igniteChance, base.characterBody.master) ? DamageType.IgniteOnHit : DamageType.Generic)
                }.Fire();
                if (base.characterMotor)
                {
                    base.characterMotor.ApplyForce(aimRay.direction * -data.r_f_recoil, false, false);
                }
            }
        }

        private void UpdateFlamethrowerEffect()
        {
            float num = data.r_f_maxDistance;
            Ray aimRay = base.GetAimRay();
            Vector3 direction = aimRay.direction;
            Vector3 direction2 = aimRay.direction;
            float num2 = data.r_f_maxDistance;
            leftFlamethrowerTransform.forward = direction;
            rightFlamethrowerTransform.forward = direction2;
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
