using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using ReinArtificerer;

namespace EntityStates.ReinArtificerer.Artificer.Weapon
{
    public class Primary : BaseState
    {
        ReinDataLibrary data;
        ReinElementTracker elements;
        ReinElementTracker.Element mainElem;
        ReinLightningBuffTracker lightning;

        int fireLevel = 0;
        int iceLevel = 0;
        int lightningLevel = 0;

        private float stopwatch;
        private float duration;
        private bool hasFiredGauntlet;
        private string muzzleString;
        private Transform muzzleTransform;
        private Animator animator;
        private ChildLocator childLocator;

        public Gauntlet gauntlet;

        public enum Gauntlet
        {
            Left,
            Right
        }

        public override void OnEnter()
        {
            base.OnEnter();            
            data = base.GetComponent<ReinDataLibrary>();
            elements = data.element;
            lightning = data.lightning;

            mainElem = elements.GetMainElement();
            fireLevel = elements.GetElementLevel(ReinElementTracker.Element.fire);
            iceLevel = elements.GetElementLevel(ReinElementTracker.Element.ice);
            lightningLevel = elements.GetElementLevel(ReinElementTracker.Element.lightning);

            stopwatch = 0f;
            duration = data.p_baseDuration / attackSpeedStat;

            Util.PlayScaledSound(data.p_fireSound, base.gameObject, data.p_fireSoundPitch);

            base.characterBody.SetAimTimer(2f);

            animator = base.GetModelAnimator();
            if (animator)
            {
                childLocator = animator.GetComponent<ChildLocator>();
            }

            Gauntlet gaunt = gauntlet;

            switch (gaunt)
            {
                case Gauntlet.Left:
                    muzzleString = "MuzzleLeft";
                    if( attackSpeedStat < data.p_attackSpeedAnimationSwitch )
                    {
                        base.PlayCrossfade("Gesture, Additive", "Cast1Left", "FireGauntlet.playbackRate", duration, 0.1f);
                        base.PlayAnimation("Gesture Left, Additive", "Empty");
                        base.PlayAnimation("Gesture Right, Additive", "Empty");
                        break;
                    }
                    base.PlayAnimation("Gesture Left, Additive", "FireGauntletLeft", "FireGauntlet.playbackRate", duration);
                    base.PlayAnimation("Gesture, Additive", "HoldGauntletsUp", "FireGauntlet.playbackRate", duration);
                    FireGauntlet();
                    break;
                case Gauntlet.Right:
                    muzzleString = "MuzzleRight";
                    if( attackSpeedStat < data.p_attackSpeedAnimationSwitch )
                    {
                        base.PlayCrossfade("Gesture, Additive", "Cast1Right", "FireGauntlet.playbackRate", duration, 0.1f);
                        base.PlayAnimation("Gesture Left, Additive", "Empty");
                        base.PlayAnimation("Gesture Right, Additive", "Empty");
                        break;
                    }
                    base.PlayAnimation("Gesture Right, Additive", "FireGauntletRight", "FireGauntlet.playbackRate", duration);
                    base.PlayAnimation("Gesture, Additive", "HoldGauntletsUp", "FireGauntlet.playbackRate", duration);
                    FireGauntlet();
                    break;
                default:
                    Debug.Log("Invalid Gauntlet");
                    break;
            }
        }
        //Good
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            stopwatch += Time.fixedDeltaTime;
			if (animator.GetFloat("FireGauntlet.fire") > 0f && !hasFiredGauntlet)
			{
				FireGauntlet();
			}
			if (stopwatch < duration || !base.isAuthority)
			{
				return;
			}
			GenericSkill primary = base.skillLocator.primary;
			if (base.inputBank.skill1.down && primary.CanExecute())
			{
				primary.DeductStock(1);
				Primary fireBolt = new Primary();
				fireBolt.gauntlet = ((gauntlet == Primary.Gauntlet.Left) ? Gauntlet.Right : Gauntlet.Left);
				outer.SetNextState(fireBolt);
				return;
			}
			outer.SetNextStateToMain();
        }

        //Good
        public override void OnExit()
        {
            if( lightning.GetBuffed() )
            {
                LightningBlink blink = new LightningBlink();
                blink.castValue = data.p_blinkCastValue;
                data.bodyState.SetNextState(blink);
            }
            base.OnExit();
        }

        private void FireGauntlet()
        {
            if (hasFiredGauntlet)
            {
                return;
            }
            base.characterBody.AddSpreadBloom(data.p_bloom);
            hasFiredGauntlet = true;

            // TODO: Primary EntityState; Elemental Scalings

            GameObject tempMuzzleFlash;
            data.p_explode.enabled = true;
            data.p_proxBeams.enabled = false;
            data.p_explode.fireChildren = false;

            data.p_explode.blastRadius = data.p_baseRadius;
            data.p_i_frostSimp.lifetime = data.p_i_baseDur;
            data.p_proxBeams.listClearInterval = data.p_l_proxListClear;
            data.p_proxBeams.attackInterval = data.p_l_proxAttackInt;
            data.p_proxBeams.attackRange = data.p_l_proxRange;

            data.p_dmg.damageType = DamageType.Generic;

            switch (mainElem)
            {
                case ReinElementTracker.Element.fire:
                    Chat.AddMessage("Fire firebolt");
                    elements.AddElement(ReinElementTracker.Element.fire, 1);
                    tempMuzzleFlash = data.p_f_muzzle;
                    data.p_control.ghostPrefab = data.p_f_projectile;
                    break;
                case ReinElementTracker.Element.ice:
                    Chat.AddMessage("Ice firebolt");
                    elements.AddElement(ReinElementTracker.Element.ice, 1);
                    data.p_control.ghostPrefab = data.p_i_projectile;
                    tempMuzzleFlash = data.p_i_muzzle;
                    break;
                case ReinElementTracker.Element.lightning:
                    Chat.AddMessage("Lightning Firebolt");
                    elements.AddElement(ReinElementTracker.Element.lightning, 1);
                    data.p_control.ghostPrefab = data.p_l_projectile;
                    tempMuzzleFlash = data.p_l_muzzle;
                    break;
                case ReinElementTracker.Element.none:
                    Chat.AddMessage("Base firebolt");
                    data.p_control.ghostPrefab = data.p_f_projectile;
                    tempMuzzleFlash = data.p_f_muzzle;
                    break;
                default:
                    Chat.AddMessage("You fucking broke it... Good job moron");
                    data.p_control.ghostPrefab = data.p_f_projectile;
                    tempMuzzleFlash = data.p_f_muzzle;
                    break;
            }
            if (fireLevel > 0)
            {
                data.p_dmg.damageType = data.p_dmg.damageType | DamageType.IgniteOnHit;
                data.p_explode.blastRadius *= 1f + (fireLevel * data.p_f_radMod);
            }
            if (iceLevel > 0)
            {
                data.p_explode.fireChildren = true;
                data.p_i_frostSimp.lifetime *= 1f + (iceLevel * data.p_i_durMod);
            }
            if (lightningLevel > 0)
            {
                data.p_proxBeams.enabled = true;
                data.p_proxBeams.attackRange *= 1f + (lightningLevel * data.p_l_proxRangeMod);
                data.p_proxBeams.attackInterval /= 1f + (lightningLevel * data.p_l_proxAttackIntMod);
                data.p_proxBeams.listClearInterval /= 1f + (lightningLevel * data.p_l_proxListClearMod);
            }


            Ray aimRay = base.GetAimRay();
            if (childLocator)
            {
                muzzleTransform = childLocator.FindChild(muzzleString);
            }
            if (data.p_f_muzzle)
            {
                EffectManager.instance.SimpleMuzzleFlash(tempMuzzleFlash, base.gameObject, muzzleString, false);
            }
            if (base.isAuthority)
            {
                // TODO: Primary EntityState; FireProjectile is outdated
                ProjectileManager.instance.FireProjectile(data.p_projectile, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, data.p_damageCoef * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);
            }
        }
        //Good
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
        //Good
        public override void OnSerialize(NetworkWriter writer )
        {
            base.OnSerialize(writer);
            writer.Write((byte)gauntlet);
        }
        //Good
        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            gauntlet = (Gauntlet)reader.ReadByte();
        }
    }
}
