using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using System;

namespace ReinSniperRework
{
    internal partial class Main
    {
        partial void CharacterBody()
        {
            this.Load += this.SetupCharacterBody;
        }

        private void SetupCharacterBody()
        {
            var charBody = this.sniperBody.GetComponent<CharacterBody>();
            charBody.baseNameToken = "SNIPER_BODY_NAME";

            charBody.crosshairPrefab = this.baseCrosshair;
        }
    }
}


