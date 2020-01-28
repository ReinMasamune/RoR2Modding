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
using System.Collections;

namespace ReinGeneralFixes
{
    internal partial class Main
    {
        partial void QoLVisionsCrosshair()
        {
            this.Load += this.GetNewCrosshairPrefab;
            this.Load += this.PopulateBadCrosshairs;
            this.Enable += this.AddFixVisionsCrosshair;
            this.Disable += this.RemoveFixVisionsCrosshair;
        }



        private GameObject newVisionsCrosshair;
        private Dictionary<LunarPrimaryReplacementSkill, VisionsContextData> origCrosshairLookup = new Dictionary<LunarPrimaryReplacementSkill, VisionsContextData>();
        private HashSet<String> badCrosshairs = new HashSet<String>();
        //private HashSet<String> ignoredCrosshairs = new HashSet<String>();

        private class VisionsContextData
        {
            public GameObject origCrosshair;
            public Coroutine checkCoroutine;
        }

        private void GetNewCrosshairPrefab()
        {
            this.newVisionsCrosshair = Resources.Load<GameObject>( "Prefabs/CharacterBodies/CommandoBody" ).GetComponent<CharacterBody>().crosshairPrefab.InstantiateClone( "VisionsCrosshair", false );
        }

        private void PopulateBadCrosshairs()
        {
            this.badCrosshairs.Add( "SimpleDotCrosshair" );
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


        private void LunarPrimaryReplacementSkill_OnUnassigned( On.RoR2.Skills.LunarPrimaryReplacementSkill.orig_OnUnassigned orig, LunarPrimaryReplacementSkill self, GenericSkill skillSlot )
        {
            orig( self, skillSlot );

            if( this.origCrosshairLookup.ContainsKey( self ) )
            {
                var data = this.origCrosshairLookup[self];
                if( data.origCrosshair != null )
                {
                    var crosshair = skillSlot.characterBody.crosshairPrefab;
                    if( crosshair.name == this.newVisionsCrosshair.name )
                    {
                        skillSlot.characterBody.crosshairPrefab = data.origCrosshair;
                    } else
                    {
                        skillSlot.StartCoroutine( this.CheckCrosshairUnset( skillSlot.characterBody, data ) );
                    }
                }
                if( data.checkCoroutine != null )
                {
                    skillSlot.StopCoroutine( data.checkCoroutine );
                }
            }
        }
        private SkillDef.BaseSkillInstanceData LunarPrimaryReplacementSkill_OnAssigned( On.RoR2.Skills.LunarPrimaryReplacementSkill.orig_OnAssigned orig, LunarPrimaryReplacementSkill self, GenericSkill skillSlot )
        {
            var ret = orig( self, skillSlot );

            var crosshair = skillSlot.characterBody.crosshairPrefab;
            if( crosshair != null )
            {
                if( !this.origCrosshairLookup.ContainsKey( self ) || this.origCrosshairLookup[self] == null )
                {
                    this.origCrosshairLookup[self] = new VisionsContextData();
                }
                if( this.badCrosshairs.Contains( crosshair.name ) && skillSlot.characterBody.crosshairPrefab != this.newVisionsCrosshair )
                {
                    this.origCrosshairLookup[self].origCrosshair = crosshair;
                    skillSlot.characterBody.crosshairPrefab = this.newVisionsCrosshair;
                } else
                {
                    this.origCrosshairLookup[self].checkCoroutine = skillSlot.StartCoroutine( this.CheckCrosshairSet( skillSlot.characterBody, this.origCrosshairLookup[self] ) );
                }
            }
            return ret;
        }

        private IEnumerator CheckCrosshairSet( CharacterBody body, VisionsContextData data )
        {
            while( true )
            {
                yield return new WaitForSeconds( 0.5f );
                if( this.badCrosshairs.Contains( body.crosshairPrefab.name ) && body.crosshairPrefab.name != this.newVisionsCrosshair.name )
                {
                    data.origCrosshair = body.crosshairPrefab;
                    body.crosshairPrefab = this.newVisionsCrosshair;
                    data.checkCoroutine = null;
                    break;
                }
            }
        }

        private IEnumerator CheckCrosshairUnset( CharacterBody body, VisionsContextData data )
        {
            while( true )
            {
                yield return new WaitForSeconds( 0.5f );
                if( body.crosshairPrefab.name == this.newVisionsCrosshair.name )
                {
                    body.crosshairPrefab = data.origCrosshair;
                    data.origCrosshair = null;
                    break;
                }
            }
        }


    }
}
