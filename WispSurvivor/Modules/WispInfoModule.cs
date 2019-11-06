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
            AddNewToken("WISP_SURVIVOR_PRIMARY_1_NAME", "Heatwave");
            AddNewToken("WISP_SURVIVOR_SECONDARY_1_NAME", "Legendary Spark");
            AddNewToken("WISP_SURVIVOR_UTILITY_1_NAME", "Burning Gaze");
            AddNewToken("WISP_SURVIVOR_SPECIAL_1_NAME", "Cremation");
            AddNewToken("WISP_SURVIVOR_PRIMARY_1_DESC", "Fire a shockwave that explodes on impact, dealing <style=cIsDamage>300% damage</style>." + 
                "\nCan hold up to <style=cIsDamage>3 stock</style> but does not require them to use. When used with stock has double attack speed and charge gain." +
                "\nGains <style=cIsDamage>damage</style> with <style=cIsUtility>Flame Charge</style> and grants <style=cIsUtility>10 Flame Charge</style> on use.");
            AddNewToken("WISP_SURVIVOR_SECONDARY_1_DESC", "Create a series of flame pillars that explode for <style=cIsDamage>150% damage</style>" +
                "\nDeals <style=cIsDamage>double damage</style> in the center of the pillar. Costs <style=cIsUtility>15% Flame Charge</style> on cast." +
                "\n<style=cIsUtility>Flame Charge</style> increases <style=cIsDamage>damage</style> and <style=cIsUtility>the number of pillars</style>.");
            AddNewToken("WISP_SURVIVOR_UTILITY_1_DESC", "Create an <style=cIsDamage>Inferno</style> lasting 10 seconds where you aim, <style=cIsDamage>igniting</style> all enemies inside for four seconds once per second." +
                "\n<style=cIsDamage>Ignited</style> enemies take <style=cIsDamage>10% damage</style> and are inflicted with <style=cIsUtility>Cursed Flame</style> twice per second." +
                "\nCosts <style=cIsUtility>20% Flame Charge</style> on cast.<style=cIsUtility>Flame Charge</style> increases <style=cIsDamage>damage</style>.");
            AddNewToken("WISP_SURVIVOR_SPECIAL_1_DESC", "Charge a ball of fire dealing from <style=cIsDamage>437.5% to 1750% damage</style> based on time." + 
                "\nYou are <style=cIsHealth>slowed</style> while charging. Costs <style=cIsUtility>10% Flame Charge</style> per second while charging" +
                "\n<style=cIsUtility>Flame Charge</style> greatly increases damage");
            AddNewToken("WISP_SURVIVOR_PASSIVE_NAME", "Cursed Flames");
            AddNewToken("WISP_SURVIVOR_PASSIVE_DESC", "Gain <style=cIsUtility>Flame Charge</style> over time when under 100 charge. Lose <style=cIsUtility>Flame Charge</style> over time when over 100." +
                "\nGain a buff when you kill an enemy inflicted with <style=cIsUtility>Cursed Flames</style>. This buff reduces decay of <style=cIsUtility>Flame Charge</style> and restores it over time." +
                "\nThe number of stacks and the duration of the buff depend on the number of stacks of <style=cIsUtility>Cursed Flame</style> on the enemy.");


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
