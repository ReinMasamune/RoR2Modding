#if ARCHAICWISP
using System;
using BepInEx.Configuration;

using RoR2;

using ReinCore;

using UnityEngine;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        private static DirectorCard ArW_dirCard;

        partial void CreateArchaicWisp()
        {
            this.Load += Arw_Awake;
        }

        private static void Arw_Awake()
        {
            var archWispSpawnCard = Resources.Load<CharacterSpawnCard>( "SpawnCards/CharacterSpawnCards/cscArchWisp");
            archWispSpawnCard.directorCreditCost = 400;

            var archWispDirCard = new DirectorCard();
            archWispDirCard.allowAmbushSpawn = true;
            archWispDirCard.forbiddenUnlockable = "";
            archWispDirCard.minimumStageCompletions = 6;
            archWispDirCard.preventOverhead = false;
            archWispDirCard.requiredUnlockable = "";
            archWispDirCard.selectionWeight = 1;
            archWispDirCard.spawnCard = archWispSpawnCard;
            archWispDirCard.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;

            ArW_dirCard = archWispDirCard;

            SpawnsCore.monsterEdits += SpawnsCore_monsterEdits1;
            SpawnsCore.familyEdits += SpawnsCore_familyEdits1;



            LanguageCore.AddLanguageToken("ARCHWISP_BODY_NAME", "Archaic Wisp");
        }

        private static void SpawnsCore_familyEdits1(ClassicStageInfo stageInfo, Run runInstance, ClassicStageInfo.MonsterFamily[] possibleFamilies)
        {
            if(runInstance.selectedDifficulty < DifficultyIndex.Hard && runInstance is not EclipseRun) return;
            for(Int32 i = 0; i < possibleFamilies.Length; ++i)
            {
                var family = possibleFamilies[i];
                if(family.familySelectionChatString != "FAMILY_WISP") continue;
                var startCount = family.monsterFamilyCategories.categories.Length;
                for(Int32 j = 0; j < startCount; ++j)
                {
                    var category = family.monsterFamilyCategories.categories[j];
                    if(category.name.ToLower() != "minibosses") continue;
                    var ind = category.cards.Length;
                    Array.Resize<DirectorCard>(ref category.cards, ind + 1);
                    category.cards[ind] = ArW_dirCard;

                    family.monsterFamilyCategories.categories[j] = category;
                    break;
                }
                possibleFamilies[i] = family;
                break;
            }
        }
        private static void SpawnsCore_monsterEdits1(ClassicStageInfo stageInfo, Run runInstance, DirectorCardCategorySelection monsterSelection)
        {
            if(runInstance.selectedDifficulty < DifficultyIndex.Hard && runInstance is not EclipseRun) return;
            for(Int32 i = 0; i < monsterSelection.categories.Length; ++i)
            {
                var category = monsterSelection.categories[i];
                if(category.name.ToLower() != "minibosses") continue;

                var ind = category.cards.Length;
                Array.Resize<DirectorCard>(ref category.cards, ind + 1);
                category.cards[ind] = ArW_dirCard;

                monsterSelection.categories[i] = category;
                break;
            }
        }
    }
}
#endif
