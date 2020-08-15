namespace ReinGeneralFixes
{
    using System;

    using Mono.Cecil.Cil;

    using MonoMod.Cil;

    using ReinCore;

    using RoR2;

    using UnityEngine;

    internal partial class Main
    {
        private static Single ScaleStageExpBase( int stage )
        {
            const float expBase = 1.16f;
            const float expScale = 0.0025f;
            const float expStartMult = 0.975f;

            const float effStart = expBase * expStartMult;
            const float effScale = expBase * expScale;

            return  effStart + (effScale * stage);
        }

        partial void BalanceGame()
        {
            this.Enable += this.Main_Enable9;
            this.Disable += this.Main_Disable9;
        }

        private void Main_Disable9() => HooksCore.RoR2.Run.OnFixedUpdate.Il -= this.OnFixedUpdate_Il;
        private void Main_Enable9() => HooksCore.RoR2.Run.OnFixedUpdate.Il += this.OnFixedUpdate_Il;

        private void OnFixedUpdate_Il( ILContext il )
        {
            var c = new ILCursor( il );

            while( c.TryGotoNext( MoveType.AfterLabel, x => x.MatchLdcR4( 1.15f ) ) )
            {
                c.Remove();
                c.EmitDelegate<Func<Single>>( () => Run.instance.selectedDifficulty >= DifficultyIndex.Hard ? ScaleStageExpBase(Run.instance.stageClearCount) : 1.15f );
            }
        }
    }
}
