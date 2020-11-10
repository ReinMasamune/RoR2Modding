namespace ReinCore
{
    using System;
    using RoR2;

    public static class SpawnsCore
    {
        public static Boolean loaded { get; internal set; } = false;
        public static event MonsterEditDelegate monsterEdits;
        public static event InteractableEditDelegate interactableEdits;
        public static event FamilyEditDelegate familyEdits;
        public static event MiscEditDelegate miscEdits;
        public delegate void MonsterEditDelegate( ClassicStageInfo stageInfo, Run runInstance, DirectorCardCategorySelection monsterSelection );
        public delegate void InteractableEditDelegate( ClassicStageInfo stageInfo, Run runInstance, DirectorCardCategorySelection interactableSelection );
        public delegate void FamilyEditDelegate( ClassicStageInfo stageInfo, Run runInstance, ClassicStageInfo.MonsterFamily[] possibleFamilies );
        public delegate void MiscEditDelegate( ClassicStageInfo stageInfo, Run runInstance );

        static SpawnsCore()
        {
            HooksCore.RoR2.ClassicStageInfo.Awake.On += Awake_On;
            loaded = true;
        }

        private static void Awake_On( HooksCore.RoR2.ClassicStageInfo.Awake.Orig orig, ClassicStageInfo self )
        {
            if( loaded )
            {
                monsterEdits?.Invoke( self, Run.instance, self.monsterCategories );
                interactableEdits?.Invoke( self, Run.instance, self.interactableCategories );
                familyEdits?.Invoke( self, Run.instance, self.possibleMonsterFamilies );
                miscEdits?.Invoke( self, Run.instance );
            }
            orig( self );
        }
    }
}
