using System;
using BepInEx;
using RoR2;

namespace ReinCore
{
    // TODO: Docs for SurvivorsCore
    /// <summary>
    /// 
    /// </summary>
    public static class SurvivorsCore
    {
        /// <summary>
        /// 
        /// </summary>
        public static Boolean loaded { get; internal set; } = false;

        static SurvivorsCore()
        {
            HooksCore.RoR2.SurvivorCatalog.Init.On += Init_On;
            vanillaSurvivorsCount = SurvivorCatalog.idealSurvivorOrder.Length;
            vanillaSurvivorsCount2 = SurvivorCatalog.survivorMaxCount;

            loaded = true;
        }



        private static Int32 vanillaSurvivorsCount;
        private static Int32 vanillaSurvivorsCount2;
        private static StaticAccessor<SurvivorDef[]> survivorDefs = new StaticAccessor<SurvivorDef[]>( typeof(SurvivorCatalog), "survivorDefs" );

        private static void Init_On( HooksCore.RoR2.SurvivorCatalog.Init.Orig orig )
        {
            orig();
            if( !loaded ) return;
            var defs = survivorDefs.Get();


            if( vanillaSurvivorsCount <= defs.Length )
            {
                var extraBoxesCount = vanillaSurvivorsCount2 - vanillaSurvivorsCount;
                var startIndex = vanillaSurvivorsCount;
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
