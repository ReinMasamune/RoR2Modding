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


        //Calc
        


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

            HooksCore.RoR2.GlobalEventManager.OnTeamLevelUp.On += this.OnTeamLevelUp_On;
        }


        private void GiveTeamExperience_On(HooksCore.RoR2.TeamManager.GiveTeamExperience.Orig orig, TeamManager self, TeamIndex teamIndex, UInt64 experience)
        {
            var ratio = Ratio((Double)self.GetTeamLevel(teamIndex)+1);
            //Main.LogM(ratio);
            experience = (UInt64)Math.Round((Double)experience * ratio);
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
