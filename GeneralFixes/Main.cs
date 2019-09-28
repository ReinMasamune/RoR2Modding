using BepInEx;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Navigation;
using R2API;
using R2API.Utils;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Reflection;
using EntityStates;

namespace ReinGeneralFixes
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.ReinGeneralBugfixes", "ReinGeneralBugfixes", "1.0.0")]
    public class ReinGeneralFixesMain : BaseUnityPlugin
    {
        Dictionary<int, CharacterBody> dict1 = new Dictionary<int, CharacterBody>();
        Dictionary<int, CharacterBody> dict2 = new Dictionary<int, CharacterBody>();

        public void Awake()
        {
            IL.RoR2.GlobalEventManager.OnHitEnemy += (il) =>
            {
                ILCursor c = new ILCursor(il);
                c.GotoNext(MoveType.Before,
                    x => x.MatchLdfld<DamageInfo>("damageType"),
                    x => x.MatchLdcI4(0x80),
                    x => x.MatchAnd(),
                    x => x.MatchLdcI4(0)
                );
                c.RemoveRange(33);
                c.Emit(OpCodes.Ldarg_2);
                c.EmitDelegate<Action<DamageInfo,GameObject>>( (di,vic) =>
                {
                    
                    if( (di.damageType & DamageType.IgniteOnHit ) > DamageType.Generic )
                    {
                        DotController.InflictDot(vic, di.attacker, DotController.DotIndex.Burn, 4f * di.procCoefficient, 1f);
                    }

                    if ((di.damageType & DamageType.PercentIgniteOnHit) > DamageType.Generic)
                    {
                        DotController.InflictDot(vic, di.attacker, DotController.DotIndex.PercentBurn, 4f * di.procCoefficient, 1f);
                    }

                    if (di.attacker.GetComponent<CharacterBody>().HasBuff(BuffIndex.AffixRed) )
                    {
                        DotController.InflictDot(vic, di.attacker, DotController.DotIndex.PercentBurn, 4f * di.procCoefficient, 1f);
                    }
                    
                });
            };

            On.RoR2.Inventory.CopyItemsFrom += (orig, self, copyFrom) =>
            {
                self.CopyEquipmentFrom(copyFrom);
                orig(self, copyFrom);
            };

            On.EntityStates.EngiTurret.EngiTurretWeapon.FireBeam.OnEnter += (orig, self) =>
            {
                orig(self);
                var prop = self.GetType().GetProperty("characterBody", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                dict2.Add(self.GetFieldValue<Transform>("modelTransform").GetInstanceID(), (CharacterBody)prop.GetValue(self));
            };

            On.EntityStates.EngiTurret.EngiTurretWeapon.FireBeam.FixedUpdate += (orig, self) =>
            {
                float timer = self.GetFieldValue<float>("fireTimer");
                float attackSpeedBonus = dict2[self.GetFieldValue<Transform>("modelTransform").GetInstanceID()].attackSpeed - 1.0f;
                timer += Time.fixedDeltaTime * attackSpeedBonus;
                self.SetFieldValue<float>("fireTimer", timer);
                orig(self);
            };

            On.EntityStates.EngiTurret.EngiTurretWeapon.FireBeam.OnExit += (orig, self) =>
            {
                dict2.Remove(self.GetFieldValue<Transform>("modelTransform").GetInstanceID());
                orig(self);
            };

            On.EntityStates.Mage.Weapon.Flamethrower.OnEnter += (orig, self) =>
            {
                orig(self);
                var prop = self.GetType().GetProperty("characterBody", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                dict1.Add(self.GetFieldValue<ChildLocator>("childLocator").GetInstanceID(), (CharacterBody)prop.GetValue(self));
            };

            On.EntityStates.Mage.Weapon.Flamethrower.FixedUpdate += (orig, self) =>
            {
                bool doThings = self.GetFieldValue<bool>("hasBegunFlamethrower");
                if( doThings )
                {
                    float attackSpeedBonus = dict1[self.GetFieldValue<ChildLocator>("childLocator").GetInstanceID()].attackSpeed - 1.0f;
                    float timer = self.GetFieldValue<float>("flamethrowerStopwatch");
                    timer += Time.fixedDeltaTime * attackSpeedBonus;
                    self.SetFieldValue<float>("flamethrowerStopwatch", timer);
                }
                orig(self);
            };

            On.EntityStates.Mage.Weapon.Flamethrower.OnExit += (orig, self) =>
            {
                dict1.Remove(self.GetFieldValue<ChildLocator>("childLocator").GetInstanceID());
                orig(self);
            };

            //On.EntityStates.Commando.CommandoWeapon.FireGrenade.OnEnter += (orig, self) =>
            //{
            //    orig(self);
            //};

            /*
            On.RoR2.CharacterBody.RecalculateStats += (orig, self) =>
            {
                float level = self.level;
                int hoofCount = 0;
                int drinkCount = 0;
                int whipCount = 0;
                int tonicAffCount = 0;
                if( self.inventory )
                {
                    hoofCount = self.inventory.GetItemCount(ItemIndex.Hoof);
                    drinkCount = self.inventory.GetItemCount(ItemIndex.SprintBonus);
                    whipCount = self.inventory.GetItemCount(ItemIndex.SprintOutOfCombat);
                    tonicAffCount = self.inventory.GetItemCount(ItemIndex.TonicAffliction);
                    level += self.inventory.GetItemCount(ItemIndex.LevelBonus);
                }
            };
            */

            On.RoR2.HealthComponent.RepeatHealComponent.AddReserve += (orig, self, amount, max) =>
            {
                max *= 1000f;
                orig(self, amount, max);
            };
        }
    }
}