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
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.ReinWickedRing", "ReinWickedRing", "1.0.0")]
    public class ReinWickedRingMain : BaseUnityPlugin
    {
        public void Awake()
        {
            //On.RoR2.GenericSkill.OnExecute += (orig, self) =>
            //{
            //    orig(self);
            //};

            On.RoR2.GlobalEventManager.OnHitEnemy += (orig, self, damageInfo, victim) =>
            {
                orig(self, damageInfo, victim);
                if ( damageInfo.attacker && damageInfo.crit )
                {
                    CharacterBody atBody = damageInfo.attacker.GetComponent<CharacterBody>();
                    if( atBody )
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

            };
        }

        public void Start()
        {
            ItemCatalog.lunarItemList.Add(ItemIndex.CooldownOnCrit);
            typeof(ItemCatalog).GetFieldValue<ItemDef[]>("itemDefs")[(int)ItemIndex.CooldownOnCrit].tier = ItemTier.Lunar;
            R2API.ItemDropAPI.AddToDefaultByTier(ItemTier.Lunar, ItemIndex.CooldownOnCrit);
        }
    }
}
