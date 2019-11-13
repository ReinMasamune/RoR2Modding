using UnityEngine;
using R2API.Utils;
using System;
using System.Collections.Generic;

namespace WispSurvivor.Modules
{
    public static class WispInfoModule
    {
        private static List<KeyValuePair<string, string>> newTokens = new List<KeyValuePair<string, string>>();

        public static void DoModule( GameObject body , Dictionary<Type,Component> dic)
        {
            AddLanguageTokens();
        }

        private static void AddLanguageTokens()
        {
            //Body
            AddNewToken("WISP_SURVIVOR_BODY_NAME", "Rogue Wisp");
            AddNewToken("WISP_SURVIVOR_BODY_DESC", "The Rogue Wisp is a short-ranged, low-mobility, and high-damage survivor." + 
                "\nThese descriptions are incredibly time consuming to write, so good luck!");

            //Skins
            AddNewToken("WISP_SURVIVOR_SKIN_1", "Default");
            AddNewToken("WISP_SURVIVOR_SKIN_2", "Lesser");
            AddNewToken("WISP_SURVIVOR_SKIN_3", "Greater");
            AddNewToken("WISP_SURVIVOR_SKIN_4", "Archaic");
            AddNewToken("WISP_SURVIVOR_SKIN_5", "Lunar");
            AddNewToken("WISP_SURVIVOR_SKIN_6", "Solar");
            AddNewToken("WISP_SURVIVOR_SKIN_7", "Iridiscent");
            AddNewToken("WISP_SURVIVOR_SKIN_8", "Ascendent");

            //Skills
           
            AddNewToken("WISP_SURVIVOR_PASSIVE_NAME", "Cursed Flames");
            AddNewToken("WISP_SURVIVOR_PASSIVE_DESC", "<style=cIsUtility>Flame Charge</style> <style=cIsDamage>Empowers</style> your abilities but decays over time." +
                "\nAbilities consume <style=cIsUtility>Flame Charge</style> on cast.");

            AddNewToken("WISP_SURVIVOR_PRIMARY_1_NAME", "Heatwave");
            AddNewToken("WISP_SURVIVOR_PRIMARY_1_DESC", "Fire a shockwave that explodes for <style=cIsDamage>300% damage.</style> <style=cIsUtility>Restores 5 Flame Charge</style> on cast." + 
                "\nCan hold up to <style=cIsDamage>3 stock.</style> When used with stock has <style=cIsUtility>double attack speed and charge gain.</style>" );

            AddNewToken("WISP_SURVIVOR_SECONDARY_1_NAME", "Legendary Spark");
            AddNewToken("WISP_SURVIVOR_SECONDARY_1_DESC", "Create a line of flame pillars that explode for <style=cIsDamage>150% damage.</style>" +
                "\nDeals <style=cIsDamage>double damage</style> in the center of the pillar.");

            AddNewToken("WISP_SURVIVOR_UTILITY_1_NAME", "Burning Gaze");
            AddNewToken("WISP_SURVIVOR_UTILITY_1_DESC", "Create an <style=cIsDamage>Inferno</style> that <style=cIsDamage>Ignites</style> all enemies for <style=cIsDamage>80% damage</style> per second." +
                "\n<style=cIsDamage>Ignited</style> enemies restore your <style=cIsUtility>Flame Charge</style> while you are inside the <style=cIsDamage>Inferno.</style>");

            AddNewToken("WISP_SURVIVOR_SPECIAL_1_NAME", "Cremation");
            AddNewToken("WISP_SURVIVOR_SPECIAL_1_DESC", "Charge up a Fireball for <style=cIsDamage>435%-1750% damage.</style>" + 
                "\n<style=cIsHealth>Cannot Sprint</style> while charging.");

            On.RoR2.Language.LoadAllFilesForLanguage += Language_LoadAllFilesForLanguage;
        }

        private static Boolean Language_LoadAllFilesForLanguage(On.RoR2.Language.orig_LoadAllFilesForLanguage orig, String language)
        {
            bool v = orig(language);

            Dictionary<string, Dictionary<string, string>> dicts = typeof(RoR2.Language).GetFieldValue<Dictionary<string, Dictionary<string, string>>>("languageDictionaries");
            
            foreach( KeyValuePair<string,string> kv in newTokens )
            {
                dicts[language][kv.Key]= kv.Value;
            }

            return v;
        }

        private static void AddNewToken(string k , string v )
        {
            newTokens.Add(new KeyValuePair<string, string>(k, v));
        }

        private static T C<T>( this Dictionary<Type,Component> dic ) where T : Component
        {
            return dic[typeof(T)] as T;
        }
    }
}
