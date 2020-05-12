namespace AlternativeArtificer
{
    using System;
    using System.Collections.Generic;

    using AlternateArtificer.SelectablePassive;

    using R2API;
    using R2API.Utils;

    using Rein.Properties;

    using RoR2;
    using RoR2.Skills;

    using UnityEngine;

    public partial class Main
    {
        private readonly HashSet<SkillDef> addedSkills = new HashSet<SkillDef>();
        private readonly HashSet<SkillFamily> addedSkillFamilies = new HashSet<SkillFamily>();

        private void EditSkills()
        {
            this.artiSkillLocator.passiveSkill.enabled = false;

            this.SelectablePassive();
            this.SetupStateMachines();
            this.TempModSpecial();
        }

        private void SelectablePassive()
        {
            GenericSkill primary = this.artiSkillLocator.primary;
            GenericSkill secondary = this.artiSkillLocator.secondary;
            GenericSkill utility = this.artiSkillLocator.utility;
            GenericSkill special = this.artiSkillLocator.special;

            SkillFamily passiveFamily = ScriptableObject.CreateInstance<SkillFamily>();
            SkillFamily primaryFamily = primary.skillFamily;
            SkillFamily secondaryFamily = secondary.skillFamily;
            SkillFamily utilityFamily = utility.skillFamily;
            SkillFamily specialFamily = special.skillFamily;

            GenericSkill passive = primary;
            primary = secondary;
            secondary = utility;
            utility = special;
            special = this.artiBody.AddComponent<GenericSkill>();

            this.artiSkillLocator.primary = primary;
            this.artiSkillLocator.secondary = secondary;
            this.artiSkillLocator.utility = utility;
            this.artiSkillLocator.special = special;

            PassiveSkillDef envSuit = ScriptableObject.CreateInstance<PassiveSkillDef>();
            PassiveSkillDef elementalIntensity = ScriptableObject.CreateInstance<PassiveSkillDef>();

            envSuit.skillNameToken = this.artiSkillLocator.passiveSkill.skillNameToken;
            envSuit.skillDescriptionToken = this.artiSkillLocator.passiveSkill.skillDescriptionToken;
            envSuit.icon = this.artiSkillLocator.passiveSkill.icon;

            envSuit.stateMachineDefaults = new PassiveSkillDef.StateMachineDefaults[1]
            {
                new PassiveSkillDef.StateMachineDefaults
                {
                    machineName = "Body",
                    initalState = new EntityStates.SerializableEntityStateType( typeof( EntityStates.Mage.MageCharacterMain ) ),
                    mainState = new EntityStates.SerializableEntityStateType( typeof( EntityStates.Mage.MageCharacterMain ) ),
                    defaultInitalState = new EntityStates.SerializableEntityStateType( typeof( EntityStates.GenericCharacterMain ) ),
                    defaultMainState = new EntityStates.SerializableEntityStateType( typeof( EntityStates.GenericCharacterMain ) )

                }
            };

            elementalIntensity.skillNameToken = "REIN_ALTARTI_PASSIVE_NAME";
            elementalIntensity.skillDescriptionToken = "REIN_ALTARTI_PASSIVE_DESC";
            Texture2D tex = Tools.LoadTexture2D( Rein.Properties.Resources.passive_2__1_ );
            elementalIntensity.icon = Sprite.Create( tex, new Rect( 0f, 0f, tex.width, tex.height ), envSuit.icon.pivot );

            elementalIntensity.stateMachineDefaults = new PassiveSkillDef.StateMachineDefaults[1]
            {
                new PassiveSkillDef.StateMachineDefaults
                {
                    machineName = "Jet",
                    initalState = new EntityStates.SerializableEntityStateType( typeof( States.Main.AltArtiPassive ) ),
                    mainState = new EntityStates.SerializableEntityStateType( typeof( States.Main.AltArtiPassive ) ),
                    defaultInitalState = new EntityStates.SerializableEntityStateType( typeof( EntityStates.Idle ) ),
                    defaultMainState = new EntityStates.SerializableEntityStateType( typeof( EntityStates.Idle ) )
                }
            };


            elementalIntensity.applyVisuals = ( model ) =>
            {
                CharacterModel.RendererInfo[] renderInfos = model.baseRendererInfos;
                if( renderInfos.Length == 10 )
                {
                    ( (SkinnedMeshRenderer)model.baseRendererInfos[9].renderer ).sharedMesh = this.artiChangedMesh;
                    model.baseRendererInfos[2].renderer.gameObject.SetActive( false );
                    model.baseRendererInfos[3].renderer.gameObject.SetActive( false );
                    model.baseRendererInfos[5].renderer.gameObject.SetActive( false );
                    model.baseRendererInfos[6].renderer.gameObject.SetActive( false );
                } else
                {
                    ( (SkinnedMeshRenderer)model.baseRendererInfos[3].renderer ).sharedMesh = this.artiChangedMesh;
                    model.baseRendererInfos[0].renderer.gameObject.SetActive( false );
                    model.baseRendererInfos[1].renderer.gameObject.SetActive( false );
                }

            };

            elementalIntensity.removeVisuals = ( model ) =>
            {
                CharacterModel.RendererInfo[] renderInfos = model.baseRendererInfos;
                if( renderInfos.Length == 10 )
                {
                    try
                    {
                        ( (SkinnedMeshRenderer)model.baseRendererInfos[9].renderer ).sharedMesh = this.artiDefaultMesh;
                    } catch { }
                    model.baseRendererInfos[2].renderer.gameObject.SetActive( true );
                    model.baseRendererInfos[3].renderer.gameObject.SetActive( true );
                    model.baseRendererInfos[5].renderer.gameObject.SetActive( true );
                    model.baseRendererInfos[6].renderer.gameObject.SetActive( true );
                } else
                {
                    ( (SkinnedMeshRenderer)model.baseRendererInfos[3].renderer ).sharedMesh = this.artiDefaultMesh;
                    model.baseRendererInfos[0].renderer.gameObject.SetActive( true );
                    model.baseRendererInfos[1].renderer.gameObject.SetActive( true );
                }
            };

            passiveFamily.variants = new SkillFamily.Variant[2]
            {
                new SkillFamily.Variant
                {
                    skillDef = envSuit,
                    unlockableName = "",
                    viewableNode = new ViewablesCatalog.Node( "envSuit" , false )
                },
                new SkillFamily.Variant
                {
                    skillDef = elementalIntensity,
                    unlockableName = "",
                    viewableNode = new ViewablesCatalog.Node( "elementalIntensity" , false )
                }
            };

            _ = LoadoutAPI.AddSkillDef( envSuit );
            _ = LoadoutAPI.AddSkillDef( elementalIntensity );
            _ = LoadoutAPI.AddSkillFamily( passiveFamily );

            _ = this.addedSkills.Add( envSuit );
            _ = this.addedSkills.Add( elementalIntensity );
            _ = this.addedSkillFamilies.Add( passiveFamily );

            passive.SetFieldValue<SkillFamily>( "_skillFamily", passiveFamily );
            primary.SetFieldValue<SkillFamily>( "_skillFamily", primaryFamily );
            secondary.SetFieldValue<SkillFamily>( "_skillFamily", secondaryFamily );
            utility.SetFieldValue<SkillFamily>( "_skillFamily", utilityFamily );
            special.SetFieldValue<SkillFamily>( "_skillFamily", specialFamily );
        }




        private void RegisterSkillTypes()
        {
            _ = LoadoutAPI.AddSkill( typeof( States.Special.IonSurge ) );
            _ = LoadoutAPI.AddSkill( typeof( States.Main.AltArtiPassive ) );
        }

        private void SetupStateMachines()
        {
            SetStateOnHurt stateOnHurt = this.artiBody.GetComponent<SetStateOnHurt>();
            EntityStateMachine[] idles = stateOnHurt.idleStateMachine;
            Array.Resize<EntityStateMachine>( ref idles, 1 );
            stateOnHurt.idleStateMachine = idles;
        }






        private void TempModSpecial()
        {
            SkillFamily specialFamily = this.artiSkillLocator.special.skillFamily;

            SkillDef specialDef = specialFamily.variants[1].skillDef;
            specialDef.activationState = new EntityStates.SerializableEntityStateType( typeof( States.Special.IonSurge ) );

            specialDef.skillNameToken = "REIN_ALTARTI_LIGHTNING_SPECIAL_NAME";
            specialDef.skillDescriptionToken = "REIN_ALTARTI_LIGHTNING_SPECIAL_DESC";
        }
    }
}
