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

    internal class WIPUnlockable : ModdedUnlockable<VanillaSpriteProvider>
    {
        public override String achievementIdentifier => "REIN_SNIPER_WIP";

        public override String unlockableIdentifier => "REIN_SNIPER_WIP";

        public override String prerequisiteUnlockableIdentifier => null;

        public override String achievementNameToken => "TOOLTIP_WIP_CONTENT_NAME";

        public override String achievementDescToken => "TOOLTIP_WIP_CONTENT_DESCRIPTION";

        public override String unlockableNameToken => "TOOLTIP_WIP_CONTENT_NAME";

        protected override VanillaSpriteProvider spriteProvider => new VanillaSpriteProvider("Textures/MiscIcons/texWIPIcon");
    }
}