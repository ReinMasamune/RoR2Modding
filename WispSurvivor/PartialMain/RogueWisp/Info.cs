using BepInEx;
using R2API.Utils;
using RoR2;
using RoR2.Networking;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RogueWispPlugin.Helpers;
using RogueWispPlugin.Modules;
using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void RW_Info()
        {
            this.Load += this.RW_AddLanguageTokens;
        }

        private void RW_AddLanguageTokens()
        {
            AddNewToken( "WISP_SURVIVOR_BODY_NAME", "Rogue Wisp" );
            AddNewToken( "WISP_SURVIVOR_BODY_DESC", "The Rogue Wisp is a short-ranged, low-mobility, and high-damage survivor." +
                "\nThese descriptions are incredibly time consuming to write, so good luck!" );

            //Skins
            AddNewToken( "WISP_SURVIVOR_SKIN_1", "Default" );
            AddNewToken( "WISP_SURVIVOR_SKIN_2", "Lesser" );
            AddNewToken( "WISP_SURVIVOR_SKIN_3", "Greater" );
            AddNewToken( "WISP_SURVIVOR_SKIN_4", "Archaic" );
            AddNewToken( "WISP_SURVIVOR_SKIN_5", "Lunar" );
            AddNewToken( "WISP_SURVIVOR_SKIN_6", "Solar" );
            AddNewToken( "WISP_SURVIVOR_SKIN_7", "Abyssal" );
            AddNewToken( "WISP_SURVIVOR_SKIN_8", "Blighted" );


            //Skills
            AddNewToken( "WISP_SURVIVOR_PASSIVE_NAME", "Cursed Flames" );
            AddNewToken( "WISP_SURVIVOR_PASSIVE_DESC", "<style=cIsUtility>Flame Charge</style> <style=cIsDamage>Empowers</style> your abilities but decays over time." +
                "\nRegain <style=cIsUtility>Shield</style> based on <style=cIsHealth>Maximum Health</style> whenever you regain <style=cIsUtility>Flame Charge.</style>" );

            AddNewToken( "WISP_SURVIVOR_PRIMARY_1_NAME", "Heatwave" );
            AddNewToken( "WISP_SURVIVOR_PRIMARY_1_DESC", "Fire a shockwave for <style=cIsDamage>300% damage.</style> <style=cIsUtility>Restores Flame Charge</style> on hit." +
                "\nCan hold up to <style=cIsDamage>3 stock.</style> When used with stock has <style=cIsUtility>increased attack speed.</style>" );

            AddNewToken( "WISP_SURVIVOR_SECONDARY_1_NAME", "Legendary Spark" );
            AddNewToken( "WISP_SURVIVOR_SECONDARY_1_DESC", "Create a line of flame pillars that explode for <style=cIsDamage>150% damage.</style>" +
                "\nDeals <style=cIsDamage>double damage</style> in the center of the pillar. Consumes <style=cIsUtility>Flame Charge</style> on cast." );

            AddNewToken( "WISP_SURVIVOR_UTILITY_1_NAME", "Burning Gaze" );
            AddNewToken( "WISP_SURVIVOR_UTILITY_1_DESC", "Create an <style=cIsDamage>Inferno</style> that <style=cIsDamage>Ignites</style> all enemies for <style=cIsDamage>80% damage</style> per second." +
                "\nWhile in an <style=cIsDamage>Inferno</style><style=cIsUtility> Move Faster</style> and <style=cIsUtility>Gain Flame Charge</style> from <style=cIsDamage>Ignited</style> enemies." );

            AddNewToken( "WISP_SURVIVOR_SPECIAL_1_NAME", "Incineration" );
            AddNewToken( "WISP_SURVIVOR_SPECIAL_1_DESC", "Fire a beam for <style=cIsDamage>500% damage</style> per second. <style=cIsUtility>Continues as long as you hold button</style>." +
                "\n<style=cIsUtility>Gain Armor</style> but <style=cIsHealth>Cannot Move</style> while firing. Drains <style=cIsUtility>Flame Charge</style> while firing." );

        }

        private void AddNewToken( String s, String s2 )
        {
            R2API.AssetPlus.Languages.AddToken( s, s2 );
        }
    }

}
