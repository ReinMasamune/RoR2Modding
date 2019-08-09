using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using System.Collections.Generic;
using RoR2.Navigation;
using System;
using ReinDirectorCardLib;
using static ReinDirectorCardLib.AddedMonsterCard;

namespace ReinArchWispDemo
{
    [BepInDependency("com.bepis.r2api")]
    [BepInDependency("com.ReinThings.ReinDirectorCardLib")]
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
            archWispCard.cost = 300;
            archWispCard.selectionWeight = 1;
            archWispCard.allowAmbushSpawn = true;
            archWispCard.forbiddenUnlockable = "";
            archWispCard.minimumStageCompletions = 0;
            archWispCard.preventOverhead = true;
            archWispCard.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;

            AddedMonsterCard archWisp = new AddedMonsterCard(MonsterCategory.Miniboss, SpawnStages.AllStages, archWispCard);
            ReinDirectorCardLib.ReinDirectorCardLib.AddedMonsterCards.Add(archWisp);

        }
    }
}