namespace ReinCore
{
    using System;
    using System.Collections.Generic;

    using RoR2;



    public static class LanguageCore
    {

        public static Boolean loaded { get; internal set; } = false;


        public static void AddLanguageToken( String key, String value, String language = "" )
        {
            RoR2.Language.onCurrentLanguageChanged += () =>
            {
                var cur = Language.currentLanguage;
                if(cur is null) return;
                if( cur.name == language || String.IsNullOrEmpty( language ) )
                {
                    cur.stringsByToken[key] = value;
                }
            };
        }


        static LanguageCore()
        {
            //Log.Warning( "LanguageCore loaded" );

            //Log.Warning( "LanguageCore loaded" );
            loaded = true;
        }

        //private static readonly StaticAccessor<Dictionary<String,Dictionary<String,String>>> languageDictionaries = new StaticAccessor<Dictionary<String, Dictionary<String, String>>>( typeof(RoR2.Language), "languageDictionaries" );
    }
}
