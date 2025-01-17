﻿namespace Rein.Sniper.Unlockables
{
    using System;

    using BepInEx;

    using ReinCore;

    using Rein.Sniper.Enums;
    using Rein.Sniper.Modules;
    using Rein.Sniper.SkillDefs;

    using UnityEngine;

    using Resources = Properties.Resources;

    internal class WIPUnlockable : ModdedUnlockable<VanillaSpriteProvider>
    {
        internal static String achievement_Identifier => "Sniper.Wip";
        internal static String unlockable_Identifier => "Sniper.Wip";
        internal static String prereq_Identifier => "";
        internal static String achievement_Name_Token => "TOOLTIP_WIP_CONTENT_NAME";
        internal static String achievement_Desc_Token => "TOOLTIP_WIP_CONTENT_DESCRIPTION";
        internal static String unlockable_Name_Token => "TOOLTIP_WIP_CONTENT_NAME";

        public override String achievementIdentifier => achievement_Identifier;

        public override String unlockableIdentifier => unlockable_Identifier;

        public override String prerequisiteUnlockableIdentifier => prereq_Identifier;

        public override String achievementNameToken => achievement_Name_Token;

        public override String achievementDescToken => achievement_Desc_Token;

        public override String unlockableNameToken => unlockable_Name_Token;

        protected override VanillaSpriteProvider spriteProvider => new VanillaSpriteProvider("Textures/MiscIcons/texWIPIcon");
    }
}