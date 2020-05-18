#if ANCIENTWISP
using System;

using ReinCore;

using RoR2;

using UnityEngine;

namespace Rein.RogueWispPlugin
{
    internal partial class Main
    {
#if CFGBOSS
        private ConfigEntry<Boolean> bossEnabled;
#endif
        partial void AW_Director()
        {
            this.Load += this.AW_SetupSpawns;

#if CFGBOSS
            this.bossEnabled = base.Config.Bind<Boolean>( "Main", "Enable new boss", true,
                "Should the new boss spawn in game? Left as an option in case of major bugs or issues with early version of boss." );
#endif
        }
        // TODO: Director
        //private R2API.DirectorAPI.DirectorCardHolder AW_dirCardHolder;
        private DirectorCard AW_dirCard;
        private void AW_SetupSpawns()
        {
            //this.bossEnabled = base.Config.Bind<Boolean>( "Main", "Enable new boss", true,
            //"Should the new boss spawn in game? Left as an option in case of major bugs or issues with early version of boss." );

            HooksCore.RoR2.CharacterSpawnCard.Awake.On += this.Awake_On;
            var spawnCard = ScriptableObject.CreateInstance<CharacterSpawnCard>();
            HooksCore.RoR2.CharacterSpawnCard.Awake.On -= this.Awake_On;
            spawnCard.directorCreditCost = 500;
            spawnCard.forbiddenAsBoss = false;
            spawnCard.forbiddenFlags = RoR2.Navigation.NodeFlags.NoCharacterSpawn;
            spawnCard.hullSize = HullClassification.Golem;
            spawnCard.loadout = new SerializableLoadout();
            spawnCard.name = "cscwispboss";
            spawnCard.nodeGraphType = RoR2.Navigation.MapNodeGroup.GraphType.Air;
            spawnCard.noElites = false;
            spawnCard.occupyPosition = false;
            spawnCard.prefab = this.AW_master;
            spawnCard.requiredFlags = RoR2.Navigation.NodeFlags.None;
            spawnCard.sendOverNetwork = true;

            var dirCard = new DirectorCard();
            dirCard.allowAmbushSpawn = true;
            dirCard.forbiddenUnlockable = "";
            dirCard.minimumStageCompletions = 0;
            dirCard.preventOverhead = false;
            dirCard.requiredUnlockable = "";
            dirCard.selectionWeight = 1000;
            dirCard.spawnCard = spawnCard;
            dirCard.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;
            this.AW_dirCard = dirCard;

            // TODO: Director
            //this.AW_dirCardHolder = new R2API.DirectorAPI.DirectorCardHolder();
            //this.AW_dirCardHolder.SetCard(dirCard);
            // TODO: Director
            //this.AW_dirCardHolder.SetInteractableCategory(R2API.DirectorAPI.InteractableCategory.None);
            // TODO: Director
            //this.AW_dirCardHolder.SetMonsterCategory(R2API.DirectorAPI.MonsterCategory.Champions);

#if CFGBOSS
            if(this.bossEnabled.Value )
            {
#endif
            SpawnsCore.monsterEdits += this.SpawnsCore_monsterEdits;
            SpawnsCore.familyEdits += this.SpawnsCore_familyEdits;
            // TODO: Director
            //R2API.DirectorAPI.MonsterActions += this.DirectorAPI_MonsterActions;
            // TODO: Director
            //R2API.DirectorAPI.FamilyActions += this.DirectorAPI_FamilyActions;
#if CFGBOSS
            }
#endif
        }

        private void Awake_On( HooksCore.RoR2.CharacterSpawnCard.Awake.Orig orig, CharacterSpawnCard self )
        {
            self.loadout = new SerializableLoadout();
            orig( self );
        }

        private void SpawnsCore_familyEdits( ClassicStageInfo stageInfo, Run runInstance, ClassicStageInfo.MonsterFamily[] possibleFamilies )
        {
            for( Int32 i = 0; i < possibleFamilies.Length; ++i )
            {
                var family = possibleFamilies[i];
                if( family.familySelectionChatString != "FAMILY_WISP" ) continue;
                var startCount = family.monsterFamilyCategories.categories.Length;
                for( Int32 j = 0; j < startCount; ++j )
                {
                    var category = family.monsterFamilyCategories.categories[j];
                    if( category.name.ToLower() != "champions" ) continue;
                    var ind = category.cards.Length;
                    Array.Resize<DirectorCard>( ref category.cards, ind + 1 );
                    category.cards[ind] = this.AW_dirCard;

                    family.monsterFamilyCategories.categories[j] = category;
                    break;
                }
                possibleFamilies[i] = family;
                break;
            }
        }

        private void SpawnsCore_monsterEdits( ClassicStageInfo stageInfo, Run runInstance, DirectorCardCategorySelection monsterSelection )
        {
            //Main.LogI( "Monster" );
            for( Int32 i = 0; i < monsterSelection.categories.Length; ++i )
            {
                var category = monsterSelection.categories[i];
                if( category.name.ToLower() != "champions" ) continue;

                var ind = category.cards.Length;
                Array.Resize<DirectorCard>( ref category.cards, ind + 1 );
                category.cards[ind] = this.AW_dirCard;

                monsterSelection.categories[i] = category;
                break;
            }
        }
    }
}
#endif