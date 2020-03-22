using System;
using System.Collections.Generic;
using BepInEx;
using RoR2.Orbs;

namespace ReinCore
{
    /// <summary>
    /// 
    /// </summary>
    public static class OrbsCore
    {
        public static Boolean loaded { get; internal set; } = false;
        public static event Action<List<Type>> getAdditionalOrbs;



        static OrbsCore()
        {
            HooksCore.RoR2.Orbs.OrbCatalog.GenerateCatalog.On += GenerateCatalog_On;

            loaded = true;
        }



        private static StaticAccessor<Type[]> indexToType = new StaticAccessor<Type[]>( typeof(OrbCatalog), "indexToType" );
        private static StaticAccessor<Dictionary<Type,Int32>> typeToIndex = new StaticAccessor<Dictionary<Type, Int32>>( typeof(OrbCatalog), "typeToIndex" );

        private static void GenerateCatalog_On( HooksCore.RoR2.Orbs.OrbCatalog.GenerateCatalog.Orig orig )
        {
            orig();
            if( !loaded ) return;
            var list = new List<Type>();
            getAdditionalOrbs?.Invoke( list );

            var extra = list.Count;
            if( extra <= 0 ) return;

            var orbs = indexToType.Get();
            var lookup = typeToIndex.Get();
            var start = orbs.Length;
            var newTotal = start + extra;
            Array.Resize<Type>( ref orbs, newTotal );
            for( Int32 i = start; i < newTotal; ++i )
            {
                orbs[i] = list[i - start];
                lookup[list[i - start]] = i;
            }
            indexToType.Set( orbs );
        }
    }
}
