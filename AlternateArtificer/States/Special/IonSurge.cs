namespace AlternativeArtificer.States.Special
{
    using AlternativeArtificer.States.Main;
    using EntityStates;
    using EntityStates.Mage;
    using RoR2;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;
    using UnityEngine.Networking;

    public class IonSurge : GenericCharacterMain
    {
        // TODO: Disable jumping during surge
        const Int32 steps = 3;

        const Single stepHalt = 0.75f;
        const Single dashTime = 0.3f;
        const Single speedConst = 0.4f;

        private Int32 stepCounter = 0;

        private Single duration;
        private Single stepTime;
        private Single[] stepTimes = new Single[steps+1];

        private Boolean halting = true;
        private Boolean haltingFirst = false;

        private Vector3 flyVector;

        private Transform inputSpace;

        private Components.Rotator rotator;
        private Transform modelTransform;
        private AltArtiPassive passive;
        private AltArtiPassive.BatchHandle[] handles = new AltArtiPassive.BatchHandle[steps];

        public override void OnEnter()
        {
            base.OnEnter();

            inputSpace = new GameObject( "inputSpace" ).transform;
            inputSpace.position = Vector3.zero;
            inputSpace.rotation = Quaternion.identity;
            inputSpace.localScale = Vector3.one;

            stepTime = stepHalt + dashTime;
            duration = stepTime * steps - stepHalt;
            for( Int32 i = 0; i < steps; i++ )
            {
                stepTimes[i] = stepTime * i - stepHalt;
            }
            stepTimes[steps] = duration;
            stepTime = (duration / steps) - stepHalt;

            modelTransform = base.GetModelTransform();
            this.rotator = modelTransform.Find( "MageArmature" ).GetComponent<Components.Rotator>();

            base.characterMotor.useGravity = false;
            base.cameraTargetParams.aimMode = CameraTargetParams.AimType.Aura;

            if( AltArtiPassive.instanceLookup.ContainsKey( base.gameObject ) )
            {
                passive = AltArtiPassive.instanceLookup[base.gameObject];
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if( base.fixedAge >= this.duration && base.isAuthority ) base.outer.SetNextStateToMain();
        }

        public override void OnExit()
        {
            rotator.ResetRotation( 0.5f );
            base.characterMotor.useGravity = true;
            base.cameraTargetParams.aimMode = CameraTargetParams.AimType.Standard;

            if( inputSpace ) UnityEngine.Object.Destroy( inputSpace.gameObject );

            if( NetworkServer.active && !base.characterBody.bodyFlags.HasFlag( CharacterBody.BodyFlags.IgnoreFallDamage ) )
            {
                base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
                base.characterMotor.onHitGround += this.CharacterMotor_onHitGround;
            }

            base.OnExit();
        }

        private void CharacterMotor_onHitGround( ref CharacterMotor.HitGroundInfo hitGroundInfo )
        {
            if( base.characterBody.bodyFlags.HasFlag( CharacterBody.BodyFlags.IgnoreFallDamage ) )
            {
                base.characterBody.bodyFlags &= ~CharacterBody.BodyFlags.IgnoreFallDamage;
            }

            // TODO: May need to redo the flag assignment?

            base.characterMotor.onHitGround -= this.CharacterMotor_onHitGround;
        }

        public override void HandleMovements()
        {
            base.HandleMovements();

            if( halting )
            {
                if( this.haltingFirst && base.fixedAge >= stepTimes[stepCounter] + stepHalt )
                {
                    //base.passive.SkillCast( skillLocator.special );
                    this.haltingFirst = false;
                }
                if( base.fixedAge >= stepTimes[stepCounter] + stepHalt )
                {
                    Vector3 aimDir = base.GetAimRay().direction;
                    Vector3 moveDir = inputBank.moveVector;
                    Vector3 aimOrientation = new Vector3( aimDir.x , 0f, aimDir.z );
                    aimOrientation = Vector3.Normalize( aimOrientation );
                    inputSpace.rotation = Quaternion.LookRotation( aimOrientation, Vector3.up );

                    aimDir = inputSpace.InverseTransformDirection( aimDir );
                    moveDir = inputSpace.InverseTransformDirection( moveDir );

                    Vector3 forward;
                    if( moveDir.sqrMagnitude != 0 )
                    {
                        forward = aimDir * moveDir.z;
                        forward.x = moveDir.x;

                    } else forward = aimDir;

                    forward.y += inputBank.jump.down ? 1f : 0f;
                    forward = Vector3.Normalize( forward );
                    this.flyVector = inputSpace.TransformDirection( forward );

                    Util.PlaySound( FlyUpState.beginSoundString, base.gameObject );
                    this.CreateBlinkEffect( Util.GetCorePosition( base.gameObject ) );
                    base.PlayCrossfade( "Body", "FlyUp", "FlyUp.playbackRate", dashTime, 0.1f );
                    base.characterMotor.Motor.ForceUnground();
                    base.characterMotor.velocity = Vector3.zero;
                    EffectManager.SimpleMuzzleFlash( FlyUpState.muzzleflashEffect, base.gameObject, "MuzzleLeft", false );
                    EffectManager.SimpleMuzzleFlash( FlyUpState.muzzleflashEffect, base.gameObject, "MuzzleRight", false );

                    rotator.SetRotation( Quaternion.LookRotation( this.flyVector, Vector3.up ), dashTime );

                    base.characterBody.isSprinting = true;
                    halting = false;
                    stepCounter++;

                    handles[stepCounter-1] = new AltArtiPassive.BatchHandle();
                    if( passive != null )
                    {
                        passive.SkillCast( handles[stepCounter-1] );
                    }
                }
            } else
            {
                if( base.fixedAge >= stepTimes[stepCounter] )
                {
                    if( passive != null )
                    {
                        handles[stepCounter - 1].Fire( stepHalt / 4f, stepHalt / 2f );
                    }
                    if( stepCounter < steps )
                    {
                        this.halting = true;
                        Util.PlaySound( FlyUpState.endSoundString, base.gameObject );
                        this.rotator.ResetRotation( stepHalt );
                        this.haltingFirst = true;
                        base.characterBody.isSprinting = false;
                    }
                }
                Single speedCoef = base.moveSpeedStat * speedConst * FlyUpState.speedCoefficientCurve.Evaluate( (base.fixedAge - stepTimes[stepCounter]) / dashTime ) / dashTime;
                base.characterMotor.rootMotion += this.flyVector * speedCoef * Time.fixedDeltaTime;

            }
            base.characterMotor.velocity.y = 0f;
        }

        protected override void UpdateAnimationParameters() => base.UpdateAnimationParameters();

        private void CreateBlinkEffect( Vector3 origin )
        {
            EffectData data = new EffectData();
            data.rotation = Util.QuaternionSafeLookRotation( this.flyVector );
            data.origin = origin;
            EffectManager.SpawnEffect( FlyUpState.blinkPrefab, data, false );
        }
    }
}
