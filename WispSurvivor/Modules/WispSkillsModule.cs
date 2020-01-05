
/*
namespace RogueWispPlugin.Modules
{
    public static class WispSkillsModule
    {
        public static void DoModule( GameObject body, Dictionary<Type, Component> dic, AssetBundle bundle )
        {
            RegisterStates();
            SkillLocator SL = SetupGenericSkills(body, dic);
            SkillFamily[] SF = SetupSkillFamilies(body, dic, SL);
            DoStatemachines( body, dic );
            DoPassiveStuff( body, SL, bundle );
            DoPrimaries( body, dic, SL, SF, bundle );
            DoSecondaries( body, dic, SL, SF, bundle );
            DoUtilities( body, dic, SL, SF, bundle );
            DoSpecials( body, dic, SL, SF, bundle );
        }

        private static void RegisterStates()
        {
            AddSkill( typeof( Skills.Primary.PrepHeatwave ) );
            AddSkill( typeof( Skills.Primary.FireHeatwave ) );
            AddSkill( typeof( Skills.Primary.HeatwaveWindDown ) );
            AddSkill( typeof( Skills.Primary.Heatwave ) );
            AddSkill( typeof( Skills.Secondary.TestSecondary ) );
            AddSkill( typeof( Skills.Utility.PrepGaze ) );
            AddSkill( typeof( Skills.Utility.FireGaze ) );
            AddSkill( typeof( Skills.Special.IncinerationWindup ) );
            AddSkill( typeof( Skills.Special.Incineration ) );
            AddSkill( typeof( Skills.Special.IncinerationRecovery ) );
        }

        private static SkillLocator SetupGenericSkills( GameObject body, Dictionary<Type, Component> dic )
        {
            foreach( GenericSkill g in body.GetComponents<GenericSkill>() )
            {
                MonoBehaviour.DestroyImmediate( g );
            }

            SkillLocator SL = body.AddOrGetComponent<SkillLocator>();
            if( !SL.primary )
            {
                SL.primary = body.AddComponent<GenericSkill>();
            }
            if( !SL.secondary )
            {
                SL.secondary = body.AddComponent<GenericSkill>();
            }
            if( !SL.utility )
            {
                SL.utility = body.AddComponent<GenericSkill>();
            }
            if( !SL.special )
            {
                SL.special = body.AddComponent<GenericSkill>();
            }

            return SL;
        }

        private static SkillFamily[] SetupSkillFamilies( GameObject body, Dictionary<Type, Component> dic, SkillLocator SL )
        {
            SkillFamily[] skillFams = new SkillFamily[4];
            skillFams[0] = GetNewSkillFamily( SL.primary );
            skillFams[1] = GetNewSkillFamily( SL.secondary );
            skillFams[2] = GetNewSkillFamily( SL.utility );
            skillFams[3] = GetNewSkillFamily( SL.special );

            return skillFams;
        }

        private static void DoStatemachines( GameObject body, Dictionary<Type, Component> dic )
        {
            NetworkStateMachine net = dic.C<NetworkStateMachine>();
            CharacterDeathBehavior death = dic.C<CharacterDeathBehavior>();
            death.idleStateMachine = new EntityStateMachine[2];
            death.deathState = new EntityStates.SerializableEntityStateType( typeof( EntityStates.Commando.DeathState ) );

            EntityStateMachine[] netStates = net.GetFieldValue<EntityStateMachine[]>("stateMachines");
            Array.Resize<EntityStateMachine>( ref netStates, 3 );

            SetStateOnHurt hurtState = dic.C<SetStateOnHurt>();
            hurtState.canBeFrozen = true;
            hurtState.canBeHitStunned = false;
            hurtState.canBeStunned = false;
            hurtState.hitThreshold = 5f;
            hurtState.hurtState = new SerializableEntityStateType( typeof( EntityStates.FrozenState ) );

            foreach( EntityStateMachine esm in body.GetComponents<EntityStateMachine>() )
            {
                switch( esm.customName )
                {
                    case "Body":
                        esm.initialStateType = new SerializableEntityStateType( typeof( SpawnTeleporterState ) );
                        esm.mainStateType = new SerializableEntityStateType( typeof( GenericCharacterMain ) );
                        netStates[0] = esm;
                        hurtState.targetStateMachine = esm;
                        death.deathStateMachine = esm;
                        break;

                    case "Weapon":
                        esm.initialStateType = new SerializableEntityStateType( typeof( Idle ) );
                        esm.mainStateType = new SerializableEntityStateType( typeof( Idle ) );
                        netStates[1] = esm;
                        death.idleStateMachine[0] = esm;
                        break;

                    case "Gaze":
                        esm.initialStateType = new SerializableEntityStateType( typeof( Idle ) );
                        esm.mainStateType = new SerializableEntityStateType( typeof( Idle ) );
                        netStates[2] = esm;
                        death.idleStateMachine[1] = esm;
                        break;

                    default:
                        Debug.Log( "Wisp has an extra statemachine" );
                        break;
                }
            }

            net.SetFieldValue<EntityStateMachine[]>( "stateMachines", netStates );
        }

        private static void DoPassiveStuff( GameObject body, SkillLocator sl, AssetBundle bundle )
        {
            sl.passiveSkill.enabled = true;
            sl.passiveSkill.icon = bundle.LoadAsset<Sprite>( "Assets/__EXPORT/WispyPassiveIcon2.png" );
            sl.passiveSkill.skillNameToken = "WISP_SURVIVOR_PASSIVE_NAME";
            sl.passiveSkill.skillDescriptionToken = "WISP_SURVIVOR_PASSIVE_DESC";
        }

        private static void DoPrimaries( GameObject body, Dictionary<Type, Component> dic, SkillLocator SL, SkillFamily[] fam, AssetBundle bundle )
        {
            SkillDef[] primaries = new SkillDef[1];
            primaries[0] = DoPrimary1( body, dic, bundle );

            AssignVariants( fam[0], primaries );
        }

        private static SkillDef DoPrimary1( GameObject body, Dictionary<Type, Component> dic, AssetBundle bundle )
        {
            SteppedSkillDef skill = ScriptableObject.CreateInstance<SteppedSkillDef>();
            skill.activationState = new SerializableEntityStateType( typeof( Skills.Primary.Heatwave ) );
            skill.activationStateMachineName = "Weapon";

            skill.baseMaxStock = 3;
            skill.baseRechargeInterval = 2.5f;
            skill.beginSkillCooldownOnSkillEnd = true;
            skill.canceledFromSprinting = false;
            skill.fullRestockOnAssign = true;
            skill.interruptPriority = InterruptPriority.Any;
            skill.isBullets = true;
            skill.isCombatSkill = true;
            skill.mustKeyPress = false;
            skill.noSprint = true;
            skill.rechargeStock = 3;
            skill.requiredStock = 0;
            skill.shootDelay = 0.5f;
            skill.stockToConsume = 0;
            skill.stepCount = 2;

            skill.icon = bundle.LoadAsset<Sprite>( "Assets/__EXPORT/wisp1.png" );
            skill.skillDescriptionToken = "WISP_SURVIVOR_PRIMARY_1_DESC";
            skill.skillName = "Primry1";
            skill.skillNameToken = "WISP_SURVIVOR_PRIMARY_1_NAME";

            return skill;
        }

        private static void DoSecondaries( GameObject body, Dictionary<Type, Component> dic, SkillLocator SL, SkillFamily[] fam, AssetBundle bundle )
        {
            SkillDef[] secondaries = new SkillDef[1];
            secondaries[0] = DoSecondary1( body, dic, bundle );

            AssignVariants( fam[1], secondaries );
        }

        private static SkillDef DoSecondary1( GameObject body, Dictionary<Type, Component> dic, AssetBundle bundle )
        {
            SkillDef skill = ScriptableObject.CreateInstance<SkillDef>();
            skill.activationState = new SerializableEntityStateType( typeof( Skills.Secondary.TestSecondary ) );
            skill.activationStateMachineName = "Weapon";

            skill.baseMaxStock = 1;
            skill.baseRechargeInterval = 6f;
            skill.beginSkillCooldownOnSkillEnd = true;
            skill.canceledFromSprinting = false;
            skill.fullRestockOnAssign = true;
            skill.interruptPriority = InterruptPriority.Skill;
            skill.isBullets = false;
            skill.isCombatSkill = true;
            skill.mustKeyPress = true;
            skill.noSprint = true;
            skill.rechargeStock = 1;
            skill.requiredStock = 1;
            skill.shootDelay = 0.5f;
            skill.stockToConsume = 1;

            skill.icon = bundle.LoadAsset<Sprite>( "Assets/__EXPORT/wisp2alt.png" );
            skill.skillDescriptionToken = "WISP_SURVIVOR_SECONDARY_1_DESC";
            skill.skillName = "Secondary1";
            skill.skillNameToken = "WISP_SURVIVOR_SECONDARY_1_NAME";

            return skill;
        }



        private static void DoUtilities( GameObject body, Dictionary<Type, Component> dic, SkillLocator SL, SkillFamily[] fam, AssetBundle bundle )
        {
            SkillDef[] utilities = new SkillDef[1];
            utilities[0] = DoUtility1( body, dic, bundle );

            AssignVariants( fam[2], utilities );
        }

        private static SkillDef DoUtility1( GameObject body, Dictionary<Type, Component> dic, AssetBundle bundle )
        {
            SkillDef skill = ScriptableObject.CreateInstance<SkillDef>();
            skill.activationState = new SerializableEntityStateType( typeof( Skills.Utility.PrepGaze ) );
            skill.activationStateMachineName = "Gaze";

            skill.baseMaxStock = 1;
            skill.baseRechargeInterval = 14.99999f;
            skill.beginSkillCooldownOnSkillEnd = true;
            skill.canceledFromSprinting = false;
            skill.fullRestockOnAssign = true;
            skill.interruptPriority = InterruptPriority.Skill;
            skill.isBullets = false;
            skill.isCombatSkill = true;
            skill.mustKeyPress = true;
            skill.noSprint = false;
            skill.rechargeStock = 1;
            skill.requiredStock = 1;
            skill.shootDelay = 0.5f;
            skill.stockToConsume = 1;

            skill.icon = bundle.LoadAsset<Sprite>( "Assets/__EXPORT/wisp3alt.png" );
            skill.skillDescriptionToken = "WISP_SURVIVOR_UTILITY_1_DESC";
            skill.skillName = "Utility1";
            skill.skillNameToken = "WISP_SURVIVOR_UTILITY_1_NAME";

            return skill;
        }



        private static void DoSpecials( GameObject body, Dictionary<Type, Component> dic, SkillLocator SL, SkillFamily[] fam, AssetBundle bundle )
        {
            SkillDef[] specials = new SkillDef[1];
            specials[0] = DoSpecial1( body, dic, bundle );

            AssignVariants( fam[3], specials );
        }

        private static SkillDef DoSpecial1( GameObject body, Dictionary<Type, Component> dic, AssetBundle bundle )
        {
            SkillDef skill = ScriptableObject.CreateInstance<SkillDef>();
            skill.activationState = new SerializableEntityStateType( typeof( Skills.Special.IncinerationWindup ) );
            skill.activationStateMachineName = "Weapon";

            skill.baseMaxStock = 1;
            skill.baseRechargeInterval = 6f;
            skill.beginSkillCooldownOnSkillEnd = true;
            skill.canceledFromSprinting = false;
            skill.fullRestockOnAssign = true;
            skill.interruptPriority = InterruptPriority.PrioritySkill;
            skill.isBullets = false;
            skill.isCombatSkill = true;
            skill.mustKeyPress = true;
            skill.noSprint = true;
            skill.rechargeStock = 1;
            skill.requiredStock = 1;
            skill.shootDelay = 0.5f;
            skill.stockToConsume = 1;

            skill.icon = bundle.LoadAsset<Sprite>( "Assets/__EXPORT/wisp4alt.png" );
            skill.skillDescriptionToken = "WISP_SURVIVOR_SPECIAL_1_DESC";
            skill.skillName = "Special1";
            skill.skillNameToken = "WISP_SURVIVOR_SPECIAL_1_NAME";

            return skill;
        }



        private static void ExFunction( GameObject body, Dictionary<Type, Component> dic )
        {

        }

        private static T C<T>( this Dictionary<Type, Component> dic ) where T : Component => dic[typeof( T )] as T;

        private static SkillFamily GetNewSkillFamily( GenericSkill s )
        {
            //if( !s.skillFamily )
            //{
            s.SetFieldValue<SkillFamily>( "_skillFamily", ScriptableObject.CreateInstance<SkillFamily>() );
            //}
            s.skillFamily.variants = new SkillFamily.Variant[0];

            AddSkillFamily( s.skillFamily );

            return s.skillFamily;
        }

        private static void AssignVariants( SkillFamily fam, SkillDef[] skills )
        {
            SkillFamily.Variant[] variants = new SkillFamily.Variant[skills.Length];

            for( Int32 i = 0; i < skills.Length; i++ )
            {
                variants[i] = new SkillFamily.Variant
                {
                    skillDef = skills[i],
                    unlockableName = "",
                    viewableNode = new ViewablesCatalog.Node( skills[i].skillNameToken, false )
                };
                AddSkillDef( skills[i] );
            }

            fam.variants = variants;
        }
    }
}
*/