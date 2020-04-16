//using System;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using BepInEx;
//using ReinCore;
//using RoR2;
//using RoR2.Skills;
//using UnityEngine;

//namespace Rein.AlternateArtificer
//{
//    internal partial class Main
//    {
//        partial void Skills()
//        {
//            base.awake += this.Main_awake8;
//        }

//        private void Main_awake8()
//        {
//            artiSkillLocator.passiveSkill.enabled = false;

//            var primary = artiSkillLocator.primary;
//            var secondary = artiSkillLocator.secondary;
//            var utility = artiSkillLocator.utility;
//            var special = artiSkillLocator.special;

//            var passiveFamily = ScriptableObject.CreateInstance<SkillFamily>();
//            var primaryFamily = primary.skillFamily;
//            var secondaryFamily = secondary.skillFamily;
//            var utilityFamily = utility.skillFamily;
//            var specialFamily = special.skillFamily;

//            var passive = primary;
//            primary = secondary;
//            secondary = utility;
//            utility = special;
//            special = this.artiBodyPrefab.AddComponent<GenericSkill>();

//            artiSkillLocator.primary = primary;
//            artiSkillLocator.secondary = secondary;
//            artiSkillLocator.utility = utility;
//            artiSkillLocator.special = special;

//            var envSuit = ScriptableObject.CreateInstance<PassiveSkillDef>();
//            var elementalIntensity = ScriptableObject.CreateInstance<PassiveSkillDef>();

//            envSuit.skillNameToken = artiSkillLocator.passiveSkill.skillNameToken;
//            envSuit.skillDescriptionToken = artiSkillLocator.passiveSkill.skillDescriptionToken;
//            envSuit.icon = artiSkillLocator.passiveSkill.icon;

//            envSuit.stateMachineDefaults = new PassiveSkillDef.StateMachineDefaults[1]
//            {
//                new PassiveSkillDef.StateMachineDefaults
//                {
//                    machineName = "Body",
//                    initalState = new EntityStates.SerializableEntityStateType( typeof( EntityStates.Mage.MageCharacterMain ) ),
//                    mainState = new EntityStates.SerializableEntityStateType( typeof( EntityStates.Mage.MageCharacterMain ) ),
//                    defaultInitalState = new EntityStates.SerializableEntityStateType( typeof( EntityStates.GenericCharacterMain ) ),
//                    defaultMainState = new EntityStates.SerializableEntityStateType( typeof( EntityStates.GenericCharacterMain ) )

//                }
//            };

//            elementalIntensity.skillNameToken = "REIN_ALTARTI_PASSIVE_NAME";
//            elementalIntensity.skillDescriptionToken = "REIN_ALTARTI_PASSIVE_DESC";

//            elementalIntensity.stateMachineDefaults = new PassiveSkillDef.StateMachineDefaults[1]
//            {
//                new PassiveSkillDef.StateMachineDefaults
//                {
//                    machineName = "Jet",
//                    initalState = new EntityStates.SerializableEntityStateType( typeof( States.Main.AltArtiPassive ) ),
//                    mainState = new EntityStates.SerializableEntityStateType( typeof( States.Main.AltArtiPassive ) ),
//                    defaultInitalState = new EntityStates.SerializableEntityStateType( typeof( EntityStates.Idle ) ),
//                    defaultMainState = new EntityStates.SerializableEntityStateType( typeof( EntityStates.Idle ) )
//                }
//            };


//            elementalIntensity.applyVisuals = ( model ) =>
//            {
//                var renderInfos = model.baseRendererInfos;
//                if( renderInfos.Length == 10 )
//                {
//                    ((SkinnedMeshRenderer)model.baseRendererInfos[9].renderer).sharedMesh = this.artiChangedMesh;
//                    model.baseRendererInfos[2].renderer.gameObject.SetActive( false );
//                    model.baseRendererInfos[3].renderer.gameObject.SetActive( false );
//                    model.baseRendererInfos[5].renderer.gameObject.SetActive( false );
//                    model.baseRendererInfos[6].renderer.gameObject.SetActive( false );
//                } else
//                {
//                    ((SkinnedMeshRenderer)model.baseRendererInfos[3].renderer).sharedMesh = this.artiChangedMesh;
//                    model.baseRendererInfos[0].renderer.gameObject.SetActive( false );
//                    model.baseRendererInfos[1].renderer.gameObject.SetActive( false );
//                }

//            };

//            elementalIntensity.removeVisuals = ( model ) =>
//            {
//                var renderInfos = model.baseRendererInfos;
//                if( renderInfos.Length == 10 )
//                {
//                    try
//                    {
//                        ((SkinnedMeshRenderer)model.baseRendererInfos[9].renderer).sharedMesh = this.artiDefaultMesh;
//                    } catch { }
//                    model.baseRendererInfos[2].renderer.gameObject.SetActive( true );
//                    model.baseRendererInfos[3].renderer.gameObject.SetActive( true );
//                    model.baseRendererInfos[5].renderer.gameObject.SetActive( true );
//                    model.baseRendererInfos[6].renderer.gameObject.SetActive( true );
//                } else
//                {
//                    ((SkinnedMeshRenderer)model.baseRendererInfos[3].renderer).sharedMesh = this.artiDefaultMesh;
//                    model.baseRendererInfos[0].renderer.gameObject.SetActive( true );
//                    model.baseRendererInfos[1].renderer.gameObject.SetActive( true );
//                }
//            };

//            passiveFamily.variants = new SkillFamily.Variant[2]
//            {
//                new SkillFamily.Variant
//                {
//                    skillDef = envSuit,
//                    unlockableName = "",
//                    viewableNode = new ViewablesCatalog.Node( "envSuit" , false )
//                },
//                new SkillFamily.Variant
//                {
//                    skillDef = elementalIntensity,
//                    unlockableName = "",
//                    viewableNode = new ViewablesCatalog.Node( "elementalIntensity" , false )
//                }
//            };

//            SkillsCore.AddSkillDef( envSuit );
//            SkillsCore.AddSkillDef( elementalIntensity );
//            SkillsCore.AddSkillFamily( passiveFamily );

//            // TODO: Switch to func in new ver skillsCore
//            passive.SetFieldValue<SkillFamily>( "_skillFamily", passiveFamily );
//            primary.SetFieldValue<SkillFamily>( "_skillFamily", primaryFamily );
//            secondary.SetFieldValue<SkillFamily>( "_skillFamily", secondaryFamily );
//            utility.SetFieldValue<SkillFamily>( "_skillFamily", utilityFamily );
//            special.SetFieldValue<SkillFamily>( "_skillFamily", specialFamily );
//        }
//    }
//}
