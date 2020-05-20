#if ANCIENTWISP
using System;
using BepInEx.Configuration;
using UnityEngine;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        private GameObject AW_body;
        private GameObject AW_master;
        //private GameObject AW_primaryProj;
        private GameObject AW_secDelayEffect;
        private GameObject AW_secExplodeEffect;
        private GameObject AW_utilProj;
        private GameObject AW_utilZoneProj;

        private ConfigEntry<Boolean> AW_lowPerf;

        partial void AW_General();
        partial void AW_Model();
        partial void AW_Director();
        partial void AW_CreateEffects();
        partial void AW_Orbs();
        partial void AW_CreateProjectiles();
        partial void AW_SetupSkills();
        partial void AW_SetupAI();
        partial void AW_Hook();

        partial void CreateAncientWisp()
        {
            this.AW_lowPerf = base.Config.Bind<Boolean>( "Graphics", "AncientWispQualityReduction", false, "Should the quality of the effects on ancient wisp be reduced? Try enabling this if you get lag when it spawns" );

            this.AW_General();
            this.AW_Model();
            this.AW_Director();
            this.AW_CreateEffects();
            this.AW_Orbs();
            this.AW_CreateProjectiles();
            this.AW_SetupSkills();
            this.AW_SetupAI();
            this.AW_Hook();
        }
    }
}
#endif
