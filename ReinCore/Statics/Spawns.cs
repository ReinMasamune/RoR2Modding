namespace ReinCore
{
    using System;

    using RoR2;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class SpawnsCore
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Boolean loaded { get; internal set; } = false;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static event MonsterEditDelegate monsterEdits;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static event InteractableEditDelegate interactableEdits;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static event FamilyEditDelegate familyEdits;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static event MiscEditDelegate miscEdits;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public delegate void MonsterEditDelegate( ClassicStageInfo stageInfo, Run runInstance, DirectorCardCategorySelection monsterSelection );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public delegate void InteractableEditDelegate( ClassicStageInfo stageInfo, Run runInstance, DirectorCardCategorySelection interactableSelection );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public delegate void FamilyEditDelegate( ClassicStageInfo stageInfo, Run runInstance, ClassicStageInfo.MonsterFamily[] possibleFamilies );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public delegate void MiscEditDelegate( ClassicStageInfo stageInfo, Run runInstance );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member



        static SpawnsCore()
        {
            HooksCore.RoR2.ClassicStageInfo.Awake.On += Awake_On;
            loaded = true;
        }



        private static readonly Accessor<ClassicStageInfo,DirectorCardCategorySelection> monsterCategories = new Accessor<ClassicStageInfo, DirectorCardCategorySelection>( "monsterCategories" );
        private static readonly Accessor<ClassicStageInfo,DirectorCardCategorySelection> interactableCategories = new Accessor<ClassicStageInfo, DirectorCardCategorySelection>( "interactableCategories" );

        private static void Awake_On( HooksCore.RoR2.ClassicStageInfo.Awake.Orig orig, ClassicStageInfo self )
        {
            if( loaded )
            {
                monsterEdits?.Invoke( self, Run.instance, monsterCategories.Get( self ) );
                interactableEdits?.Invoke( self, Run.instance, interactableCategories.Get( self ) );
                familyEdits?.Invoke( self, Run.instance, self.possibleMonsterFamilies );
                miscEdits?.Invoke( self, Run.instance );
            }
            orig( self );
        }
    }
}
