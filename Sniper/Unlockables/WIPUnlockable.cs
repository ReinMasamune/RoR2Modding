namespace Sniper.Unlockables
{
    using System;

    using BepInEx;

    using ReinCore;

    using Sniper.Enums;
    using Sniper.Modules;
    using Sniper.SkillDefs;

    using UnityEngine;

    using Resources = Properties.Resources;

    //internal class ExampleUnlockable : ModdedUnlockable<IAchievementSpriteProvider>
    //{
    //    public override String achievementIdentifier { get; } = "???";
    //    public override String unlockableIdentifier { get; } = "???";
    //    public override String prerequisiteUnlockableIdentifier { get; } = "???";
    //    public override String achievementNameToken { get; } = "MYNAME_MYEXAMPLEMOD_EXAMPLEUNLOCKABLE_ACHIEVEMENT_NAME";
    //    public override String achievementDescToken { get; } = "MYNAME_MYEXAMPLEMOD_EXAMPLEUNLOCKABLE_ACHIEVEMENT_DESC";
    //    public override String unlockableNameToken { get; } = "MYNAME_MYEXAMPLEMOD_EXAMPLEUNLOCKABLE_UNLOCKABLE_NAME";
    //    protected override IAchievementSpriteProvider spriteProvider { get; } = new VanillaSpriteProvider( "VANILLA PATH" );


    //    public override void OnInstall()
    //    {
    //        base.OnInstall();

    //        //SniperReloadableFireSkillDef.SniperPrimaryInstanceData.onReload += this.YouDidIt;
    //    }

    //    public override void OnUninstall()
    //    {
    //        base.OnUninstall();

    //        //SniperReloadableFireSkillDef.SniperPrimaryInstanceData.onReload -= this.YouDidIt;
    //    }

    //    public void YouDidIt( ReloadTier tier )
    //    {
    //        if( base.localUser != null && base.localUser.cachedBody && tier == ReloadTier.Perfect )
    //        {
    //            base.Grant();
    //        }
    //    }
    //}
}
/*
 *         public override void OnInstall()
        {
            base.OnInstall();

            SniperReloadableFireSkillDef.SniperPrimaryInstanceData.onReload += this.YouDidIt;
        }

        public override void OnUninstall()
        {
            base.OnUninstall();

            SniperReloadableFireSkillDef.SniperPrimaryInstanceData.onReload -= this.YouDidIt;
        }

        public void YouDidIt( ReloadTier tier )
        {
            if( base.localUser != null && base.localUser.cachedBody && tier == ReloadTier.Perfect )
            {
                base.Grant();
            }
        }
*/