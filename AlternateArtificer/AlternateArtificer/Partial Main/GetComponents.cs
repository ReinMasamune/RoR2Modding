using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using ReinCore;
using RoR2;

namespace Rein.AlternateArtificer
{
    internal partial class Main
    {
        partial void GetComponents()
        {
            this.awake += this.Main_awake1;
        }

        private void Main_awake1()
        {
            this.artiCharBody = this.artiBodyPrefab.GetComponent<CharacterBody>();
            this.artiSkillLocator = this.artiBodyPrefab.GetComponent<SkillLocator>();
        }
    }
}
