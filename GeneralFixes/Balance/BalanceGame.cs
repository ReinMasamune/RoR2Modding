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
        private static Single Difficulty( Int32 stages )
        {
            const Single exponentialBase = 1.16f;
            const Single scaleFactor = 0.003f;
            const Single startingFrac = 0.975f;

            const Single effectiveStartingFrac = exponentialBase * startingFrac;
            const Single effectiveScaleFactor = exponentialBase * scaleFactor;

            return  effectiveStartingFrac + (effectiveScaleFactor * stages);
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
                c.EmitDelegate<Func<Single>>( () => Run.instance.selectedDifficulty >= DifficultyIndex.Hard ? Difficulty(Run.instance.stageClearCount) : 1.15f );
            }
        }
    }
}
