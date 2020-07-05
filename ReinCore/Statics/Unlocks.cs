//namespace ReinCore
//{
//    using System;
//    using System.Collections.Generic;

//    using BepInEx;

//    using RoR2;
//    using RoR2.Achievements;
//    public static class UnlocksCore
//    {
//        public static Boolean loaded { get; internal set; }

//        public static void AddUnlockable<TUnlockable>() where TUnlockable : BaseAchievement, IModdedUnlockable, new()
//        {
//            var instance = new TUnlockable();
//            var ach = new AchievementDef
//            {
//                identifier = 
//            }
//        }




//        static UnlocksCore()
//        {




//            loaded = true;
//        }

//        private static List<(AchievementDef achDef, UnlockableDef unlockableDef)> moddedUnlocks = new List<(AchievementDef achDef, UnlockableDef unlockableDef)>();
//    }
//}
