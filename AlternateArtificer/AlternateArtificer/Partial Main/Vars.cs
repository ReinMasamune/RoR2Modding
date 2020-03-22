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
        #region Instance
        #region Private
        private GameObject artiBodyPrefab;
        private SkillLocator artiSkillLocator;
        private CharacterBody artiCharBody;
        
        #endregion
        #endregion

        #region Static
        #region Internal
        internal static BuffIndex fireBuff;
        internal static BuffIndex burnBuff;
        internal static DotController.DotIndex burnDot;

        internal static Mesh artiDefaultMesh;
        internal static Mesh artiModifiedMesh;

        internal static GameObject[] swordProjectiles;
        internal static GameObject[] swordGhosts;

        #endregion
        #region Private
        private static Texture2D swordRamp;
        private static Texture2D iceRamp;
        private static Texture2D mainTex;
        private static Material swordMaterial;
        

        #endregion
        #endregion
    }
}
