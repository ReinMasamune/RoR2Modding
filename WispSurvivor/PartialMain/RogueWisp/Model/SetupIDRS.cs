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
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
#if ROGUEWISP
    internal partial class Main
    {
        partial void RW_SetupIDRS()
        {
            this.Load += this.RW_DoIDRSSetup;
        }

        private void RW_DoIDRSSetup()
        {
            ItemDisplayRuleSet refidrs = Resources.Load<GameObject>("Prefabs/CharacterBodies/MageBody").GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>().itemDisplayRuleSet;
            //ItemDisplayRuleSet idrs = ScriptableObject.CreateInstance<ItemDisplayRuleSet>();

            //var refEquip = refidrs.GetFieldValue<Item>


            this.RW_body.GetComponent<ModelLocator>().modelTransform.GetComponent<CharacterModel>().itemDisplayRuleSet = refidrs;
        }
    }
#endif
}
