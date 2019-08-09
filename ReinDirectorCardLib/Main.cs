using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using System.Collections.Generic;
using RoR2.Navigation;
using System;
using ReinDirectorCardLibrary;

namespace ReinDirectorCardLibrary
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.ReinDirectorCardLibrary", "ReinDirectorCardLibrary", "1.0.0")]

    public class ReinDirectorCardManager : BaseUnityPlugin
    {
        public static List<AddedMonsterCard> AddedMonsterCards = new List<AddedMonsterCard>();

        //[Serializable]
        public struct AddedMonsterCard
        {
            public string modNameSpace;
            public MonsterCategory category;
            public SpawnStages stages;
            public DirectorCard monster;
        }

        public enum MonsterCategory
        {
            Champion = 0,
            Miniboss = 1,
            BasicMonster = 2
        }

        [Flags]
        public enum SpawnStages
        {
            DistantRoost = 1,
            TitanicPlains = 2,
            WetlandAspect = 4,
            AbandonedAqueduct = 8,
            RallypointDelta = 16,
            ScorchedAcres = 32,
            AbyssalDepths = 64,
            GildedCoast = 128,
            AllStages = 255,
            InvalidStage = 256
        }

        public void Awake()
        {
            /* Some Example Code
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

            AddedMonsterCard archWisp = new AddedMonsterCard();
            archWisp.modNameSpace = "ReinMonsterLib Testing";
            archWisp.monster = archWispCard;
            archWisp.stages = SpawnStages.AllStages;
            archWisp.category = MonsterCategory.Miniboss;

            AddedMonsterCards.Add(archWisp);
            */
        }
        
        public void Start()
        {
            int extraChampions = 0;
            int extraMiniBosses = 0;
            int extraBasicMonsters = 0;

            On.RoR2.ClassicStageInfo.Awake += (orig, self) =>
            {
                SpawnStages stage = SpawnStages.InvalidStage;
                Debug.Log("Preparing to load new directorcards");
                SceneInfo sceneInfo = self.GetComponent<SceneInfo>();
                if( sceneInfo )
                {
                    SceneDef sceneDef = sceneInfo.sceneDef;
                    if( sceneDef )
                    {
                        switch( sceneDef.sceneName )
                        {
                            case "golemplains":
                                stage = SpawnStages.TitanicPlains;
                                break;

                            case "blackbeach":
                                stage = SpawnStages.DistantRoost;
                                break;

                            case "goolake":
                                stage = SpawnStages.AbandonedAqueduct;
                                break;

                            case "foggyswamp":
                                stage = SpawnStages.WetlandAspect;
                                break;

                            case "frozenwall":
                                stage = SpawnStages.RallypointDelta;
                                break;

                            case "dampcavesimple":
                                stage = SpawnStages.AbyssalDepths;
                                break;

                            case "goldshores":
                                stage = SpawnStages.GildedCoast;
                                break;

                            default:
                                Debug.Log(sceneDef.sceneName + " is not registered as a stage for directorcard library");
                                break;
                        }

                        List<DirectorCard> newChampions = new List<DirectorCard>();
                        List<DirectorCard> newMinibosses = new List<DirectorCard>();
                        List<DirectorCard> newBasicMonsters = new List<DirectorCard>();

                        foreach( AddedMonsterCard amc in AddedMonsterCards )
                        {
                            if( amc.stages.HasFlag( stage ) )
                            {
                                switch( amc.category )
                                {
                                    case MonsterCategory.BasicMonster:
                                        newBasicMonsters.Add(amc.monster);
                                        break;

                                    case MonsterCategory.Miniboss:
                                        newMinibosses.Add(amc.monster);
                                        break;

                                    case MonsterCategory.Champion:
                                        newChampions.Add(amc.monster);
                                        break;
                                }
                            }
                        }

                        extraBasicMonsters = newBasicMonsters.Count;
                        extraChampions = newChampions.Count;
                        extraMiniBosses = newMinibosses.Count;


                        DirectorCardCategorySelection cats = self.GetFieldValue<DirectorCardCategorySelection>("monsterCategories");
                        int baseChampions = cats.categories[0].cards.Length;
                        int baseMiniBosses = cats.categories[1].cards.Length;
                        int baseBasicMonsters = cats.categories[2].cards.Length;

                        if (extraChampions != 0)
                        {
                            DirectorCardCategorySelection.Category miniBosses = new DirectorCardCategorySelection.Category();
                            miniBosses.name = cats.categories[0].name;
                            miniBosses.selectionWeight = cats.categories[0].selectionWeight;
                            miniBosses.cards = new DirectorCard[baseMiniBosses + extraMiniBosses];
                            for (int i = 0; i < baseMiniBosses; i++)
                            {
                                miniBosses.cards[i] = cats.categories[0].cards[i];
                            }
                            for (int i = 0; i < extraMiniBosses; i++)
                            {
                                miniBosses.cards[baseMiniBosses + i] = newMinibosses[i];
                            }
                            cats.categories[0] = miniBosses;
                        }
                        if (extraMiniBosses != 0)
                        {
                            DirectorCardCategorySelection.Category miniBosses = new DirectorCardCategorySelection.Category();
                            miniBosses.name = cats.categories[1].name;
                            miniBosses.selectionWeight = cats.categories[1].selectionWeight;
                            miniBosses.cards = new DirectorCard[baseMiniBosses + extraMiniBosses];
                            for (int i = 0; i < baseMiniBosses; i++)
                            {
                                miniBosses.cards[i] = cats.categories[1].cards[i];
                            }
                            for (int i = 0; i < extraMiniBosses; i++)
                            {
                                miniBosses.cards[baseMiniBosses + i] = newMinibosses[i];
                            }
                            cats.categories[1] = miniBosses;
                        }
                        if (extraBasicMonsters != 0)
                        {
                            DirectorCardCategorySelection.Category miniBosses = new DirectorCardCategorySelection.Category();
                            miniBosses.name = cats.categories[2].name;
                            miniBosses.selectionWeight = cats.categories[2].selectionWeight;
                            miniBosses.cards = new DirectorCard[baseMiniBosses + extraMiniBosses];
                            for (int i = 0; i < baseMiniBosses; i++)
                            {
                                miniBosses.cards[i] = cats.categories[2].cards[i];
                            }
                            for (int i = 0; i < extraMiniBosses; i++)
                            {
                                miniBosses.cards[baseMiniBosses + i] = newMinibosses[i];
                            }
                            cats.categories[2] = miniBosses;
                        }

                        self.SetFieldValue<DirectorCardCategorySelection>("monsterCategories", cats);

                    }
                    else
                    {
                        Debug.Log("There is no SceneDef, you are wrong.");
                    }
                }
                else
                {
                    Debug.Log("There is no scene info component attached, wtf are you doing?");
                }

                orig(self);
            };
        }
    }
}