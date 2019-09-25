using BepInEx;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Navigation;
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
            //To add a monster spawn, add an AddedMonsterCard to the list AddedMonsterCards
            //To edit an existing monster spawn, add an EditMonsterCard to the list EditMonsterCards

            //To create an AddedMonsterCard you need to use the constructor.
            //The constructor args are as follows:
            //MonsterCategory   An enum for the three classes of monster. Champion (bosses), Miniboss (Beetle guards, greater wisps, ect.), and Basic Monsters (beetles).
            //SpawnStages       A flags enum that chooses what stages the monster should spawn on. Multiple stages can be set by adding them, or using the | operator.
            //DirectorCard      An internal ror2 class that is used by the director to pick things, info on creating one further down

            //To create an EditMonsterCard you also need to use the constructor
            //Args:
            //string name       This should be the name of the in game master prefab that is being edited. IE: GreaterWispMaster
            //MonsterCategory   The category of the monster. This needs to match vanilla.
            //SpawnStages       This is the stages this edit should happen on. This edit will do nothing on stages other than these
            //DirectorCard      The card that will replace the vanilla directorcard. More on creating these below

            //When to use an AddedMonsterCard:
            //You want to add a new entry to the list of spawns on a particular map
            //That includes making an existing enemy spawn on a map it does not normally appear on

            //When to use an EditMonsterCard
            //To change settings for an enemy that already spawns
            //To disallow spawning for an enemy (set weight to 0)

            //When you need to use both
            //To make an enemy spawn on all stages (use an edit card to remove them from all stages they normally spawn on, then use an add to add a new one to all stages)

            //Constructing a DirectorCard
            //DirectorCards must be constructed at runtime, there are no prefabs you can load and there is no real access to them in code (there is 1 place, and that is already hooked by the lib)
            //To construct one, just use new DirectorCard()
            //DirectorCards have a bunch of fields, here is a list:
            //SpawnCard spawnCard   This is the spawncard for the enemy that is being spawned, more info on these further down
            //int cost              The number of credits that this card costs to spawn. In the words of the devs: Should not be zero! EVER.
            //int selectionWeight   A modifier for the likelyhood of a card being selected. Should be 1 in most cases
            //DirectorCore.MonsterSpawnDistance     An enum for where to spawn. Close, Standard, Far
            //bool allowAmbushSpawn     This should generally be true, although for bosses may want it to be false. Usually a burst of spawns around the player.
            //bool preventOverhead      not entirely certain what this does. Set to true usually?
            //int minimumStageCompletions   When should this enemy start spawning. Set to 4 for it to start on loop
            //string requiredUnlockable     This ties into the acheivments, have not tested much
            //string forbiddenUnlockable    same as above

            //Getting a SpawnCard
            //You generally will want to be loading an existing SpawnCard and editing properties
            //Sometimes this isn't possible though. If you need to make a new one create it like a ScriptableObject
            //CharacterSpawnCard csc = ScriptableObject.CreateInstance<CharacterSpawnCard>();
            //InteractableSpawnCard isc = ScriptableObject.CreateInstance<InteractableSpawnCard>();
            //Note that there are two types of spawn card. Both of them derive from SpawnCard
            //CharacterSpawnCard is for enemies, InteractableSpawnCard is for interactables

            //Fields for all SpawnCards
            //GameObject prefab         The gameobject spawned by the card, for enemies make sure to use a characterMaster
            //bool sendOverNetwork      True for 99% of cases, if false this won't sync across network
            //HullClassification hullSize   Not sure what this does, but generally you want it to be Human
            //MapNodeGroup.GraphType nodeGraphType      This is related to the location the card spawns its prefab.
            //NodeFlags requiredFlags       have not played around with this much
            //NodeFlags forbiddenFlagd      For enemies, set this to NodeFlags.NoCharacterSpawn
            //bool occupyPosition           Should usually be true for enemies and interactables or you may end up with things stacked ontop eachother

            //Fields for CharacterSpawnCards
            //bool noElites          Set to true if this enemy is not allowed to be an elite

            //Fields for InteractableSpawnCards
            //bool orientToFloor                    ---
            //bool slightlyRandomizeOrientation     ---


            //Here are some code examples

            //Adding the unused Archaic Wisp enemy to the game
            //First thing we need is a SpawnCard, there are two ways to get it.
            //First, load one from the game files. This is what that code looks like
            //CharacterSpawnCard archWispCSC = Resources.Load<CharacterSpawnCard>("spawncards/CharacterSpawnCards/cscArchWisp");
            //For the sake of demonstration, instead we will make a new spawncard
            CharacterSpawnCard archWispCSC = ScriptableObject.CreateInstance<CharacterSpawnCard>();                                      //Constructor
            archWispCSC.loadout = new SerializableLoadout();                                                //This is the loadout for the body. Have not tested this much yet
            archWispCSC.noElites = false;                                                                   //This enemy can be an elite                
            archWispCSC.forbiddenFlags = NodeFlags.NoCharacterSpawn;                                        //Keep it from spawning in weird places
            archWispCSC.requiredFlags = NodeFlags.None;                                                     //This field is rarely used
            archWispCSC.hullSize = HullClassification.Human;                                                //Just keep this at human
            archWispCSC.occupyPosition = false;                                                             //Flying enemies don't need this at true
            archWispCSC.sendOverNetwork = true;                                                             //This is an enemy, we want it networked
            archWispCSC.nodeGraphType = MapNodeGroup.GraphType.Air;                                         //Flying enemies spawn in the air, not the floor
            archWispCSC.prefab = Resources.Load<GameObject>("prefabs/charactermasters/ArchWispMaster");     //Loading the characterMaster prefab for archwisp


            //Now we need a DirectorCard for the enemy              
            DirectorCard archWispDirectorCard = new DirectorCard();                                         //Constructor
            archWispDirectorCard.spawnCard = archWispCSC;                                                   //Set the spawn card we just made
            archWispDirectorCard.cost = 300;                                                                //Greater Wisps cost 200, beetle queens cost 600
            archWispDirectorCard.selectionWeight = 1;                                                       //Weight of 1, because that is fine
            archWispDirectorCard.allowAmbushSpawn = true;                                                   //Surprise!
            archWispDirectorCard.forbiddenUnlockable = "";                                                  //Not messing with these atm
            archWispDirectorCard.minimumStageCompletions = 3;                                               //Same spawn settings as ror1        
            archWispDirectorCard.preventOverhead = true;                                                    //Leaving this at default
            archWispDirectorCard.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;                //Standard spawn distance

            //Now, for the part that actually uses the lib
            //We want to add this card so the director uses it, and ideally we don't want to conflict with other mods
            //The lib provides a public list you can add all your cards to, and does all the edits at once
            //There still can be conflicts if two mods are changing the same thing.

            //We need to make an AddedMonsterCard, because this is an entirely new entry
            //Before that, we want to select the stages it can spawn on
            SpawnStages stages = SpawnStages.AllStages;             //This starts us off with all default stages
            stages -= SpawnStages.WetlandAspect;                    //We remove Wetland Aspect, because greater wisps can't spawn there either

            //Now we make the AddedMonsterCard
            AddedMonsterCard archWisp = new AddedMonsterCard(MonsterCategory.Miniboss, stages, archWispDirectorCard);

            //Once we have that we simply add it to the list with the .Add method.
            AddedMonsterCards.Add(archWisp);
            //This adding should be done ahead of time, but there is nothing stopping you from doing it later on. The director won't update until a new stage loads though
            //In Start() I have some edits to the hitbox for ArchWisps just to make this feel better to play.


            //Next example, we will edit an existing card.
            //For this, let's allow overloading worms to be elite

            //First, we want to get the existing CharacterSpawnCard for them
            CharacterSpawnCard zapSnokCSC = Resources.Load<CharacterSpawnCard>("Spawncards/CharacterSpawnCards/cscElectricWorm");
            //Now, we make our change
            zapSnokCSC.noElites = false;
            //And we are done with the spawnCard

            //Now we need to create a DirectorCard
            DirectorCard zapSnokDirCard = new DirectorCard();       //Constructor
            zapSnokDirCard.spawnCard = zapSnokCSC;                  //Set the spawn card we edited to be used
            zapSnokDirCard.cost = 4000;                             //This is the default cost
            zapSnokDirCard.selectionWeight = 1;                     //Default weight of 1
            zapSnokDirCard.allowAmbushSpawn = true;                 //Because why not?
            zapSnokDirCard.forbiddenUnlockable = "";                //No need to change this
            zapSnokDirCard.requiredUnlockable = "";                 //No need to change this
            zapSnokDirCard.minimumStageCompletions = 0;             //They cost so much that we don't need to worry about this much
            zapSnokDirCard.preventOverhead = true;                  //Sure I guess, haven't tested enough to know what this does
            zapSnokDirCard.spawnDistance = DirectorCore.MonsterSpawnDistance.Far; //Not sure if this is the default for worms, but it seems to work well
            //And the director card is done
            //Now we need create an EditMonsterCard

            EditMonsterCard zapSnokEdit = new EditMonsterCard(zapSnokCSC.prefab.name, MonsterCategory.Champion, SpawnStages.AllStages, zapSnokDirCard);
            //Using CharacterSpawnCard.prefab.name will always make sure you are editing the right thing.
            //Only time you don't want to do that is if you are replacing one enemy with another. 
            //In that case you want the name from the enemy you want to replace

            //Final thing is to add it to the edit list
            EditMonsterCards.Add(zapSnokEdit);
        }

        public void Start()
        {
            //Nothing to see here, just hitbox edits so shots register more consistently.
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
            //CharacterBody AWBody = archWispBody.GetComponent<CharacterBody>();
            //if( AWBody )
            //{
            //    AWBody.baseNameToken = "Archaic Wisp";
            //}
        }
    }
}