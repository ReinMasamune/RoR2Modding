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
using BepInEx.Configuration;

namespace ReinGeneralFixes
{
    internal partial class Main
    {
        private ConfigEntry<Boolean> kinCharacterPerformance;

        partial void PerformanceKinCharController()
        {
            this.kinCharacterPerformance = base.Config.Bind<Boolean>( "Performance:", "Movement performance", true, "Experimental multithreaded version for character movement" );

            if( this.kinCharacterPerformance.Value )
            {
                this.Enable += this.Main_Enable1;
                this.Disable += this.Main_Disable1;
            }
        }

        private void Main_Disable1()
        {
            //On.KinematicCharacterController.KinematicCharacterMotor.CharacterCollisionsOverlap -= this.KinematicCharacterMotor_CharacterCollisionsOverlap;
        }
        private void Main_Enable1()
        {
            //On.KinematicCharacterController.KinematicCharacterMotor.CharacterCollisionsOverlap += this.KinematicCharacterMotor_CharacterCollisionsOverlap;
        }

    }
}
