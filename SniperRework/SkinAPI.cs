using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using RoR2;
using UnityEngine;

namespace RoR2Plugin
{
    // TODO: SkinAPI docs
    public static class SkinAPI
    {
        /// <summary>
        /// Creates and returns a new SkinDef
        /// </summary>
        /// <param name="skin"></param>
        /// <returns></returns>
        public static SkinDef CreateNewSkinDef( SkinDefInfo skin )
        {
            On.RoR2.SkinDef.Awake += DoNothing;

            SkinDef newSkin = ScriptableObject.CreateInstance<SkinDef>();

            newSkin.baseSkins = skin.baseSkins;
            newSkin.icon = skin.icon;
            newSkin.unlockableName = skin.unlockableName;
            newSkin.rootObject = skin.rootObject;
            newSkin.rendererInfos = skin.rendererInfos;
            newSkin.nameToken = skin.nameToken;
            newSkin.name = skin.name;

            On.RoR2.SkinDef.Awake -= DoNothing;
            return newSkin;
        }

        /// <summary>
        /// Struct used to define the fields for a SkinDef before actually creating it
        /// </summary>
        public struct SkinDefInfo
        {
            public SkinDef[] baseSkins;
            public Sprite icon;
            public System.String nameToken;
            public System.String unlockableName;
            public GameObject rootObject;
            public CharacterModel.RendererInfo[] rendererInfos;
            public System.String name;
        }

        #region Internal
        private static void DoNothing( On.RoR2.SkinDef.orig_Awake orig, SkinDef self )
        {
            //Intentionally do nothing
        }
        #endregion
    }
}
