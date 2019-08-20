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

        private float stopwatch;
        private float duration;
        private bool hasFiredGauntlet;
        private string muzzleString;
        private Transform muzzleTransform;
        private Animator animator;
        private ChildLocator childLocator;
        private GameObject tempMuzzleFlash;


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

            int elem = Mathf.RoundToInt(Random.Range(1f, 3f));

            tempMuzzleFlash = data.p_f_muzzle;

    
            data.p_dmg.damageType = DamageType.Generic;

            data.p_explode.enabled = true;
            data.p_trailProj.enabled = false;
            data.p_proxBeams.enabled = false;
            data.p_explode.fireChildren = false;


            switch ( elem )
            {
                case 1:
                    Chat.AddMessage("Fire");
                    data.p_control.ghostPrefab = data.p_f_projectile;
                    tempMuzzleFlash = data.p_f_muzzle;
                    break;

                case 2:
                    Chat.AddMessage("Ice");
                    data.p_control.ghostPrefab = data.p_i_projectile;
                    tempMuzzleFlash = data.p_i_muzzle;
                    break;

                case 3:
                    Chat.AddMessage("Lightning");
                    data.p_control.ghostPrefab = data.p_l_projectile;
                    tempMuzzleFlash = data.p_l_muzzle;
                    break;

                default:
                    Debug.Log("wtf...");
                    break;
            }

            if( elem == 1 )
            {
                data.p_dmg.damageType = data.p_dmg.damageType | DamageType.IgniteOnHit;
                //data.p_trailProj.enabled = true;
            }
            if( elem == 2 )
            {
                data.p_dmg.damageType = data.p_dmg.damageType | DamageType.SlowOnHit;
                data.p_explode.fireChildren = true;
            }
            if( elem == 3 )
            {
                data.p_proxBeams.enabled = true;
            }

            this.stopwatch = 0f;
            this.duration = data.p_baseDuration / this.attackSpeedStat;
            Util.PlayScaledSound(data.p_fireSound, base.gameObject, data.p_fireSoundPitch);
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();
            if (this.animator)
            {
                this.childLocator = this.animator.GetComponent<ChildLocator>();
            }
            Gauntlet gauntlet = this.gauntlet;
            if (gauntlet != Gauntlet.Left)
            {
                if (gauntlet != Gauntlet.Right)
                {
                    return;
                }
                this.muzzleString = "MuzzleRight";
                if (this.attackSpeedStat < data.p_attackSpeedAnimationSwitch)
                {
                    base.PlayCrossfade("Gesture, Additive", "Cast1Right", "FireGauntlet.playbackRate", this.duration, 0.1f);
                    base.PlayAnimation("Gesture Left, Additive", "Empty");
                    base.PlayAnimation("Gesture Right, Additive", "Empty");
                    return;
                }
                base.PlayAnimation("Gesture Right, Additive", "FireGauntletRight", "FireGauntlet.playbackRate", this.duration);
                base.PlayAnimation("Gesture, Additive", "HoldGauntletsUp", "FireGauntlet.playbackRate", this.duration);
                this.FireGauntlet();
                return;
            }
            else
            {
                this.muzzleString = "MuzzleLeft";
                if (this.attackSpeedStat < data.p_attackSpeedAnimationSwitch)
                {
                    base.PlayCrossfade("Gesture, Additive", "Cast1Left", "FireGauntlet.playbackRate", this.duration, 0.1f);
                    base.PlayAnimation("Gesture Left, Additive", "Empty");
                    base.PlayAnimation("Gesture Right, Additive", "Empty");
                    return;
                }
                base.PlayAnimation("Gesture Left, Additive", "FireGauntletLeft", "FireGauntlet.playbackRate", this.duration);
                base.PlayAnimation("Gesture, Additive", "HoldGauntletsUp", "FireGauntlet.playbackRate", this.duration);
                this.FireGauntlet();
                return;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.stopwatch += Time.fixedDeltaTime;
			if (this.animator.GetFloat("FireGauntlet.fire") > 0f && !this.hasFiredGauntlet)
			{
				this.FireGauntlet();
			}
			if (this.stopwatch < this.duration || !base.isAuthority)
			{
				return;
			}
			GenericSkill primary = base.skillLocator.primary;
			if (base.inputBank.skill1.down && primary.CanExecute())
			{
				primary.DeductStock(1);
				Primary fireBolt = new Primary();
				fireBolt.gauntlet = ((this.gauntlet == Primary.Gauntlet.Left) ? Gauntlet.Right : Gauntlet.Left);
				this.outer.SetNextState(fireBolt);
				return;
			}
			this.outer.SetNextStateToMain();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void FireGauntlet()
        {
            if (this.hasFiredGauntlet)
            {
                return;
            }
            base.characterBody.AddSpreadBloom(data.p_bloom);
            this.hasFiredGauntlet = true;
            Ray aimRay = base.GetAimRay();
            if (this.childLocator)
            {
                this.muzzleTransform = this.childLocator.FindChild(this.muzzleString);
            }
            if (data.p_f_muzzle)
            {
                EffectManager.instance.SimpleMuzzleFlash(tempMuzzleFlash, base.gameObject, this.muzzleString, false);
            }
            if (base.isAuthority)
            {
                ProjectileManager.instance.FireProjectile(data.p_projectile, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, data.p_f_damageCoef * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

        public override void OnSerialize(NetworkWriter writer )
        {
            base.OnSerialize(writer);
            writer.Write((byte)this.gauntlet);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.gauntlet = (Gauntlet)reader.ReadByte();
        }
    }
}
