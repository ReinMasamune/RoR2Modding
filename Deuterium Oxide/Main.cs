using System;
using System.Collections.Generic;
using BepInEx;
using R2API;
using R2API.Utils;
using RoR2;
using RoR2Plugin;
using UnityEngine;
using UnityEngine.Networking;

namespace Deuterium_Oxide
{
    [R2APISubmoduleDependency( nameof( R2API.DifficultyAPI ) )]
    [BepInDependency( "com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.DeuteriumOxide", "Deuterium Oxide" , "1.0.0" )]
    public class Main : RoR2Plugin.RoR2Plugin
    {
        public RoR2.DifficultyIndex diffInd1;
        public RoR2.DifficultyIndex diffInd2;

        private Dictionary<string,SpawnParams> cardAdjustments = new Dictionary<string, SpawnParams>();

        private struct SpawnParams
        {
            public float weightMult;
            public float costMult;
            public bool disableRestrictions;

            public SpawnParams( Single weightMult, Single costMult, Boolean disableRestrictions )
            {
                this.weightMult = weightMult;
                this.costMult = costMult;
                this.disableRestrictions = disableRestrictions;
            }
        }

        public override void CreateHooks()
        {
            On.RoR2.ClassicStageInfo.Awake += this.ModifySpawnWeights;
            On.RoR2.CharacterBody.Start += this.GiveHelpers;
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

        private void ModifySpawnWeights( On.RoR2.ClassicStageInfo.orig_Awake orig, RoR2.ClassicStageInfo self )
        {
            if( Run.instance.selectedDifficulty == diffInd1 || Run.instance.selectedDifficulty == diffInd2 )
            {
                var cats = self.GetFieldValue<DirectorCardCategorySelection>( "monsterCategories" );
                foreach( DirectorCardCategorySelection.Category cat in cats.categories )
                {
                    foreach( DirectorCard card in cat.cards )
                    {
                        card.selectionWeight *= 100;
                        string s =  card.spawnCard.prefab.name;
                        if( cardAdjustments.ContainsKey( s ) )
                        {
                            card.selectionWeight = (int)(card.selectionWeight * cardAdjustments[s].weightMult);
                            card.cost = (int)(card.cost * cardAdjustments[s].costMult);
                            if( cardAdjustments[s].disableRestrictions )
                            {
                                card.minimumStageCompletions = 0;
                                card.requiredUnlockable = "";
                                ((CharacterSpawnCard)card.spawnCard).noElites = false;
                            }
                        } else
                        {
                            Debug.Log( "Unregistered Monster: " + s );
                        }
                    }
                }
            }
            orig( self );
        }

        public override void RemoveHooks()
        {
            On.RoR2.ClassicStageInfo.Awake -= this.ModifySpawnWeights;
            On.RoR2.CharacterBody.Start -= this.GiveHelpers;
        }

        public override void OnLoad()
        {
            var diff = new RoR2.DifficultyDef(3f, "REIN_DIFFICULTY_DEUTERIUM_NAME" , "Textures/ItemIcons/texMaskIcon", "REIN_DIFFICULTY_DEUTERIUM_DESC", new Color( 0f, 0f, 0f, 1f ));
            var diff2 = new RoR2.DifficultyDef( 4f, "REIN_DIFFICULTY_TRITIUM_NAME" , "Textures/ItemIcons/texMaskIcon", "REIN_DIFFICULTY_TRITIUM_DESC" , new Color( 0f, 0f, 0f, 1f ) );

            diffInd1 = DifficultyAPI.AddDifficulty( diff );
            diffInd2 = DifficultyAPI.AddDifficulty( diff2 );


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
        }

        public override void OnUnload()
        {

        }
    }
}
