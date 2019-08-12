using BepInEx;
using RoR2;
using UnityEngine;
using R2API.Utils;
using System.Collections.Generic;
using static ReinDirectorCardLib.AddedMonsterCard;

namespace ReinDirectorCardLib
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.ReinDirectorCardLib", "ReinDirectorCardLibrary", "1.0.0")]

    public class ReinDirectorCardLib : BaseUnityPlugin
    {
        public static List<AddedMonsterCard> AddedMonsterCards = new List<AddedMonsterCard>();
        
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
                            DirectorCardCategorySelection.Category champions = new DirectorCardCategorySelection.Category
                            {
                                name = cats.categories[0].name,
                                selectionWeight = cats.categories[0].selectionWeight,
                                cards = new DirectorCard[baseChampions + extraChampions]
                            };

                            for (int i = 0; i < baseChampions; i++)
                            {
                                champions.cards[i] = cats.categories[0].cards[i];
                            }
                            for (int i = 0; i < extraChampions; i++)
                            {
                                champions.cards[baseChampions + i] = newChampions[i];
                            }
                            cats.categories[0] = champions;
                        }
                        if (extraMiniBosses != 0)
                        {
                            DirectorCardCategorySelection.Category miniBosses = new DirectorCardCategorySelection.Category
                            {
                                name = cats.categories[1].name,
                                selectionWeight = cats.categories[1].selectionWeight,
                                cards = new DirectorCard[baseMiniBosses + extraMiniBosses]
                            };

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
                            DirectorCardCategorySelection.Category basicMonsters = new DirectorCardCategorySelection.Category
                            {
                                name = cats.categories[2].name,
                                selectionWeight = cats.categories[2].selectionWeight,
                                cards = new DirectorCard[baseBasicMonsters + extraBasicMonsters]
                            };

                            for (int i = 0; i < baseBasicMonsters; i++)
                            {
                                basicMonsters.cards[i] = cats.categories[2].cards[i];
                            }
                            for (int i = 0; i < extraBasicMonsters; i++)
                            {
                                basicMonsters.cards[baseBasicMonsters + i] = newBasicMonsters[i];
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