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
            //Log.Warning( "OrbsCore loaded" );
            HooksCore.RoR2.Orbs.OrbCatalog.GenerateCatalog.On += GenerateCatalog_On;
            //Log.Warning( "OrbsCore loaded" );
            loaded = true;
        }

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

            Int32 start = OrbCatalog.indexToType.Length;
            Int32 newTotal = start + extra;
            Array.Resize<Type>( ref OrbCatalog.indexToType, newTotal );
            for( Int32 i = start; i < newTotal; ++i )
            {
                OrbCatalog.indexToType[i] = list[i - start];
                OrbCatalog.typeToIndex[list[i - start]] = i;
            }

            foreach(var v in OrbCatalog.indexToType)
            {
                Log.Message($"{v.FullName}");
            }
        }
    }
}
