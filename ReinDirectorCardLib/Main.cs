using BepInEx;
using RoR2;
using UnityEngine;
using R2API.Utils;
using System.Collections.Generic;
using static ReinDirectorCardLib.AddedMonsterCard;
using System;

namespace ReinDirectorCardLib
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.ReinDirectorCardLib", "ReinDirectorCardLibrary", "1.0.0")]

    public class ReinDirectorCardLib : BaseUnityPlugin
    {
        public static List<AddedMonsterCard> AddedMonsterCards = new List<AddedMonsterCard>();
        public static List<EditMonsterCard> EditMonsterCards = new List<EditMonsterCard>();

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
            On.RoR2.CharacterSpawnCard.Awake += (orig, self) =>
            {
                if( self.loadout == null )
                {
                    self.loadout = new SerializableLoadout();
                }
            };
        }

        public void Start()
        {
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
                        int extraChampions = 0;
                        int extraMiniBosses = 0;
                        int extraBasicMonsters = 0;

                        switch ( sceneDef.sceneName )
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

                            case "wispgraveyard":
                                stage = SpawnStages.ScorchedAcres;
                                break;

                            case "dampcavesimple":
                                stage = SpawnStages.AbyssalDepths;
                                break;

                            case "goldshores":
                                stage = SpawnStages.GildedCoast;
                                break;

                            default:
                                Debug.Log(sceneDef.sceneName + " is not registered as a stage for directorcardlib.");
                                break;
                        }

                        List<AddedMonsterCard> newChampions = new List<AddedMonsterCard>();
                        List<AddedMonsterCard> newMinibosses = new List<AddedMonsterCard>();
                        List<AddedMonsterCard> newBasicMonsters = new List<AddedMonsterCard>();

                        foreach( AddedMonsterCard amc in AddedMonsterCards )
                        {
                            if( amc.stages.HasFlag( stage ) )
                            {
                                switch( amc.category )
                                {
                                    case MonsterCategory.BasicMonster:
                                        newBasicMonsters.Add(amc);
                                        break;

                                    case MonsterCategory.Miniboss:
                                        newMinibosses.Add(amc);
                                        break;

                                    case MonsterCategory.Champion:
                                        newChampions.Add(amc);
                                        break;
                                }
                            }
                        }

                        Dictionary<string, EditMonsterCard> championEdits = new Dictionary<string, EditMonsterCard>();
                        Dictionary<string, EditMonsterCard> minibossEdits = new Dictionary<string, EditMonsterCard>();
                        Dictionary<string, EditMonsterCard> basicEdits = new Dictionary<string, EditMonsterCard>();
                        bool editChamps = false;
                        bool editMiniboss = false;
                        bool editBasic = false;

                        foreach ( EditMonsterCard emc in EditMonsterCards )
                        {
                            if( emc.stages.HasFlag( stage ) && emc.doEdits )
                            {
                                switch( emc.category )
                                {
                                    case MonsterCategory.BasicMonster:
                                        basicEdits.Add(emc.monsterName, emc);
                                        editBasic = true;
                                        break;

                                    case MonsterCategory.Miniboss:
                                        minibossEdits.Add(emc.monsterName, emc);
                                        editMiniboss = true;
                                        break;

                                    case MonsterCategory.Champion:
                                        championEdits.Add(emc.monsterName, emc);
                                        editChamps = true;
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

                        if (extraChampions != 0 || editChamps)
                        {
                            Debug.Log("Generating Champion cards");
                            DirectorCardCategorySelection.Category champions = new DirectorCardCategorySelection.Category
                            {
                                name = cats.categories[0].name,
                                selectionWeight = cats.categories[0].selectionWeight,
                                cards = new DirectorCard[baseChampions + extraChampions]
                            };

                            DirectorCard temp;
                            EditMonsterCard temp2;
                            AddedMonsterCard temp3;

                            for (int i = 0; i < baseChampions; i++)
                            {
                                temp = cats.categories[0].cards[i];
                                if( editChamps && championEdits.ContainsKey(temp.spawnCard.prefab.name ))
                                {
                                    temp2 = championEdits[temp.spawnCard.prefab.name];
                                    if( temp2.doEdits )
                                    {
                                        Debug.Log("Applying edit to: " + temp2.monsterName + " from: " + temp2.modFilePath + " Method: " + temp2.modMethodName + " Line: " + temp2.modLineNumber);
                                        champions.cards[i] = temp2.monster;
                                        continue;
                                    }
                                    else
                                    {
                                        Debug.Log("Skipping edit to: " + temp2.monsterName + " from: " + temp2.modFilePath + " Method: " + temp2.modMethodName + " Line: " + temp2.modLineNumber);
                                    }
                                }
                                champions.cards[i] = cats.categories[0].cards[i];
                            }
                            for (int i = 0; i < extraChampions; i++)
                            {
                                temp3 = newChampions[i];
                                Debug.Log("New monster: " + temp3.monster.spawnCard.prefab.name + " added by: " + temp3.modFilePath + " Method: " + temp3.modMethodName + " Line: " + temp3.modLineNumber);
                                champions.cards[baseChampions + i] = temp3.monster;
                            }
                            cats.categories[0] = champions;
                        }
                        if (extraMiniBosses != 0 || editMiniboss)
                        {
                            Debug.Log("Generating Miniboss cards");
                            DirectorCardCategorySelection.Category miniBosses = new DirectorCardCategorySelection.Category
                            {
                                name = cats.categories[1].name,
                                selectionWeight = cats.categories[1].selectionWeight,
                                cards = new DirectorCard[baseMiniBosses + extraMiniBosses]
                            };

                            DirectorCard temp;
                            EditMonsterCard temp2;
                            AddedMonsterCard temp3;

                            for (int i = 0; i < baseMiniBosses; i++)
                            {
                                temp = cats.categories[1].cards[i];
                                if (editMiniboss && minibossEdits.ContainsKey(temp.spawnCard.prefab.name))
                                {
                                    temp2 = minibossEdits[temp.spawnCard.prefab.name];
                                    if (temp2.doEdits)
                                    {
                                        Debug.Log("Applying edit to: " + temp2.monsterName + " from: " + temp2.modFilePath + " Method: " + temp2.modMethodName + " Line: " + temp2.modLineNumber);
                                        miniBosses.cards[i] = minibossEdits[temp.spawnCard.prefab.name].monster;
                                        continue;
                                    }
                                    else
                                    {
                                        Debug.Log("Skipping edit to: " + temp2.monsterName + " from: " + temp2.modFilePath + " Method: " + temp2.modMethodName + " Line: " + temp2.modLineNumber);
                                    }
                                }
                                miniBosses.cards[i] = cats.categories[1].cards[i];
                            }
                            for (int i = 0; i < extraMiniBosses; i++)
                            {
                                temp3 = newMinibosses[i];
                                Debug.Log("New monster: " + temp3.monster.spawnCard.prefab.name + " added by: " + temp3.modFilePath + " Method: " + temp3.modMethodName + " Line: " + temp3.modLineNumber);
                                miniBosses.cards[baseMiniBosses + i] = temp3.monster;
                            }
                            cats.categories[1] = miniBosses;
                        }
                        if (extraBasicMonsters != 0 || editBasic)
                        {
                            Debug.Log("Generating Basic Monster cards");
                            DirectorCardCategorySelection.Category basicMonsters = new DirectorCardCategorySelection.Category
                            {
                                name = cats.categories[2].name,
                                selectionWeight = cats.categories[2].selectionWeight,
                                cards = new DirectorCard[baseBasicMonsters + extraBasicMonsters]
                            };

                            DirectorCard temp;
                            EditMonsterCard temp2;
                            AddedMonsterCard temp3;

                            for (int i = 0; i < baseBasicMonsters; i++)
                            {
                                temp = cats.categories[2].cards[i];
                                if (editBasic && basicEdits.ContainsKey(temp.spawnCard.prefab.name))
                                {
                                    temp2 = basicEdits[temp.spawnCard.prefab.name];
                                    if (temp2.doEdits)
                                    {
                                        Debug.Log("Applying edit to: " + temp2.monsterName + " from: " + temp2.modFilePath + " Method: " + temp2.modMethodName + " Line: " + temp2.modLineNumber);
                                        basicMonsters.cards[i] = basicEdits[temp.spawnCard.prefab.name].monster;
                                        continue;
                                    }
                                    else
                                    {
                                        Debug.Log("Skipping edit to: " + temp2.monsterName + " from: " + temp2.modFilePath + " Method: " + temp2.modMethodName + " Line: " + temp2.modLineNumber);
                                    }
                                }
                                basicMonsters.cards[i] = cats.categories[2].cards[i];
                            }
                            for (int i = 0; i < extraBasicMonsters; i++)
                            {
                                temp3 = newBasicMonsters[i];
                                Debug.Log("New monster: " + temp3.monster.spawnCard.prefab.name + " added by: " + temp3.modFilePath + " Method: " + temp3.modMethodName + " Line: " + temp3.modLineNumber);
                                basicMonsters.cards[baseBasicMonsters + i] = temp3.monster;
                            }
                            cats.categories[2] = basicMonsters;
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