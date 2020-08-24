#define RazorWireBleedDurationProcCoef
//#define RazorWireBleedDamageProcCoef
namespace ReinGeneralFixes
{
    using System;

    using MonoMod.Cil;

    using ReinCore;

    using RoR2;
    using RoR2.Orbs;

    using UnityEngine;

    partial class Main
    {
        const Single baseDistanceDecayCoef = 0.5f;
        const Single baseBleedDuration = 2f;
        const Single bleedDamageMultiplier = 1f;

        partial void BalanceRazorWire()
        {
            this.Enable += this.Main_Enable3;
            this.Disable += this.Main_Disable;
        }

        private void Main_Disable()
        {

        }
        private void Main_Enable3()
        {
            HooksCore.RoR2.HealthComponent.TakeDamage.Il += this.TakeDamage_Il3;
            HooksCore.RoR2.Orbs.LightningOrb.OnArrival.On += this.OnArrival_On;
        }

        private void OnArrival_On(HooksCore.RoR2.Orbs.LightningOrb.OnArrival.Orig orig, LightningOrb self)
        {
            if(self.lightningType == LightningOrb.LightningType.RazorWire)
            {
                if(self.target is not null)
                {
#if RazorWireBleedDurationProcCoef
                    var duration = baseBleedDuration * self.procCoefficient;
#else
                    var duration = baseBleedDuration;
#endif
#if RazorWireBleedDamageProcCoef
                    var damageMult = bleedDamageMultiplier * self.procCoefficient;
#else
                    var damageMult = bleedDamageMultiplier;
#endif
                    DotController.InflictDot(self.target.healthComponent.gameObject, self.attacker, DotController.DotIndex.Bleed, duration, damageMult);
                }
            } else
            {
                orig(self);
            }
        }

        private static LightningOrb SetupRazorOrb(LightningOrb orb, HealthComponent health, Single range, DamageReport report, HurtBox target)
        {
            var distDecayCoef = range * baseDistanceDecayCoef;
            orb.attacker = health.gameObject;
            orb.bouncedObjects = null;
            orb.bouncesRemaining = 0;
            orb.damageColorIndex = DamageColorIndex.Bleed;
            orb.damageCoefficientPerBounce = 1f;
            orb.damageValue = 0f;
            orb.isCrit = false;
            orb.lightningType = LightningOrb.LightningType.RazorWire;
            orb.origin = report.damageInfo.position;
            orb.procChainMask = default;
            orb.procCoefficient = distDecayCoef / (distDecayCoef + (target.transform.position - report.damageInfo.position).sqrMagnitude);
            orb.range = 0f;
            orb.teamIndex = health.body.teamComponent.teamIndex;
            orb.target = target;
            return orb;
        }
        private static Int32 curPos = 0;
        private static Int32 endPos = 0;

        private void TakeDamage_Il3(MonoMod.Cil.ILContext il) => new ILCursor(il)
            .GotoNext(MoveType.After,
                x => x.MatchNewobj<LightningOrb>(),
                x => x.MatchStloc(out _),
                x => x.MatchLdloc(out _),
                x => x.MatchLdarg(out _)
            ).Position(out curPos)
            .GotoNext(MoveType.After, x => x.MatchLdelemRef(), x => x.MatchStfld(out _))
            .Position(out endPos)
            .GotoPrev(MoveType.After,
                x => x.MatchNewobj<LightningOrb>(),
                x => x.MatchStloc(out _),
                x => x.MatchLdloc(out _),
                x => x.MatchLdarg(out _)
            ).RemoveRange(endPos - curPos)
            .LdLoc_(51)
            .LdLoc_(10)
            .LdLoc_(55)
            .LdLoc_(57)
            .LdElem_<HurtBox>()
            .CallDel_<Func<LightningOrb, HealthComponent, Single, DamageReport, HurtBox, LightningOrb>>(SetupRazorOrb)
            .StLoc_(58);
    }
}