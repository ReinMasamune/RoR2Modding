namespace ReinGeneralFixes
{
    using System;
    using System.Collections.ObjectModel;
    using System.Reflection;
    using BF = System.Reflection.BindingFlags;

    using Mono.Cecil.Cil;

    using MonoMod.Cil;

    using ReinCore;

    using RoR2;

    using UnityEngine;
    using UnityEngine.Experimental.UIElements;
    using UnityObject = UnityEngine.Object;
    using System.Linq;
    using System.Collections.Generic;
    using BepInEx.Logging;
    using System.ComponentModel;
    using RoR2.Projectile;
    using EntityStates;

    internal partial class Main
    {
        private static Int32 crocoBodyIndex;
        private static GameObject sawmerang = Resources.Load<GameObject>("Prefabs/Projectiles/Sawmerang");
        private static GameObject acridBody = Resources.Load<GameObject>("Prefabs/CharacterBodies/CrocoBody");
        

        partial void FixDoTs()
        {
            this.Enable += this.Main_Enable7;
            this.FirstFrame += () => crocoBodyIndex = BodyCatalog.FindBodyIndex(Resources.Load<GameObject>("Prefabs/CharacterBodies/CrocoBody"));
            //this.Disable += this.Main_Disable7;
        }

        private unsafe void Main_Enable7()
        {
            OnHitManager.AddOnHit(Bleed_DamageType);
            OnHitManager.AddOnHit(Bleed_TriTip);
            OnHitManager.AddOnHit(Bleed_ImpItem);
            OnHitManager.AddOnHit(Blight_All);
            OnHitManager.AddOnHit(Blight_DamageType);
            OnHitManager.AddOnHit(Burn_DamageType1);
            OnHitManager.AddOnHit(Burn_DamageType2);
            OnHitManager.AddOnHit(Burn_AffixRed);

            sawmerang.GetComponent<ProjectileDamage>().damageType = DamageType.BleedOnHit;
            acridBody.GetComponent<CharacterBody>().baseJumpPower = 20f;
            //acridBody.AddOrGetComponent<CustomGravity>();
        }





        private static void Bleed_DamageType(DamageInfo damage, CharacterBody attacker, Inventory inv, CharacterBody victim)
        {
            if(damage.damageType.HasFlag(DamageType.BleedOnHit))
            {
                //LogW("Bleed damagetype");
                DotController.InflictDot(victim.gameObject, attacker.gameObject, DotController.DotIndex.Bleed, 3f * damage.procCoefficient, 1f);
            }
        }
        private static void Bleed_TriTip(DamageInfo damage, CharacterBody attacker, Inventory inv, CharacterBody victim)
        {
            if(inv is null) return;
            var count = inv.GetItemCount(ItemIndex.BleedOnHit);
            if(count <= 0) return;
            if(Util.CheckRoll(7f * count * Mathf.Sqrt(damage.procCoefficient), attacker.master))
            {
                //LogW("Bleed tritip");
                DotController.InflictDot(victim.gameObject, attacker.gameObject, DotController.DotIndex.Bleed, 3f * Mathf.Sqrt(damage.procCoefficient), 1f);
            }
        }
        private static void Bleed_ImpItem(DamageInfo damage, CharacterBody attacker, Inventory inv, CharacterBody victim)
        {
            if(damage.crit && inv && inv.GetItemCount(ItemIndex.BleedOnHitAndExplode) > 0)
            {
                //LogW("Bleed impitem");
                DotController.InflictDot(victim.gameObject, attacker.gameObject, DotController.DotIndex.Bleed, 3f * damage.procCoefficient, 1f);
            }
        }

        private static void Blight_All(DamageInfo damage, CharacterBody attacker, Inventory inv, CharacterBody victim)
        {
            if(damage.dotIndex != DotController.DotIndex.None) return;
            if(attacker.bodyIndex != crocoBodyIndex) return;
            if(!damage.inflictor || damage.inflictor.gameObject != attacker.gameObject) return;
            if(attacker.GetComponent<CrocoDamageTypeController>() is not CrocoDamageTypeController controller) return;
            if(controller.passiveSkillSlot.skillDef == controller.blightSkillDef)
            {
                ApplyBlight(damage, attacker, victim);
            }

        }
        private static void Blight_DamageType(DamageInfo damage, CharacterBody attacker, Inventory inv, CharacterBody victim)
        {
            if(damage.damageType.HasFlag(DamageType.BlightOnHit))
            {
                ApplyBlight(damage, attacker, victim, 1);
            }
        }
        private static void ApplyBlight(DamageInfo damage, CharacterBody attacker, CharacterBody victim, Int32 stacks = 1)
        {
            var duration = damage.procCoefficient * 4f;
            var controller = DotController.FindDotController(victim.gameObject);
            if(controller is not null)
            {
                foreach(var stack in controller.dotStackList)
                {
                    if(stack.dotIndex == DotController.DotIndex.Blight && stack.timer < duration) stack.timer = duration;
                }
            }
            for(Int32 i = 0; i < stacks; ++i)
            {
                DotController.InflictDot(victim.gameObject, attacker.gameObject, DotController.DotIndex.Blight, duration, 0.5f);
            }
        }

        private static void Burn_DamageType1(DamageInfo damage, CharacterBody attacker, Inventory inv, CharacterBody victim)
        {
            if(damage.damageType.HasFlag(DamageType.IgniteOnHit))
            {
                DotController.InflictDot(victim.gameObject, attacker.gameObject, DotController.DotIndex.Burn, damage.procCoefficient * 4f, 1f);
            }
        }
        private static void Burn_DamageType2(DamageInfo damage, CharacterBody attacker, Inventory inv, CharacterBody victim)
        {
            if(damage.damageType.HasFlag(DamageType.PercentIgniteOnHit))
            {
                DotController.InflictDot(victim.gameObject, attacker.gameObject, DotController.DotIndex.PercentBurn, damage.procCoefficient * 4f, 1f);
            }
        }
        private static void Burn_AffixRed(DamageInfo damage, CharacterBody attacker, Inventory inv, CharacterBody victim)
        {
            if(attacker.HasBuff(BuffIndex.AffixRed))
            {
                DotController.InflictDot(victim.gameObject, attacker.gameObject, DotController.DotIndex.PercentBurn, damage.procCoefficient * 4f, 1f);
            }
        }
    }

    internal static class OnHitManager
    {
        internal static Boolean Skip(Instruction _) => true;

        internal delegate void OnHitDelegate(DamageInfo damage, CharacterBody attacker, Inventory inventory, CharacterBody victim);
        internal unsafe static void AddOnHit(OnHitDelegate onHit)
        {
            onHits.Add(onHit.Method);

            if(applied) HooksCore.RoR2.GlobalEventManager.OnHitEnemy.Il -= OnHitEnemy_Il;
            HooksCore.RoR2.GlobalEventManager.OnHitEnemy.Il += OnHitEnemy_Il;
            applied = true;
        }
        //private static unsafe delegate*<DamageInfo, CharacterBody, Inventory, CharacterBody, void>[] onHits = new delegate*<DamageInfo, CharacterBody, Inventory, CharacterBody, void>[0];
        private static readonly List<MethodInfo> onHits = new List<MethodInfo>();


        private static Boolean applied = false;
        private static readonly FieldInfo procChainMask = typeof(DamageInfo).GetField("procChainMask", BF.Instance | BF.NonPublic | BF.Public);
        private static readonly MethodInfo hasProc = typeof(ProcChainMask).GetMethod("HasProc", BF.Instance | BF.NonPublic | BF.Public);
        private static readonly FieldInfo damageType = typeof(DamageInfo).GetField("damageType", BF.Instance | BF.NonPublic | BF.Public );
        private unsafe static ILCursor EmitOnHits(this ILCursor cursor)
        {
            foreach(var fn in onHits)
            {
                _ = cursor
                    .LdArg_(1)
                    .LdLoc_(0)
                    .LdLoc_(3)
                    .LdLoc_(1)
                    .Emit(OpCodes.Call, fn);
            }
            return cursor;
        }
        private static ILCursor LogIL(this ILCursor cursor)
        {
            Main.LogE(cursor.Context);
            return cursor;
        }
        private static ILCursor LogIL(this ILCursor cursor, String before)
        {
            Main.LogW(before);
            Main.LogE(cursor.Context);
            return cursor;
        }

        private static Int32 prevLoc = 0;
        private static void OnHitEnemy_Il(ILContext il) => new ILCursor(il)
            .DefLabel(out var skip)
            .GotoNext(MoveType.AfterLabel,
                x => x.MatchLdarg(1),
                x => x.MatchLdflda(procChainMask),
                x => x.MatchLdcI4((Int32)ProcType.BleedOnHit),
                x => x.MatchCallOrCallvirt(out _),
                x => x.MatchBrtrue(out skip)
            ).Br_(skip).GotoLabel(skip)
            .GotoNext(MoveType.AfterLabel,
                x => x.MatchLdarg(1),
                x => x.MatchLdfld(damageType),
                x => x.MatchLdcI4((Int32)DamageType.BlightOnHit),
                Skip,
                Skip,
                Skip,
                Skip,
                Skip,
                x => x.MatchBrfalse(out skip)
            ).Br_(skip).GotoLabel(skip)
            .GotoNext(MoveType.AfterLabel,
                x => x.MatchLdarg(1),
                x => x.MatchLdfld(damageType),
                x => x.MatchLdcI4((Int32)DamageType.IgniteOnHit),
                Skip,
                Skip,
                Skip,
                Skip,
                Skip,
                Skip,
                Skip,
                Skip,
                Skip,
                Skip,
                Skip,
                Skip,
                Skip,
                Skip,
                Skip,
                Skip,
                x => x.MatchBrfalse(out skip)
            ).Br_(skip).GotoLabel(skip)
            .GotoNext(MoveType.AfterLabel,
                x => x.MatchLdarg(1),
                x => x.MatchBrfalse(out skip),
                x => x.MatchLdarg(1),
                x => x.MatchLdfld(typeof(DamageInfo).GetField("inflictor", BF.Instance | BF.NonPublic | BF.Public)),
                x => x.MatchLdnull(),
                x => x.MatchCallOrCallvirt(out _),
                x => x.MatchBrfalse(out _),
                x => x.MatchLdarg(1),
                x => x.MatchLdfld(typeof(DamageInfo).GetField("inflictor", BF.Instance | BF.NonPublic | BF.Public)),
                x => x.MatchCallOrCallvirt(typeof(GameObject).GetMethods(BF.Instance | BF.Public).First((m) => m.ContainsGenericParameters).MakeGenericMethod(typeof(BoomerangProjectile)))
            ).Br_(skip).GotoLabel(skip)
            .GotoNext(MoveType.AfterLabel,
                x => x.MatchLdarg(1),
                x => x.MatchLdflda(procChainMask),
                x => x.MatchLdcI4((Int32)ProcType.BleedOnHit),
                x => x.MatchCallOrCallvirt(hasProc),
                x => x.MatchBrtrue(out skip)
            ).Br_(skip).GotoLabel(skip)
            .EmitOnHits();
    }
}
