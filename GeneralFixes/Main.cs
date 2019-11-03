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
    [BepInPlugin("com.ReinThings.ReinGeneralBugfixes", "ReinGeneralBugfixes", "1.0.1")]
    public class ReinGeneralFixesMain : BaseUnityPlugin
    {
        public void Awake()
        {
            On.RoR2.Loadout.BodyLoadoutManager.BodyLoadout.EnforceUnlockables += (orig, self, prof) =>
            {
                int i = self.GetFieldValue<int>("bodyIndex");
                GameObject obj = BodyCatalog.GetBodyPrefab(i);
                if (obj)
                {
                    orig(self, prof);
                } else
                {
                    self.InvokeMethod("ResetSkin");
                    orig(self, prof);
                }
            };
        }


        /*
        Texture2D testTex;
        Texture2D bgTex;
        Rect posRect = new Rect(32f, 32f, 1524f, 96f);

        public void Awake()
        {
            bgTex = CreateBGTexture();

            Gradient grad = new Gradient();

            GradientAlphaKey[] aKeys = new GradientAlphaKey[2];
            aKeys[0] = new GradientAlphaKey(0.75f, 0f);
            aKeys[1] = new GradientAlphaKey(0f, 1f);

            GradientColorKey[] cKeys = new GradientColorKey[2];
            cKeys[0] = new GradientColorKey(new Color(0f, 0f, 0f), 1f);
            cKeys[1] = new GradientColorKey(new Color(1f, 0.690f, 0.906f), 0f);
            //cKeys[2] = new GradientColorKey(new Color(0.1f, 0.11f, 0.6f), 0.7f);
            //cKeys[3] = new GradientColorKey(new Color(0.19f, 0.19f, 0.85f), 0.63f);
            //cKeys[4] = new GradientColorKey(new Color(0.4f, 0.55f, 1.0f), 0.32f);
            //cKeys[5] = new GradientColorKey(new Color(0.48f, 0.69f, 1.0f), 0.25f);
            //cKeys[6] = new GradientColorKey(new Color(0.5f, 0.75f, 1.0f), 0f);

            grad.SetKeys(cKeys, aKeys);

            testTex = CreateNewRampTex(grad);
            
        }

        public void OnGUI()
        {
            GUI.DrawTexture(posRect, bgTex);
            GUI.DrawTexture(posRect, testTex);
        }

        private static Texture2D CreateNewRampTex(Gradient grad)
        {
            Texture2D tex = new Texture2D(256, 16, TextureFormat.ARGB32, false);

            Color tempC;
            Color[] tempCs = new Color[16];

            for (int i = 0; i < 256; i++)
            {
                tempC = grad.Evaluate( (255f - i) / 255f);
                for (int j = 0; j < 16; j++)
                {
                    tempCs[j] = tempC;
                }

                tex.SetPixels(i, 0, 1, 16, tempCs);
            }

            tex.Apply();
            return tex;
        }

        private Texture2D CreateBGTexture()
        {
            Texture2D tex = new Texture2D(256, 16, TextureFormat.ARGB32, false);

            bool swap = false;

            Color b = new Color(0f, 0f, 0f, 1f);
            Color w = new Color(1f, 1f, 1f, 1f);

            Color[] white = new Color[64];
            Color[] black = new Color[64];

            for( int i = 0; i < 64; i++ )
            {
                white[i] = w;
                black[i] = b;
            }

            for( int i =0; i<256; i+=8)
            {
                tex.SetPixels(i, 0, 8, 8, (swap ? white : black));
                tex.SetPixels(i, 8, 8, 8, (swap ? black : white));
                swap = !swap;
            }

            tex.Apply();
            return tex;
        }
        */
    }
}

//Old stuff
/*
        Dictionary<int, CharacterBody> dict1 = new Dictionary<int, CharacterBody>();
        Dictionary<int, CharacterBody> dict2 = new Dictionary<int, CharacterBody>();




On.RoR2.Loadout.BodyLoadoutManager.BodyLoadout.IsSkillVariantLocked += (orig, self, i, prof) =>
            {
                if (!self.InvokeMethod<bool>("IsSkillVariantValid", i))
                {
                    return true;
                }
                return orig(self, i, prof);
            };

            On.RoR2.Loadout.BodyLoadoutManager.BodyLoadout.IsSkinLocked += (orig, self, prof) =>
            {
                if (!self.InvokeMethod<bool>("IsSkinValid"))
                {
                    return true;
                }
                return orig(self, prof);
            };



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
dict1.Add(self.GetFieldValue<ChildLocator>("childLocator").GetInstanceID(), (CharacterBody) prop.GetValue(self));
            };

            On.EntityStates.Mage.Weapon.Flamethrower.FixedUpdate += (orig, self) =>
            {
                bool doThings = self.GetFieldValue<bool>("hasBegunFlamethrower");
                if(doThings )
                {
                    float attackSpeedBonus = dict1[self.GetFieldValue<ChildLocator>("childLocator").GetInstanceID()].attackSpeed - 1.0f;
float timer = self.GetFieldValue<float>("flamethrowerStopwatch");
timer += Time.fixedDeltaTime* attackSpeedBonus;
self.SetFieldValue<float>("flamethrowerStopwatch", timer);
                }
                orig(self);
            };

            On.EntityStates.Mage.Weapon.Flamethrower.OnExit += (orig, self) =>
            {
                dict1.Remove(self.GetFieldValue<ChildLocator>("childLocator").GetInstanceID());
                orig(self);
            };

            On.EntityStates.Commando.CommandoWeapon.FireGrenade.OnEnter += (orig, self) =>
            {
                orig(self);
            };

            
            On.RoR2.CharacterBody.RecalculateStats += (orig, self) =>
            {
                float level = self.level;
int hoofCount = 0;
int drinkCount = 0;
int whipCount = 0;
int tonicAffCount = 0;
                if(self.inventory )
                {
                    hoofCount = self.inventory.GetItemCount(ItemIndex.Hoof);
                    drinkCount = self.inventory.GetItemCount(ItemIndex.SprintBonus);
                    whipCount = self.inventory.GetItemCount(ItemIndex.SprintOutOfCombat);
                    tonicAffCount = self.inventory.GetItemCount(ItemIndex.TonicAffliction);
                    level += self.inventory.GetItemCount(ItemIndex.LevelBonus);
                }
            };
            
            
            On.RoR2.HealthComponent.RepeatHealComponent.AddReserve += (orig, self, amount, max) =>
            {
                max *= 1000f;
                orig(self, amount, max);
            };


            
        }
*/
