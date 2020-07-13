using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ReinWickedRing
{
    [R2APISubmoduleDependency(
        nameof( R2API.AssetPlus.AssetPlus ),
        nameof( ItemAPI ),
        nameof( ResourcesAPI )
    )]
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.Rein.ReinWickedRing", "ReinWickedRing", "1.2.1")]
    public class ReinWickedRingMain : BaseUnityPlugin
    {
        private HashSet<Int32> blacklistedBodies = new HashSet<Int32>();
        private AssetBundle bundle;
        public void Awake()
        {
            Assembly execAssembly = Assembly.GetExecutingAssembly();
            System.IO.Stream stream = execAssembly.GetManifestResourceStream( "ReinLunarWickedRing.Bundle.wickedring" );
            this.bundle = AssetBundle.LoadFromStream( stream );

            ResourcesAPI.AddProvider( new AssetBundleResourcesProvider( "@ReinWickedRing", this.bundle ) );

            On.RoR2.ItemCatalog.DefineItems += ( orig ) =>
            {
                orig();

                ItemCatalog.lunarItemList.Add( ItemIndex.CooldownOnCrit );
                var def = ItemCatalog.GetItemDef( ItemIndex.CooldownOnCrit );
                def.canRemove = true;
                def.descriptionToken = "WICKEDRING_LOG_DESC";
                def.hidden = false;
                def.pickupIconPath = "@ReinWickedRing:Assets/AssetBundle/WickedRingIcon.png";
                def.pickupToken = "WICKEDRING_PICKUP_DESC";
                def.tier = ItemTier.Tier3;
                def.tags = new ItemTag[]
                {
                    //ItemTag.Cleansable,
                    ItemTag.Damage,
                    ItemTag.Utility
                };
                def.unlockableName = "";
            };

            //On.RoR2.CharacterBody.RecalculateStats += (orig, self) =>
            //{
            //    orig(self);
            //    float startCrit = self.crit;
            //    //Debug.Log(startCrit);
            //    if( self.inventory && self.inventory.GetItemCount(ItemIndex.CooldownOnCrit) > 0 )
            //    {
            //        startCrit -= 5f;
            //        startCrit -= (self.baseCrit + self.levelCrit * (self.level - 1f));
            //    }
            //    //Debug.Log(startCrit);
            //    self.SetPropertyValue<float>("crit" , startCrit);
            //    //Debug.Log(self.crit);
            //};


            //On.RoR2.GlobalEventManager.OnHitEnemy += (orig, self, damageInfo, victim) =>
            //{
            //    orig(self, damageInfo, victim);
            //    if ( damageInfo.attacker && damageInfo.crit )
            //    {
            //        CharacterBody atBody = damageInfo.attacker.GetComponent<CharacterBody>();
            //        if( atBody )
            //        {
            //            if( !this.blacklistedBodies.Contains(atBody.bodyIndex ) )
            //            {
            //                CharacterMaster master = atBody.master;
            //                if (master)
            //                {
            //                    Inventory inv = master.inventory;
            //                    if (inv)
            //                    {
            //                        int count = inv.GetItemCount(ItemIndex.CooldownOnCrit);
            //                        if (count > 0 && damageInfo.crit && damageInfo.procCoefficient > 0f)
            //                        {
            //                            float val1 = damageInfo.procCoefficient * damageInfo.damage / atBody.damage * Mathf.Pow(2f, (float)(inv.GetItemCount(ItemIndex.LunarDagger) + 1f));
            //                            float val2 = 0.9f * (1f - Mathf.Pow(1.15f, -1f * Mathf.Sqrt(val1) * (float)count));
            //                            var hc = atBody.healthComponent;

            //                            DamageInfo recoil = new DamageInfo();
            //                            recoil.damage = ( hc.health + hc.shield + hc.barrier ) * val2;
            //                            recoil.position = atBody.corePosition;
            //                            recoil.force = Vector3.zero;
            //                            recoil.damageColorIndex = DamageColorIndex.Default;
            //                            recoil.crit = false;
            //                            recoil.attacker = null;
            //                            recoil.inflictor = null;
            //                            recoil.damageType = DamageType.BypassArmor | DamageType.NonLethal;
            //                            recoil.procCoefficient = 0f;
            //                            recoil.procChainMask = default;
            //                            hc.TakeDamage(recoil);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }

            //};

            //IL.RoR2.HealthComponent.TakeDamage += ( il ) =>
            //{
            //    ILCursor c = new ILCursor( il );

            //    c.GotoNext( MoveType.After,
            //        x => x.MatchLdcI4( 90 ),
            //        x => x.MatchCallOrCallvirt<RoR2.Inventory>( "GetItemCount" )
            //    );
            //    c.Emit( OpCodes.Ldarg_1 );
            //    c.EmitDelegate<Func<Int32, DamageInfo, Int32>>( ( count, damage ) =>
            //    {
            //        if( ( damage.damageType & DamageType.NonLethal ) > DamageType.Generic )
            //        {
            //            return 0;
            //        } else
            //        {
            //            return count;
            //        }
            //    } );

            //    c.GotoNext( MoveType.After, x => x.MatchCallOrCallvirt<RoR2.CharacterBody>( "RollCrit" ) );
            //    c.Index -= 3;
            //    c.RemoveRange( 3 );
            //    c.Emit( OpCodes.Ldc_I4_0 );
            //};

            R2API.AssetPlus.Languages.AddToken( "WICKEDRING_PICKUP_DESC", "<style=cIsDamage>Critical Strikes</style> <style=cIsUtility>reduce all cooldowns</style>" );// +
                                                                                                                                                                       //"<style=cIsHealth>at the cost of health.</style>" );
            R2API.AssetPlus.Languages.AddToken( "WICKEDRING_LOG_DESC", "On <style=cIsDamage>Critical Strike</style> <style=cIsUtility>reduce all cooldowns</style> " +
                "by <style=cIsUtility>1s</style><style=cStack>(+1s per stack)</style>" );// <style=cIsHealth>but, lose a percentage of current health</style> based on " +
                //"the damage of the attack." );
        }

        public void Start()
        {
            this.blacklistedBodies.Add( BodyCatalog.FindBodyIndex( "EngiTurretBody" ) );
            this.blacklistedBodies.Add( BodyCatalog.FindBodyIndex( "EngiWalkerTurretBody" ) );
            this.blacklistedBodies.Add( BodyCatalog.FindBodyIndex( "EngiBeamTurretBody" ) );

        }
    }
}
