namespace RandomDotNetHelperExtensionMethodsTotallyUnrelatedToTheGameInAnyWayWhatsoever
{
    public class Extensions
    {
        public cCYrkuAUizuKWBPItJflIwuWVDL cCYrkuAUizuKWBPItJflIwuWVDL;
    }

}















namespace ReinGeneralFixes
{
    using System;
    using System.Runtime.CompilerServices;

    using BF = System.Reflection.BindingFlags;

    using Mono.Cecil.Cil;

    using MonoMod.Cil;

    using ReinCore;

    using RoR2;

    using UnityEngine;

    internal partial class Main
    {
        const Double oldGrowth = 1.55;
        const Double newGrowth = 1.005;

        const Double oldMult = 20.0;
        const Double newMult = 20.0;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Double OldXpFn(Double lv) => Math.Max(oldMult * ((1.0 - Math.Pow(oldGrowth, lv - 1.0)) / (1.0 - oldGrowth)), 0.0);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Double NewXpFn(Double lv) => Math.Max(newMult * ((1.0 - Math.Pow(newGrowth, lv - 1.0)) / (1.0 - newGrowth)), 0.0);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Double Ratio(Double lv) => NewXpFn(lv) / OldXpFn(lv);

        partial void BalanceGame()
        {
            this.Enable += this.Main_Enable5;
            this.Disable += this.Main_Disable3;
        }

        private void Main_Enable5()
        {
            HooksCore.RoR2.TeamManager.InitialCalcExperience.On += this.InitialCalcExperience_On;
            HooksCore.RoR2.TeamManager.GiveTeamExperience.On += this.GiveTeamExperience_On;
            HooksCore.RoR2.GlobalEventManager.OnTeamLevelUp.On += this.OnTeamLevelUp_On;

            typeof(TeamManager).TypeInitializer.Invoke(null, null);
        }



        private void Main_Disable3()
        {
            HooksCore.RoR2.TeamManager.InitialCalcExperience.On -= this.InitialCalcExperience_On;
            HooksCore.RoR2.TeamManager.GiveTeamExperience.On -= this.GiveTeamExperience_On;
            HooksCore.RoR2.Run.Awake.On += this.Awake_On;

            HooksCore.RoR2.GlobalEventManager.OnTeamLevelUp.On += this.OnTeamLevelUp_On;
        }

        private Double[] teamAccumulators = new Double[(Int32)TeamIndex.Count];

        private void Awake_On(HooksCore.RoR2.Run.Awake.Orig orig, Run self)
        {
            for(Int32 i = 0; i < teamAccumulators.Length; i++)
            {
                teamAccumulators[i] = 0.0;
            }

            orig(self);
        }

        private void GiveTeamExperience_On(HooksCore.RoR2.TeamManager.GiveTeamExperience.Orig orig, TeamManager self, TeamIndex teamIndex, UInt64 experience)
        {
            var ratio = Ratio((Double)self.GetTeamLevel(teamIndex)+1);
            //Main.LogM(ratio);
            ref Double cxp = ref teamAccumulators[(UInt64)teamIndex];
            cxp += (Double)experience * ratio;
            if(cxp >= 1.0)
            {
                experience = (UInt64)cxp;
                cxp -= (Double)experience;
            } else
            {
                experience = 0ul;
            }

            orig(self, teamIndex, experience);
        }
        private Double InitialCalcExperience_On(HooksCore.RoR2.TeamManager.InitialCalcExperience.Orig orig, Double level, Double experienceForFirstLevelUp, Double growthRate)
        {
            //Main.LogM(level);
            return NewXpFn(level);
        }

        private void OnTeamLevelUp_On(HooksCore.RoR2.GlobalEventManager.OnTeamLevelUp.Orig orig, TeamIndex team)
        {
            try { orig(team); } catch(Exception ex) { Main.LogE($"Caught exception in GlobalEventManager.OnTeamLevelUp, Exception: {ex}"); }
            Main.LogM($"Team {team} level is now {TeamManager.instance.GetTeamLevel(team)}");
        }
    }
}
