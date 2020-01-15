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
        partial void General()
        {
            this.Load += this.GetAndRegisterBody;
            this.Load += this.AddComponentsToBody;
            this.Load += this.CreateCrosshairs;
        }

        private void CreateCrosshairs()
        {
            this.baseCrosshair = Resources.Load<GameObject>("Prefabs/Crosshair/CrocoCrosshair").InstantiateClone( "SniperCrosshair", false );
            this.scopeCrosshair = Resources.Load<GameObject>( "Prefabs/Crosshair/SniperCrosshair" ).InstantiateClone( "ScopeCrosshair", false );
        }

        private void AddComponentsToBody()
        {
            this.sniperBody.AddComponent<SniperUIController>();
        }

        private void GetAndRegisterBody()
        {
            this.sniperBody = Resources.Load<GameObject>( "Prefabs/CharacterBodies/CommandoBody" ).InstantiateClone( "sniperbody" );
            BodyCatalog.getAdditionalEntries += ( list ) => list.Add( this.sniperBody );

            var sniperDisplay = this.sniperBody.GetComponent<ModelLocator>().modelTransform.gameObject;

            var survivor = new SurvivorDef
            {
                bodyPrefab = this.sniperBody,
                descriptionToken = "SNIPER_BODY_DESC",
                name = "Sniper",
                displayPrefab = sniperDisplay,
                primaryColor = new Color( 0.4f, 0.4f, 0.4f ),
                unlockableName = ""
            };

            if( !R2API.SurvivorAPI.AddSurvivor( survivor ) ) this.borkedAF = true;
        }
    }
}


