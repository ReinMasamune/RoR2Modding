namespace ReinGeneralFixes
{
    using System;
    using System.Collections;
    using System.Reflection;

    using BF = System.Reflection.BindingFlags;

    using MonoMod.Cil;

    using ReinCore;

    using RoR2;

    using UnityEngine;

    internal partial class Main
    {
        partial void BalanceOSP()
        {
            this.Enable += this.AddOSPFix;
            this.Disable += this.RemoveOSPFix;
        }

        private void RemoveOSPFix() => HooksCore.RoR2.HealthComponent.TakeDamage.Il -= this.TakeDamage_Il;
        private void AddOSPFix() => HooksCore.RoR2.HealthComponent.TakeDamage.Il += this.TakeDamage_Il;

        private static Single OSPCalc(HealthComponent hc, Single incomingDamage, Single totalThisUpdate)
        {
            if(totalThisUpdate == 0f) hc.magnetiCharge = 0f;
            Single block = hc.barrier + hc.shield;
            Single damageAfterShields = incomingDamage - block;
            if(damageAfterShields <= 0f)
            {
                hc.magnetiCharge += incomingDamage;
                return incomingDamage;
            }
            if( hc.ospTimer > 0f)
            {
                var init = hc.ospTimer;
                hc.ospTimer -= damageAfterShields;
                LogM($"OSP active, buffer before: {init}, buffer after: {hc.ospTimer}");
                return hc.ospTimer <= 0f ? block - hc.ospTimer : block;
            }
            Single newCurrent = (totalThisUpdate - hc.magnetiCharge) + damageAfterShields;
            hc.magnetiCharge += block;
            Single minimumToTrigger = hc.fullHealth * (1f - hc.body.oneShotProtectionFraction);
            if(newCurrent >= minimumToTrigger)
            {
                
                var overMinBy = newCurrent - minimumToTrigger;
                var passthrough = damageAfterShields - overMinBy;
                var toBuffer = overMinBy - passthrough;
                var init = hc.ospTimer = (minimumToTrigger * 3f) + 0.1f;       
                hc.ospTimer -= toBuffer;
                LogM($"OSP Activated with {init} initial buffer and now has {hc.ospTimer} remaining buffer");
                if(hc.ospTimer < 0f) passthrough -= hc.ospTimer;
                _ = hc.StartCoroutine(ResetThreshold(hc));
                return block + passthrough;
            } else
            {
                return incomingDamage;
            }
        }
        private static IEnumerator ResetThreshold(HealthComponent hc)
        {
            yield return new WaitForSeconds(0.1f);
            LogM($"OSP ended, {hc.ospTimer} buffer unused");
            hc.ospTimer = 0f;
        }
        private static Single PassThroughLog(Single val)
        {
            LogW(val);
            return val;
        }
        private void TakeDamage_Il(ILContext il) => new ILCursor(il)
            .DefLabel(out var lab)
            .GotoNext(MoveType.AfterLabel,
                x => x.MatchLdarg(0),
                x => x.MatchLdfld(typeof(HealthComponent).GetField("ospTimer", BF.NonPublic | BF.Public | BF.Instance)),
                x => x.MatchLdcR4(out _),
                x => x.MatchBleUn(out lab)
            ).RemoveRange(5)
            .Br_(lab)
            .GotoNext(x => x.MatchCallOrCallvirt(typeof(HealthComponent).GetMethod("TriggerOneShotProtection", BF.Public | BF.NonPublic | BF.Instance)))
            .GotoPrev(MoveType.Before, x => x.MatchLdfld(typeof(HealthComponent).GetField("barrier", BF.NonPublic | BF.Public | BF.Instance ) ) )
            .Move(-2)
            .RemoveRange(29)
            .LdLoc_(5)
            .LdArg_(0)
            .LdFld_(typeof(HealthComponent).GetField(nameof(HealthComponent.serverDamageTakenThisUpdate), BF.Instance | BF.Public | BF.NonPublic))
            .CallDel_<Func<HealthComponent, Single, Single, Single>>(OSPCalc)
            .StLoc_(5);
    }
}
/*
Changelist:
Commando Phase round 2s cooldown, phase blast 3s cooldown



















*/
