namespace Scavangest
{
    using BepInEx;
    using RoR2;
    using R2API;
    using System;
    using UnityEngine;

    internal partial class Main
    {
        partial void ModelEdits()
        {
            this.Load += this.ScaleModel;
            this.Load += this.CreateSkins;
        }

        private void CreateSkins()
        {

        }
        private void ScaleModel()
        {
            var model = this.body.GetComponent<ModelLocator>().modelTransform;
            var modelBase = this.body.GetComponent<ModelLocator>().modelBaseTransform;

            modelBase.localScale = new Vector3( 0.2f, 0.2f, 0.2f );
        }
    }
}
