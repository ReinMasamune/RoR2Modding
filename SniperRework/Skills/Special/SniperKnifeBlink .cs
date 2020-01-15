using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using System;
using UnityEngine.Networking;
using System.Collections.Generic;

namespace ReinSniperRework
{
    internal partial class Main
    {
        internal class SniperKnifeBlink : BaseState
        {
            const Single baseDuration = 0.2f;


            private Boolean shouldBlink = false;
            private Single duration;
            private Vector3 destVec;
            private Vector3 startVec;

            private Transform targetTransform;
            private Transform modelTransform;
            private CharacterModel charModel;
            private HurtBoxGroup boxGroup;


            public override void OnEnter()
            {
                base.OnEnter();
                this.duration = baseDuration / (this.moveSpeedStat / base.characterBody.baseMoveSpeed);
                var deployList = this.characterBody.master.GetFieldValue<List<DeployableInfo>>( "deployablesList" );

                foreach( DeployableInfo info in deployList )
                {
                    if( info.slot == (DeployableSlot)7)
                    {
                        this.targetTransform = info.deployable.transform;
                        this.destVec = this.targetTransform.position;
                        this.startVec = base.transform.position;
                        this.shouldBlink = true;
                    }
                }

                //Util.PlaySound( "")
                this.modelTransform = base.GetModelTransform();
                if( this.modelTransform )
                {
                    this.charModel = this.modelTransform.GetComponent<CharacterModel>();
                    this.boxGroup = this.modelTransform.GetComponent<HurtBoxGroup>();
                }
                if( this.charModel )
                {
                    this.charModel.invisibilityCount++;
                }
                if( this.boxGroup )
                {
                    var boxTemp = this.boxGroup;
                    var deactivCounter = boxTemp.hurtBoxesDeactivatorCounter + 1;
                    boxTemp.hurtBoxesDeactivatorCounter = deactivCounter;
                }

                var data = new EffectData();
                data.rotation = Util.QuaternionSafeLookRotation( (this.destVec - this.startVec).normalized );
                data.origin = Util.GetCorePosition( base.gameObject );
                EffectManager.SpawnEffect( Main.instance.knifeBlink, data, false );


                //this.characterMotor.Motor.

                Util.PlaySound( "Play_huntress_shift_start", base.gameObject );
            }

            public override void FixedUpdate()
            {
                base.FixedUpdate();
                if( this.shouldBlink )
                {
                    if( this.targetTransform != null )
                    {
                        this.destVec = this.targetTransform.position;
                    }

                    base.characterMotor.velocity = Vector3.zero;

                    base.characterMotor.rootMotion += (Vector3.Lerp( this.startVec, this.destVec, base.fixedAge / this.duration ) - base.transform.position);
                    //base.transform.position += (Vector3.Lerp( this.startVec, this.destVec, base.fixedAge / this.duration ) - base.transform.position);
                }
                if( (base.fixedAge >= this.duration || !this.shouldBlink) && base.isAuthority )
                {
                    base.outer.SetNextStateToMain();
                    return;
                }
            }

            public override void OnExit()
            {
                Util.PlaySound( "Play_huntress_shift_end", base.gameObject );
                if( !this.outer.destroying )
                {
                    var data = new EffectData();
                    data.rotation = Util.QuaternionSafeLookRotation( (this.destVec - this.startVec).normalized );
                    data.origin = Util.GetCorePosition( base.gameObject );
                    EffectManager.SpawnEffect( Main.instance.knifeBlink, data, false );

                    if( this.modelTransform )
                    {
                        var overlay = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                        overlay.duration = 0.6f;
                        overlay.animateShaderAlpha = true;
                        overlay.alphaCurve = AnimationCurve.EaseInOut( 0f, 1f, 1f, 0f );
                        overlay.destroyComponentOnEnd = true;
                        overlay.originalMaterial = Main.instance.blinkMaterial1;
                        overlay.AddToCharacerModel( this.charModel );
                        var overlay2 = this.modelTransform.gameObject.AddComponent<TemporaryOverlay>();
                        overlay2.duration = 0.7f;
                        overlay2.animateShaderAlpha = true;
                        overlay2.alphaCurve = AnimationCurve.EaseInOut( 0f, 1f, 1f, 0f );
                        overlay2.destroyComponentOnEnd = true;
                        overlay2.originalMaterial = Main.instance.blinkMaterial2;
                        overlay2.AddToCharacerModel( this.charModel );
                    }
                }
                if( this.charModel )
                {
                    this.charModel.invisibilityCount--;
                }
                if( this.boxGroup )
                {
                    var boxTemp = this.boxGroup;
                    var deactivCounter = boxTemp.hurtBoxesDeactivatorCounter - 1;
                    boxTemp.hurtBoxesDeactivatorCounter = deactivCounter;
                }

                base.OnExit();
            }
        }
    }
}


