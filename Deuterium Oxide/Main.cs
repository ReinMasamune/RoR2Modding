using System;
using System.Collections.Generic;
using BepInEx;
using GeneralPluginStuff;
using R2API;
using R2API.Utils;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace Deuterium_Oxide
{
    [R2APISubmoduleDependency( nameof( R2API.DifficultyAPI ), nameof( R2API.DirectorAPI ) )]
    [BepInDependency( "com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.DeuteriumOxide", "Deuterium Oxide" , "1.0.0" )]
    public class Main : BaseUnityPlugin
    {
        public RoR2.DifficultyIndex diffInd1;
        public RoR2.DifficultyIndex diffInd2;
        public DirectorAPI.DirectorCardHolder archWispCard;

        private void OnEnable()
        {
            On.RoR2.CharacterBody.Start += this.GiveHelpers;
            DirectorAPI.MonsterActions += this.DirectorAPI_MonsterActions;
        }

        private void DirectorAPI_MonsterActions( List<DirectorAPI.DirectorCardHolder> cards, DirectorAPI.StageInfo stage )
        {
            if( Run.instance.selectedDifficulty == diffInd1 || Run.instance.selectedDifficulty == diffInd2 )
            {
                cards.Add( archWispCard );

                foreach( DirectorAPI.DirectorCardHolder card in cards )
                {
                    ModifyCard( card.GetCard() );
                }
            }
        }

        private void ModifyCard( DirectorCard card )
        {
            var dirCard = card;
            var name = dirCard.spawnCard.name.ToLower();

            card.selectionWeight *= 4;

            
            if( name == "cscNullifier".ToLower() )
            {
                card.selectionWeight *= 2;
                card.spawnCard.directorCreditCost /= 2;
                return;
            }
            if( name == "cscScav".ToLower() )
            {
                return;
            }


            
            if( name == DirectorAPI.Helpers.MonsterNames.AlloyVulture.ToLower() )
            {
                card.spawnCard.directorCreditCost = 14;
                card.minimumStageCompletions = 0;
            }
            if( name == DirectorAPI.Helpers.MonsterNames.Beetle.ToLower() )
            {
                card.selectionWeight = 0;
            }
            if( name == "cscHermitCrab".ToLower() )
            {
                card.spawnCard.directorCreditCost = 12;
                card.minimumStageCompletions = 0;
            }
            if( name == DirectorAPI.Helpers.MonsterNames.Imp.ToLower() )
            {
                card.spawnCard.directorCreditCost = 12;
            }
            if( name == DirectorAPI.Helpers.MonsterNames.Jellyfish.ToLower() )
            {
                card.selectionWeight = 0;
            }
            if( name == DirectorAPI.Helpers.MonsterNames.Lemurian.ToLower() )
            {

            }
            if( name == DirectorAPI.Helpers.MonsterNames.LesserWisp.ToLower() )
            {
                card.selectionWeight *= 2;
                card.spawnCard.directorCreditCost = 9;
            }
            if( name == DirectorAPI.Helpers.MonsterNames.SolusProbe.ToLower() )
            {

            }
            


            if( name == DirectorAPI.Helpers.MonsterNames.ArchaicWisp.ToLower() )
            {
                card.selectionWeight *= 3;
                card.spawnCard.directorCreditCost = 150;
            }
            if( name == DirectorAPI.Helpers.MonsterNames.BeetleGuard.ToLower() )
            {

            }
            if( name == DirectorAPI.Helpers.MonsterNames.BighornBison.ToLower() )
            {

            }
            if( name == DirectorAPI.Helpers.MonsterNames.BrassContraption.ToLower() )
            {
                card.selectionWeight *= 2;
            }
            if( name == DirectorAPI.Helpers.MonsterNames.ClayTemplar.ToLower() )
            {
                card.selectionWeight *= 2;
                card.minimumStageCompletions = 0;
            }
            if( name == DirectorAPI.Helpers.MonsterNames.ElderLemurian.ToLower() )
            {
                card.selectionWeight *= 2;
            }
            if( name == DirectorAPI.Helpers.MonsterNames.GreaterWisp.ToLower() )
            {
                card.selectionWeight *= 2;
                card.spawnCard.directorCreditCost = 100;
            }
            if( name == DirectorAPI.Helpers.MonsterNames.StoneGolem.ToLower() )
            {
                card.selectionWeight *= 2;
            }
            


            if( name == DirectorAPI.Helpers.MonsterNames.BeetleQueen.ToLower() )
            {
                card.selectionWeight /= 4;
            }
            if( name == DirectorAPI.Helpers.MonsterNames.ClayDunestrider.ToLower() )
            {

            }
            if( name == DirectorAPI.Helpers.MonsterNames.Grovetender.ToLower() )
            {

            }
            if( name == DirectorAPI.Helpers.MonsterNames.ImpOverlord.ToLower() )
            {

            }
            if( name == DirectorAPI.Helpers.MonsterNames.MagmaWorm.ToLower() )
            {
                
            }
            if( name == DirectorAPI.Helpers.MonsterNames.OverloadingWorm.ToLower() )
            {
                card.spawnCard.directorCreditCost = 3000;
                ((CharacterSpawnCard)card.spawnCard).noElites = false;
            }
            if( name == DirectorAPI.Helpers.MonsterNames.SolusControlUnit.ToLower() )
            {

            }
            if( name == DirectorAPI.Helpers.MonsterNames.WanderingVagrant.ToLower() )
            {

            }
            if( name == DirectorAPI.Helpers.MonsterNames.StoneTitanTitanicPlains.ToLower() || name == DirectorAPI.Helpers.MonsterNames.StoneTitanDistantRoost.ToLower() || name == DirectorAPI.Helpers.MonsterNames.StoneTitanAbandonedAqueduct.ToLower() || name == DirectorAPI.Helpers.MonsterNames.StoneTitanAbyssalDepths.ToLower() )
            {

            }
        }

        private void GiveHelpers( On.RoR2.CharacterBody.orig_Start orig, CharacterBody self )
        {
            orig( self );
            if( Run.instance && self && self.inventory && self.teamComponent )
            {
                if( Run.instance.selectedDifficulty == diffInd1)
                {
                    if( self.teamComponent.teamIndex == TeamIndex.Player && self.inventory.GetItemCount( ItemIndex.MonsoonPlayerHelper ) < 1 )
                    {
                        self.inventory.GiveItem( ItemIndex.MonsoonPlayerHelper );
                    }
                }

                if( Run.instance.selectedDifficulty == diffInd2 )
                {
                    switch( self.teamComponent.teamIndex )
                    {
                        case TeamIndex.Player:
                            if( self.inventory.GetItemCount( ItemIndex.MonsoonPlayerHelper ) < 1 ) self.inventory.GiveItem( ItemIndex.MonsoonPlayerHelper );
                            break;
                        case TeamIndex.Monster:
                            if( self.inventory.GetItemCount( ItemIndex.DrizzlePlayerHelper ) < 1 ) self.inventory.GiveItem( ItemIndex.DrizzlePlayerHelper );
                            break;
                    }
                }

            }
        }

        private void OnDisable()
        {
            On.RoR2.CharacterBody.Start -= this.GiveHelpers;
        }

        private void Awake()
        {
            var archWispSpawnCard = Resources.Load<CharacterSpawnCard>( "SpawnCards/CharacterSpawnCards/cscArchWisp");
            archWispSpawnCard.directorCreditCost = 300;

            var archWispDirCard = new DirectorCard();
            archWispDirCard.allowAmbushSpawn = true;
            archWispDirCard.forbiddenUnlockable = "";
            archWispDirCard.minimumStageCompletions = 4;
            archWispDirCard.preventOverhead = false;
            archWispDirCard.requiredUnlockable = "";
            archWispDirCard.selectionWeight = 1;
            archWispDirCard.spawnCard = archWispSpawnCard;
            archWispDirCard.spawnDistance = DirectorCore.MonsterSpawnDistance.Standard;

            archWispCard = new DirectorAPI.DirectorCardHolder();
            archWispCard.SetCard( archWispDirCard );
            archWispCard.SetInteractableCategory( DirectorAPI.InteractableCategory.None );
            archWispCard.SetMonsterCategory( DirectorAPI.MonsterCategory.Minibosses );

            var diff = new RoR2.DifficultyDef(3f, "REIN_DIFFICULTY_DEUTERIUM_NAME" , "Textures/ItemIcons/texMaskIcon", "REIN_DIFFICULTY_DEUTERIUM_DESC", new Color( 0f, 0f, 0f, 1f ));
            var diff2 = new RoR2.DifficultyDef( 4f, "REIN_DIFFICULTY_TRITIUM_NAME" , "Textures/ItemIcons/texMaskIcon", "REIN_DIFFICULTY_TRITIUM_DESC" , new Color( 0f, 0f, 0f, 1f ) );

            diffInd1 = DifficultyAPI.AddDifficulty( diff );
            diffInd2 = DifficultyAPI.AddDifficulty( diff2 );


            /*
            //Disabled cards
            cardAdjustments["BeetleMaster"] = new SpawnParams( 0.0f, 1.0f, true );
            cardAdjustments["JellyfishMaster"] = new SpawnParams( 0.0f, 1.0f, true );

            //Reduced cards
            cardAdjustments["BeetleQueenMaster"] = new SpawnParams( 0.25f, 1.0f, true );

            //Neutral cards
            cardAdjustments["TitanMaster"] = new SpawnParams( 1.0f, 1.0f, true );
            cardAdjustments["VagrantMaster"] = new SpawnParams( 1.0f, 1.0f, true );
            cardAdjustments["GolemMaster"] = new SpawnParams( 1.0f, 1.0f, true );            
            cardAdjustments["LemurianMaster"] = new SpawnParams( 1.0f, 1.0f, true );            
            cardAdjustments["ClayBossMaster"] = new SpawnParams( 1.0f, 1.0f, true );
            cardAdjustments["BeetleGuardMaster"] = new SpawnParams( 1.0f, 1.0f, true );           
            cardAdjustments["HermitCrabMaster"] = new SpawnParams( 1.0f, 1.0f, true );
            cardAdjustments["BellMaster"] = new SpawnParams( 1.0f, 1.0f, true );
            cardAdjustments["GravekeeperMaster"] = new SpawnParams( 1.0f, 1.0f, true );
            cardAdjustments["ImpBossMaster"] = new SpawnParams( 1.0f, 1.0f, true );
            cardAdjustments["ImpMaster"] = new SpawnParams( 1.0f, 1.0f, true );
            cardAdjustments["MagmaWormMaster"] = new SpawnParams( 1.0f, 1.0f, true );
            cardAdjustments["RoboBallbossMaster"] = new SpawnParams( 1.0f, 1.0f, true );            
            cardAdjustments["VultureMaster"] = new SpawnParams( 1.0f, 1.0f, true );            
            cardAdjustments["BisonMaster"] = new SpawnParams( 1.0f, 1.0f, true );

            //Buffed cards
            cardAdjustments["WispMaster"] = new SpawnParams( 3.0f, 1.0f, true );
            cardAdjustments["ClayBruiserMaster"] = new SpawnParams( 2.0f, 1.0f, true );
            cardAdjustments["GreaterWispMaster"] = new SpawnParams( 2.0f, 0.5f, true );
            cardAdjustments["ElectricWormMaster"] = new SpawnParams( 2.0f, 0.5f, true );
            cardAdjustments["LemurianBruiserMaster"] = new SpawnParams( 2.0f, 1.0f, true );
            */
        }
    }
}
