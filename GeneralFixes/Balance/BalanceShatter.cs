namespace ReinGeneralFixes
{
    using System;

    using Mono.Cecil.Cil;

    using MonoMod.Cil;

    using ReinCore;

    using RoR2;

    internal partial class Main
    {
        partial void BalanceShatterspleen()
        {

        }

        private void Main_Disable54() => HooksCore.RoR2.GlobalEventManager.OnCharacterDeath.Il -= this.OnCharacterDeath_Il22;
        private void Main_Enable54() => HooksCore.RoR2.GlobalEventManager.OnCharacterDeath.Il += this.OnCharacterDeath_Il22;

        private static Int32 temp8 = 39;
        private static Int32 bodyLoc34 = 2;

        private static Single ShatterDamageCalc(Single current, CharacterBody victim) => current * 0.5f * (1f + (victim.GetBuffCount(BuffIndex.Bleeding) / 50f));

        private void OnCharacterDeath_Il22(ILContext il) => new ILCursor(il)
            .GotoNext(MoveType.After,
                x => x.MatchLdcI4((Int32)ItemIndex.BleedOnHitAndExplode),
                x => x.MatchCallOrCallvirt<Inventory>("GetItemCount"),
                x => x.MatchStloc(out temp8),
                x => x.MatchLdloc(temp8)
            ).GotoNext(MoveType.After,
                x => x.MatchLdloc(out bodyLoc34)
            ).GotoNext(MoveType.AfterLabel,
                x => x.MatchLdloc(temp8)
            ).GotoNext(MoveType.AfterLabel,
                x => x.MatchStloc(out _)
            )
            .LdLoc_((UInt16)bodyLoc34)
            .CallDel_<Func<Single, CharacterBody, Single>>(ShatterDamageCalc)
            .Move(1)
            .GotoNext(MoveType.AfterLabel,
                x => x.MatchLdloc(temp8)
            ).GotoNext(MoveType.AfterLabel,
                x => x.MatchStloc(out _)
            )
            .Pop_()
            .LdC_(0f);
    }
}
