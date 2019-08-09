using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using RoR2;

namespace ReinDirectorCardLib
{
    public class AddedMonsterCard
    {
        public MonsterCategory category;
        public SpawnStages stages;
        public DirectorCard monster;
        public string modFilePath;
        public string modMethodName;
        public int modLineNumber;

        public enum MonsterCategory
        {
            Champion = 0,
            Miniboss = 1,
            BasicMonster = 2
        }

        [Flags]
        public enum SpawnStages
        {
            DistantRoost = 1,
            TitanicPlains = 2,
            WetlandAspect = 4,
            AbandonedAqueduct = 8,
            RallypointDelta = 16,
            ScorchedAcres = 32,
            AbyssalDepths = 64,
            GildedCoast = 128,
            AllStages = 255,
            InvalidStage = 256
        }

        public AddedMonsterCard(MonsterCategory category, SpawnStages stages, DirectorCard monster, [CallerFilePath] string file = null, [CallerMemberName] string name = null, [CallerLineNumber] int lineNumber = 0 )
        {
            this.category = category;
            this.stages = stages;
            this.monster = monster ?? throw new ArgumentNullException(nameof(monster));
            this.modFilePath = file;
            this.modMethodName = name;
            this.modLineNumber = lineNumber;
        }
    }
}
