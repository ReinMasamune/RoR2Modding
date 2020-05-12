namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using BepInEx;
    using RoR2.Orbs;

    /// <summary>
    /// 
    /// </summary>
    public static class OrbsCore
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Boolean loaded { get; internal set; } = false;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static event Action<List<Type>> getAdditionalOrbs;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member



        static OrbsCore()
        {
            HooksCore.RoR2.Orbs.OrbCatalog.GenerateCatalog.On += GenerateCatalog_On;

            loaded = true;
        }



        private static readonly StaticAccessor<Type[]> indexToType = new StaticAccessor<Type[]>( typeof(OrbCatalog), "indexToType" );
        private static readonly StaticAccessor<Dictionary<Type,Int32>> typeToIndex = new StaticAccessor<Dictionary<Type, Int32>>( typeof(OrbCatalog), "typeToIndex" );

        private static void GenerateCatalog_On( HooksCore.RoR2.Orbs.OrbCatalog.GenerateCatalog.Orig orig )
        {
            orig();
            if( !loaded )
            {
                return;
            }

            var list = new List<Type>();
            getAdditionalOrbs?.Invoke( list );

            Int32 extra = list.Count;
            if( extra <= 0 )
            {
                return;
            }

            Type[] orbs = indexToType.Get();
            Dictionary<Type, Int32> lookup = typeToIndex.Get();
            Int32 start = orbs.Length;
            Int32 newTotal = start + extra;
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
