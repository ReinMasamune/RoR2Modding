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
                primaryColor = new Color( 0f, 0.3f, 0.1f, 1f ),
                unlockableName = "",
            };

            SurvivorCatalog.getAdditionalSurvivorDefs += ( list ) =>
            {
                list.Add( survivorDef );
            };
        }

        internal static void RegisterBody()
        {
            BodyCatalog.getAdditionalEntries += ( list ) => list.Add( SniperMain.sniperBodyPrefab );
        }

        internal static void RegisterMaster()
        {

        }

        internal static DotController.DotIndex plasmaBurnIndex { get; private set; }
        internal static DotController.DotIndex critPlasmaBurnIndex { get; private set; }
        internal static void RegisterDoTType()
        {
            plasmaBurnIndex = ReinCore.DoTsCore.AddDotType( new DoTDef
            {
                associatedBuff = BuffIndex.Blight,
                damageCoefficient = 0.2f,
                interval = 0.2f,
                damageColorIndex = DamageColorIndex.Heal,
            }, DoTDamage );

            critPlasmaBurnIndex = ReinCore.DoTsCore.AddDotType( new DoTDef
            {
                associatedBuff = BuffIndex.Blight,
                damageCoefficient = 0.2f,
                interval = 0.2f,
                damageColorIndex = DamageColorIndex.CritHeal
            }, CritDoTDamage );
        }


        private static void DoTDamage( HealthComponent health, DamageInfo damage )
        {
            damage.procCoefficient = 0.3f;

            GlobalEventManager.instance.OnHitEnemy( damage, health.gameObject );
            GlobalEventManager.instance.OnHitAll( damage, health.gameObject );
            health.TakeDamage( damage );
        }

        private static void CritDoTDamage( HealthComponent health, DamageInfo damage )
        {
            damage.procCoefficient = 0.3f;
            damage.crit = true;

            GlobalEventManager.instance.OnHitEnemy( damage, health.gameObject );
            GlobalEventManager.instance.OnHitAll( damage, health.gameObject );
            health.TakeDamage( damage );
        }
    }

}
