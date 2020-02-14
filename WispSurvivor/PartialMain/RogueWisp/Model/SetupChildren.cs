#if ROGUEWISP
using RogueWispPlugin.Helpers;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        private CharacterModel RW_charModel;
        private WispModelBitSkinController RW_skinController;
        private ChildLocator RW_childLoc;
        private RagdollController RW_ragdollController;
        private HurtBoxGroup RW_boxGroup;
        private HealthComponent RW_healthComponent;
        private WispFlamesController RW_flameController;
        private WispPassiveController RW_passiveController;

        private List<(Transform transform,String name)> pairsList = new List<(Transform transform,String name)>();

        partial void RW_SetupChildren()
        {
            this.Load += this.RW_ChildSetup;
        }

        private void RW_ChildSetup()
        {
            Transform modelTransform = this.RW_body.GetComponent<ModelLocator>().modelTransform;
            this.RW_charModel = modelTransform.GetComponent<CharacterModel>();
            this.RW_skinController = this.RW_charModel.AddComponent<WispModelBitSkinController>();
            this.RW_childLoc = modelTransform.GetComponent<ChildLocator>();
            this.RW_ragdollController = modelTransform.AddComponent<RagdollController>();
            this.RW_boxGroup = modelTransform.GetComponent<HurtBoxGroup>();
            this.RW_healthComponent = this.RW_body.GetComponent<HealthComponent>();
            this.RW_flameController = this.RW_body.GetComponent<WispFlamesController>();
            this.RW_passiveController = this.RW_body.GetComponent<WispPassiveController>();

            foreach( var val in this.RW_charModel.baseLightInfos ) UnityEngine.Object.DestroyImmediate( val.light.gameObject );
            this.RW_charModel.baseLightInfos = Array.Empty<CharacterModel.LightInfo>();
            foreach( var val in this.RW_charModel.baseParticleSystemInfos ) UnityEngine.Object.DestroyImmediate( val.particleSystem.gameObject );
            this.RW_charModel.baseParticleSystemInfos = Array.Empty<CharacterModel.ParticleSystemInfo>();
            UnityEngine.Object.DestroyImmediate( modelTransform.GetComponent<AncientWispFireController>() );
            foreach( var val in this.RW_boxGroup.hurtBoxes )
            {
                UnityEngine.Object.DestroyImmediate( val.gameObject );
            }
            this.RW_boxGroup.hurtBoxes = Array.Empty<HurtBox>();
            this.RW_boxGroup.mainHurtBox = null;
            this.RW_boxGroup.bullseyeCount = 0;
            this.RW_ragdollController.bones = Array.Empty<Transform>();
            this.RW_flameController.passive = this.RW_passiveController;
            //Path stuff here
            #region Transform paths
            var meshTransform = modelTransform.Find( "CannonPivot/AncientWispMesh" );
            var armaTransform = modelTransform.Find( "CannonPivot/AncientWispArmature" );

            #region chest
            {
                var chestTransform = armaTransform.Find("chest");

                #region ChestCannon1
                {
                    var chestCannon1Transform = chestTransform.Find("ChestCannon1");

                    this.AddFireParticles( 
                        parent:     chestCannon1Transform, 
                        position:   new Vector3( 0f, 1f, 0f ), 
                        rotation:   new Vector3( 90f, 0f, 0f ), 
                        scale:      new Vector3( 0.25f, 0.2f, 0.6f ) 
                        );

                    var colliderInfo01 = new BoxColliderInfo(
                        center:     new Vector3( 0f, 0.4f, 0f ),
                        size:       new Vector3( 0.5f, 0.9f, 0.1f ) 
                        );

                    this.AddRagdollCollider(
                        parent:     chestCannon1Transform,
                        info:       colliderInfo01 
                        );

                    this.AddHurtBox(
                        parent:     chestCannon1Transform,
                        info:       colliderInfo01,
                        isMain:     true,
                        isBullseye: true,
                        damageMod:  HurtBox.DamageModifier.Normal 
                        );

                    #region ChestCannonGuard1
                    {
                        var chestCannonGuard1Transform = chestCannon1Transform.Find("ChestCannonGuard1");

                        var colliderInfo02 = new BoxColliderInfo(
                            center:     new Vector3( 0f, 0.25f, 0.05f ),
                            size:       new Vector3( 0.5f, 0.9f, 0.1f )
                            );

                        this.AddRagdollCollider(
                            parent:     chestCannonGuard1Transform,
                            info:       colliderInfo02
                            );

                        this.AddHurtBox(
                            parent:     chestCannonGuard1Transform,
                            info:       colliderInfo02
                            );

                        #region ChestCannonGuard1_end
                        {
                            var chestCannonGuard1EndTransform = chestCannonGuard1Transform.Find( "ChestCannonGuard1_end" );
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
                #region ChestCannon2
                {
                    var chestCannon2Transform = chestTransform.Find("ChestCannon2");

                    this.AddToChildLocator( chestTransform, "ChestCannon2" );

                    this.AddFireParticles( 
                        parent:     chestCannon2Transform,
                        position:   new Vector3( 0f, 1f, 0f ),
                        rotation:   new Vector3( 90f, 0f, 0f ),
                        scale:      new Vector3( 0.25f, 0.2f, 0.6f ) 
                        );

                    var colliderInfo03 = new BoxColliderInfo(
                        center:     new Vector3( 0f, 0.4f, 0f ),
                        size:       new Vector3( 0.5f, 0.9f, 0.1f )
                        );

                    this.AddRagdollCollider(
                        parent:     chestCannon2Transform,
                        info:       colliderInfo03
                        );

                    this.AddHurtBox(
                        parent:     chestCannon2Transform,
                        info:       colliderInfo03,
                        isMain:     false,
                        isBullseye: false,
                        damageMod:  HurtBox.DamageModifier.Normal
                        );

                    #region ChestCannonGuard2
                    {
                        var chestCannonGuard2Transform = chestCannon2Transform.Find("ChestCannonGuard2");

                        var colliderInfo04 = new BoxColliderInfo(
                            center:     new Vector3( 0f, 0.17f, -0.05f ),
                            size:       new Vector3( 0.7f, 0.5f, 0.2f )
                            );

                        this.AddRagdollCollider(
                            parent:     chestCannonGuard2Transform,
                            info:       colliderInfo04
                            );

                        this.AddHurtBox(
                            parent:     chestCannonGuard2Transform,
                            info:       colliderInfo04,
                            isMain:     false,
                            isBullseye: false,
                            damageMod:  HurtBox.DamageModifier.Normal
                            );

                        #region ChestCannonGuard2_end
                        {
                            var chestCannonGuard2EndTransform = chestCannonGuard2Transform.Find( "ChestCannonGuard2_end");
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
            #endregion
            #region Head
            {
                var headTransform = armaTransform.Find( "Head" );

                this.AddToChildLocator( headTransform, "Head" );

                this.AddFireParticles( 
                    parent:     headTransform, 
                    position:   new Vector3( 0f, 0.15f, 0f ), 
                    rotation:   new Vector3( 180f, 0f, 0f ), 
                    scale:      new Vector3( 0.1f, 0.1f, 0.1f ) 
                    );

                var colliderInfo05 = new CapsuleColliderInfo(
                    center:     new Vector3( 0f, 0.25f, 0.15f ),
                    direction:  CapsuleColliderInfo.Direction.X,
                    radius:     0.25f,
                    height:     1f
                    );

                this.AddRagdollCollider(
                    parent:     headTransform,
                    info:       colliderInfo05
                    );

                this.AddHurtBox(
                    parent:     headTransform,
                    info:       colliderInfo05,
                    isMain:     false,
                    isBullseye: false,
                    damageMod:  HurtBox.DamageModifier.Normal
                    );

                #region Head_end
                {
                    var headEndTransform = headTransform.Find( "Head_end" );
                }
                #endregion
            }
            #endregion
            #region shoulder.l
            {
                var shoulderL = armaTransform.Find( "shoulder.l");

                this.AddToChildLocator( shoulderL, "shoulder.l" );

                var colliderInfo06 = new BoxColliderInfo(
                    center:     new Vector3( -0.075f, 0.3f, 0f ),
                    size:       new Vector3( 0.3f, 0.5f, 0.5f )
                    );

                this.AddRagdollCollider(
                    parent:     shoulderL,
                    info:       colliderInfo06
                    );

                this.AddHurtBox(
                    parent:     shoulderL,
                    info:       colliderInfo06,
                    isMain:     false,
                    isBullseye: false,
                    damageMod:  HurtBox.DamageModifier.Normal
                    );

                #region upperArm1.l
                {
                    var upperArm1L = shoulderL.Find("upperArm1.l");

                    this.AddFireParticles( upperArm1L,
                        position:   new Vector3( 0f, 0.4f, 0f ),
                        rotation:   new Vector3( 90f, 0f, 0f ),
                        scale:      new Vector3( 0.15f, 0.15f, 0.5f ) 
                        );

                    var colliderInfo07 = new BoxColliderInfo(
                        center:     new Vector3( 0f, 0.15f, 0f ),
                        size:       new Vector3( 0.2f, 0.4f, 0.4f )
                        );

                    this.AddRagdollCollider(
                        parent:     upperArm1L,
                        info:       colliderInfo07
                        );

                    this.AddHurtBox(
                        parent:     upperArm1L,
                        info:       colliderInfo07,
                        isMain:     false,
                        isBullseye: false,
                        damageMod:  HurtBox.DamageModifier.Normal
                        );


                    #region upperArm2.l
                    {
                        var upperArm2L = upperArm1L.Find( "upperArm2.l" );

                        var colliderInfo08 = new BoxColliderInfo(
                            center:     new Vector3( 0.12f, 0.3f, 0.3f ),
                            size:       new Vector3( 0.01f, 0.17f, 0f )
                            );

                        this.AddRagdollCollider(
                            parent:     upperArm2L,
                            info:       colliderInfo08
                            );

                        this.AddHurtBox(
                            parent:     upperArm2L,
                            info:       colliderInfo08,
                            isMain:     false,
                            isBullseye: false,
                            damageMod:  HurtBox.DamageModifier.Normal
                            );

                        #region lowerArm.l
                        {
                            var lowerArmL = upperArm2L.Find( "lowerArm.l");

                            #region finger1.l
                            {
                                var finger1L = lowerArmL.Find( "finger1.l");

                                var colliderInfo09 = new CapsuleColliderInfo(
                                    center:     new Vector3(0f, 0.09f, 0f ),
                                    direction:  CapsuleColliderInfo.Direction.Y,
                                    radius:     0.035f,
                                    height:     0.3f
                                    );

                                this.AddRagdollCollider(
                                    parent:     finger1L,
                                    info:       colliderInfo09
                                    );

                                this.AddHurtBox(
                                    parent:     finger1L,
                                    info:       colliderInfo09,
                                    isMain:     false,
                                    isBullseye: false,
                                    damageMod:  HurtBox.DamageModifier.Normal
                                    );

                                #region finger1.l_end
                                {
                                    var finger1LEnd = finger1L.Find( "finger1.l_end" );
                                }
                                #endregion
                            }
                            #endregion
                            #region finger2.l
                            {
                                var finger2L = lowerArmL.Find( "finger2.l");

                                var colliderInfo10 = new CapsuleColliderInfo(
                                    center:     new Vector3( 0f, 0.075f, 0f ),
                                    direction:  CapsuleColliderInfo.Direction.Y,
                                    radius:     0.035f,
                                    height:     0.25f
                                    );

                                this.AddRagdollCollider(
                                    parent:     finger2L,
                                    info:       colliderInfo10
                                    );

                                this.AddHurtBox(
                                    parent:     finger2L,
                                    info:       colliderInfo10,
                                    isMain:     false,
                                    isBullseye: false,
                                    damageMod:  HurtBox.DamageModifier.Normal
                                    );

                                #region finger2.l_end
                                {
                                    var finger2LEnd = finger2L.Find( "finger2.l_end");
                                }
                                #endregion
                            }
                            #endregion
                            #region thumb.l
                            {
                                var thumbL = lowerArmL.Find( "thumb.l");

                                var colliderInfo11 = new CapsuleColliderInfo(
                                    center:     new Vector3( -0.005f, 0.085f, 0f ),
                                    direction:  CapsuleColliderInfo.Direction.Y,
                                    radius:     0.05f,
                                    height:     0.2f
                                    );

                                this.AddRagdollCollider(
                                    parent:     thumbL,
                                    info:       colliderInfo11
                                    );

                                this.AddHurtBox(
                                    parent:     thumbL,
                                    info:       colliderInfo11,
                                    isMain:     false,
                                    isBullseye: false,
                                    damageMod:  HurtBox.DamageModifier.Normal
                                    );

                                #region thumb.l_end
                                {
                                    var thumbLEnd = thumbL.Find( "thumb.l_end" );
                                }
                                #endregion
                            }
                            #endregion
                            #region MuzzleLeft
                            {
                                var muzzleLeft = lowerArmL.Find( "MuzzleLeft" );

                                this.AddFireParticles( muzzleLeft,
                                    position:   new Vector3( 0f, 0f, 0.1f ),
                                    rotation:   new Vector3( 180f, 0f, 0f ),
                                    scale:      new Vector3( 0.1f, 0.1f, 0.5f ) );

                                this.AddToChildLocator( muzzleLeft, "MuzzleLeft" );
                            }
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
            #endregion
            #region shoulder.r
            {
                var shoulderR = armaTransform.Find( "shoulder.r");

                this.AddToChildLocator( shoulderR, "shoulder.r" );

                var colliderInfo12 = new BoxColliderInfo(
                    center:     new Vector3( 0.075f, 0.3f, 0f ),
                    size:       new Vector3( 0.3f, 0.5f, 0.5f )
                    );

                this.AddRagdollCollider(
                    parent:     shoulderR,
                    info:       colliderInfo12
                    );

                this.AddHurtBox(
                    parent:     shoulderR,
                    info:       colliderInfo12,
                    isMain:     false,
                    isBullseye: false,
                    damageMod:  HurtBox.DamageModifier.Normal
                    );

                #region upperArm1.r
                {
                    var upperArm1R = shoulderR.Find( "upperArm1.r" );

                    this.AddFireParticles( upperArm1R,
                        position:   new Vector3( 0f, 0.4f, 0f ),
                        rotation:   new Vector3( 90f, 0f, 0f ),
                        scale:      new Vector3( 0.15f, 0.15f, 0.5f ) 
                        );

                    var colliderInfo13 = new BoxColliderInfo(
                        center:     new Vector3( 0f, 0.15f, 0f ),
                        size:       new Vector3( 0.2f, 0.4f, 0.4f )
                        );

                    this.AddRagdollCollider(
                        parent:     upperArm1R,
                        info:       colliderInfo13
                        );

                    this.AddHurtBox(
                        parent:     upperArm1R,
                        info:       colliderInfo13,
                        isMain:     false,
                        isBullseye: false,
                        damageMod:  HurtBox.DamageModifier.Normal
                        );


                    #region RightSwingCenter
                    {
                        var rightSwingCenter = upperArm1R.Find( "RightSwingCenter" );

                        this.AddToChildLocator( rightSwingCenter, "RightSwingCenter" );
                    }
                    #endregion
                    #region upperArm2.r
                    {
                        var upperArm2R = upperArm1R.Find( "upperArm2.r" );

                        var colliderInfo14 = new BoxColliderInfo(
                            center:     new Vector3( -0.01f, 0.17f, 0f ),
                            size:       new Vector3( 0.12f, 0.3f, 0.3f )
                            );

                        this.AddRagdollCollider(
                            parent:     upperArm2R,
                            info:       colliderInfo14
                            );

                        this.AddHurtBox(
                            parent:     upperArm2R,
                            info:       colliderInfo14,
                            isMain:     false,
                            isBullseye: false,
                            damageMod:  HurtBox.DamageModifier.Normal
                            );


                        #region lowerArm.r
                        {
                            var lowerArmR = upperArm2R.Find( "lowerArm.r" );

                            #region finger1.r
                            {
                                var finger1R = lowerArmR.Find( "finger1.r" );

                                var colliderInfo15 = new CapsuleColliderInfo(
                                    center:     new Vector3( 0f, 0.09f, 0f ),
                                    direction:  CapsuleColliderInfo.Direction.Y,
                                    radius:     0.035f,
                                    height:     0.3f
                                    );

                                this.AddRagdollCollider(
                                    parent:     finger1R,
                                    info:       colliderInfo15
                                    );

                                this.AddHurtBox(
                                    parent:     finger1R,
                                    info:       colliderInfo15,
                                    isMain:     false,
                                    isBullseye: false,
                                    damageMod:  HurtBox.DamageModifier.Normal
                                    );


                                #region finger1.r_end
                                {
                                    var finger1REnd = finger1R.Find( "finger1.r_end" );
                                }
                                #endregion
                            }
                            #endregion
                            #region finger2.r
                            {
                                var finger2R = lowerArmR.Find( "finger2.r" );

                                var colliderInfo16 = new CapsuleColliderInfo(
                                    center:     new Vector3( 0f, 0.075f, 0f ),
                                    direction:  CapsuleColliderInfo.Direction.Y,
                                    radius:     0.035f,
                                    height:     0.25f
                                    );

                                this.AddRagdollCollider(
                                    parent:     finger2R,
                                    info:       colliderInfo16
                                    );

                                this.AddHurtBox(
                                    parent:     finger2R,
                                    info:       colliderInfo16,
                                    isMain:     false,
                                    isBullseye: false,
                                    damageMod:  HurtBox.DamageModifier.Normal
                                    );

                                #region finger2.r_end
                                {
                                    var finger2REnd = finger2R.Find( "finger2.r_end" );
                                }
                                #endregion
                            }
                            #endregion
                            #region thumb.r
                            {
                                var thumbR = lowerArmR.Find( "thumb.r" );

                                var colliderInfo17 = new CapsuleColliderInfo(
                                    center:     new Vector3( -0.005f, 0.085f, 0f ),
                                    direction:  CapsuleColliderInfo.Direction.Y,
                                    radius:     0.05f,
                                    height:     0.2f
                                    );

                                this.AddRagdollCollider(
                                    parent:     thumbR,
                                    info:       colliderInfo17
                                    );

                                this.AddHurtBox(
                                    parent:     thumbR,
                                    info:       colliderInfo17,
                                    isMain:     false,
                                    isBullseye: false,
                                    damageMod:  HurtBox.DamageModifier.Normal
                                    );

                                #region thumb.r_end
                                {
                                    var thumbREnd = thumbR.Find( "thumb.r_end" );
                                }
                                #endregion
                            }
                            #endregion
                            #region MuzzleRight
                            {
                                var muzzleRight = lowerArmR.Find( "MuzzleRight" );

                                this.AddFireParticles( muzzleRight,
                                    position:   new Vector3( 0f, 0f, 0f ),
                                    rotation:   new Vector3( 180f, 0f, 0f ),
                                    scale:      new Vector3( 0.1f, 0.1f, 0.5f ) );

                                this.AddToChildLocator( muzzleRight, "MuzzleRight" );

                            }
                            #endregion
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
            #endregion
            #region thigh.l
            {
                var thighL = armaTransform.Find( "thigh.l" );

                var colliderInfo18 = new CapsuleColliderInfo(
                    center:     new Vector3( 0f, 0.2f, 0f ),
                    direction:  CapsuleColliderInfo.Direction.Y,
                    radius:     0.15f,
                    height:     0.5f
                    );

                this.AddRagdollCollider(
                    parent:     thighL,
                    info:       colliderInfo18
                    );

                this.AddHurtBox(
                    parent:     thighL,
                    info:       colliderInfo18,
                    isMain:     false,
                    isBullseye: false,
                    damageMod:  HurtBox.DamageModifier.Normal
                    );


                #region calf.l
                {
                    var calfL = thighL.Find( "calf.l" );

                    this.AddFireParticles( 
                        parent:     calfL,
                        position:   new Vector3( 0f, 0.6f, 0f ),
                        rotation:   new Vector3( 90f, 0f, 0f ),
                        scale:      new Vector3( 0.1f, 0.1f, 0.5f ) 
                        );

                    var colliderInfo19 = new CapsuleColliderInfo(
                        center:     new Vector3( 0f, 0.3f, 0f ),
                        direction:  CapsuleColliderInfo.Direction.Y,
                        radius:     0.125f,
                        height:     0.7f
                        );

                    this.AddRagdollCollider(
                        parent:     calfL,
                        info:       colliderInfo19
                        );

                    this.AddHurtBox(
                        parent:     calfL,
                        info:       colliderInfo19,
                        isMain:     false,
                        isBullseye: false,
                        damageMod:  HurtBox.DamageModifier.Normal
                        );


                    #region toe1.l
                    {
                        var toe1L = calfL.Find( "toe1.l" );

                        var colliderInfo20 = new BoxColliderInfo(
                            center:     new Vector3( 0f, 0.08f, 0.01f ),
                            size:       new Vector3( 0.12f, 0.3f, 0.08f )
                            );

                        this.AddRagdollCollider(
                            parent:     toe1L,
                            info:       colliderInfo20
                            );

                        this.AddHurtBox(
                            parent:     toe1L,
                            info:       colliderInfo20,
                            isMain:     false,
                            isBullseye: false,
                            damageMod:  HurtBox.DamageModifier.Normal
                            );

                        #region toe1.l_end
                        {
                            var toe1LEnd = toe1L.Find( "toe1.l_end" );
                        }
                        #endregion
                    }
                    #endregion
                    #region toe2.l
                    {
                        var toe2L = calfL.Find( "toe2.l" );

                        var colliderInfo21 = new BoxColliderInfo(
                            center:     new Vector3( 0f, 0.06f, 0f ),
                            size:       new Vector3( 0.08f, 0.2f, 0.06f )
                            );

                        this.AddRagdollCollider(
                            parent:     toe2L,
                            info:       colliderInfo21
                            );

                        this.AddHurtBox(
                            parent:     toe2L,
                            info:       colliderInfo21,
                            isMain:     false,
                            isBullseye: false,
                            damageMod:  HurtBox.DamageModifier.Normal
                            );

                        #region toe2.l_end
                        {
                            var toe2LEnd = toe2L.Find( "toe2.l_end" );
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
            #endregion
            #region thigh.r
            {
                var thighR = armaTransform.Find( "thigh.r" );

                var colliderInfo22 = new CapsuleColliderInfo(
                    center:     new Vector3( 0f, 0.2f, 0f ),
                    direction:  CapsuleColliderInfo.Direction.Y,
                    radius:     0.15f,
                    height:     0.5f
                    );

                this.AddRagdollCollider(
                    parent:     thighR,
                    info:       colliderInfo22
                    );

                this.AddHurtBox(
                    parent:     thighR,
                    info:       colliderInfo22,
                    isMain:     false,
                    isBullseye: false,
                    damageMod:  HurtBox.DamageModifier.Normal
                    );

                #region calf.r
                {
                    var calfR = thighR.Find( "calf.r" );

                    this.AddFireParticles(
                        parent: calfR,
                        position: new Vector3( 0f, 0.6f, 0f ),
                        rotation: new Vector3( 90f, 0f, 0f ),
                        scale: new Vector3( 0.1f, 0.1f, 0.5f )
                    );

                    var colliderInfo23 = new CapsuleColliderInfo(
                        center:     new Vector3( 0f, 0.3f, 0f ),
                        direction:  CapsuleColliderInfo.Direction.Y,
                        radius:     0.125f,
                        height:     0.7f
                        );

                    this.AddRagdollCollider(
                        parent:     calfR,
                        info:       colliderInfo23
                        );

                    this.AddHurtBox
                    (
                        parent:     calfR,
                        info:       colliderInfo23,
                        isMain:     false,
                        isBullseye: false,
                        damageMod:  HurtBox.DamageModifier.Normal
                    );

                    #region toe1.r
                    {
                        var toe1R = calfR.Find( "toe1.r" );

                        var colliderInfo24 = new BoxColliderInfo(
                            center:     new Vector3( 0f, 0.08f, 0.01f ),
                            size:       new Vector3( 0.12f, 0.3f, 0.08f )
                            );

                        this.AddRagdollCollider(
                            parent:     toe1R,
                            info:       colliderInfo24
                            );

                        this.AddHurtBox(
                            parent:     toe1R,
                            info:       colliderInfo24,
                            isMain:     false,
                            isBullseye: false,
                            damageMod:  HurtBox.DamageModifier.Normal
                            );

                        #region toe1.r_end
                        {
                            var toe1REnd = toe1R.Find( "toe1.r_end" );
                        }
                        #endregion
                    }
                    #endregion
                    #region toe2.r
                    {
                        var toe2R = calfR.Find( "toe2.r" );

                        var colliderInfo25 = new BoxColliderInfo(
                            center:     new Vector3( 0f, 0.06f, 0f ),
                            size:       new Vector3( 0.08f, 0.2f, 0.06f )
                            );

                        this.AddRagdollCollider(
                            parent:     toe2R,
                            info:       colliderInfo25
                            );

                        this.AddHurtBox(
                            parent:     toe2R,
                            info:       colliderInfo25,
                            isMain:     false,
                            isBullseye: false,
                            damageMod:  HurtBox.DamageModifier.Normal
                            );

                        #region toe2.r_end
                        {
                            var toe2REnd = toe2R.Find( "toe2.r_end" );
                        }
                        #endregion
                    }
                    #endregion
                }
                #endregion
            }
            #endregion

            #endregion
            var allFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static;
            var field = typeof(ChildLocator).GetField( "transformPairs", allFlags );
            var arrayType = field.FieldType;
            var pairType = arrayType.GetElementType();
            var funcType = typeof(Func<,,>).MakeGenericType( typeof(String), typeof(Transform), pairType );
            var nameField = pairType.GetField( "name", allFlags ) as MemberInfo;
            var transformField = pairType.GetField( "transform", allFlags ) as MemberInfo;
            var nameParam = Expression.Parameter( typeof(String), "name" );
            var transformParam = Expression.Parameter( typeof(Transform), "transform" );
            var newPair = Expression.New( pairType );
            var assignName = Expression.Bind( nameField, nameParam );
            var assignTransform = Expression.Bind( transformField, transformParam );
            var init = Expression.MemberInit( newPair, assignName, assignTransform );
            var initFunc = Expression.Lambda( funcType, init, nameParam, transformParam );
            Expression initArray = Expression.NewArrayInit( pairType,
                this.pairsList.Select<(Transform transform, String name),Expression>(
                    (val) => Expression.Invoke( initFunc, Expression.Constant( val.name ), Expression.Constant( val.transform ) ) ) );
            var childLocParam = Expression.Parameter( typeof(ChildLocator), "childLocator" );
            var assignArray = Expression.Assign( Expression.Field( childLocParam, field ), initArray );
            Expression.Lambda<Action<ChildLocator>>( assignArray, childLocParam ).Compile()( this.RW_childLoc );
        }

        private void AddToChildLocator( Transform target, String name )
        {
            this.pairsList.Add((target,name));
        }

        private void AddHurtBox( Transform parent, ICollider info, Boolean isMain = false, Boolean isBullseye = false, HurtBox.DamageModifier damageMod = HurtBox.DamageModifier.Normal )
        {
            var obj = new GameObject( "HurtBox" );
            obj.transform.parent = parent;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;
            obj.layer = LayerIndex.entityPrecise.intVal;

            var col = info.Apply( obj );
            col.isTrigger = false;

            var hurtBox = obj.AddComponent<HurtBox>();
            hurtBox.isBullseye = isBullseye;
            hurtBox.healthComponent = this.RW_healthComponent;
            hurtBox.damageModifier = damageMod;
            hurtBox.hurtBoxGroup = this.RW_boxGroup;

            if( isMain ) this.RW_boxGroup.mainHurtBox = hurtBox;

            var ind = (Int16)this.RW_boxGroup.hurtBoxes.Length;
            Array.Resize<HurtBox>( ref this.RW_boxGroup.hurtBoxes, ind + 1 );
            this.RW_boxGroup.hurtBoxes[ind] = hurtBox;
            hurtBox.indexInGroup = ind;

            if( isBullseye ) ++this.RW_boxGroup.bullseyeCount;
        }

        private void AddRagdollCollider( Transform parent, ICollider info )
        {
            var col = info.Apply( parent.gameObject );
            col.isTrigger = false;
            col.enabled = false;

            var rb = parent.gameObject.AddComponent<Rigidbody>();

            var ind = this.RW_ragdollController.bones.Length;
            Array.Resize<Transform>( ref this.RW_ragdollController.bones, ind + 1 );
            this.RW_ragdollController.bones[ind] = parent;
        }

        private void AddLight( Transform parent, Single intensity, Single range )
        {
            var obj = new GameObject( "Light" );
            obj.transform.parent = parent;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;

            var light = obj.AddComponent<Light>();
            light.intensity = intensity;
            light.range = range;

            var ind = this.RW_charModel.baseLightInfos.Length;
            Array.Resize( ref this.RW_charModel.baseLightInfos, ind + 1 );
            this.RW_charModel.baseLightInfos[ind] = new CharacterModel.LightInfo( light );
        }

        private void AddFireParticles( Transform parent, Vector3 position, Vector3 rotation, Vector3 scale )
        {
            var obj = new GameObject( "Particles" );
            obj.transform.parent = parent;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;

            var ps = obj.AddComponent<ParticleSystem>();
            var psr = obj.AddOrGetComponent<ParticleSystemRenderer>();


            var ind = this.RW_charModel.baseParticleSystemInfos.Length;
            Array.Resize( ref this.RW_charModel.baseParticleSystemInfos, ind + 1 );
            this.RW_charModel.baseParticleSystemInfos[ind] = new CharacterModel.ParticleSystemInfo( ps );

            // TODO: Actually set up the particle system

            var psMain = ps.main;
            psMain.duration = 1f;
            psMain.loop = true;
            psMain.prewarm = false;
            psMain.startDelay = 0f;
            psMain.startLifetime = new ParticleSystem.MinMaxCurve( 0.35f, 0.85f );
            psMain.startSpeed = 1f;
            psMain.startSize3D = false;
            psMain.startSize = 1f;
            psMain.startRotation3D = false;
            psMain.startRotation = new ParticleSystem.MinMaxCurve( 0f, 360f );
            psMain.flipRotation = 0.5f;
            psMain.startColor = Color.white;
            psMain.gravityModifier = -0.3f;
            psMain.simulationSpace = ParticleSystemSimulationSpace.World;
            psMain.simulationSpeed = 1f;
            psMain.useUnscaledTime = false;
            psMain.scalingMode = ParticleSystemScalingMode.Local;
            psMain.playOnAwake = true;
            psMain.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Transform;
            psMain.maxParticles = 1000;
            psMain.stopAction = ParticleSystemStopAction.None;
            psMain.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            psMain.ringBufferMode = ParticleSystemRingBufferMode.Disabled;

            var psEmis = ps.emission;
            psEmis.enabled = true;
            psEmis.rateOverTime = 15f;
            psEmis.rateOverDistance = 0f;

            var psShape = ps.shape;
            psShape.enabled = true;
            psShape.shapeType = ParticleSystemShapeType.Cone;
            psShape.angle = 38.26f;
            psShape.radius = 0.75f;
            psShape.radiusThickness = 1f;
            psShape.arc = 360f;
            psShape.arcMode = ParticleSystemShapeMultiModeValue.Random;
            psShape.arcSpread = 0f;
            psShape.position = position;
            psShape.rotation = rotation;
            psShape.scale = scale;
            psShape.alignToDirection = false;

            var psVOL = ps.velocityOverLifetime;
            psVOL.enabled = false;

            var pslimVOL = ps.limitVelocityOverLifetime;
            pslimVOL.enabled = false;

            var psInheritVel = ps.inheritVelocity;
            psInheritVel.enabled = true;
            psInheritVel.mode = ParticleSystemInheritVelocityMode.Current;
            psInheritVel.curve = 0.8f;

            var psFOL = ps.forceOverLifetime;
            psFOL.enabled = false;

            var psCOL = ps.colorOverLifetime;
            psCOL.enabled = true;
            psCOL.color = new ParticleSystem.MinMaxGradient( new Gradient
            {
                mode = GradientMode.Blend,
                alphaKeys = new GradientAlphaKey[]
                {
                    new GradientAlphaKey( 0f, 0f ),
                    new GradientAlphaKey( 0.9f, 0.1f ),
                    new GradientAlphaKey( 0.6f, 0.6f ),
                    new GradientAlphaKey( 0f, 1f ),
                },
                colorKeys = new GradientColorKey[]
                {
                    new GradientColorKey( Color.white, 0f ),
                    new GradientColorKey( Color.white, 1f ),
                }
            } );

            var psCBS = ps.colorBySpeed;
            psCBS.enabled = false;

            var psSOL = ps.sizeOverLifetime;
            psSOL.enabled = true;
            psSOL.size = new ParticleSystem.MinMaxCurve( 1.5f, new AnimationCurve
            {
                preWrapMode = WrapMode.Clamp,
                postWrapMode = WrapMode.Clamp,
                keys = new[]
                {
                    new Keyframe( 0f, 0.2f ),
                    new Keyframe( 0.47f, 0.51f ),
                    new Keyframe( 1f, 0.0025f )
                }
            });

            var psSBS = ps.sizeBySpeed;
            psSBS.enabled = false;

            var psROL = ps.rotationOverLifetime;
            psROL.enabled = true;
            psROL.separateAxes = false;
            psROL.z = 3f;

            var psRBS = ps.rotationBySpeed;
            psRBS.enabled = false;

            var psExtForce = ps.externalForces;
            psExtForce.enabled = false;

            var psNoise = ps.noise;
            psNoise.enabled = false;

            var psCollide = ps.collision;
            psCollide.enabled = false;

            var psTrig = ps.trigger;
            psTrig.enabled = false;

            var psSubEmit = ps.subEmitters;
            psSubEmit.enabled = false;

            var psTexSheetAnim = ps.textureSheetAnimation;
            psTexSheetAnim.enabled = false;

            var psLights = ps.lights;
            psLights.enabled = false;

            var psTrail = ps.trails;
            psTrail.enabled = false;

            var psCustData = ps.customData;
            psCustData.enabled = false;

        }

        private interface ICollider
        {
            Collider Apply( GameObject obj );
        }

        private struct SphereColliderInfo : ICollider
        {
            public Vector3 center;
            public Single radius;

            public SphereColliderInfo( Vector3 center, Single radius )
            {
                this.center = center;
                this.radius = radius;
            }


            public Collider Apply( GameObject obj )
            {
                var col = obj.AddComponent<SphereCollider>();

                col.center = this.center;
                col.radius = this.radius;

                return col;
            }
        }

        private struct BoxColliderInfo : ICollider
        {
            public Vector3 center;
            public Vector3 size;

            public BoxColliderInfo( Vector3 center, Vector3 size )
            {
                this.center = center;
                this.size = size;
            }

            public Collider Apply( GameObject obj )
            {
                var col = obj.AddComponent<BoxCollider>();

                col.center = this.center;
                col.size = this.size;

                return col;
            }
        }

        private struct CapsuleColliderInfo : ICollider
        {
            public Single radius;
            public Single height;
            public Vector3 center;
            public Direction direction;

            public CapsuleColliderInfo( Vector3 center, Direction direction, Single radius, Single height )
            {
                this.radius = radius;
                this.height = height;
                this.center = center;
                this.direction = direction;
            }

            public Collider Apply( GameObject obj )
            {
                var col = obj.AddComponent<CapsuleCollider>();

                col.center = this.center;
                col.direction = (Int32)this.direction;
                col.radius = this.radius;
                col.height = this.height;

                return col;
            }

            public enum Direction
            {
                X = 0,
                Y = 1,
                Z = 2,
            }
        }
    }
}
#endif