using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using UnityEngine;

namespace Sniper.Modules
{
    internal static class CatalogModule
    {
        internal static void RegisterSurvivor()
        {
            if( !SurvivorsCore.loaded )
            {
                Log.Error( "Cannot add survivor" );
                return;
            }
            var survivorDef = new SurvivorDef
            {
                bodyPrefab = SniperMain.sniperBodyPrefab,
                descriptionToken = Properties.Tokens.SNIPER_DESC,
                displayPrefab = SniperMain.sniperDisplayPrefab,
                name = "Sniper",
                primaryColor = new Color( 1f, 1f, 1f, 1f ),
                unlockableName = "",
            };

            Log.Message( "Queued add of Sniper to survivor catalog" );
            SurvivorCatalog.getAdditionalSurvivorDefs += ( list ) =>
            {
                list.Add( survivorDef );
                Log.Message( "Sniper added to SurvivorCatalog" );
            };
        }

        internal static void RegisterBody()
        {
            BodyCatalog.getAdditionalEntries += ( list ) => list.Add( SniperMain.sniperBodyPrefab );
        }

        internal static void RegisterMaster()
        {

        }
    }

}
