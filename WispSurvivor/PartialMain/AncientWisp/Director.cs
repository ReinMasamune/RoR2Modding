#if ANCIENTWISP
using RoR2;
using System.Collections.Generic;
using UnityEngine;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        partial void AW_Director()
        {
            this.Load += this.AW_SetupSpawns;
        }

        private R2API.DirectorAPI.DirectorCardHolder AW_dirCard;
        private void AW_SetupSpawns()
        {
            On.RoR2.CharacterSpawnCard.Awake += this.CharacterSpawnCard_Awake;
            var spawnCard = ScriptableObject.CreateInstance<CharacterSpawnCard>();
            On.RoR2.CharacterSpawnCard.Awake -= this.CharacterSpawnCard_Awake;
            spawnCard.directorCreditCost = 600;
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
            dirCard.selectionWeight = 100;
            dirCard.spawnCard = spawnCard;
            dirCard.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;

            this.AW_dirCard = new R2API.DirectorAPI.DirectorCardHolder();
            this.AW_dirCard.card = dirCard;
            this.AW_dirCard.interactableCategory = R2API.DirectorAPI.InteractableCategory.None;
            this.AW_dirCard.monsterCategory = R2API.DirectorAPI.MonsterCategory.Champions;

            R2API.DirectorAPI.MonsterActions += this.DirectorAPI_MonsterActions;
        }

        private void DirectorAPI_MonsterActions( List<R2API.DirectorAPI.DirectorCardHolder> cards, R2API.DirectorAPI.StageInfo stage )
        {
            cards.Add( this.AW_dirCard );
        }

        private void CharacterSpawnCard_Awake( On.RoR2.CharacterSpawnCard.orig_Awake orig, CharacterSpawnCard self )
        {
            self.loadout = new SerializableLoadout();
            orig( self );
        }
    }
}
#endif