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
        private Material blinkMaterial1;
        private Material blinkMaterial2;

        partial void Materials()
        {
            this.Load += this.GetBodyMaterial;
            this.Load += this.GetPistolMaterial;
            this.Load += this.GetRifleMaterial;
            this.Load += this.GetBlinkMaterial1;
            this.Load += this.GetBlinkMaterial2;
        }

        private void GetBlinkMaterial2()
        {
            this.blinkMaterial2 = Instantiate<Material>( Resources.Load<Material>( "Materials/matHuntressFlashExpanded" ) );
        }

        private void GetBlinkMaterial1()
        {
            this.blinkMaterial1 = Instantiate<Material>( Resources.Load<Material>( "Materials/matHuntressFlashBright" ) );
        }

        private void GetRifleMaterial()
        {
            var mat = UnityEngine.Object.Instantiate<Material>( this.sniperBody.GetComponent<ModelLocator>().modelTransform.GetComponent<ModelSkinController>().skins[1].rendererInfos[1].defaultMaterial );
            this.sniperRifleMaterial = mat;
        }
        private void GetPistolMaterial()
        {
            var mat = UnityEngine.Object.Instantiate<Material>( this.sniperBody.GetComponent<ModelLocator>().modelTransform.GetComponent<ModelSkinController>().skins[1].rendererInfos[0].defaultMaterial );
            this.sniperPistolMaterial = mat;
        }
        private void GetBodyMaterial()
        {
            var mat = UnityEngine.Object.Instantiate<Material>( this.sniperBody.GetComponent<ModelLocator>().modelTransform.GetComponent<ModelSkinController>().skins[1].rendererInfos[2].defaultMaterial );
            this.sniperBodyMaterial = mat;
        }
    }
}


