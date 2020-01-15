namespace Scavangest
{
    using BepInEx;
    using RoR2;
    using R2API;
    using System;
    using UnityEngine;

    internal partial class Main
    {
        partial void BodySetup()
        {
            this.Load += this.GetBody;
            this.Load += this.AddToSurvivorCatalog;
            this.Load += this.EditCharBody;
        }

        private void EditCharBody()
        {
            var charBody = this.body.GetComponent<CharacterBody>();
            charBody.preferredInitialStateType = this.body.GetComponent<EntityStateMachine>().initialStateType;
        }

        private void AddToSurvivorCatalog()
        {
            var def = new UnskinnableSurvivorDef
            {
                bodyPrefab = this.body,
                descriptionToken = "REIN_SCAV_DESC",
                displayPrefab = this.body.GetComponent<ModelLocator>().modelBaseTransform.gameObject,
                primaryColor = new Color( 0.4f, 0.4f, 0.4f, 1f ),
                unlockableName = ""
            };

            SurvivorAPI.AddSurvivor( def );
        }

        private void GetBody()
        {
            this.body = Resources.Load<GameObject>( "Prefabs/CharacterBodies/ScavBody" ).InstantiateClone( "Scavangest" );
            BodyCatalog.getAdditionalEntries += ( list ) => list.Add( this.body );
            this.body.tag = "Finish";
        }

        private class UnskinnableSurvivorDef : SurvivorDef
        {

        }
    }
}
