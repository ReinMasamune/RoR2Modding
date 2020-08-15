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
        partial void BalanceElites()
        {
            this.FirstFrame += this.Main_FirstFrame1;
        }

        private void Main_FirstFrame1()
        {
            foreach(var def in CombatDirector.eliteTiers)
            {
                var c = def.costMultiplier;
                if(c == 1.0f) continue;
                def.costMultiplier *= 5.75f / 6f;
            }
        }
    }
}
