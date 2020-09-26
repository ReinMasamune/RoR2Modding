namespace ReinGeneralFixes
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using BF = System.Reflection.BindingFlags;

    using EntityStates;

    using Mono.Cecil.Cil;

    using MonoMod.Cil;

    using ReinCore;

    using RoR2;

    using UnityEngine;
    using EntityStates.CaptainSupplyDrop;
    using EntityStates.Captain.Weapon;

    internal partial class Main
    {
        internal const Single shockDur = .5f;
        internal const Single shockDamageMult = 4f;
        internal const Single shockDamageToCancel = 0.1f;
        internal const Single captainPrimaryDamage = 1f;
        internal const Single captainBasePrimaryMinCharge = 0.05f;
        internal const Single captainBasePrimaryMaxCharge = 1f;
        internal const Single captainBasePrimaryDuration = 1.05f;

        internal const Single shockBeaconDamageCoef = 1f;
        internal const Single shockBeaconProcCoef = 1f;
        private static readonly GameObject captainBody = Resources.Load<GameObject>("Prefabs/CharacterBodies/CaptainBody");
        internal static readonly BuffDef shockBuff = new BuffDef
        {
            buffColor = new Color(0.3f, 0.7f, 1f, 1.0f),
            canStack = false,
            eliteIndex = EliteIndex.None,
            iconPath = "Textures/BuffIcons/texBuffTeslaIcon",
            isDebuff = true,
            name = "Shocked",
        };
        internal static EffectIndex novaIndex;

        private static Material shockMaterial;
        private static readonly Dictionary<CharacterBody, TemporaryOverlay> overlays = new();

        partial void BalanceCaptain()
        {
            this.Enable += this.Main_Enable;
            this.FirstFrame += this.Main_FirstFrame2;
        }

        private void Main_FirstFrame2()
        {
            shockMaterial = Resources.Load<Material>("Materials/matIsShocked");
            ShockState.healthFractionToForceExit = shockDamageToCancel;
            novaIndex = EffectCatalog.FindEffectIndexFromPrefab(Resources.Load<GameObject>("prefabs/effects/lightningstakenova"));
        }

        private void Main_Enable()
        {
            captainBody.GetComponent<SkillLocator>().utility.skillFamily.variants[0].skillDef.baseRechargeInterval = 18f;
            HooksCore.RoR2.CharacterBody.FixedUpdate.On += this.FixedUpdate_On;
            BuffsCore.getAdditionalEntries += this.BuffsCore_getAdditionalEntries;
            HooksCore.EntityStates.ShockState.OnEnter.Il += this.OnEnter_Il;
            OnHitManager.AddOnHit(Shock_DamageType);
            HooksCore.EntityStates.Captain.Weapon.FireCaptainShotgun.OnEnter.On += this.OnEnter_On;
            HooksCore.EntityStates.Captain.Weapon.ChargeCaptainShotgun.OnEnter.On += this.OnEnter_On4;
            //HooksCore.EntityStates.Captain.Weapon.FireCaptainShotgun.ModifyBullet.Il += this.ModifyBullet_Il;
            HooksCore.EntityStates.CaptainSupplyDrop.ShockZoneMainState.Shock.Il += this.Shock_Il;
            //HooksCore.EntityStates.Captain.Weapon.CallAirstrike1.OnEnter.On += this.OnEnter_On1;
            //HooksCore.EntityStates.Captain.Weapon.CallAirstrike2.OnEnter.On += this.OnEnter_On2;
            //HooksCore.EntityStates.Captain.Weapon.CallAirstrike3.OnEnter.On += this.OnEnter_On3;
            //HooksCore.RoR2.CaptainDefenseMatrixController.Start.Il += this.Start_Il;
        }


        private static Boolean CheckIsFirstStage() => Run.instance.stageClearCount == 0;
        private void Start_Il(ILContext il) => new ILCursor(il)
            .DefLabel(out var label)
            .GotoNext(x => x.MatchBrfalse(out label))
            .GotoNext(x => x.MatchBgt(out _))
            .GotoNext(MoveType.AfterLabel, x => x.MatchLdarg(0))
            .CallDel_<Func<Boolean>>(CheckIsFirstStage)
            .BrFalse_(label);

        private void OnEnter_On1(HooksCore.EntityStates.Captain.Weapon.CallAirstrike1.OnEnter.Orig orig, EntityStates.Captain.Weapon.CallAirstrike1 self)
        {
            self.damageCoefficient = 7.5f;
            orig(self);
        }
        private void OnEnter_On2(HooksCore.EntityStates.Captain.Weapon.CallAirstrike2.OnEnter.Orig orig, EntityStates.Captain.Weapon.CallAirstrike2 self)
        {
            self.damageCoefficient = 7.5f;
            orig(self);
        }
        private void OnEnter_On3(HooksCore.EntityStates.Captain.Weapon.CallAirstrike3.OnEnter.Orig orig, EntityStates.Captain.Weapon.CallAirstrike3 self)
        {
            self.damageCoefficient = 7.5f;
            orig(self);
        }

        private static BlastAttack ModifyShockBlastAttack(BlastAttack attack, ShockZoneMainState state)
        {
            var ownership = state?.gameObject?.GetComponent<GenericOwnership>();
            var owner = ownership?.ownerObject;
            var body = owner?.GetComponent<CharacterBody>();
            if(body is not null) state.damageStat = body.damage;
            attack.attacker = owner;
            attack.baseDamage = state.damageStat * shockBeaconDamageCoef;
            attack.procCoefficient = shockBeaconProcCoef;
            attack.damageType = DamageType.Shock5s;
            return attack;
        }
        private void Shock_Il(ILContext il) => new ILCursor(il)
            .GotoNext(MoveType.AfterLabel, x => x.MatchCallOrCallvirt(typeof(BlastAttack).GetMethod("Fire", BF.Public | BF.NonPublic | BF.Instance)))
            .LdArg_(0)
            .CallDel_<Func<BlastAttack, ShockZoneMainState, BlastAttack>>(ModifyShockBlastAttack);
        private void ModifyBullet_Il(ILContext il) => new ILCursor(il)
            .GotoNext(MoveType.AfterLabel, x => x.MatchLdcI4(out _))
            .Remove()
            .LdC_(1);

        private void OnEnter_On(HooksCore.EntityStates.Captain.Weapon.FireCaptainShotgun.OnEnter.Orig orig, EntityStates.Captain.Weapon.FireCaptainShotgun self)
        {
            //LogW($"BaseDuration:{self.baseDuration}");
            //LogW($"BaseMinCharge:{FireCaptainShotgun.baseMinChargeDuration}");
            self.baseDuration = captainBasePrimaryDuration;
            //self.procCoefficient = 0.75f;
            self.damageCoefficient = captainPrimaryDamage;
            //self.baseDuration = 1.
            orig(self);
        }
        private void OnEnter_On4(HooksCore.EntityStates.Captain.Weapon.ChargeCaptainShotgun.OnEnter.Orig orig, EntityStates.Captain.Weapon.ChargeCaptainShotgun self)
        {
            //LogW($"BaseCharge:{ChargeCaptainShotgun.baseChargeDuration}");
            //LogW($"BaseMinCharge:{ChargeCaptainShotgun.baseMinChargeDuration}");
            ChargeCaptainShotgun.baseMinChargeDuration = captainBasePrimaryMinCharge;
            ChargeCaptainShotgun.baseChargeDuration = captainBasePrimaryMaxCharge;

            orig(self);
        }

        private void OnEnter_Il(ILContext il) => new ILCursor(il)
            .GotoNext(MoveType.AfterLabel,
                x => x.MatchLdloc(0),
                x => x.MatchCallvirt(out _),
                x => x.MatchStloc(1)
            ).RemoveRange(2)
            .Emit(OpCodes.Ldnull);
                


        private void FixedUpdate_On(HooksCore.RoR2.CharacterBody.FixedUpdate.Orig orig, CharacterBody self)
        {
            orig(self);
            if(self.HasBuff(shockBuff.buffIndex))
            {
                if(!overlays.ContainsKey(self))
                {
                    var temp = base.gameObject.AddComponent<TemporaryOverlay>();
                    temp.duration = shockDur;
                    temp.originalMaterial = shockMaterial;
                    temp.AddToCharacerModel(self.modelLocator.modelTransform.GetComponent<CharacterModel>());
                    overlays[self] = temp;
                }
            } else
            {
                if(overlays.ContainsKey(self))
                {
                    Destroy(overlays[self]);
                    _ = overlays.Remove(self);
                }
            }
        }

        private void BuffsCore_getAdditionalEntries(List<BuffDef> buffList)
        {
            buffList.Add(shockBuff);
        }

        private static void Shock_DamageType(DamageInfo damage, CharacterBody attacker, Inventory inv, CharacterBody victim)
        {
            if(damage.damageType.HasFlag(DamageType.Shock5s) && !victim.HasBuff(shockBuff.buffIndex))
            {
                victim.AddBuff(shockBuff.buffIndex);
                var comp = victim.AddComponent<ShockController>();
                comp.Init(victim, attacker);
            }
        }
    }

    internal class ShockController : MonoBehaviour
    {
        internal CharacterBody targetBody;
        internal CharacterBody attackerBody;


        internal void Init(CharacterBody target, CharacterBody attacker)
        {
            this.targetBody = target;
            this.attackerBody = attacker;


        }
        protected void Start()
        {
            this.healthAtStart = this.targetBody.healthComponent.combinedHealthFraction;
        }

        private Single timer = 0f;
        private Single healthAtStart;
        protected void FixedUpdate()
        {
            timer += Time.fixedDeltaTime;
            if((this.healthAtStart - this.targetBody.healthComponent.combinedHealthFraction) >= ShockState.healthFractionToForceExit)
            {
                this.DoDamage();
                this.CleanupAndDestroy();
                return;
            }
            if(timer >= Main.shockDur)
            {
                this.CleanupAndDestroy();
                return;
            }
        }

        private void DoDamage()
        {
            var res = new BlastAttack
            {
                attacker = this.attackerBody.gameObject,
                attackerFiltering = AttackerFiltering.NeverHit,
                baseDamage = this.attackerBody.damage * Main.shockDamageMult,
                baseForce = 0f,
                bonusForce = Vector3.zero,
                crit = this.attackerBody.RollCrit(),
                damageColorIndex = DamageColorIndex.Item,
                damageType = DamageType.AOE,
                falloffModel = BlastAttack.FalloffModel.SweetSpot,
                losType = BlastAttack.LoSType.None,
                position = base.transform.position,
                procChainMask = default,
                procCoefficient = 1f,
                radius = 5f,
                teamIndex = this.attackerBody.teamComponent.teamIndex,
            }.Fire();


            //var dmg = new DamageInfo
            //{
            //    attacker = this.attackerBody.gameObject,
            //    crit = this.attackerBody.RollCrit(),
            //    damage = this.attackerBody.damage * Main.shockDamageMult,
            //    damageColorIndex = DamageColorIndex.Item,
            //    damageType = DamageType.AOE,
            //    dotIndex = DotController.DotIndex.None,
            //    force = Vector3.zero,
            //    inflictor = null,
            //    position = base.transform.position,
            //    procChainMask = default,
            //    procCoefficient = 1.0f,
            //};
            EffectManager.SpawnEffect(Main.novaIndex, new EffectData
            {
                origin = base.transform.position,
                scale = 5f,
            }, true);

            //GlobalEventManager.instance.OnHitAll(dmg, base.gameObject);
            //GlobalEventManager.instance.OnHitEnemy(dmg, base.gameObject);
            //targetBody.healthComponent.TakeDamage(dmg);
        }

        private void CleanupAndDestroy()
        {
            this.targetBody.RemoveBuff(Main.shockBuff.buffIndex);
            Destroy(this);
        }
    }
}
