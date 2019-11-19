using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;

namespace ReinWickedRing
{
    [R2APISubmoduleDependency("ItemDropAPI")]
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.ReinWickedRing", "ReinWickedRing", "1.0.1")]
    public class ReinWickedRingMain : BaseUnityPlugin
    {
        public void Awake()
        {
            On.RoR2.CharacterBody.RecalculateStats += (orig, self) =>
            {
                orig(self);
                float startCrit = self.crit;
                //Debug.Log(startCrit);
                if( self.inventory && self.inventory.GetItemCount(ItemIndex.CooldownOnCrit) > 0 )
                {
                    startCrit -= 5f;
                    startCrit -= (self.baseCrit + self.levelCrit * (self.level - 1f));
                }
                //Debug.Log(startCrit);
                self.SetPropertyValue<float>("crit" , startCrit);
                //Debug.Log(self.crit);
            };


            On.RoR2.GlobalEventManager.OnHitEnemy += (orig, self, damageInfo, victim) =>
            {
                orig(self, damageInfo, victim);
                if ( damageInfo.attacker && damageInfo.crit )
                {
                    CharacterBody atBody = damageInfo.attacker.GetComponent<CharacterBody>();
                    if( atBody )
                    {
                        if( atBody.bodyIndex != 26 && atBody.bodyIndex != 27 )
                        {
                            CharacterMaster master = atBody.master;
                            if (master)
                            {
                                Inventory inv = master.inventory;
                                if (inv)
                                {
                                    int count = inv.GetItemCount(ItemIndex.CooldownOnCrit);
                                    if (count > 0 && damageInfo.crit && damageInfo.procCoefficient > 0f)
                                    {
                                        float val1 = damageInfo.damage / atBody.damage * Mathf.Pow(2f, (float)(inv.GetItemCount(ItemIndex.LunarDagger) + 1f));
                                        float val2 = damageInfo.procCoefficient * 0.9f * (1f - Mathf.Pow(1.15f, -1f * Mathf.Sqrt(val1) * (float)count));

                                        DamageInfo recoil = new DamageInfo();
                                        recoil.damage = atBody.healthComponent.combinedHealth * val2;
                                        recoil.position = atBody.corePosition;
                                        recoil.force = Vector3.zero;
                                        recoil.damageColorIndex = DamageColorIndex.Default;
                                        recoil.crit = false;
                                        recoil.attacker = null;
                                        recoil.inflictor = null;
                                        recoil.damageType = DamageType.BypassArmor;
                                        recoil.procCoefficient = 0f;
                                        recoil.procChainMask = default(ProcChainMask);
                                        atBody.healthComponent.TakeDamage(recoil);
                                    }
                                }
                            }
                        }
                    }
                }

            };
        }

        public void Start()
        {
            ItemCatalog.lunarItemList.Add(ItemIndex.CooldownOnCrit);
            typeof(ItemCatalog).GetFieldValue<ItemDef[]>("itemDefs")[(int)ItemIndex.CooldownOnCrit].tier = ItemTier.Lunar;
            R2API.ItemDropAPI.AddToDefaultByTier(ItemTier.Lunar, ItemIndex.CooldownOnCrit);

            ItemDef ringDef = typeof(ItemCatalog).GetFieldValue<ItemDef[]>("itemDefs")[(int)ItemIndex.CooldownOnCrit];
            ringDef.tier = ItemTier.Lunar;
            ringDef.pickupToken = "Reduce all cooldowns by on crit at the cost of health.";
            ringDef.descriptionToken = "Reduces all cooldowns by 1 (+1 per stack) second(s) on critical hit. Take a percentage of current hp as damage based on the damage of the hit (multiplied by stacks) divided by your base damage stat pm critical hit.";
        }
    }
}
