using BepInEx;
using RoR2;
using RoR2.Skills;
using RoR2Plugin;
using System;
using UnityEngine;

namespace Engi133769OMEGA
{
    [R2API.Utils.R2APISubmoduleDependency( nameof( R2API.AssetAPI ) )]
    [BepInDependency( "com.bepis.r2api" )]
    [BepInPlugin( "com.ReinThings.EngiStuff", "Rein-EngiStuff", "1.0.0" )]
    public partial class OmegaTurretMain : RoR2Plugin.RoR2Plugin
    {
        public const Boolean _DEBUGGING = true;

        public static OmegaTurretMain instance;

        public static GameObject body;
        public static GameObject master;
        public static GameObject engi;

        public void Awake()
        {
            instance = this;

            GetObjects();
            CreateEngiSkill();

            EditBodyStats();
            EditModel();

            EditBodyVehicle();

            EditMaster();

            RegisterBody();
            RegisterMaster();
            RegisterSkills();
        }

        public void OnDisable()
        {
            instance = null;
        }




        public void Start()
        {
            // TODO: Make this into its own mod
            //EntityStates.Mage.Weapon.Flamethrower.recoilForce = 450f;
        }
    }
}
