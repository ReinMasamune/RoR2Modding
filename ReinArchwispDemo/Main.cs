using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using System.Collections.Generic;
using RoR2.Navigation;
using System;
using ReinDirectorCardLibrary;

namespace ReinArchWispDemo
{
    [BepInDependency("com.bepis.r2api")]
    [BepInDependency("com.ReinThings.ReinDirectorCardLibrary")]
    [BepInPlugin("com.ReinThings.ReinDirectorCardDemoArchWisp", "ReinArchWispsDemo", "1.0.0")]

    public class ReinArchWispDemo : BaseUnityPlugin
    {
        public void Awake()
        {

            CharacterSpawnCard archWispCSC = ScriptableObject.CreateInstance<CharacterSpawnCard>();
            archWispCSC.noElites = false;
            archWispCSC.prefab = Resources.Load<GameObject>("prefabs/charactermasters/ArchWispMaster");
            archWispCSC.forbiddenFlags = NodeFlags.NoCharacterSpawn;
            archWispCSC.requiredFlags = NodeFlags.None;
            archWispCSC.hullSize = HullClassification.Human;
            archWispCSC.occupyPosition = false;
            archWispCSC.sendOverNetwork = true;
            archWispCSC.nodeGraphType = MapNodeGroup.GraphType.Air;

            DirectorCard archWispCard = new DirectorCard();
            archWispCard.spawnCard = archWispCSC;
            archWispCard.cost = 10;
            archWispCard.selectionWeight = 1;
            archWispCard.allowAmbushSpawn = true;
            archWispCard.forbiddenUnlockable = "";
            archWispCard.minimumStageCompletions = 4;
            archWispCard.preventOverhead = true;
            archWispCard.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;

            ReinDirectorCardManager.AddedMonsterCard archWisp = new ReinDirectorCardManager.AddedMonsterCard();
            archWisp.modNameSpace = "ReinMonsterLib Testing";
            archWisp.monster = archWispCard;
            archWisp.stages = ReinDirectorCardManager.SpawnStages.AllStages;
            archWisp.category = ReinDirectorCardManager.MonsterCategory.Miniboss;

            ReinDirectorCardManager.AddedMonsterCards.Add(archWisp);

        }
    }
}