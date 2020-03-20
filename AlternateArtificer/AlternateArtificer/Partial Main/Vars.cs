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
        private GameObject artiBodyPrefab;
        private SkillLocator artiSkillLocator;
        private CharacterBody artiCharBody;

        internal static BuffIndex fireBuff;
        internal static BuffIndex burnBuff;
        internal static DotController.DotIndex burnDot;
    }
}
