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
            //Loading the body here so changes can be made if needed, in this case we want to fix the hitbox so it covers more than 1/2 of the enemy and adjust the mass a bit
            GameObject archWispBody = Resources.Load<GameObject>("prefabs/characterbodies/ArchWispBody");
            CharacterMotor archWispMotor = archWispBody.GetComponent<CharacterMotor>();
            archWispMotor.mass *= 2f;
            HurtBox archWispHurtBox = archWispBody.GetComponentInChildren<HurtBox>();
            if( archWispHurtBox )
            {
                GameObject hurtBoxObject = archWispHurtBox.gameObject;
                //Remove old collider
                Destroy(hurtBoxObject.GetComponent<Collider>());
                //Add shiny new collider
                BoxCollider collider = hurtBoxObject.AddComponent<BoxCollider>();
                collider.isTrigger = false;
                collider.center = new Vector3(0f, -0.5f, 0.25f);
                collider.size = new Vector3(2.25f, 1.25f, 4f);
            }

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