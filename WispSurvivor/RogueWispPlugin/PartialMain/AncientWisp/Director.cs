#if ANCIENTWISP
using BepInEx.Configuration;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using ReinCore;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        //private ConfigEntry<Boolean> bossEnabled;
        partial void AW_Director()
        {
            this.Load += this.AW_SetupSpawns;

            //this.bossEnabled = base.Config.Bind<Boolean>( "Main", "Enable new boss", true,
            //    "Should the new boss spawn in game? Left as an option in case of major bugs or issues with early version of boss." );
        }
        // TODO: Director
        //private R2API.DirectorAPI.DirectorCardHolder AW_dirCardHolder;
        private DirectorCard AW_dirCard;
        private void AW_SetupSpawns()
        {
            //this.bossEnabled = base.Config.Bind<Boolean>( "Main", "Enable new boss", true,
            //"Should the new boss spawn in game? Left as an option in case of major bugs or issues with early version of boss." );

            HooksCore.on_RoR2_CharacterSpawnCard_Awake += this.HooksCore_on_RoR2_CharacterSpawnCard_Awake;
            var spawnCard = ScriptableObject.CreateInstance<CharacterSpawnCard>();
            HooksCore.on_RoR2_CharacterSpawnCard_Awake -= this.HooksCore_on_RoR2_CharacterSpawnCard_Awake;
            spawnCard.directorCreditCost = 2000;
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
            dirCard.minimumStageCompletions = 6;
            dirCard.preventOverhead = false;
            dirCard.requiredUnlockable = "";
            dirCard.selectionWeight = 1;
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

            //if(this.bossEnabled.Value )
            //{
                SpawnsCore.monsterEdits += this.SpawnsCore_monsterEdits;
                SpawnsCore.familyEdits += this.SpawnsCore_familyEdits;
                // TODO: Director
                //R2API.DirectorAPI.MonsterActions += this.DirectorAPI_MonsterActions;
                // TODO: Director
                //R2API.DirectorAPI.FamilyActions += this.DirectorAPI_FamilyActions;
            //}
        }

        private void HooksCore_on_RoR2_CharacterSpawnCard_Awake( HooksCore.orig_RoR2_CharacterSpawnCard_Awake orig, CharacterSpawnCard self )
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
            Main.LogI( "Monster" );
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

        // TODO: Director
        //private void DirectorAPI_FamilyActions( List<R2API.DirectorAPI.MonsterFamilyHolder> families, R2API.DirectorAPI.StageInfo stage )
        //{
        //    foreach( var family in families )
        //    {
        //        if( family.SelectionChatString == "FAMILY_WISP" )
        //        {
        //            family.FamilyChampions.Add( this.AW_dirCard );
        //        }
        //    }
        //}

        //// TODO: Director
        //private void DirectorAPI_MonsterActions( List<R2API.DirectorAPI.DirectorCardHolder> cards, R2API.DirectorAPI.StageInfo stage )
        //{
        //    cards.Add( this.AW_dirCardHolder );
        //}

        //private void CharacterSpawnCard_Awake( On.RoR2.CharacterSpawnCard.orig_Awake orig, CharacterSpawnCard self )
        //{
        //    self.loadout = new SerializableLoadout();
        //    orig( self );
        //}
    }
}
#endif