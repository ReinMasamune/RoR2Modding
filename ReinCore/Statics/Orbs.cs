namespace ReinCore
{
    using System;
    using System.Collections.Generic;

    using RoR2.Orbs;


    public static class OrbsCore
    {

        public static Boolean loaded { get; internal set; } = false;

        public static event Action<List<Type>> getAdditionalOrbs;




        static OrbsCore()
        {
            Log.Warning( "OrbsCore loaded" );
            HooksCore.RoR2.Orbs.OrbCatalog.GenerateCatalog.On += GenerateCatalog_On;
            Log.Warning( "OrbsCore loaded" );
            loaded = true;
        }



        //private static readonly StaticAccessor<Type[]> indexToType = new StaticAccessor<Type[]>( typeof(OrbCatalog), "indexToType" );
        //private static readonly StaticAccessor<Dictionary<Type,Int32>> typeToIndex = new StaticAccessor<Dictionary<Type, Int32>>( typeof(OrbCatalog), "typeToIndex" );

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

            Type[] orbs = OrbCatalog.indexToType;
            Dictionary<Type, Int32> lookup = OrbCatalog.typeToIndex;//.Get();
            Int32 start = orbs.Length;
            Int32 newTotal = start + extra;
            Array.Resize<Type>( ref orbs, newTotal );
            for( Int32 i = start; i < newTotal; ++i )
            {
                orbs[i] = list[i - start];
                lookup[list[i - start]] = i;
            }
            OrbCatalog.indexToType = orbs;
        }
    }
}
