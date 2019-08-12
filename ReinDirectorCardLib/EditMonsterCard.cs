using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using RoR2;
using static ReinDirectorCardLib.ReinDirectorCardLib;

namespace ReinDirectorCardLib
{
    public class EditMonsterCard
    {
        public bool doEdits = false;
        public string monsterName;
        public MonsterCategory category;
        public SpawnStages stages;
        public DirectorCard monster;
        public string modFilePath;
        public string modMethodName;
        public int modLineNumber;

        public EditMonsterCard(string monsterName , MonsterCategory category, SpawnStages stages, DirectorCard monster,[CallerFilePath] string file = null, [CallerMemberName] string name = null, [CallerLineNumber] int lineNumber = 0 )
        {
            doEdits = true;
            this.monsterName = monsterName;
            this.category = category;
            this.stages = stages;
            this.monster = monster ?? throw new ArgumentNullException(nameof(monster));
            this.modFilePath = file;
            this.modMethodName = name;
            this.modLineNumber = lineNumber;
        }
    }
}
