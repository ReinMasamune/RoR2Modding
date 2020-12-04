namespace Rein.Sniper.States.Special
{
	using System;
	using System.Collections.Generic;
	using EntityStates;
	using KinematicCharacterController;
	using RoR2;
	using Rein.Sniper.Modules;
	using Rein.Sniper.SkillDefs;
	using Rein.Sniper.States.Bases;
	using UnityEngine;
    using UnityEngine.Networking;

    internal class KnifeReactivation : ReactivationBaseState<KnifeSkillData>
	{
        private const Single baseDuration = 0.275f * 7f;
		private const Single baseMovespeedMult = 2f;
		private const Single maxDurationMult = 5f;
		private const Single slashMaxDistance = 10f;
		private const Single endSpeedCarryover = 10f;
        private const Single distanceJumpCancelThreshold = 2f;
        private static GameObject blinkStartEffect = VFXModule.GetKnifeBlinkPrefab();
		internal static GameObject blinkEndEffect;


        private Single duration;

        private Vector3 startPos;
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
            this.startPos = base.transform.position;
            if(base.isAuthority)
            {
                if(this.target != null)
                {
                    var baseMovespeed = Math.Max(base.moveSpeedStat / (base.characterBody.isSprinting ? base.characterBody.sprintingSpeedMultiplier : 1.0f), 3f);
                    var effectiveMovespeed = baseMovespeed * baseMovespeedMult;
                    var dir = this.target.position - base.transform.position;
                    var dist = dir.magnitude;
                    dir = dir.normalized;

                    this.duration = Math.Min(baseDuration, dist / baseMovespeedMult) / baseMovespeed;

                    if(base.isAuthority)
                    {
                        this.PlayStartEffects(new Ray(base.transform.position, dir));
                    }
                } else
                {
                    this.duration = 0f;
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
            Util.PlaySound("Play_huntress_shift_start", base.gameObject);
        }

        public override void FixedUpdate()
		{
			base.FixedUpdate();
			var dist = Single.MaxValue;
            
			if( this.target != null && base.isAuthority )
			{
                var cDest = this.duration == 0f ? this.target.position : Vector3.Lerp(this.startPos, this.target.position, base.fixedAge / this.duration);
                var cDir = cDest - base.transform.position;
                dist = Vector3.Distance(base.transform.position, this.target.position);




				if( dist > 5f )
				{
					this.lastDirection = cDir.normalized;
				}

				if( this.charMotor != null )
				{
					this.charMotor.rootMotion += cDir;
				}
			}

			if( base.isAuthority )
			{
                if(base.fixedAge >= this.duration || this.target == null || dist > distanceJumpCancelThreshold * this.lastDistance)
                {
                    base.outer.SetNextStateToMain();
                    if(dist <= slashMaxDistance)
                    {
                        //Log.MessageT("Slash");
                        _ = base.skillData.targetStateMachine.SetInterruptState(base.skillData.InstantiateNextState(base.skillData.knifeInstance), InterruptPriority.Death);
                    } else
                    {
                        Log.MessageT($"NoSlash, dist: {dist}");
                    }
                }
			}
            this.lastDistance = dist;
		}

		public override void OnExit()
		{
            Util.PlaySound("Play_huntress_shift_end", base.gameObject);

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
