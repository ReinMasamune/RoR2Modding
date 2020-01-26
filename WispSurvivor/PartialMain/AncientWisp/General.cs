#if ANCIENTWISP
using R2API;
using RoR2;
using UnityEngine;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void AW_General()
        {
            this.Load += this.AW_GetBody;
            this.Load += this.AW_GetMaster;
            this.Load += this.AW_Register;
        }

        private void AW_Register()
        {
            BodyCatalog.getAdditionalEntries += ( list ) => list.Add( this.AW_body );
            MasterCatalog.getAdditionalEntries += ( list ) => list.Add( this.AW_master );

            var def = new SurvivorDef
            {
                bodyPrefab = this.AW_body,
                descriptionToken = "AAA",
                displayPrefab = this.AW_body.GetComponent<ModelLocator>().modelBaseTransform.gameObject,
                primaryColor = Color.white,
                unlockableName = ""
            };
            SurvivorAPI.AddSurvivor( def );

            this.AW_body.GetComponent<CharacterBody>().preferredInitialStateType = this.AW_body.GetComponent<EntityStateMachine>().initialStateType;



        }
        private void AW_GetMaster()
        {
            this.AW_master = Resources.Load<GameObject>( "Prefabs/CharacterMasters/AncientWispMaster" ).InstantiateClone( "WispBossMaster" );
            var charMaster = this.AW_master.GetComponent<CharacterMaster>();
            charMaster.bodyPrefab = this.AW_body;
        }
        private void AW_GetBody()
        {
            this.AW_body = Resources.Load<GameObject>( "Prefabs/CharacterBodies/AncientWispBody" ).InstantiateClone( "WispBossBody" );
        }
    }
}
#endif