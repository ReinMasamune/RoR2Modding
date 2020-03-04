#if ANCIENTWISP
using BepInEx.Configuration;
using GeneralPluginStuff;
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        private ConfigEntry<Boolean> bossEnabled;
        partial void AW_Director()
        {
            this.Load += this.AW_SetupSpawns;

            this.bossEnabled = base.Config.Bind<Boolean>( "Main", "Enable new boss", true,
                "Should the new boss spawn in game? Left as an option in case of major bugs or issues with early version of boss." );
        }

        private R2API.DirectorAPI.DirectorCardHolder AW_dirCardHolder;
        private DirectorCard AW_dirCard;
        private void AW_SetupSpawns()
        {
            //this.bossEnabled = base.Config.Bind<Boolean>( "Main", "Enable new boss", true,
    //"Should the new boss spawn in game? Left as an option in case of major bugs or issues with early version of boss." );


            On.RoR2.CharacterSpawnCard.Awake += this.CharacterSpawnCard_Awake;
            var spawnCard = ScriptableObject.CreateInstance<CharacterSpawnCard>();
            On.RoR2.CharacterSpawnCard.Awake -= this.CharacterSpawnCard_Awake;
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

            this.AW_dirCardHolder = new R2API.DirectorAPI.DirectorCardHolder();
            this.AW_dirCardHolder.SetCard(dirCard);
            this.AW_dirCardHolder.SetInteractableCategory(R2API.DirectorAPI.InteractableCategory.None);
            this.AW_dirCardHolder.SetMonsterCategory(R2API.DirectorAPI.MonsterCategory.Champions);

            if(this.bossEnabled.Value )
            {
                R2API.DirectorAPI.MonsterActions += this.DirectorAPI_MonsterActions;
                R2API.DirectorAPI.FamilyActions += this.DirectorAPI_FamilyActions;
            }
        }

        private void DirectorAPI_FamilyActions( List<R2API.DirectorAPI.MonsterFamilyHolder> families, R2API.DirectorAPI.StageInfo stage )
        {
            foreach( var family in families )
            {
                if( family.SelectionChatString == "FAMILY_WISP" )
                {
                    family.FamilyChampions.Add( this.AW_dirCard );
                }
            }
        }

        private void DirectorAPI_MonsterActions( List<R2API.DirectorAPI.DirectorCardHolder> cards, R2API.DirectorAPI.StageInfo stage )
        {
            cards.Add( this.AW_dirCardHolder );
        }

        private void CharacterSpawnCard_Awake( On.RoR2.CharacterSpawnCard.orig_Awake orig, CharacterSpawnCard self )
        {
            self.loadout = new SerializableLoadout();
            orig( self );
        }
    }
}
#endif