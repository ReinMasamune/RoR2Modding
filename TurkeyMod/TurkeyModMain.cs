using System;
using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using RoR2Plugin;

namespace TurkeyMod
{
    [R2APISubmoduleDependency(nameof(R2API.AssetPlus))]
    [BepInDependency( "com.bepis.r2api" )]
    [BepInPlugin( "com.ReinThings.TurkeyMod", "Rein-TurkeyMod", "1.0.0" )]
    public class TurkeyModMain : RoR2Plugin.RoR2Plugin
    {
        public override void RemoveHooks()
        {
            DirectorAPI.monsterActions -= this.DirectorAPI_monsterActions;
        }

        public override void CreateHooks()
        {
            DirectorAPI.monsterActions += this.DirectorAPI_monsterActions;
        }

        private void DirectorAPI_monsterActions( System.Collections.Generic.List<DirectorAPI.DirectorCardHolder> cards, DirectorAPI.StageInfo stage )
        {
            cards.Clear();
            CharacterSpawnCard turkeyCSC = Resources.Load<CharacterSpawnCard>("SpawnCards/CharacterSpawnCards/cscVulture");
            DirectorCard turkeyCard = new DirectorCard
            {
                allowAmbushSpawn = true,
                cost = 15,
                forbiddenUnlockable = "",
                minimumStageCompletions = 0,
                preventOverhead = false,
                requiredUnlockable = "",
                selectionWeight = 5,
                spawnDistance = DirectorCore.MonsterSpawnDistance.Standard,
                spawnCard = turkeyCSC
            };
            cards.Add( new DirectorAPI.DirectorCardHolder
            {
                card = turkeyCard,
                interactableCategory = DirectorAPI.InteractableCategory.None,
                monsterCategory = DirectorAPI.MonsterCategory.BasicMonsters
            });
            cards.Add( new DirectorAPI.DirectorCardHolder
            {
                card = turkeyCard,
                interactableCategory = DirectorAPI.InteractableCategory.None,
                monsterCategory = DirectorAPI.MonsterCategory.Minibosses
            }); 
            cards.Add( new DirectorAPI.DirectorCardHolder
            {
                card = turkeyCard,
                interactableCategory = DirectorAPI.InteractableCategory.None,
                monsterCategory = DirectorAPI.MonsterCategory.Champions
            });
        }


        public void Awake()
        {
            DirectorAPI.AddHook();
            R2API.AssetPlus.Languages.AddToken( "VULTURE_BODY_NAME", "Turkey" );
            DirectorAPI.Helpers.AddSceneMonsterCredits( 100, DirectorAPI.Stage.TitanicPlains );
            DirectorAPI.Helpers.AddSceneMonsterCredits( 100, DirectorAPI.Stage.DistantRoost );
        }
    }
}
