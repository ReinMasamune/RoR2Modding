namespace ReinGeneralFixes
{
    using System;
    using System.Collections;
    using System.Collections.Generic;

    using ReinCore;

    using RoR2;
    using RoR2.Skills;

    using UnityEngine;

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
        private readonly Dictionary<LunarPrimaryReplacementSkill, VisionsContextData> origCrosshairLookup = new Dictionary<LunarPrimaryReplacementSkill, VisionsContextData>();
        private readonly HashSet<String> badCrosshairs = new HashSet<String>();
        //private HashSet<String> ignoredCrosshairs = new HashSet<String>();

        private class VisionsContextData
        {
            public GameObject origCrosshair;
            public Coroutine checkCoroutine;
        }

        private void GetNewCrosshairPrefab() => this.newVisionsCrosshair = Resources.Load<GameObject>( "Prefabs/CharacterBodies/CommandoBody" ).GetComponent<CharacterBody>().crosshairPrefab.ClonePrefab( "VisionsCrosshair", false );

        private void PopulateBadCrosshairs() => _ = this.badCrosshairs.Add( "SimpleDotCrosshair" );

        private void RemoveFixVisionsCrosshair()
        {
            HooksCore.RoR2.Skills.LunarPrimaryReplacementSkill.OnAssigned.On -= this.OnAssigned_On1;
            HooksCore.RoR2.Skills.LunarPrimaryReplacementSkill.OnUnassigned.On -= this.OnUnassigned_On;
        }
        private void AddFixVisionsCrosshair()
        {
            HooksCore.RoR2.Skills.LunarPrimaryReplacementSkill.OnAssigned.On += this.OnAssigned_On1;
            HooksCore.RoR2.Skills.LunarPrimaryReplacementSkill.OnUnassigned.On += this.OnUnassigned_On;
        }

        private void OnUnassigned_On( HooksCore.RoR2.Skills.LunarPrimaryReplacementSkill.OnUnassigned.Orig orig, LunarPrimaryReplacementSkill self, GenericSkill skillSlot )
        {
            orig( self, skillSlot );

            if( this.origCrosshairLookup.ContainsKey( self ) )
            {
                VisionsContextData data = this.origCrosshairLookup[self];
                if( data.origCrosshair != null )
                {
                    GameObject crosshair = skillSlot.characterBody.crosshairPrefab;
                    if( crosshair.name == this.newVisionsCrosshair.name )
                    {
                        skillSlot.characterBody.crosshairPrefab = data.origCrosshair;
                    } else
                    {
                        _ = skillSlot.StartCoroutine( this.CheckCrosshairUnset( skillSlot.characterBody, data ) );
                    }
                }
                if( data.checkCoroutine != null )
                {
                    skillSlot.StopCoroutine( data.checkCoroutine );
                }
            }
        }

        private SkillDef.BaseSkillInstanceData OnAssigned_On1( HooksCore.RoR2.Skills.LunarPrimaryReplacementSkill.OnAssigned.Orig orig, LunarPrimaryReplacementSkill self, GenericSkill skillSlot )
        {
            SkillDef.BaseSkillInstanceData ret = orig( self, skillSlot );

            GameObject crosshair = skillSlot.characterBody.crosshairPrefab;
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
