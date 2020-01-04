using RoR2;
using UnityEngine;
using ReinSniperRework;
using RoR2.Orbs;
using System.Collections.Generic;
using System.Linq;

namespace EntityStates.ReinSniperRework.SniperWeapon
{
    /*
    public class SniperUtility : BaseState
    {
        ReinDataLibrary data;

        private ChildLocator childLocator;
        private float timer;
        private float timer2;
        private Vector3 forward;
        private Animator animations;
        private int counter;

        public override void OnEnter()
        {
            base.OnEnter();
            data = base.GetComponent<ReinDataLibrary>();

            Transform modelTransform = base.GetModelTransform();
            this.childLocator = modelTransform.GetComponent<ChildLocator>();

            base.characterMotor.velocity.y = Mathf.Max(base.characterMotor.velocity.y, 0f);
            base.characterMotor.velocity.y += data.u_smallHopStrength;

            animations = base.GetModelAnimator();

            Util.PlaySound(data.u_dodgeSound, base.gameObject);

            this.timer2 = -data.u_orbPrefireDuration;

            if( base.isAuthority && base.inputBank )
            {
                forward = -Vector3.ProjectOnPlane(base.inputBank.aimDirection, Vector3.up);
            }

            base.characterDirection.moveVector = -forward;

            base.PlayAnimation("Body", "DodgeBackward", "Dodge.playbackRate", data.u_duration / 2f);
            //Animation: "FullBody, Override" "Backflip" "Backflip.playbackRate"
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            timer += Time.fixedDeltaTime;
            timer2 += Time.fixedDeltaTime;

            if( base.cameraTargetParams )
            {
                //FOV change here
            }

            if( base.characterMotor && base.characterDirection )
            {
                Vector3 vel1 = base.characterMotor.velocity;
                Vector3 vel2 = forward * (this.moveSpeedStat * Mathf.Lerp(data.u_initSpeedCoef, data.u_endSpeedCoef, timer / data.u_duration));
                base.characterMotor.velocity = vel2;
                base.characterMotor.velocity.y = vel1.y;
                base.characterMotor.moveDirection = forward;
            }

            if( timer2 >= 1f / data.u_orbFreq / attackSpeedStat && counter < data.u_orbMax )
            {
                timer2 -= 1f / data.u_orbFreq / attackSpeedStat;
                //fireOrb();
            }

            if( timer >= data.u_duration && base.isAuthority )
            {
                outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            if( base.cameraTargetParams )
            {
                //Fov stuff
            }
            int layerIndex = animations.GetLayerIndex("Impact");
            if( layerIndex >= 0 )
            {
                this.animations.SetLayerWeight(layerIndex, 1.5f);
                this.animations.PlayInFixedTime("LightImpact", layerIndex, 0f);
            }
        }
    }
    */

    
    public class SniperUtility : BaseState
    {
        ReinDataLibrary data;
        public override void OnEnter()
        {
            base.OnEnter();
            data = base.GetComponent<ReinDataLibrary>();
            base.characterBody.AddTimedBuff(BuffIndex.Cloak, data.u_duration * 2f);
            Util.PlaySound(data.u_beginSoundString, base.gameObject);
            this.modelTransform = base.GetModelTransform();
            if (this.modelTransform)
            {
                this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
                this.hurtboxGroup = this.modelTransform.GetComponent<HurtBoxGroup>();
            }
            if (this.characterModel)
            {
                this.characterModel.invisibilityCount++;
            }
            if (this.hurtboxGroup)
            {
                HurtBoxGroup hurtBoxGroup = this.hurtboxGroup;
                int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter + 1;
                hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
            }
            Vector3 tempVec = base.characterMotor.velocity;
            this.blinkVector = tempVec.normalized * 1f;
            this.CreateBlinkEffect(Util.GetCorePosition(base.gameObject));
            base.characterMotor.Motor.SafeMovement = true;
        }

        private void CreateBlinkEffect(Vector3 origin)
        {
            EffectData effectData = new EffectData();
            effectData.rotation = Util.QuaternionSafeLookRotation(this.blinkVector);
            effectData.origin = origin;
            EffectManager.SpawnEffect(data.u_blinkPrefab, effectData, false);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.stopwatch += Time.fixedDeltaTime;
            if (base.characterMotor && base.characterDirection)
            {
                float mod = 1f;
                if (base.characterBody.isSprinting)
                {
                    mod = 1.45f;
                }
                base.characterMotor.rootMotion += this.blinkVector * (this.moveSpeedStat * data.u_speedCoef * Time.fixedDeltaTime / mod);
            }
            if (this.stopwatch >= data.u_duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
            }
        }

        public override void OnExit()
        {
            if (!this.outer.destroying)
            {
                Util.PlaySound(data.u_endSoundString, base.gameObject);
                this.CreateBlinkEffect(Util.GetCorePosition(base.gameObject));
                this.modelTransform = base.GetModelTransform();
                if (this.modelTransform)
                {
                    TemporaryOverlay temporaryOverlay = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                    temporaryOverlay.duration = 0.6f;
                    temporaryOverlay.animateShaderAlpha = true;
                    temporaryOverlay.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    temporaryOverlay.destroyComponentOnEnd = true;
                    temporaryOverlay.originalMaterial = Resources.Load<Material>("Materials/mattpinout");
                    temporaryOverlay.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
                    TemporaryOverlay temporaryOverlay2 = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                    temporaryOverlay2.duration = 0.7f;
                    temporaryOverlay2.animateShaderAlpha = true;
                    temporaryOverlay2.alphaCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
                    temporaryOverlay2.destroyComponentOnEnd = true;
                    temporaryOverlay2.originalMaterial = Resources.Load<Material>("Materials/matHuntressFlashExpanded");
                    temporaryOverlay2.AddToCharacerModel(this.modelTransform.GetComponent<CharacterModel>());
                }
            }
            if (this.characterModel)
            {
                this.characterModel.invisibilityCount--;
            }
            if (this.hurtboxGroup)
            {
                HurtBoxGroup hurtBoxGroup = this.hurtboxGroup;
                int hurtBoxesDeactivatorCounter = hurtBoxGroup.hurtBoxesDeactivatorCounter - 1;
                hurtBoxGroup.hurtBoxesDeactivatorCounter = hurtBoxesDeactivatorCounter;
            }
            base.characterMotor.Motor.SafeMovement = false;
            base.characterBody.AddTimedBuff(BuffIndex.Cloak,data.u_duration * 4f);
            base.OnExit();
        }

        private Transform modelTransform;
        private float stopwatch;
        private Vector3 blinkVector = Vector3.zero;
        private CharacterModel characterModel;
        private HurtBoxGroup hurtboxGroup;
    }
    
}