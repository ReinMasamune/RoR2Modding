#if ROGUEWISP
using System;

using ReinCore;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace Rein.RogueWispPlugin
{

	internal partial class Main
	{
		partial void RW_Info() => this.Load += this.RW_AddLanguageTokens;

		private void RW_AddLanguageTokens()
		{
			Rein.Properties.Tools.RegisterLanguageTokens();

			//this.AddNewToken( "WISP_SURVIVOR_BODY_NAME", "Rogue Wisp" );
			//this.AddNewToken( "WISP_SURVIVOR_BODY_DESC", "The Rogue Wisp is a short-ranged, low-mobility, and high-damage survivor." +
			//    "\nThese descriptions are incredibly time consuming to write, so good luck!" );


			////Skills
			//this.AddNewToken( "WISP_SURVIVOR_PASSIVE_NAME", "Cursed Flames" );
			//this.AddNewToken( "WISP_SURVIVOR_PASSIVE_DESC", "<style=cIsUtility>Flame Charge</style> <style=cIsDamage>Empowers</style> your abilities but decays over time." +
			//    "\nGain <style=cIsDamage>Barrier</style> based on <style=cIsHealth>Maximum Health</style> whenever you regain <style=cIsUtility>Flame Charge.</style>" );

			//this.AddNewToken( "WISP_SURVIVOR_PRIMARY_1_NAME", "Heatwave" );
			//this.AddNewToken( "WISP_SURVIVOR_PRIMARY_1_DESC", "Fire a shockwave for <style=cIsDamage>165% damage.</style> <style=cIsUtility>Restores Flame Charge</style> on hit." );

			//this.AddNewToken( "WISP_SURVIVOR_SECONDARY_1_NAME", "Legendary Spark" );
			//this.AddNewToken( "WISP_SURVIVOR_SECONDARY_1_DESC", "Create a line of flame pillars that explode for <style=cIsDamage>250% damage.</style>" +
			//    "\nEnemies hit more than once take reduced damage. Consumes <style=cIsUtility>Flame Charge</style> on cast." );

			//this.AddNewToken( "WISP_SURVIVOR_UTILITY_1_NAME", "Burning Gaze" );
			//this.AddNewToken( "WISP_SURVIVOR_UTILITY_1_DESC", "Create an <style=cIsDamage>Inferno</style> that <style=cIsDamage>Ignites</style> all enemies for <style=cIsDamage>80% damage</style> per second." +
			//    "\nWhile in an <style=cIsDamage>Inferno</style><style=cIsUtility> Move Faster</style> and <style=cIsUtility>Gain Flame Charge</style> from <style=cIsDamage>Ignited</style> enemies." );

			//this.AddNewToken( "WISP_SURVIVOR_SPECIAL_1_NAME", "Incineration" );
			//this.AddNewToken( "WISP_SURVIVOR_SPECIAL_1_DESC", "Channel a beam for <style=cIsDamage>500% damage</style> per second. <style=cIsUtility>Continues as long as you hold button</style>." +
			//    "\n<style=cIsUtility>Gain Armor</style> but <style=cIsHealth>Cannot Move</style> while firing. Drains <style=cIsUtility>Flame Charge</style> per second while firing." );

		}

		private void AddNewToken( String s, String s2 ) => LanguageCore.AddLanguageToken( s, s2 );
	}

}
#endif
