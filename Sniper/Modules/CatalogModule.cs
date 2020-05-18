namespace Sniper.Modules
{
    using System;

    using ReinCore;

    using RoR2;
    using Sniper.Enums;
    using Sniper.SkillDefs;
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

        private static readonly Lazy<DamageColorIndex> _plasmaDamageColor = new Lazy<DamageColorIndex>( () => DamageColorsCore.AddDamageColor( new Color( 0.9f, 0.5f, 0.9f ) ));
        internal static DamageColorIndex plasmaDamageColor { get => _plasmaDamageColor.Value; }


        internal static void RegisterDamageTypes()
        {
            sniperResetDamageType = DamageTypesCore.RegisterNewDamageType( DoNothing );
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
        }



        internal static DamageType sniperResetDamageType { get; private set; }

        private static void DoNothing() { }

        private static void GlobalEventManager_onServerDamageDealt( DamageReport obj )
        {
            if( obj.damageInfo.damageType.Flag( sniperResetDamageType ) )
            {
                obj.victimBody.AddTimedBuff( sniperResetDebuff.Value, 4f );
            }
        }
        


        internal static void RegisterBuffTypes()
        {
            BuffsCore.getAdditionalEntries += BuffsCore_getAdditionalEntries;
            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
        }



        private static void BuffsCore_getAdditionalEntries( System.Collections.Generic.List<BuffDef> buffList )
        {
            // TODO: Add custom debuff for plasma dot
            buffList.Add( new BuffDef
            {
                buffColor = new Color( 0.5f, 1f, 0.6f, 1f ),
                canStack = false,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/texBuffFullCritIcon",
                isDebuff = true,
                name = "SniperResetOnKillDebuff"
            } );
        }
        private static Lazy<BuffIndex> sniperResetDebuff = new Lazy<BuffIndex>( () => BuffCatalog.FindBuffIndex( "SniperResetOnKillDebuff" ));

        private static void GlobalEventManager_onCharacterDeathGlobal( DamageReport obj )
        {
            if( obj.victimBody.HasBuff( sniperResetDebuff.Value ) )
            {
                var loc = obj.attackerBody.skillLocator;
                var primaryData = loc.primary.skillInstanceData as SniperReloadableFireSkillDef.SniperPrimaryInstanceData;
                primaryData.ForceReload( ReloadTier.Perfect );
                var sec = loc.secondary;
                sec.stock = Mathf.Max( Mathf.Min( sec.maxStock, sec.stock + 1 ), sec.stock );
                sec.rechargeStopwatch = sec.stock >= sec.maxStock ? 0f : sec.rechargeStopwatch;
                var util = loc.utility;
                util.stock = Mathf.Max( Mathf.Min( util.maxStock, util.stock + 1 ), util.stock );
                util.rechargeStopwatch = util.stock >= util.maxStock ? 0f : util.rechargeStopwatch;
                var spec = loc.special;
                spec.stock = Mathf.Max( Mathf.Min( spec.maxStock, spec.stock + 1 ), spec.stock );
                spec.rechargeStopwatch = spec.stock >= spec.maxStock ? 0f : spec.rechargeStopwatch;
            }
        }
    }

}
