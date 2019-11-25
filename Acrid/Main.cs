using BepInEx;
using RoR2.UI;
using MonoMod.Cil;
using R2API.Utils;
using RoR2;
using RoR2.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
//using Acrid.Modules;
using Acrid.Helpers;
using static Acrid.Helpers.PrefabHelpers;
using static Acrid.Helpers.CatalogHelpers;
using static RoR2Plugin.ComponentHelpers;
using static RoR2Plugin.MiscHelpers;
using System.IO;
using UnityEngine.Networking;
using MonoMod.RuntimeDetour;


namespace Acrid
{
    [R2APISubmoduleDependency( nameof( R2API.SurvivorAPI ) )]
    [BepInDependency( "com.bepis.r2api" )]
    [BepInPlugin( "com.Acrid.Acrid", "Acrid", "1.2.6" )]
    public partial class AcridMain : RoR2Plugin.RoR2Plugin
    {
        public GameObject body;

        public void Awake()
        {
            this.body = Resources.Load<GameObject>( "Prefabs/CharacterBodies/BeetleGuardAllyBody" ).InstantiateClone("AcridBody");

            RegisterNewBody( this.body );

            EditModel();
            EditStats();

            //Temp shit
            MiscEdits();

            GameObject display = this.body.GetComponent<ModelLocator>().modelBaseTransform.gameObject;

            SurvivorDef bodySurvivorDef = new SurvivorDef
            {
                bodyPrefab = body,
                descriptionToken = "ACRID_BODY_DESC",
                displayPrefab = display,
                primaryColor = new Color(0.7f, 0.2f, 0.9f),
            };

            R2API.SurvivorAPI.AddSurvivor( bodySurvivorDef );
        }
    }
}
