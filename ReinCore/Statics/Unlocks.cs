//using System;
//using BepInEx;
//using RoR2;

//namespace ReinCore
//{
//    public static class UnlocksCore
//    {
//        public static Boolean loaded { get; internal set; }

//        public static String AddUnlockable( this BaseUnityPlugin plugin, String unlockableName )
//        {
//            if( !loaded ) throw new CoreNotLoadedException( nameof( UnlocksCore ) );
//            if( plugin == null ) throw new Exception( "Must be called from within your plugin class" );
//            var info = plugin.Info;
//            if( info == null ) throw new Exception( "Plugin is invalid" );
//            var data = info.Metadata;
//            if( data == null ) throw new Exception( "Plugin missing metadata attribute" );
//            var guid = data.GUID;
//            if( String.IsNullOrEmpty( guid ) ) throw new Exception( "Null or whitespace GUID for plugin" );

//            var generatedName = String.Format( "{0}.{1}", guid, unlockableName );



//            return generatedName;
//        }






//        static UnlocksCore()
//        {




//            loaded = true;
//        }
//    }
//}
