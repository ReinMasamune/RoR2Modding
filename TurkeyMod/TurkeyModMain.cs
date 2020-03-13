using System;
using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using GeneralPluginStuff;

namespace TurkeyMod
{
    [R2APISubmoduleDependency(
        nameof(R2API.AssetPlus.AssetPlus),
        nameof(R2API.DirectorAPI)
    )]
    [BepInDependency( "com.bepis.r2api" )]
    [BepInPlugin( "com.ReinThings.TurkeyMod", "Rein-TurkeyMod", "1.0.1" )]
    public class TurkeyModMain : BaseUnityPlugin
    {
        public void OnDisable()
        {
            DirectorAPI.MonsterActions -= this.DirectorAPI_monsterActions;
        }

        public void OnEnable()
        {
            DirectorAPI.MonsterActions += this.DirectorAPI_monsterActions;
        }

        private void DirectorAPI_monsterActions( System.Collections.Generic.List<DirectorAPI.DirectorCardHolder> cards, DirectorAPI.StageInfo stage )
        {
            cards.Clear();
            CharacterSpawnCard turkeyCSC = Resources.Load<CharacterSpawnCard>("SpawnCards/CharacterSpawnCards/cscVulture");
            DirectorCard turkeyCard = new DirectorCard
            {
                allowAmbushSpawn = true,
                forbiddenUnlockable = "",
                minimumStageCompletions = 0,
                preventOverhead = false,
                requiredUnlockable = "",
                selectionWeight = 5,
                spawnDistance = DirectorCore.MonsterSpawnDistance.Standard,
                spawnCard = turkeyCSC
            };

            var tempCard1 = new DirectorAPI.DirectorCardHolder();
            tempCard1.SetCard( turkeyCard );
            tempCard1.SetInteractableCategory( DirectorAPI.InteractableCategory.None );
            tempCard1.SetMonsterCategory( DirectorAPI.MonsterCategory.BasicMonsters );
            cards.Add( tempCard1 );
            var tempCard2 = new DirectorAPI.DirectorCardHolder();
            tempCard2.SetCard( turkeyCard );
            tempCard2.SetInteractableCategory( DirectorAPI.InteractableCategory.None );
            tempCard2.SetMonsterCategory( DirectorAPI.MonsterCategory.Minibosses );
            cards.Add( tempCard2 );
            var tempCard3 = new DirectorAPI.DirectorCardHolder();
            tempCard3.SetCard( turkeyCard );
            tempCard3.SetInteractableCategory( DirectorAPI.InteractableCategory.None );
            tempCard3.SetMonsterCategory( DirectorAPI.MonsterCategory.Champions );
            cards.Add( tempCard3 );

        }


        public void Awake()
        {
            R2API.AssetPlus.Languages.AddToken( "VULTURE_BODY_NAME", "Turkey" );
        }
    }
}
