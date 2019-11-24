using RoR2;
using R2API.Utils;
using System;
using System.Linq;
using System.Collections.Generic;

namespace RoR2Plugin
{
    public abstract partial class RoR2Plugin
    {
        public static class DirectorAPI
        {
            public enum Stage
            {
                Custom = 0,
                TitanicPlains = 1,
                DistantRoost = 2,
                WetlandAspect = 3,
                AbandonedAqueduct = 4,
                RallypointDelta = 5,
                ScorchedAcres = 6,
                AbyssalDepths = 7,
                SirensCall = 8,
                GoldShores = 9,
                MomentFractured = 10,
                Bazaar = 11
            }

            public static event Action<List<DirectorCard> , Stage> monsterActions;
            public static event Action<List<DirectorCard> , Stage> interactableActions;

            private static bool inUse = false;

            private static void AddHook()
            {
                if( inUse ) return;
                inUse = true;

                On.RoR2.ClassicStageInfo.Awake += ClassicStageInfo_Awake;
            }

            private static void ClassicStageInfo_Awake( On.RoR2.ClassicStageInfo.orig_Awake orig, ClassicStageInfo self )
            {
                var monsters = self.GetFieldValue<DirectorCard[]>("monsterCards").ToList<DirectorCard>();
                var interactables = self.GetFieldValue<DirectorCard[]>("interactableCards").ToList<DirectorCard>();

                var stage = GetStage(self);

                Action<List<DirectorCard>,Stage> monsterAct = monsterActions;
                if( monsterAct != null )
                {
                    monsterAct( monsters, stage );
                }
                Action<List<DirectorCard>,Stage> interactableAct = interactableActions;
                if( interactableAct != null )
                {
                    interactableAct( interactables, stage );
                }

            }

            private static Stage GetStage( ClassicStageInfo stage )
            {
                return Stage.Custom;
            }
        }
    }
}
