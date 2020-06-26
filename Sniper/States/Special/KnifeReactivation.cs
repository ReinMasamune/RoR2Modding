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
    using UnityEngine.Networking;

    internal class KnifeReactivation : ReactivationBaseState<KnifeSkillData>
	{
		private const Single baseMovespeedMult = 30f;
		private const Single maxDurationMult = 5f;
		private const Single cancelDistance = 3f;
		private const Single endSpeedCarryover = 10f;
        private const Single distanceJumpCancelThreshold = 2f;
        private static GameObject blinkStartEffect = VFXModule.GetKnifeBlinkPrefab();
		internal static GameObject blinkEndEffect;


		private Single maxDuration;

		private Vector3 lastDirection = Vector3.zero;
        private Single lastDistance;

		private Transform target;
		private CharacterMotor charMotor;
		private CharacterModel model;
		private HurtBoxGroup hbGroup;

		public override void OnEnter()
		{
			base.OnEnter();

			this.target = base.skillData?.knifeInstance?.transform;
            if( this.target != null && base.isAuthority )
            {
                var dir = this.target.position - base.transform.position;
                var dist = dir.magnitude;
                dir = dir.normalized;
                this.maxDuration = dist / ( base.moveSpeedStat * baseMovespeedMult / maxDurationMult );

                if( base.isAuthority )
                {
                    this.PlayStartEffects( new Ray( base.transform.position, dir ) );
                }
            }

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
            this.lastDistance = Single.MaxValue;
		}

        public override void FixedUpdate()
		{
			base.FixedUpdate();
			var dist = Single.PositiveInfinity;

			if( this.target != null && base.isAuthority )
			{
				var diff =  this.target.position - base.transform.position ;

				dist = diff.magnitude;
				var dir = diff.normalized;

				if( dist > 5f )
				{
					this.lastDirection = dir;
				}

				if( this.charMotor != null )
				{
					this.charMotor.rootMotion += dir * ( base.moveSpeedStat * baseMovespeedMult * Time.fixedDeltaTime );
				}
			}

			if( base.isAuthority )
			{
                if( dist <= cancelDistance )
                {
                    _ = base.skillData.targetStateMachine.SetInterruptState( base.skillData.InstantiateNextState( base.skillData.knifeInstance ), InterruptPriority.Death );
                    base.outer.SetNextStateToMain();
                } else if( base.fixedAge >= this.maxDuration || this.target == null || dist > distanceJumpCancelThreshold * this.lastDistance )
                {
                    base.outer.SetNextStateToMain();
                }
			}
            this.lastDistance = dist;
		}

		public override void OnExit()
		{

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
			if( this.charMotor != null )
			{
				this.charMotor.velocity = this.lastDirection * endSpeedCarryover;
			}
			base.OnExit();
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

        private void DestroyKnife()
        {
            if( this.target != null )
            {
                //Destroy( this.target.gameObject );
                
            }
        }


		public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Death;
	}
}
