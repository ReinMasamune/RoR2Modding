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
//using static RogueWispPlugin.APIInterface;

namespace RogueWispPlugin
{
#if ROGUEWISP
    internal partial class Main
    {
        // TEMP
        private UInt32[] restoreIndex = new UInt32[8];


        private GameObject RW_body;
        private AssetBundle RW_assetBundle;

        partial void RW_General();
        partial void RW_Hook();
        partial void RW_Material();
        partial void RW_Effect();
        partial void RW_Model();
        partial void RW_Motion();
        partial void RW_UI();
        partial void RW_Body();
        partial void RW_Camera();
        partial void RW_Animation();
        partial void RW_Orb();
        partial void RW_Skill();
        partial void RW_Buff();
        partial void RW_Info();


        partial void CreateRogueWisp()
        {
            this.RW_General();
            this.RW_Hook();
            this.RW_Material();
            this.RW_Effect();
            this.RW_Model();
            this.RW_Motion();
            this.RW_UI();
            this.RW_Body();
            this.RW_Camera();
            this.RW_Animation();
            this.RW_Orb();
            this.RW_Skill();
            this.RW_Buff();
            this.RW_Info();
        }
    }
#endif
}
