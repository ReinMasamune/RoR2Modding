namespace Sniper.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using BepInEx.Logging;
    using ReinCore;
    using RoR2;
    using UnityEngine;

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

            SurvivorCatalog.getAdditionalSurvivorDefs += ( list ) => list.Add( survivorDef );
        }

        internal static void RegisterBody() => BodyCatalog.getAdditionalEntries += ( list ) => list.Add( SniperMain.sniperBodyPrefab );

        internal static void RegisterMaster()
        {

        }

        internal static DotController.DotIndex plasmaBurnIndex { get; private set; }
        internal static DotController.DotIndex critPlasmaBurnIndex { get; private set; }
        internal static void RegisterDoTType()
        {
            plasmaBurnIndex = DoTsCore.AddDotType( new DoTDef
            {
                associatedBuff = BuffIndex.Blight,
                damageCoefficient = 1f,
                interval = 0.35f,
                damageColorIndex = plasmaDamageColor,
            }, true, DoTDamage );

            critPlasmaBurnIndex = DoTsCore.AddDotType( new DoTDef
            {
                associatedBuff = BuffIndex.Blight,
                damageCoefficient = 1f,
                interval = 0.35f,
                damageColorIndex = plasmaDamageColor
            }, true, CritDoTDamage );
        }


        private static void DoTDamage( HealthComponent health, DamageInfo damage )
        {
            damage.procCoefficient = 0.5f;
            GlobalEventManager.instance.OnHitEnemy( damage, health.gameObject );
            GlobalEventManager.instance.OnHitAll( damage, health.gameObject );
            health.TakeDamage( damage );
        }

        private static void CritDoTDamage( HealthComponent health, DamageInfo damage )
        {
            damage.procCoefficient = 0.5f;
            damage.crit = true;

            GlobalEventManager.instance.OnHitEnemy( damage, health.gameObject );
            GlobalEventManager.instance.OnHitAll( damage, health.gameObject );
            health.TakeDamage( damage );
        }


#pragma warning disable IDE1006 // Naming Styles
        private static readonly Lazy<DamageColorIndex> _plasmaDamageColor = new Lazy<DamageColorIndex>( () => DamageColorsCore.AddDamageColor( new Color( 0.9f, 0.5f, 0.9f ) ));
#pragma warning restore IDE1006 // Naming Styles
        internal static DamageColorIndex plasmaDamageColor { get => _plasmaDamageColor.Value; }
    }

}
