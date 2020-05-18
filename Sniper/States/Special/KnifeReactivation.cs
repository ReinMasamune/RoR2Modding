namespace Sniper.States.Special
{
	using System;
	using System.Collections.Generic;
	using EntityStates;
	using KinematicCharacterController;
	using RoR2;
    using Sniper.Modules;
    using Sniper.SkillDefs;
	using Sniper.States.Bases;
	using UnityEngine;

	internal class KnifeReactivation : ReactivationBaseState<KnifeSkillData>
	{
		private const Single baseMovespeedMult = 30f;
		private const Single maxDurationMult = 5f;
		private const Single cancelDistance = 1f;
        private const Single endSpeedCarryover = 10f;

		private static GameObject blinkStartEffect = VFXModule.GetKnifeBlinkPrefab();
		internal static GameObject blinkEndEffect;


		private Single maxDuration;

        private Vector3 lastDirection = Vector3.zero;

		private Transform target;
		private CharacterMotor charMotor;
		private CharacterModel model;
		private HurtBoxGroup hbGroup;

		public override void OnEnter()
		{
			base.OnEnter();

			this.target = base.skillData.knifeInstance.transform;
            var dir = this.target.position - base.transform.position;
            var dist = dir.magnitude;
            dir = dir.normalized;
            this.maxDuration = dist / ( base.moveSpeedStat * baseMovespeedMult / maxDurationMult );

			this.charMotor = base.characterMotor;

			//this.charMotor.muteWalkMotion = true;

			this.model = base.GetModelTransform().GetComponent<CharacterModel>();
			this.hbGroup = base.characterBody.mainHurtBox.hurtBoxGroup;

            if( this.model != null )
            {
                this.model.invisibilityCount++;
            }

            if( this.hbGroup != null )
            {
                this.hbGroup.hurtBoxesDeactivatorCounter++;
            }

			if( base.isAuthority )
			{
				this.PlayStartEffects( new Ray( base.transform.position, dir ) );
			}
		}

		public override void FixedUpdate()
		{
			base.FixedUpdate();
            var dist = Single.PositiveInfinity;

            if( this.target != null )
            {
                var diff =  this.target.position - base.transform.position ;

                dist = diff.magnitude;
                var dir = diff.normalized;

                if( dist > 5f )
                {
                    this.lastDirection = dir;
                }


                this.charMotor.rootMotion += dir * ( base.moveSpeedStat * baseMovespeedMult * Time.fixedDeltaTime );
            }

			if( base.isAuthority && ( base.fixedAge >= this.maxDuration || dist <= cancelDistance || this.target == null ) )
			{
				base.outer.SetNextStateToMain();
			}
		}

		public override void OnExit()
		{
			base.OnExit();
            this.PlayEndEffects();
            if( this.model != null )
            {
                this.model.invisibilityCount--;
            }
            if( this.hbGroup != null )
            {
                this.hbGroup.hurtBoxesDeactivatorCounter--;
            }
            //this.charMotor.muteWalkMotion = false;
            this.charMotor.velocity = this.lastDirection * endSpeedCarryover;
		}



		private void PlayStartEffects(Ray move)
		{
            var data = new EffectData
            {
                rotation = Util.QuaternionSafeLookRotation( move.direction ),
                origin = move.origin
            };
            EffectManager.SpawnEffect( blinkStartEffect, data, true );
		}

		private void PlayEndEffects()
		{
		}


		public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Death;
	}
}
