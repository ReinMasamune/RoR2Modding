#if ANCIENTWISP
using GeneralPluginStuff;
using RogueWispPlugin.Helpers;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEngine;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void AW_Model()
        {
            this.Load += this.AW_Model1;
            this.Load += this.AW_ChildSetup;
        }

        private void AW_Model1()
        {
            ModelLocator modelLocator = this.AW_body.GetComponent<ModelLocator>();
            var modelBaseTransform = modelLocator.modelBaseTransform;
            var modelTransform = modelLocator.modelTransform;
            modelBaseTransform.gameObject.name = "ModelBase";
            //modelBaseTransform.localPosition = new Vector3( 0f, 0.0f, 0f );
            //modelTransform.localScale = new Vector3( 0.7f, 0.7f, 0.7f );
            //modelTransform.localPosition = new Vector3( 0f, 0.2f, 0f );

            //GameObject pivotPoint = new GameObject("CannonPivot");
            //pivotPoint.transform.parent = modelTransform;
            //pivotPoint.transform.localPosition = new Vector3( 0f, 1.5f, 0f );
            //pivotPoint.transform.localEulerAngles = new Vector3( -90f, 0f, 0f );

            //Transform armatureTransform = modelTransform.Find("AncientWispArmature");
            //armatureTransform.parent = pivotPoint.transform;

            //GameObject beamParent = new GameObject("BeamParent");
            //beamParent.transform.parent = pivotPoint.transform;
            //beamParent.transform.localPosition = new Vector3( 0f, 0.5f, -1f );
            //beamParent.transform.localEulerAngles = new Vector3( 0f, 0f, 0f );


            // FLARE STUFF HERE?
        }

        private CharacterModel AW_charModel;
        private WispModelBitSkinController AW_skinController;
        private ChildLocator AW_childLoc;
        private RagdollController AW_ragdollController;
        private HurtBoxGroup AW_boxGroup;
        private HealthComponent AW_healthComponent;
        //private WispFlamesController AW_flameController;
        //private WispPassiveController AW_passiveController;

        private List<(Transform transform,String name)> AW_pairsList = new List<(Transform transform,String name)>();

        private void AW_ChildSetup()
        {
            Transform modelTransform = this.AW_body.GetComponent<ModelLocator>().modelTransform;
            this.AW_charModel = modelTransform.GetComponent<CharacterModel>();
            this.AW_skinController = this.AW_charModel.AddComponent<WispModelBitSkinController>();
            this.AW_childLoc = modelTransform.GetComponent<ChildLocator>();
            this.AW_ragdollController = modelTransform.AddComponent<RagdollController>();
            this.AW_boxGroup = modelTransform.GetComponent<HurtBoxGroup>();
            this.AW_healthComponent = this.AW_body.GetComponent<HealthComponent>();
            //this.AW_flameController = this.AW_body.GetComponent<WispFlamesController>();
            //this.AW_passiveController = this.AW_body.GetComponent<WispPassiveController>();

            //enrageController.enrageSkinIndex = 0b1001_0000_0001_1110_1011_0111_0011_1111u;

            foreach( var val in this.AW_charModel.baseLightInfos ) UnityEngine.Object.DestroyImmediate( val.light.gameObject );
            this.AW_charModel.baseLightInfos = Array.Empty<CharacterModel.LightInfo>();
            foreach( var val in this.AW_charModel.baseParticleSystemInfos ) UnityEngine.Object.DestroyImmediate( val.particleSystem.gameObject );
            this.AW_charModel.baseParticleSystemInfos = Array.Empty<CharacterModel.ParticleSystemInfo>();
            UnityEngine.Object.DestroyImmediate( modelTransform.GetComponent<AncientWispFireController>() );
            foreach( var val in this.AW_boxGroup.hurtBoxes )
            {
                UnityEngine.Object.DestroyImmediate( val.gameObject );
            }
            this.AW_boxGroup.hurtBoxes = Array.Empty<HurtBox>();
            this.AW_boxGroup.mainHurtBox = null;
            this.AW_boxGroup.bullseyeCount = 0;
            this.AW_ragdollController.bones = Array.Empty<Transform>();
            //this.AW_flameController.passive = this.AW_passiveController;
            this.AW_AddFireParticlesToMesh( this.AW_body.GetComponentInChildren<SkinnedMeshRenderer>().transform );



            //var enrageController = this.AW_body.AddComponent<WispBossEnrageController>();
            //enrageController.baseEmissionRate = 1000f;
            //enrageController.baseFlareIntensity = 0.1f;
            ////enrageController.baseSkinIndex = 0b0001_0000_0000_0000_0000_0000_0000_0000u;
            //enrageController.body = this.AW_body.GetComponent<CharacterBody>();
            //enrageController.bodySkin = this.AW_skinController;
            //enrageController.bodyParticles = 
            ////enrageController.soundMult = 3;
            ////enrageController.soundScale = 1f;
            //enrageController.enrageBuffIndex = BuffIndex.EnrageAncientWisp;
            //enrageController.enrageEmissionRate = 1500f;
            //enrageController.enrageFlareIntensity = 2f;
            ////enrageController.enrageSound = "";
            //enrageController.flare1 = this.AW_body.GetComponentInChildren<SpriteRenderer>();
            //enrageController.baseSkinIndex =    0b0001_0000_0000_0010_1011_0111_0011_1111u;
            ////enrageController.enrageSkinIndex =  0b1001_0000_0000_1110_1011_0111_0011_1111u;
            //enrageController.enrageSkinIndex =  0b1001_0000_0000_0110_1011_0111_0011_1111u;
            ////enrageController.enrageSkinIndex = 0b1001_0000_0001_1110_1011_0111_0011_1111u;


            //Path stuff here
            #region Transform paths
            var meshTransform = modelTransform.Find( "AncientWispMesh" );
            var armaTransform = modelTransform.Find( "AncientWispArmature" );

            #region chest
            {
                var chestTransform = armaTransform.Find("chest");

                #region ChestCannon1
                {
                    var chestCannon1Transform = chestTransform.Find("ChestCannon1");

                    this.AW_AddFireParticles(
                        parent: chestCannon1Transform,
                        position: new Vector3( 0f, 1f, 0f ),
                        rotation: new Vector3( 90f, 0f, 0f ),
                        scale: new Vector3( 0.25f, 0.2f, 0.6f )
                        );

                    var colliderInfo01 = new BoxColliderInfo(
                        center:     new Vector3( 0f, 0.4f, 0f ),
                        size:       new Vector3( 0.5f, 0.9f, 0.1f )
                        );

                    this.AW_AddRagdollCollider(
                        parent: chestCannon1Transform,
                        info: colliderInfo01
                        );

                    this.AW_AddHurtBox(
                        parent: chestCannon1Transform,
                        info: colliderInfo01,
                        isMain: true,
                        isBullseye: true,
                        damageMod: HurtBox.DamageModifier.Normal
                        );

                    #region ChestCannonGuard1
                    {
                        var chestCannonGuard1Transform = chestCannon1Transform.Find("ChestCannonGuard1");

                        var colliderInfo02 = new BoxColliderInfo(
                            center:     new Vector3( 0f, 0.25f, 0.05f ),
                            size:       new Vector3( 0.5f, 0.9f, 0.1f )
                            );

                        this.AW_AddRagdollCollider(
                            parent: chestCannonGuard1Transform,
                            info: colliderInfo02
                            );

                        this.AW_AddHurtBox(
                            parent: chestCannonGuard1Transform,
                            info: colliderInfo02
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

                    this.AW_AddToChildLocator( chestTransform, "ChestCannon2" );

                    this.AW_AddFireParticles(
                        parent: chestCannon2Transform,
                        position: new Vector3( 0f, 1f, 0f ),
                        rotation: new Vector3( 90f, 0f, 0f ),
                        scale: new Vector3( 0.25f, 0.2f, 0.6f )
                        );

                    var colliderInfo03 = new BoxColliderInfo(
                        center:     new Vector3( 0f, 0.4f, 0f ),
                        size:       new Vector3( 0.5f, 0.9f, 0.1f )
                        );

                    this.AW_AddRagdollCollider(
                        parent: chestCannon2Transform,
                        info: colliderInfo03
                        );

                    this.AW_AddHurtBox(
                        parent: chestCannon2Transform,
                        info: colliderInfo03,
                        isMain: false,
                        isBullseye: false,
                        damageMod: HurtBox.DamageModifier.Normal
                        );

                    #region ChestCannonGuard2
                    {
                        var chestCannonGuard2Transform = chestCannon2Transform.Find("ChestCannonGuard2");

                        var colliderInfo04 = new BoxColliderInfo(
                            center:     new Vector3( 0f, 0.17f, -0.05f ),
                            size:       new Vector3( 0.7f, 0.5f, 0.2f )
                            );

                        this.AW_AddRagdollCollider(
                            parent: chestCannonGuard2Transform,
                            info: colliderInfo04
                            );

                        this.AW_AddHurtBox(
                            parent: chestCannonGuard2Transform,
                            info: colliderInfo04,
                            isMain: false,
                            isBullseye: false,
                            damageMod: HurtBox.DamageModifier.Normal
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

                this.AW_AddToChildLocator( headTransform, "Head" );

                this.AW_AddFireParticles(
                    parent: headTransform,
                    position: new Vector3( 0f, 0.15f, 0f ),
                    rotation: new Vector3( 180f, 0f, 0f ),
                    scale: new Vector3( 0.1f, 0.1f, 0.1f )
                    );

                var colliderInfo05 = new CapsuleColliderInfo(
                    center:     new Vector3( 0f, 0.25f, 0.15f ),
                    direction:  CapsuleColliderInfo.Direction.X,
                    radius:     0.25f,
                    height:     1f
                    );

                this.AW_AddRagdollCollider(
                    parent: headTransform,
                    info: colliderInfo05
                    );

                this.AW_AddHurtBox(
                    parent: headTransform,
                    info: colliderInfo05,
                    isMain: false,
                    isBullseye: false,
                    damageMod: HurtBox.DamageModifier.Normal
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

                this.AW_AddToChildLocator( shoulderL, "shoulder.l" );

                var colliderInfo06 = new BoxColliderInfo(
                    center:     new Vector3( -0.075f, 0.3f, 0f ),
                    size:       new Vector3( 0.3f, 0.5f, 0.5f )
                    );

                this.AW_AddRagdollCollider(
                    parent: shoulderL,
                    info: colliderInfo06
                    );

                this.AW_AddHurtBox(
                    parent: shoulderL,
                    info: colliderInfo06,
                    isMain: false,
                    isBullseye: false,
                    damageMod: HurtBox.DamageModifier.Normal
                    );

                #region upperArm1.l
                {
                    var upperArm1L = shoulderL.Find("upperArm1.l");

                    this.AW_AddFireParticles( upperArm1L,
                        position: new Vector3( 0f, 0.4f, 0f ),
                        rotation: new Vector3( 90f, 0f, 0f ),
                        scale: new Vector3( 0.15f, 0.15f, 0.5f )
                        );

                    var colliderInfo07 = new BoxColliderInfo(
                        center:     new Vector3( 0f, 0.15f, 0f ),
                        size:       new Vector3( 0.2f, 0.4f, 0.4f )
                        );

                    this.AW_AddRagdollCollider(
                        parent: upperArm1L,
                        info: colliderInfo07
                        );

                    this.AW_AddHurtBox(
                        parent: upperArm1L,
                        info: colliderInfo07,
                        isMain: false,
                        isBullseye: false,
                        damageMod: HurtBox.DamageModifier.Normal
                        );


                    #region upperArm2.l
                    {
                        var upperArm2L = upperArm1L.Find( "upperArm2.l" );

                        var colliderInfo08 = new BoxColliderInfo(
                            center:     new Vector3( 0.12f, 0.3f, 0.3f ),
                            size:       new Vector3( 0.01f, 0.17f, 0f )
                            );

                        this.AW_AddRagdollCollider(
                            parent: upperArm2L,
                            info: colliderInfo08
                            );

                        this.AW_AddHurtBox(
                            parent: upperArm2L,
                            info: colliderInfo08,
                            isMain: false,
                            isBullseye: false,
                            damageMod: HurtBox.DamageModifier.Normal
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

                                this.AW_AddRagdollCollider(
                                    parent: finger1L,
                                    info: colliderInfo09
                                    );

                                this.AW_AddHurtBox(
                                    parent: finger1L,
                                    info: colliderInfo09,
                                    isMain: false,
                                    isBullseye: false,
                                    damageMod: HurtBox.DamageModifier.Normal
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

                                this.AW_AddRagdollCollider(
                                    parent: finger2L,
                                    info: colliderInfo10
                                    );

                                this.AW_AddHurtBox(
                                    parent: finger2L,
                                    info: colliderInfo10,
                                    isMain: false,
                                    isBullseye: false,
                                    damageMod: HurtBox.DamageModifier.Normal
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

                                this.AW_AddRagdollCollider(
                                    parent: thumbL,
                                    info: colliderInfo11
                                    );

                                this.AW_AddHurtBox(
                                    parent: thumbL,
                                    info: colliderInfo11,
                                    isMain: false,
                                    isBullseye: false,
                                    damageMod: HurtBox.DamageModifier.Normal
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

                                this.AW_AddFireParticles( muzzleLeft,
                                    position: new Vector3( 0f, 0f, 0.1f ),
                                    rotation: new Vector3( 180f, 0f, 0f ),
                                    scale: new Vector3( 0.1f, 0.1f, 0.5f ) );

                                this.AW_AddToChildLocator( muzzleLeft, "MuzzleLeft" );
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

                this.AW_AddToChildLocator( shoulderR, "shoulder.r" );

                var colliderInfo12 = new BoxColliderInfo(
                    center:     new Vector3( 0.075f, 0.3f, 0f ),
                    size:       new Vector3( 0.3f, 0.5f, 0.5f )
                    );

                this.AW_AddRagdollCollider(
                    parent: shoulderR,
                    info: colliderInfo12
                    );

                this.AW_AddHurtBox(
                    parent: shoulderR,
                    info: colliderInfo12,
                    isMain: false,
                    isBullseye: false,
                    damageMod: HurtBox.DamageModifier.Normal
                    );

                #region upperArm1.r
                {
                    var upperArm1R = shoulderR.Find( "upperArm1.r" );

                    this.AW_AddFireParticles( upperArm1R,
                        position: new Vector3( 0f, 0.4f, 0f ),
                        rotation: new Vector3( 90f, 0f, 0f ),
                        scale: new Vector3( 0.15f, 0.15f, 0.5f )
                        );

                    var colliderInfo13 = new BoxColliderInfo(
                        center:     new Vector3( 0f, 0.15f, 0f ),
                        size:       new Vector3( 0.2f, 0.4f, 0.4f )
                        );

                    this.AW_AddRagdollCollider(
                        parent: upperArm1R,
                        info: colliderInfo13
                        );

                    this.AW_AddHurtBox(
                        parent: upperArm1R,
                        info: colliderInfo13,
                        isMain: false,
                        isBullseye: false,
                        damageMod: HurtBox.DamageModifier.Normal
                        );


                    #region RightSwingCenter
                    {
                        var rightSwingCenter = upperArm1R.Find( "RightSwingCenter" );

                        this.AW_AddToChildLocator( rightSwingCenter, "RightSwingCenter" );
                    }
                    #endregion
                    #region upperArm2.r
                    {
                        var upperArm2R = upperArm1R.Find( "upperArm2.r" );

                        var colliderInfo14 = new BoxColliderInfo(
                            center:     new Vector3( -0.01f, 0.17f, 0f ),
                            size:       new Vector3( 0.12f, 0.3f, 0.3f )
                            );

                        this.AW_AddRagdollCollider(
                            parent: upperArm2R,
                            info: colliderInfo14
                            );

                        this.AW_AddHurtBox(
                            parent: upperArm2R,
                            info: colliderInfo14,
                            isMain: false,
                            isBullseye: false,
                            damageMod: HurtBox.DamageModifier.Normal
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

                                this.AW_AddRagdollCollider(
                                    parent: finger1R,
                                    info: colliderInfo15
                                    );

                                this.AW_AddHurtBox(
                                    parent: finger1R,
                                    info: colliderInfo15,
                                    isMain: false,
                                    isBullseye: false,
                                    damageMod: HurtBox.DamageModifier.Normal
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

                                this.AW_AddRagdollCollider(
                                    parent: finger2R,
                                    info: colliderInfo16
                                    );

                                this.AW_AddHurtBox(
                                    parent: finger2R,
                                    info: colliderInfo16,
                                    isMain: false,
                                    isBullseye: false,
                                    damageMod: HurtBox.DamageModifier.Normal
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

                                this.AW_AddRagdollCollider(
                                    parent: thumbR,
                                    info: colliderInfo17
                                    );

                                this.AW_AddHurtBox(
                                    parent: thumbR,
                                    info: colliderInfo17,
                                    isMain: false,
                                    isBullseye: false,
                                    damageMod: HurtBox.DamageModifier.Normal
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

                                this.AW_AddFireParticles( muzzleRight,
                                    position: new Vector3( 0f, 0f, 0f ),
                                    rotation: new Vector3( 180f, 0f, 0f ),
                                    scale: new Vector3( 0.1f, 0.1f, 0.5f ) );

                                this.AW_AddToChildLocator( muzzleRight, "MuzzleRight" );

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

                this.AW_AddRagdollCollider(
                    parent: thighL,
                    info: colliderInfo18
                    );

                this.AW_AddHurtBox(
                    parent: thighL,
                    info: colliderInfo18,
                    isMain: false,
                    isBullseye: false,
                    damageMod: HurtBox.DamageModifier.Normal
                    );


                #region calf.l
                {
                    var calfL = thighL.Find( "calf.l" );

                    this.AW_AddFireParticles(
                        parent: calfL,
                        position: new Vector3( 0f, 0.6f, 0f ),
                        rotation: new Vector3( 90f, 0f, 0f ),
                        scale: new Vector3( 0.1f, 0.1f, 0.5f )
                        );

                    var colliderInfo19 = new CapsuleColliderInfo(
                        center:     new Vector3( 0f, 0.3f, 0f ),
                        direction:  CapsuleColliderInfo.Direction.Y,
                        radius:     0.125f,
                        height:     0.7f
                        );

                    this.AW_AddRagdollCollider(
                        parent: calfL,
                        info: colliderInfo19
                        );

                    this.AW_AddHurtBox(
                        parent: calfL,
                        info: colliderInfo19,
                        isMain: false,
                        isBullseye: false,
                        damageMod: HurtBox.DamageModifier.Normal
                        );


                    #region toe1.l
                    {
                        var toe1L = calfL.Find( "toe1.l" );

                        var colliderInfo20 = new BoxColliderInfo(
                            center:     new Vector3( 0f, 0.08f, 0.01f ),
                            size:       new Vector3( 0.12f, 0.3f, 0.08f )
                            );

                        this.AW_AddRagdollCollider(
                            parent: toe1L,
                            info: colliderInfo20
                            );

                        this.AW_AddHurtBox(
                            parent: toe1L,
                            info: colliderInfo20,
                            isMain: false,
                            isBullseye: false,
                            damageMod: HurtBox.DamageModifier.Normal
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

                        this.AW_AddRagdollCollider(
                            parent: toe2L,
                            info: colliderInfo21
                            );

                        this.AW_AddHurtBox(
                            parent: toe2L,
                            info: colliderInfo21,
                            isMain: false,
                            isBullseye: false,
                            damageMod: HurtBox.DamageModifier.Normal
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

                this.AW_AddRagdollCollider(
                    parent: thighR,
                    info: colliderInfo22
                    );

                this.AW_AddHurtBox(
                    parent: thighR,
                    info: colliderInfo22,
                    isMain: false,
                    isBullseye: false,
                    damageMod: HurtBox.DamageModifier.Normal
                    );

                #region calf.r
                {
                    var calfR = thighR.Find( "calf.r" );

                    this.AW_AddFireParticles(
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

                    this.AW_AddRagdollCollider(
                        parent: calfR,
                        info: colliderInfo23
                        );

                    this.AW_AddHurtBox
                    (
                        parent: calfR,
                        info: colliderInfo23,
                        isMain: false,
                        isBullseye: false,
                        damageMod: HurtBox.DamageModifier.Normal
                    );

                    #region toe1.r
                    {
                        var toe1R = calfR.Find( "toe1.r" );

                        var colliderInfo24 = new BoxColliderInfo(
                            center:     new Vector3( 0f, 0.08f, 0.01f ),
                            size:       new Vector3( 0.12f, 0.3f, 0.08f )
                            );

                        this.AW_AddRagdollCollider(
                            parent: toe1R,
                            info: colliderInfo24
                            );

                        this.AW_AddHurtBox(
                            parent: toe1R,
                            info: colliderInfo24,
                            isMain: false,
                            isBullseye: false,
                            damageMod: HurtBox.DamageModifier.Normal
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

                        this.AW_AddRagdollCollider(
                            parent: toe2R,
                            info: colliderInfo25
                            );

                        this.AW_AddHurtBox(
                            parent: toe2R,
                            info: colliderInfo25,
                            isMain: false,
                            isBullseye: false,
                            damageMod: HurtBox.DamageModifier.Normal
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
                this.AW_pairsList.Select<(Transform transform, String name),Expression>(
                    (val) => Expression.Invoke( initFunc, Expression.Constant( val.name ), Expression.Constant( val.transform ) ) ) );
            var childLocParam = Expression.Parameter( typeof(ChildLocator), "childLocator" );
            var assignArray = Expression.Assign( Expression.Field( childLocParam, field ), initArray );
            Expression.Lambda<Action<ChildLocator>>( assignArray, childLocParam ).Compile()( this.AW_childLoc );


            this.AW_DoIDRSSetup();
        }

        private void AW_AddToChildLocator( Transform target, String name )
        {
            this.AW_pairsList.Add( (target, name) );
        }

        private void AW_AddHurtBox( Transform parent, ICollider info, Boolean isMain = false, Boolean isBullseye = false, HurtBox.DamageModifier damageMod = HurtBox.DamageModifier.Normal )
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
            hurtBox.healthComponent = this.AW_healthComponent;
            hurtBox.damageModifier = damageMod;
            hurtBox.hurtBoxGroup = this.AW_boxGroup;

            if( isMain ) this.AW_boxGroup.mainHurtBox = hurtBox;

            var ind = (Int16)this.AW_boxGroup.hurtBoxes.Length;
            Array.Resize<HurtBox>( ref this.AW_boxGroup.hurtBoxes, ind + 1 );
            this.AW_boxGroup.hurtBoxes[ind] = hurtBox;
            hurtBox.indexInGroup = ind;

            if( isBullseye ) ++this.AW_boxGroup.bullseyeCount;
        }

        private void AW_AddRagdollCollider( Transform parent, ICollider info )
        {
            var col = info.Apply( parent.gameObject );
            col.isTrigger = false;
            col.enabled = false;

            var rb = parent.gameObject.AddComponent<Rigidbody>();

            var ind = this.AW_ragdollController.bones.Length;
            Array.Resize<Transform>( ref this.AW_ragdollController.bones, ind + 1 );
            this.AW_ragdollController.bones[ind] = parent;
        }

        private void AW_AddLight( Transform parent, Single intensity, Single range )
        {
            var obj = new GameObject( "Light" );
            obj.transform.parent = parent;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;

            var light = obj.AddComponent<Light>();
            light.intensity = intensity;
            light.range = range;

            var ind = this.AW_charModel.baseLightInfos.Length;
            Array.Resize( ref this.AW_charModel.baseLightInfos, ind + 1 );
            this.AW_charModel.baseLightInfos[ind] = new CharacterModel.LightInfo( light );
        }

        private void AW_AddFireParticles( Transform parent, Vector3 position, Vector3 rotation, Vector3 scale )
        {
            //var obj = new GameObject( "Particles" );
            //obj.transform.parent = parent;
            //obj.transform.localPosition = Vector3.zero;
            //obj.transform.localScale = Vector3.one;
            //obj.transform.localRotation = Quaternion.identity;

            //var ps = obj.AddComponent<ParticleSystem>();
            //var psr = obj.AddOrGetComponent<ParticleSystemRenderer>();


            //var ind = this.AW_charModel.baseParticleSystemInfos.Length;
            //Array.Resize( ref this.AW_charModel.baseParticleSystemInfos, ind + 1 );
            //this.AW_charModel.baseParticleSystemInfos[ind] = new CharacterModel.ParticleSystemInfo( ps );

            //// TODO: Actually set up the particle system

            //var psMain = ps.main;
            //psMain.duration = 1f;
            //psMain.loop = true;
            //psMain.prewarm = false;
            //psMain.startDelay = 0f;
            //psMain.startLifetime = new ParticleSystem.MinMaxCurve( 0.35f, 0.85f );
            //psMain.startSpeed = new ParticleSystem.MinMaxCurve( 0.2f, 1f );
            //psMain.startSize3D = false;
            //psMain.startSize = 1f;
            //psMain.startRotation3D = false;
            //psMain.startRotation = new ParticleSystem.MinMaxCurve( 0f, 360f );
            //psMain.flipRotation = 0.5f;
            //psMain.startColor = Color.white;
            //psMain.gravityModifier = -0.3f;
            //psMain.simulationSpace = ParticleSystemSimulationSpace.World;
            //psMain.simulationSpeed = 1f;
            //psMain.useUnscaledTime = false;
            //psMain.scalingMode = ParticleSystemScalingMode.Local;
            //psMain.playOnAwake = true;
            //psMain.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Transform;
            //psMain.maxParticles = 1000;
            //psMain.stopAction = ParticleSystemStopAction.None;
            //psMain.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            //psMain.ringBufferMode = ParticleSystemRingBufferMode.Disabled;

            //var psEmis = ps.emission;
            //psEmis.enabled = true;
            //psEmis.rateOverTime = 15f;
            //psEmis.rateOverDistance = 0f;

            //var psShape = ps.shape;
            //psShape.enabled = true;
            //psShape.shapeType = ParticleSystemShapeType.Cone;
            //psShape.angle = 38.26f;
            //psShape.radius = 0.75f;
            //psShape.radiusThickness = 1f;
            //psShape.arc = 360f;
            //psShape.arcMode = ParticleSystemShapeMultiModeValue.Random;
            //psShape.arcSpread = 0f;
            //psShape.position = position;
            //psShape.rotation = rotation;
            //psShape.scale = scale;
            //psShape.alignToDirection = false;

            //var psVOL = ps.velocityOverLifetime;
            //psVOL.enabled = false;

            //var pslimVOL = ps.limitVelocityOverLifetime;
            //pslimVOL.enabled = false;

            //var psInheritVel = ps.inheritVelocity;
            //psInheritVel.enabled = true;
            //psInheritVel.mode = ParticleSystemInheritVelocityMode.Current;
            //psInheritVel.curve = new ParticleSystem.MinMaxCurve( 1f, new AnimationCurve
            //{
            //    preWrapMode = WrapMode.Clamp,
            //    postWrapMode = WrapMode.Clamp,
            //    keys = new[]
            //    {
            //        new Keyframe(0f, 1f, 0f, 0f ),
            //        new Keyframe(0.65f, 0.85f, -0.15f, -0.5f ),
            //        new Keyframe(1f, 0f, 0f, 0f ),
            //    }
            //} );

            //var psFOL = ps.forceOverLifetime;
            //psFOL.enabled = false;

            //var psCOL = ps.colorOverLifetime;
            //psCOL.enabled = true;
            //psCOL.color = new ParticleSystem.MinMaxGradient( new Gradient
            //{
            //    mode = GradientMode.Blend,
            //    alphaKeys = new GradientAlphaKey[]
            //    {
            //        new GradientAlphaKey( 0f, 0f ),
            //        new GradientAlphaKey( 0.9f, 0.1f ),
            //        new GradientAlphaKey( 0.6f, 0.6f ),
            //        new GradientAlphaKey( 0f, 1f ),
            //    },
            //    colorKeys = new GradientColorKey[]
            //    {
            //        new GradientColorKey( Color.white, 0f ),
            //        new GradientColorKey( Color.white, 1f ),
            //    }
            //} );

            //var psCBS = ps.colorBySpeed;
            //psCBS.enabled = false;

            //var psSOL = ps.sizeOverLifetime;
            //psSOL.enabled = true;
            //psSOL.size = new ParticleSystem.MinMaxCurve( 1.5f, new AnimationCurve
            //{
            //    preWrapMode = WrapMode.Clamp,
            //    postWrapMode = WrapMode.Clamp,
            //    keys = new[]
            //    {
            //        new Keyframe( 0f, 0.2f ),
            //        new Keyframe( 0.47f, 0.51f ),
            //        new Keyframe( 1f, 0.0025f )
            //    }
            //});

            //var psSBS = ps.sizeBySpeed;
            //psSBS.enabled = false;

            //var psROL = ps.rotationOverLifetime;
            //psROL.enabled = true;
            //psROL.separateAxes = false;
            //psROL.z = 3f;

            //var psRBS = ps.rotationBySpeed;
            //psRBS.enabled = false;

            //var psExtForce = ps.externalForces;
            //psExtForce.enabled = false;

            //var psNoise = ps.noise;
            //psNoise.enabled = false;

            //var psCollide = ps.collision;
            //psCollide.enabled = false;

            //var psTrig = ps.trigger;
            //psTrig.enabled = false;

            //var psSubEmit = ps.subEmitters;
            //psSubEmit.enabled = false;

            //var psTexSheetAnim = ps.textureSheetAnimation;
            //psTexSheetAnim.enabled = false;

            //var psLights = ps.lights;
            //psLights.enabled = false;

            //var psTrail = ps.trails;
            //psTrail.enabled = false;

            //var psCustData = ps.customData;
            //psCustData.enabled = false;

        }

        private ParticleSystem AW_AddFireParticlesToMesh( Transform model )
        {
            var obj = new GameObject( "Particles" );
            obj.transform.parent = model.parent;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;

            var ps = obj.AddComponent<ParticleSystem>();
            var psr = obj.AddOrGetComponent<ParticleSystemRenderer>();

            psr.renderMode = ParticleSystemRenderMode.Billboard;
            psr.normalDirection = 1;
            psr.sortMode = ParticleSystemSortMode.None;
            psr.sortingFudge = 0;
            psr.minParticleSize = 0f;
            psr.maxParticleSize = 1f;
            psr.alignment = ParticleSystemRenderSpace.View;
            psr.flip = Vector3.zero;
            psr.allowRoll = true;
            psr.pivot = Vector3.zero;
            psr.maskInteraction = SpriteMaskInteraction.None;
            psr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            psr.receiveShadows = true;
            psr.shadowBias = 0;
            psr.motionVectorGenerationMode = MotionVectorGenerationMode.Object;
            psr.sortingLayerID = LayerIndex.defaultLayer.intVal;
            psr.sortingOrder = 0;
            psr.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.BlendProbes;
            psr.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Simple;
            psr.probeAnchor = null;


            var ind = this.AW_charModel.baseParticleSystemInfos.Length;
            Array.Resize( ref this.AW_charModel.baseParticleSystemInfos, ind + 1 );
            this.AW_charModel.baseParticleSystemInfos[ind] = new CharacterModel.ParticleSystemInfo( ps );

            // TODO: Actually set up the particle system

            var psMain = ps.main;
            psMain.duration = 1f;
            psMain.loop = true;
            psMain.prewarm = false;
            psMain.startDelay = 0f;
            psMain.startLifetime = new ParticleSystem.MinMaxCurve( 0.35f, 0.6f );
            psMain.startSpeed = new ParticleSystem.MinMaxCurve( 0.2f, 1f );
            psMain.startSize3D = false;
            psMain.startSize = 1f;
            psMain.startRotation3D = false;
            psMain.startRotation = new ParticleSystem.MinMaxCurve( 0f, 360f );
            psMain.flipRotation = 0.5f;
            psMain.startColor = Color.white;
            psMain.gravityModifier = -1f;
            psMain.simulationSpace = ParticleSystemSimulationSpace.World;
            psMain.simulationSpeed = 1f;
            psMain.useUnscaledTime = false;
            psMain.scalingMode = ParticleSystemScalingMode.Local;
            psMain.playOnAwake = true;
            psMain.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Transform;
            psMain.maxParticles = 100000;
            psMain.stopAction = ParticleSystemStopAction.None;
            psMain.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;
            psMain.ringBufferMode = ParticleSystemRingBufferMode.Disabled;

            var psEmis = ps.emission;
            psEmis.enabled = true;
            psEmis.rateOverTime = 1000f;
            psEmis.rateOverDistance = 0f;

            var psShape = ps.shape;
            psShape.enabled = true;
            psShape.shapeType = ParticleSystemShapeType.SkinnedMeshRenderer;
            psShape.skinnedMeshRenderer = model.GetComponent<SkinnedMeshRenderer>();
            psShape.meshShapeType = ParticleSystemMeshShapeType.Vertex;
            psShape.meshSpawnMode = ParticleSystemShapeMultiModeValue.Random;
            psShape.meshSpawnSpread = 0.05f;
            psShape.normalOffset = -0.02f;
            psShape.useMeshColors = false;
            //psShape.meshSpawnSpeed = 100f;

            var psVOL = ps.velocityOverLifetime;
            psVOL.enabled = false;

            var pslimVOL = ps.limitVelocityOverLifetime;
            pslimVOL.enabled = false;

            var psInheritVel = ps.inheritVelocity;
            psInheritVel.enabled = true;
            psInheritVel.mode = ParticleSystemInheritVelocityMode.Current;
            psInheritVel.curve = new ParticleSystem.MinMaxCurve( 1f, new AnimationCurve
            {
                preWrapMode = WrapMode.Clamp,
                postWrapMode = WrapMode.Clamp,
                keys = new[]
                {
                    new Keyframe(0f, 1f, 0f, 0f ),
                    new Keyframe(0.65f, 0.85f, -0.15f, -0.5f ),
                    new Keyframe(1f, 0f, 0f, 0f ),
                }
            } );

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
            psSOL.size = new ParticleSystem.MinMaxCurve( 1.35f, new AnimationCurve
            {
                preWrapMode = WrapMode.Clamp,
                postWrapMode = WrapMode.Clamp,
                keys = new[]
                {
                    new Keyframe( 0f, 0.2f ),
                    new Keyframe( 0.47f, 0.51f ),
                    new Keyframe( 1f, 0.0025f )
                }
            } );

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
            psNoise.enabled = true;
            psNoise.frequency = 1;
            psNoise.scrollSpeed = 1f;
            psNoise.damping = false;
            psNoise.quality = ParticleSystemNoiseQuality.High;
            psNoise.positionAmount = 0.1f;

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




            return ps;
        }





        private void AW_DoIDRSSetup()
        {
            this.PopulateDisplayDict();

            var idrs = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();

            var itemDisplays = new List<ItemDisplayRuleSet.NamedRuleGroup>();
            var equipmentDisplays = new List<ItemDisplayRuleSet.NamedRuleGroup>();

            #region Equipment
            //Wings
            equipmentDisplays.Add( new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "Jetpack",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay("DisplayBugWings"),
                            childName = "ChestCannon2",
                            localPos = new Vector3( 0f, 0.126f, 0.177f ),
                            localAngles = new Vector3( -180f, 0f, 0f ),
                            localScale = new Vector3( 0.15f, 0.15f, 0.15f ),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            } );

            //Crowdfunder
            equipmentDisplays.Add( new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "GoldGat",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay("DisplayGoldGat"),
                            childName = "shoulder.r",
                            localPos = new Vector3( -0.452f, 0.146f, 0f ),
                            localAngles = new Vector3( -110f, 90f, 0f ),
                            localScale = new Vector3( 0.2f, 0.2f, 0.2f ),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            } );
            #endregion
            #region Affixes
            //AffixRed
            equipmentDisplays.Add( new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixRed",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay("DisplayEliteHorn"),
                            childName = "Head",
                            localPos = new Vector3( 0.111f, 0.404f, 0.045f ),
                            localAngles = new Vector3( 30f, 0f, -30f ),
                            localScale = new Vector3( 0.1f, 0.1f, 0.1f ),
                            limbMask = LimbFlags.None
                        },
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay("DisplayEliteHorn"),
                            childName = "Head",
                            localPos = new Vector3( -0.111f, 0.404f, 0.045f ),
                            localAngles = new Vector3( 30f, 0f, 30f ),
                            localScale = new Vector3( -0.1f, 0.1f, 0.1f ),
                            limbMask = LimbFlags.None
                        }
                    }
                }
            } );

            //AffixBlue
            equipmentDisplays.Add( new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixBlue",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay("DisplayEliteRhinoHorn"),
                            childName = "Head",
                            localPos = new Vector3( 0f, 0.355f, 0.096f ),
                            localAngles = new Vector3( -30f, 0f, 0f ),
                            localScale = new Vector3( 0.3f, 0.3f, 0.3f ),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            } );

            //AffixWhite
            equipmentDisplays.Add( new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixWhite",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay("DisplayEliteIceCrown"),
                            childName = "Head",
                            localPos = new Vector3( 0f, 0.529f, 0f ),
                            localAngles = new Vector3( -90f, 0f, 0f ),
                            localScale = new Vector3( 0.03f, 0.03f, 0.03f ),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            } );

            //AffixPoison
            equipmentDisplays.Add( new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixPoison",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay("DisplayEliteUrchinCrown"),
                            childName = "Head",
                            localPos = new Vector3( 0f, 0.541f, 0.046f ),
                            localAngles = new Vector3( -90f, 0f, 0f ),
                            localScale = new Vector3( 0.05f, 0.05f, 0.05f ),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            } );

            //AffixHaunted
            equipmentDisplays.Add( new ItemDisplayRuleSet.NamedRuleGroup
            {
                name = "AffixHaunted",
                displayRuleGroup = new DisplayRuleGroup
                {
                    rules = new ItemDisplayRule[]
                    {
                        new ItemDisplayRule
                        {
                            ruleType = ItemDisplayRuleType.ParentedPrefab,
                            followerPrefab = LoadDisplay("DisplayEliteStealthCrown"),
                            childName = "Head",
                            localPos = new Vector3( 0f, 0.552f, 0.045f ),
                            localAngles = new Vector3( -90f, 0f, 0f ),
                            localScale = new Vector3( 0.06f, 0.06f, 0.06f ),
                            limbMask = LimbFlags.None
                        },
                    }
                }
            } );
            #endregion
            #region Items

            #endregion


            var allFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;

            var itemArray = itemDisplays.ToArray();
            var equipArray = equipmentDisplays.ToArray();

            typeof( ItemDisplayRuleSet ).GetField( "namedItemRuleGroups", allFlags ).SetValue( idrs, itemArray );
            typeof( ItemDisplayRuleSet ).GetField( "namedEquipmentRuleGroups", allFlags ).SetValue( idrs, equipArray );


            this.AW_charModel.itemDisplayRuleSet = idrs;
        }

    }
}
#endif