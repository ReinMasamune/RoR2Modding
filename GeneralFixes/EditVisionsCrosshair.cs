using BepInEx;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Navigation;
using R2API;
using R2API.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Reflection;
using EntityStates;
using RoR2.Skills;

namespace ReinGeneralFixes
{
    internal partial class Main
    {
        partial void EditVisionsCrosshair()
        {
            this.Load += this.GetNewCrosshairPrefab;
            this.Enable += this.AddFixVisionsCrosshair;
            this.Disable += this.RemoveFixVisionsCrosshair;
        }

        private void GetNewCrosshairPrefab()
        {
            this.newVisionsCrosshair = Resources.Load<GameObject>( "Prefabs/CharacterBodies/CommandoBody" ).GetComponent<CharacterBody>().crosshairPrefab.InstantiateClone( "VisionsCrosshair", false );
        }

        private void RemoveFixVisionsCrosshair()
        {
            On.RoR2.Skills.LunarPrimaryReplacementSkill.OnAssigned -= this.LunarPrimaryReplacementSkill_OnAssigned;
            On.RoR2.Skills.LunarPrimaryReplacementSkill.OnUnassigned -= this.LunarPrimaryReplacementSkill_OnUnassigned;
        }
        private void AddFixVisionsCrosshair()
        {
            On.RoR2.Skills.LunarPrimaryReplacementSkill.OnAssigned += this.LunarPrimaryReplacementSkill_OnAssigned;
            On.RoR2.Skills.LunarPrimaryReplacementSkill.OnUnassigned += this.LunarPrimaryReplacementSkill_OnUnassigned;
        }

        private GameObject newVisionsCrosshair;
        private Dictionary<SkillDef.BaseSkillInstanceData, GameObject> origCrosshairLookup = new Dictionary<SkillDef.BaseSkillInstanceData, GameObject>();
        private void LunarPrimaryReplacementSkill_OnUnassigned( On.RoR2.Skills.LunarPrimaryReplacementSkill.orig_OnUnassigned orig, LunarPrimaryReplacementSkill self, GenericSkill skillSlot )
        {
            orig( self, skillSlot );

            base.Logger.LogWarning( "Vision unassign" );
            if( this.origCrosshairLookup.ContainsKey( skillSlot.skillInstanceData ) )
            {
                skillSlot.characterBody.crosshairPrefab = this.origCrosshairLookup[skillSlot.skillInstanceData];
            }
        }
        private SkillDef.BaseSkillInstanceData LunarPrimaryReplacementSkill_OnAssigned( On.RoR2.Skills.LunarPrimaryReplacementSkill.orig_OnAssigned orig, LunarPrimaryReplacementSkill self, GenericSkill skillSlot )
        {
            var ret = orig( self, skillSlot );

            var crosshair = skillSlot.characterBody.crosshairPrefab;
            base.Logger.LogWarning( "Vision assign" );
            if( crosshair.name == "simpledotcrosshair" )
            {
                base.Logger.LogWarning( "Vision assign + crosshair" );
                this.origCrosshairLookup[skillSlot.skillInstanceData] = crosshair;
                skillSlot.characterBody.crosshairPrefab = this.newVisionsCrosshair;
            }
            return ret;
        }
    }
}
