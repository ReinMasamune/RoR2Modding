namespace Rein.Sniper.Modules
{
    using System;

    using ReinCore;

    using RoR2;

    using Rein.Sniper.Components;
    using Rein.Sniper.Enums;
    using Rein.Sniper.SkillDefs;

    using UnityEngine;
    using UnityEngine.Networking;

    internal static class CatalogModule
    {
        internal static void RegisterSurvivor()
        {
            if(!SurvivorsCore.loaded)
            {
                Log.Fatal("Cannot add survivor");
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
                outroFlavorToken = Properties.Tokens.SNIPER_OUTRO_FLAVOR,
                displayNameToken = Properties.Tokens.SNIPER_DISPLAY_NAME,
            };
            SurvivorsCore.AddEclipseUnlocks("Rein_Sniper", survivorDef);

            SurvivorCatalog.getAdditionalSurvivorDefs += (list) => list.Add(survivorDef);
        }

        internal static Lazy<Int32> sniperBodyIndex = new Lazy<Int32>( () => BodyCatalog.FindBodyIndex( SniperMain.sniperBodyPrefab.GetComponent<CharacterBody>() ) );
        internal static void RegisterBody() => BodyCatalog.getAdditionalEntries += (list) => list.Add(SniperMain.sniperBodyPrefab);

        internal static void RegisterMaster()
        {

        }

        internal static DotController.DotIndex plasmaBurnIndex { get; private set; }
        internal static DotController.DotIndex critPlasmaBurnIndex { get; private set; }
        internal static void RegisterDoTType()
        {
            //plasmaBurnIndex = DoTsCore.AddDotType(new DoTDef
            //{
            //    associatedBuff = BuffIndex.Blight,
            //    damageCoefficient = 1f,
            //    interval = 0.35f,
            //    damageColorIndex = plasmaDamageColor,
            //}, true, DoTDamage);

            //critPlasmaBurnIndex = DoTsCore.AddDotType(new DoTDef
            //{
            //    associatedBuff = BuffIndex.Blight,
            //    damageCoefficient = 1f,
            //    interval = 0.35f,
            //    damageColorIndex = plasmaDamageColor
            //}, true, CritDoTDamage);
        }


        private static void DoTDamage(HealthComponent health, DamageInfo damage)
        {
            damage.procCoefficient = 0.5f;
            GlobalEventManager.instance.OnHitEnemy(damage, health.gameObject);
            GlobalEventManager.instance.OnHitAll(damage, health.gameObject);
            health.TakeDamage(damage);
        }

        private static void CritDoTDamage(HealthComponent health, DamageInfo damage)
        {
            damage.procCoefficient = 0.5f;
            damage.crit = true;

            GlobalEventManager.instance.OnHitEnemy(damage, health.gameObject);
            GlobalEventManager.instance.OnHitAll(damage, health.gameObject);
            health.TakeDamage(damage);
        }

        private static readonly Lazy<DamageColorIndex> _plasmaDamageColor = new Lazy<DamageColorIndex>( () => DamageColorsCore.AddDamageColor( new Color( 0.9f, 0.5f, 0.9f ) ));
        internal static DamageColorIndex plasmaDamageColor { get => _plasmaDamageColor.Value; }


        internal static void RegisterDamageTypes()
        {
            sniperResetDamageType = DamageTypesCore.RegisterNewDamageType(DoNothing);
            GlobalEventManager.onServerDamageDealt += GlobalEventManager_onServerDamageDealt;
        }



        internal static DamageType sniperResetDamageType { get; private set; }

        private static void DoNothing() { }

        private static void GlobalEventManager_onServerDamageDealt(DamageReport obj)
        {
            if(obj.damageInfo.damageType.Flag(sniperResetDamageType))
            {
                obj.victimBody.AddTimedBuff(sniperResetDebuff.Value, 4f);
            }
        }



        internal static void RegisterBuffTypes()
        {
            BuffsCore.getAdditionalEntries += BuffsCore_getAdditionalEntries;
            GlobalEventManager.onCharacterDeathGlobal += GlobalEventManager_onCharacterDeathGlobal;
        }



        private static void BuffsCore_getAdditionalEntries(System.Collections.Generic.List<BuffDef> buffList)
        {
            // FUTURE: Add custom debuff for plasma dot
            buffList.Add(new BuffDef
            {
                buffColor = new Color(0.5f, 1f, 0.6f, 1f),
                canStack = false,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/texBuffFullCritIcon",
                isDebuff = true,
                name = "SniperResetOnKillDebuff"
            });
        }
        private static Lazy<BuffIndex> sniperResetDebuff = new Lazy<BuffIndex>( () => BuffCatalog.FindBuffIndex( "SniperResetOnKillDebuff" ));

        private static void GlobalEventManager_onCharacterDeathGlobal(DamageReport obj)
        {
            if(obj.victimBody.HasBuff(sniperResetDebuff.Value) && obj.attackerBodyIndex == sniperBodyIndex.Value && obj.attackerBody != null && obj.attackerBody is SniperCharacterBody body)
            {
                ResetSkills(body);

                //SkillLocator loc = obj?.attackerBody?.skillLocator;
                //if( loc is null ) return;


                //GenericSkill pri = loc.primary;
                //if( pri.skillInstanceData is SniperReloadableFireSkillDef.SniperPrimaryInstanceData primaryData )
                //{
                //    primaryData.ForceReload( ReloadTier.Perfect );
                //} else if( pri != null )
                //{
                //    pri.stock = Mathf.Max( Mathf.Min( pri.maxStock, pri.stock + 1 ), pri.stock );
                //    pri.rechargeStopwatch = pri.stock >= pri.maxStock ? 0f : pri.rechargeStopwatch;
                //}

                //GenericSkill sec = loc.secondary;
                //if( sec != null )
                //{
                //    sec.stock = Mathf.Max( Mathf.Min( sec.maxStock, sec.stock + 1 ), sec.stock );
                //    sec.rechargeStopwatch = sec.stock >= sec.maxStock ? 0f : sec.rechargeStopwatch;
                //}
                //GenericSkill util = loc.utility;
                //if( util != null )
                //{
                //    util.stock = Mathf.Max( Mathf.Min( util.maxStock, util.stock + 1 ), util.stock );
                //    util.rechargeStopwatch = util.stock >= util.maxStock ? 0f : util.rechargeStopwatch;
                //}
                //GenericSkill spec = loc.special;
                //if( spec != null )
                //{
                //    spec.stock = Mathf.Max( Mathf.Min( spec.maxStock, spec.stock + 1 ), spec.stock );
                //    spec.rechargeStopwatch = spec.stock >= spec.maxStock ? 0f : spec.rechargeStopwatch;
                //}
            }
        }

        internal static void ResetSkills(SniperCharacterBody body, Boolean canSend = true)
        {
            if(body is null || !body || body.networkIdentity is null || !body.networkIdentity)
            {
                return;
            }

            if(Util.HasEffectiveAuthority(body.networkIdentity))
            {
                #region Impl
                SkillLocator loc = body.skillLocator;
                if(loc is null) return;


                GenericSkill pri = loc.primary;
                if(pri.skillInstanceData is SniperReloadableFireSkillDef.SniperPrimaryInstanceData primaryData)
                {
                    primaryData.ForceReload(ReloadTier.Perfect);
                } else if(pri != null)
                {
                    pri.stock = Mathf.Max(Mathf.Min(pri.maxStock, pri.stock + 1), pri.stock);
                    pri.rechargeStopwatch = pri.stock >= pri.maxStock ? 0f : pri.rechargeStopwatch;
                }

                GenericSkill sec = loc.secondary;
                if(sec != null)
                {
                    sec.stock = Mathf.Max(Mathf.Min(sec.maxStock, sec.stock + 1), sec.stock);
                    sec.rechargeStopwatch = sec.stock >= sec.maxStock ? 0f : sec.rechargeStopwatch;
                }
                GenericSkill util = loc.utility;
                if(util != null)
                {
                    util.stock = Mathf.Max(Mathf.Min(util.maxStock, util.stock + 1), util.stock);
                    util.rechargeStopwatch = util.stock >= util.maxStock ? 0f : util.rechargeStopwatch;
                }
                GenericSkill spec = loc.special;
                if(spec != null)
                {
                    spec.stock = Mathf.Max(Mathf.Min(spec.maxStock, spec.stock + 1), spec.stock);
                    spec.rechargeStopwatch = spec.stock >= spec.maxStock ? 0f : spec.rechargeStopwatch;
                }
                #endregion
            } else if(NetworkServer.active && canSend)
            {
                new NetworkModule.ResetSkillsMessage(body).Send(NetworkDestination.Clients);
            }
        }
    }

}
