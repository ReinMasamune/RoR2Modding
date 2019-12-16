namespace AlternateArtificer.States.Special
{
    using AlternateArtificer.States.Main;
    using EntityStates.Mage;
    using RoR2;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using UnityEngine;

    public class IonSurge : AltArtiMain
    {
        const Int32 steps = 3;

        const Single stepHalt = 0.75f;
        const Single dashTime = 0.3f;
        const Single speedConst = 2.5f;



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
            // Blast attack was here, need to do an overlap instead
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if( base.fixedAge >= duration && base.isAuthority ) base.outer.SetNextStateToMain();
        }

        public override void OnExit()
        {
            //if( !base.outer.destroying ) Util.PlaySound( FlyUpState.endSoundString, base.gameObject );
            //modelTransform.localRotation = baseRotation;
            rotator.ResetRotation( 0.5f );
            base.characterMotor.useGravity = true;
            base.cameraTargetParams.aimMode = CameraTargetParams.AimType.Standard;

            if( inputSpace ) UnityEngine.Object.Destroy( inputSpace.gameObject );

            base.OnExit();
        }

        public override void HandleMovements()
        {
            base.HandleMovements();

            if( halting )
            {
                if( this.haltingFirst && base.fixedAge >= stepTimes[stepCounter] + stepHalt )
                {
                    base.passive.SkillCast( skillLocator.special );
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
                    EffectManager.instance.SimpleMuzzleFlash( FlyUpState.muzzleflashEffect, base.gameObject, "MuzzleLeft", false );
                    EffectManager.instance.SimpleMuzzleFlash( FlyUpState.muzzleflashEffect, base.gameObject, "MuzzleRight", false );

                    rotator.SetRotation( Quaternion.LookRotation( this.flyVector, Vector3.up ), dashTime );


                    halting = false;
                    stepCounter++;
                }
            } else
            {
                if( base.fixedAge >= stepTimes[stepCounter] )
                {
                    if( stepCounter < steps )
                    {
                        this.halting = true;
                        Util.PlaySound( FlyUpState.endSoundString, base.gameObject );
                        this.rotator.ResetRotation( stepHalt );
                        //base.PlayCrossfade( "Body", "FlyUp", "FlyUp.playbackRate", this.stepTime + stepHalt, 0.1f );
                        this.haltingFirst = true;
                    }
                }
                Single speedCoef = speedConst * FlyUpState.speedCoefficientCurve.Evaluate( (base.fixedAge - stepTimes[stepCounter]) / dashTime ) / dashTime;
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
            EffectManager.instance.SpawnEffect( FlyUpState.blinkPrefab, data, false );
        }
    }
}
