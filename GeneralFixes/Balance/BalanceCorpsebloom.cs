namespace ReinGeneralFixes
{
    using System;

    using Mono.Cecil.Cil;

    using MonoMod.Cil;

    using ReinCore;

    using RoR2;

    internal partial class Main
    {
        partial void BalanceCorpsebloom()
        {
            this.Enable += this.AddCorpsebloomEdits;
            this.Disable += this.RemoveCorpsebloomEdits;
        }

        private void RemoveCorpsebloomEdits()
        {
            HooksCore.RoR2.HealthComponent.Heal.Il -= this.Heal_Il;
            HooksCore.RoR2.HealthComponent.RepeatHealComponent.FixedUpdate.Il -= this.FixedUpdate_Il;
        }
        private void AddCorpsebloomEdits()
        {
            HooksCore.RoR2.HealthComponent.Heal.Il += this.Heal_Il;
            HooksCore.RoR2.HealthComponent.RepeatHealComponent.FixedUpdate.Il += this.FixedUpdate_Il;
        }

        private void FixedUpdate_Il( ILContext il )
        {
            var c = new ILCursor( il );
            _ = c.GotoNext( MoveType.Before, x => x.MatchCallOrCallvirt<HealthComponent>( "get_fullHealth" ) );
            _ = c.Remove();
            _ = c.EmitDelegate<Func<HealthComponent, Single>>( CorpseMaxHPCalc );
        }

        private void Heal_Il(ILContext il) => new ILCursor(il)
            .GotoNext(MoveType.After, x => x.MatchLdfld<HealthComponent>("repeatHealComponent"))
            .GotoNext(MoveType.AfterLabel, x => x.MatchLdcR4(0.1f))
            .Remove()
            .LdC_(0.15f)
            .GotoNext(MoveType.AfterLabel, x => x.MatchCallOrCallvirt<HealthComponent>("get_fullHealth"))
            .Remove()
            .CallDel_<Func<HealthComponent, Single>>(CorpseMaxHPCalc);

        private static Single CorpseMaxHPCalc(HealthComponent hc) => hc.itemCounts.repeatHeal * 2f * (hc.fullHealth + ( hc.fullShield * 0.25f ));
    }
}
