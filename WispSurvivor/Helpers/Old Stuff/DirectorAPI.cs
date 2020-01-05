using R2API.Utils;
using RoR2;
using System;
using System.Collections.Generic;
using System.Linq;

/*
namespace RoR2Plugin.OldStuff
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

            private static Boolean inUse = false;

            private static void AddHook()
            {
                if( inUse ) return;
                inUse = true;

                On.RoR2.ClassicStageInfo.Awake += ClassicStageInfo_Awake;
            }

            private static void ClassicStageInfo_Awake( On.RoR2.ClassicStageInfo.orig_Awake orig, ClassicStageInfo self )
            {
                List<DirectorCard> monsters = self.GetFieldValue<DirectorCard[]>( "monsterCards" ).ToList<DirectorCard>();
                List<DirectorCard> interactables = self.GetFieldValue<DirectorCard[]>( "interactableCards" ).ToList<DirectorCard>();

                Stage stage = GetStage(self);

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

            private static Stage GetStage( ClassicStageInfo stage ) => Stage.Custom;
        }
    }
}
*/