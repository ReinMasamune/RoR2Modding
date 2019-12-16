using RoR2;
using R2API.Utils;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace RoR2Plugin
{
    public static partial class DirectorAPI
    {
        private static bool inUse = false;

        private static void StageAwake( On.RoR2.ClassicStageInfo.orig_Awake orig, ClassicStageInfo self )
        {
            var stage = GetStage(self);

            ApplySettingsChanges( self, stage );

            ApplyMonsterChanges( self, stage );
            ApplyInteractableChanges( self, stage );

            ApplyFamilyChanges( self, stage );

            orig( self );
        }

        private static T[] AddToArray<T>( ref T[] array, T obj )
        {
            int size = array.Length;
            Array.Resize<T>( ref array, size + 1 );
            array[size] = obj;

            return array;
        }

        private static StageInfo GetStage( ClassicStageInfo stage )
        {
            StageInfo stageInfo = new StageInfo
            {
                stage = Stage.Custom,
                customStageName = "",
            };
            SceneInfo info = stage.GetComponent<SceneInfo>();
            if( !info ) return stageInfo;
            SceneDef scene = info.sceneDef;
            if( !scene ) return stageInfo;

            switch( scene.sceneName )
            {
                case "golemplains":
                    stageInfo.stage = Stage.TitanicPlains;
                    break;

                case "blackbeach":
                    stageInfo.stage = Stage.DistantRoost;
                    break;

                case "goolake":
                    stageInfo.stage = Stage.AbandonedAqueduct;
                    break;

                case "foggyswamp":
                    stageInfo.stage = Stage.WetlandAspect;
                    break;

                case "frozenwall":
                    stageInfo.stage = Stage.RallypointDelta;
                    break;

                case "wispgraveyard":
                    stageInfo.stage = Stage.ScorchedAcres;
                    break;

                case "dampcavesimple":
                    stageInfo.stage = Stage.AbyssalDepths;
                    break;

                case "shipgraveyard":
                    stageInfo.stage = Stage.SirensCall;
                    break;

                case "goldshores":
                    stageInfo.stage = Stage.GildedCoast;
                    break;
                default:
                    stageInfo.stage = Stage.Custom;
                    stageInfo.customStageName = scene.sceneName;
                    break;
            }
            return stageInfo;
        }

        private static InteractableCategory GetInteractableCategory( string s )
        {
            switch( s )
            {
                default:
                    return InteractableCategory.None;
                case "Chests":
                    return InteractableCategory.Chests;
                case "Barrels":
                    return InteractableCategory.Barrels;
                case "Shrines":
                    return InteractableCategory.Shrines;
                case "Drones":
                    return InteractableCategory.Drones;
                case "Misc":
                    return InteractableCategory.Misc;
                case "Rare":
                    return InteractableCategory.Rare;
                case "Duplicator":
                    return InteractableCategory.Duplicator;
            }
        }

        private static MonsterCategory GetMonsterCategory( string s )
        {
            switch( s )
            {
                default:
                    return MonsterCategory.None;
                case "Champions":
                    return MonsterCategory.Champions;
                case "Minibosses":
                    return MonsterCategory.Minibosses;
                case "Basic Monsters":
                    return MonsterCategory.BasicMonsters;
            }
        }

        private static StageSettings GetStageSettings( ClassicStageInfo self )
        {
            StageSettings set = new StageSettings
            {
                sceneDirectorInteractableCredits = self.sceneDirectorInteractibleCredits,
                sceneDirectorMonsterCredits = self.sceneDirectorMonsterCredits
            };

            set.bonusCreditObjects = new Dictionary<GameObject, int>();
            for( int i = 0; i < self.bonusInteractibleCreditObjects.Length; i++ )
            {
                var bonusObj = self.bonusInteractibleCreditObjects[i];
                set.bonusCreditObjects[bonusObj.objectThatGrantsPointsIfEnabled] = bonusObj.points;
            }

            set.interactableCategoryWeights = new Dictionary<InteractableCategory, float>();
            var interCats = self.GetFieldValue<DirectorCardCategorySelection>("interactableCategories");
            for( int i = 0; i < interCats.categories.Length; i++ )
            {
                var cat = interCats.categories[i];
                set.interactableCategoryWeights[GetInteractableCategory( cat.name )] = cat.selectionWeight;
            }

            set.monsterCategoryWeights = new Dictionary<MonsterCategory, float>();
            var monstCats = self.GetFieldValue<DirectorCardCategorySelection>("monsterCategories");
            for( int i = 0; i < monstCats.categories.Length; i++ )
            {
                var cat = monstCats.categories[i];
                set.monsterCategoryWeights[GetMonsterCategory( cat.name )] = cat.selectionWeight;
            }

            return set;
        }

        private static void SetStageSettings( ClassicStageInfo self, StageSettings set )
        {
            self.sceneDirectorInteractibleCredits = set.sceneDirectorInteractableCredits;
            self.sceneDirectorMonsterCredits = set.sceneDirectorMonsterCredits;

            var keys = set.bonusCreditObjects.Keys.ToArray();
            var bonuses = new ClassicStageInfo.BonusInteractibleCreditObject[keys.Length];
            for( int i = 0; i < keys.Length; i++ )
            {
                bonuses[i] = new ClassicStageInfo.BonusInteractibleCreditObject
                {
                    objectThatGrantsPointsIfEnabled = keys[i],
                    points = set.bonusCreditObjects[keys[i]]
                };
            }
            self.bonusInteractibleCreditObjects = bonuses;

            var interCats = self.GetFieldValue<DirectorCardCategorySelection>("interactableCategories");
            for( int i = 0; i < interCats.categories.Length; i++ )
            {
                var cat = interCats.categories[i];
                InteractableCategory intCat = GetInteractableCategory( cat.name );
                cat.selectionWeight = set.interactableCategoryWeights[intCat];
                interCats.categories[i] = cat;
            }

            var monstCats = self.GetFieldValue<DirectorCardCategorySelection>("monsterCategories");
            for( int i = 0; i < monstCats.categories.Length; i++ )
            {
                var cat = monstCats.categories[i];
                MonsterCategory monCat = GetMonsterCategory( cat.name );
                cat.selectionWeight = set.monsterCategoryWeights[monCat];
                monstCats.categories[i] = cat;
            }
        }

        private static MonsterFamilyHolder GetMonsterFamilyHolder( ClassicStageInfo.MonsterFamily family )
        {
            MonsterFamilyHolder hold = new MonsterFamilyHolder
            {
                maxStageCompletion = family.maximumStageCompletion,
                minStageCompletion = family.minimumStageCompletion,
                familySelectionWeight = family.selectionWeight,
                selectionChatString = family.familySelectionChatString
            };

            var cards = family.monsterFamilyCategories.categories;

            for( int i = 0; i < cards.Length; i++ )
            {
                var cat = cards[i];

                switch( cat.name )
                {
                    case "Basic Monsters":
                        hold.familyBasicMonsterWeight = cat.selectionWeight;
                        hold.familyBasicMonsters = cat.cards.ToList();
                        break;
                    case "Minibosses":
                        hold.familyMinibossWeight = cat.selectionWeight;
                        hold.familyMinibosses = cat.cards.ToList();
                        break;
                    case "Champions":
                        hold.familyChampionWeight = cat.selectionWeight;
                        hold.familyChampions = cat.cards.ToList();
                        break;
                }
            }

            return hold;
        }

        private static ClassicStageInfo.MonsterFamily GetMonsterFamily( MonsterFamilyHolder holder )
        {
            return new ClassicStageInfo.MonsterFamily
            {
                familySelectionChatString = holder.selectionChatString,
                maximumStageCompletion = holder.maxStageCompletion,
                minimumStageCompletion = holder.minStageCompletion,
                selectionWeight = holder.familySelectionWeight,
                monsterFamilyCategories = new DirectorCardCategorySelection
                {
                    categories = new DirectorCardCategorySelection.Category[3]
                    {
                        new DirectorCardCategorySelection.Category
                        {
                            name = "Champions",
                            selectionWeight = holder.familyChampionWeight,
                            cards = holder.familyChampions.ToArray()
                        },
                        new DirectorCardCategorySelection.Category
                        {
                            name = "Minibosses",
                            selectionWeight = holder.familyMinibossWeight,
                            cards = holder.familyMinibosses.ToArray()
                        },
                        new DirectorCardCategorySelection.Category
                        {
                            name = "Basic Monsters",
                            selectionWeight = holder.familyBasicMonsterWeight,
                            cards = holder.familyBasicMonsters.ToArray()
                        }
                    }
                }
            };
        }

        private static void ApplySettingsChanges( ClassicStageInfo self, StageInfo stage )
        {
            StageSettings settings = GetStageSettings( self );
            stageSettingsActions?.Invoke( settings, stage );
            SetStageSettings( self, settings );
        }

        private static void ApplyMonsterChanges( ClassicStageInfo self, StageInfo stage )
        {
            var monsters = self.GetFieldValue<DirectorCardCategorySelection>("monsterCategories");
            List<DirectorCardHolder> monsterCards = new List<DirectorCardHolder>();

            for( int i = 0; i < monsters.categories.Length; i++ )
            {
                DirectorCardCategorySelection.Category cat = monsters.categories[i];
                MonsterCategory monstCat = GetMonsterCategory( cat.name );
                InteractableCategory interCat = GetInteractableCategory( cat.name);
                for( int j = 0; j < cat.cards.Length; j++ )
                {
                    monsterCards.Add( new DirectorCardHolder
                    {
                        interactableCategory = interCat,
                        monsterCategory = monstCat,
                        card = cat.cards[j]
                    } );
                }
            }

            monsterActions?.Invoke( monsterCards, stage );

            DirectorCard[] monsterBasic = new DirectorCard[0];
            DirectorCard[] monsterSub = new DirectorCard[0];
            DirectorCard[] monsterChamp = new DirectorCard[0];

            for( int i = 0; i < monsterCards.Count; i++ )
            {
                DirectorCardHolder hold = monsterCards[i];
                switch( hold.monsterCategory )
                {
                    default:
                        Debug.Log( "Wtf are you doing..." );
                        break;
                    case MonsterCategory.BasicMonsters:
                        AddToArray<DirectorCard>( ref monsterBasic, hold.card );
                        break;
                    case MonsterCategory.Champions:
                        AddToArray<DirectorCard>( ref monsterChamp, hold.card );
                        break;
                    case MonsterCategory.Minibosses:
                        AddToArray<DirectorCard>( ref monsterSub, hold.card );
                        break;
                }
            }
            for( int i = 0; i < monsters.categories.Length; i++ )
            {
                DirectorCardCategorySelection.Category cat = monsters.categories[i];
                switch( cat.name )
                {
                    default:
                        Debug.Log( cat.name );
                        break;
                    case "Champions":
                        cat.cards = monsterChamp;
                        break;
                    case "Minibosses":
                        cat.cards = monsterSub;
                        break;
                    case "Basic Monsters":
                        cat.cards = monsterBasic;
                        break;
                }
                monsters.categories[i] = cat;
            }
        }

        private static void ApplyInteractableChanges( ClassicStageInfo self, StageInfo stage )
        {
            var interactables = self.GetFieldValue<DirectorCardCategorySelection>("interactableCategories");
            List<DirectorCardHolder> interactableCards = new List<DirectorCardHolder>();

            for( int i = 0; i < interactables.categories.Length; i++ )
            {
                DirectorCardCategorySelection.Category cat = interactables.categories[i];
                MonsterCategory monstCat = GetMonsterCategory( cat.name );
                InteractableCategory interCat = GetInteractableCategory( cat.name );
                for( int j = 0; j < cat.cards.Length; j++ )
                {
                    interactableCards.Add( new DirectorCardHolder
                    {
                        interactableCategory = interCat,
                        monsterCategory = monstCat,
                        card = cat.cards[j]
                    } );
                }
            }

            interactableActions?.Invoke( interactableCards, stage );

            DirectorCard[] interChests = new DirectorCard[0];
            DirectorCard[] interBarrels = new DirectorCard[0];
            DirectorCard[] interShrines = new DirectorCard[0];
            DirectorCard[] interDrones = new DirectorCard[0];
            DirectorCard[] interMisc = new DirectorCard[0];
            DirectorCard[] interRare = new DirectorCard[0];
            DirectorCard[] interDupe = new DirectorCard[0];

            for( int i = 0; i < interactableCards.Count; i++ )
            {
                DirectorCardHolder hold = interactableCards[i];
                switch( hold.interactableCategory )
                {
                    default:
                        Debug.Log( "Wtf are you doing..." );
                        break;
                    case InteractableCategory.Chests:
                        AddToArray<DirectorCard>( ref interChests, hold.card );
                        break;
                    case InteractableCategory.Barrels:
                        AddToArray<DirectorCard>( ref interBarrels, hold.card );
                        break;
                    case InteractableCategory.Drones:
                        AddToArray<DirectorCard>( ref interDrones, hold.card );
                        break;
                    case InteractableCategory.Duplicator:
                        AddToArray<DirectorCard>( ref interDupe, hold.card );
                        break;
                    case InteractableCategory.Misc:
                        AddToArray<DirectorCard>( ref interMisc, hold.card );
                        break;
                    case InteractableCategory.Rare:
                        AddToArray<DirectorCard>( ref interRare, hold.card );
                        break;
                    case InteractableCategory.Shrines:
                        AddToArray<DirectorCard>( ref interShrines, hold.card );
                        break;
                }
            }
            for( int i = 0; i < interactables.categories.Length; i++ )
            {
                DirectorCardCategorySelection.Category cat = interactables.categories[i];
                switch( cat.name )
                {
                    default:
                        Debug.Log( cat.name );
                        break;
                    case "Chests":
                        cat.cards = interChests;
                        break;
                    case "Barrels":
                        cat.cards = interBarrels;
                        break;
                    case "Shrines":
                        cat.cards = interShrines;
                        break;
                    case "Drones":
                        cat.cards = interDrones;
                        break;
                    case "Misc":
                        cat.cards = interMisc;
                        break;
                    case "Rare":
                        cat.cards = interRare;
                        break;
                    case "Duplicator":
                        cat.cards = interDupe;
                        break;
                }
                interactables.categories[i] = cat;
            }
        }

        private static void ApplyFamilyChanges( ClassicStageInfo self, StageInfo stage )
        {
            List<MonsterFamilyHolder> familyHolds = new List<MonsterFamilyHolder>(self.possibleMonsterFamilies.Length);
            for( int i = 0; i < self.possibleMonsterFamilies.Length; i++ )
            {
                //Debug.Log( i );
                familyHolds[i] = GetMonsterFamilyHolder( self.possibleMonsterFamilies[i] );
            }
            familyActions?.Invoke( familyHolds , stage);
            self.possibleMonsterFamilies = new ClassicStageInfo.MonsterFamily[familyHolds.Count];
            for( int i = 0; i < familyHolds.Count; i++ )
            {
                //Debug.Log( i );
                self.possibleMonsterFamilies[i] = GetMonsterFamily(familyHolds[i]);
            }

        }
    }
}
