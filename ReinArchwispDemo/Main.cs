using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using System.Collections.Generic;
using RoR2.Navigation;
using System;
using ReinDirectorCardLib;
using static ReinDirectorCardLib.ReinDirectorCardLib;

namespace ReinArchWispDemo
{
    [BepInDependency("com.bepis.r2api")]
    [BepInDependency("com.ReinThings.ReinDirectorCardLib")]
    [BepInPlugin("com.ReinThings.ReinDirectorCardDemoArchWisp", "ReinArchWispsDemo", "1.0.0")]

    public class ReinArchWispDemo : BaseUnityPlugin
    {
        public void Awake()
        {
            //Spawn card should be added to the list during Awake()
            //Adding later should be ok, but results could be strange so try to avoid it (especially during stage load time)
            //Edits to the body can be done at any time
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
            AddedMonsterCards.Add(archWisp);


            //Now going to allow elite overloading worms, hopefully
            CharacterSpawnCard owCard = Resources.Load<CharacterSpawnCard>("SpawnCards/CharacterSpawnCards/cscElectricWorm");
            Debug.Log("----------------------------------------");
            Debug.Log(owCard.name);
            owCard.noElites = false;

            DirectorCard owDirCard = new DirectorCard();
            owDirCard.spawnCard = owCard;
            owDirCard.cost = 4000;
            owDirCard.selectionWeight = 1;
            owDirCard.allowAmbushSpawn = true;
            owDirCard.forbiddenUnlockable = "";
            owDirCard.minimumStageCompletions = 0;
            owDirCard.preventOverhead = true;
            owDirCard.spawnDistance = DirectorCore.MonsterSpawnDistance.Far;

            EditMonsterCard owEdit = new EditMonsterCard(owCard.prefab.name, MonsterCategory.Champion, SpawnStages.AllStages, owDirCard);
            EditMonsterCards.Add(owEdit);
            //foreach( CharacterSpawnCard csc in owCard )
            //{
            //    Debug.Log(csc.name);
            //}

        }

        public void Start()
        {
            GameObject archWispBody = Resources.Load<GameObject>("prefabs/characterbodies/ArchWispBody");
            HurtBox archWispHurtBox = archWispBody.GetComponentInChildren<HurtBox>();
            if (archWispHurtBox)
            {
                GameObject hurtBoxObject = archWispHurtBox.gameObject;
                //Remove old collider
                Destroy(hurtBoxObject.GetComponent<SphereCollider>());
                //Add shiny new collider
                MeshCollider collider = hurtBoxObject.AddComponent<MeshCollider>();
                collider.sharedMesh = archWispBody.GetComponentInChildren<SkinnedMeshRenderer>().sharedMesh;
                collider.convex = true;
            }
        }
    }
}