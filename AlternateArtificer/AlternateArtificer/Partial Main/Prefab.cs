using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using ReinCore;
using RoR2;
using UnityEngine;

namespace Rein.AlternateArtificer
{
    internal partial class Main
    {
        partial void Prefab()
        {
            base.awake += this.Main_awake;
        }

        private void Main_awake()
        {
            this.artiBodyPrefab = Resources.Load<GameObject>( "Prefabs/CharacterBodies/MageBody" );
            this.artiCharBody = this.artiBodyPrefab.GetComponent<CharacterBody>();
            this.artiSkillLocator = this.artiCharBody.skillLocator;
        }
    }
}
