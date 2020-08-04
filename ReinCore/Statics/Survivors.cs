namespace ReinCore
{
    using System;
    using RoR2;

    public static class SurvivorsCore
    {
        public static Boolean loaded { get; internal set; } = false;

        static SurvivorsCore()
        {
            Log.Warning( "SurvivorsCore loaded" );
            HooksCore.RoR2.SurvivorCatalog.Init.On += Init_On;
            vanillaSurvivorsCount = SurvivorCatalog.idealSurvivorOrder.Length;
            vanillaSurvivorsCount2 = SurvivorCatalog.survivorMaxCount;
            Log.Warning( "SurvivorsCore loaded" );
            loaded = true;
        }

        private static readonly Int32 vanillaSurvivorsCount;
        private static readonly Int32 vanillaSurvivorsCount2;
        //private static readonly StaticAccessor<SurvivorDef[]> survivorDefs = new StaticAccessor<SurvivorDef[]>( typeof(SurvivorCatalog), "survivorDefs" );

        private static void Init_On( HooksCore.RoR2.SurvivorCatalog.Init.Orig orig )
        {
            orig();
            if( !loaded ) return;

            SurvivorDef[] defs = SurvivorCatalog.survivorDefs;

            if( vanillaSurvivorsCount <= defs.Length )
            {
                Int32 extraBoxesCount = vanillaSurvivorsCount2 - vanillaSurvivorsCount;
                Int32 startIndex = vanillaSurvivorsCount;
                Array.Resize<SurvivorIndex>( ref SurvivorCatalog.idealSurvivorOrder, defs.Length - 1 );
                for( Int32 i = startIndex; i < SurvivorCatalog.idealSurvivorOrder.Length; ++i )
                {
                    SurvivorCatalog.idealSurvivorOrder[i] = defs[i + 1].survivorIndex;
                }
                SurvivorCatalog.survivorMaxCount = SurvivorCatalog.idealSurvivorOrder.Length + extraBoxesCount;
            }
        }
    }
}
