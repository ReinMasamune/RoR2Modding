namespace ReinGeneralFixes
{
    using BepInEx;
    using RoR2;
    using UnityEngine;
    using System.Collections.Generic;
    using RoR2.Navigation;
    using Mono.Cecil.Cil;
    using MonoMod.Cil;
    using System;
    using System.Reflection;
    using EntityStates;
    using RoR2.Skills;
    using System.Collections;
    using ReinCore;

    internal partial class Main
    {
        partial void BalanceEngiTurrets()
        {
            this.Enable += this.Main_Enable8;
            this.Disable += this.Main_Disable8;
        }

        private GameObject walkerBody;
        private GameObject walkerMaster;
        private GameObject defaultBody;
        private GameObject defaultMaster;
        private GenericSkill walkerPrimary;
        private GenericSkill defaultPrimary;
        private SkillFamily walkerFamily;
        private SkillFamily defaultFamily;
        private void Main_Disable8()
        {

        }
        private void Main_Enable8()
        {
            this.walkerBody = Resources.Load<GameObject>( "Prefabs/CharacterBodies/EngiWalkerTurretBody" );
            this.walkerMaster = Resources.Load<GameObject>( "Prefabs/CharacterMasters/EngiWalkerTurretMaster" );

            this.defaultBody = Resources.Load<GameObject>( "Prefabs/CharacterBodies/EngiTurretBody" );
            this.defaultMaster = Resources.Load<GameObject>( "Prefabs/CharacterMasters/EngiTurretMaster" );

            this.walkerPrimary = this.walkerBody.GetComponent<SkillLocator>().primary;
            this.defaultPrimary = this.defaultBody.GetComponent<SkillLocator>().primary;

            this.walkerFamily = this.walkerPrimary.skillFamily;
            this.defaultFamily = this.defaultPrimary.skillFamily;

            this.walkerPrimary.SetSkillFamily( this.defaultFamily );
            this.defaultPrimary.SetSkillFamily( this.walkerFamily );

        }
    }
}
