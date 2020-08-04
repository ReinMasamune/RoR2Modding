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
                String cur = RoR2.Language.currentLanguage;
                if( cur == language || String.IsNullOrEmpty( language ) )
                {
                    Dictionary<String, Dictionary<String, String>> langDict = Language.languageDictionaries;
                    if( langDict.TryGetValue( cur, out Dictionary<String, String> dict ) )
                    {
                        dict[key] = value;
                    }
                }
            };
        }


        static LanguageCore()
        {
            Log.Warning( "LanguageCore loaded" );

            Log.Warning( "LanguageCore loaded" );
            loaded = true;
        }

        //private static readonly StaticAccessor<Dictionary<String,Dictionary<String,String>>> languageDictionaries = new StaticAccessor<Dictionary<String, Dictionary<String, String>>>( typeof(RoR2.Language), "languageDictionaries" );
    }
}
