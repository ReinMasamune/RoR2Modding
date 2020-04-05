using BepInEx;
using RoR2;
using UnityEngine;
using System.Collections.Generic;
using RoR2.Navigation;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using System.Collections;
using ReinCore;

namespace ReinGeneralFixes
{
    internal partial class Main
    {
        private InteractableSpawnCard stealthedISC;

        partial void QoLCloakedChestSacrifice()
        {
            this.Enable += this.Main_Enable;
            this.Disable += this.Main_Disable;
        }

        private void Main_Disable()
        {
            this.stealthedISC.skipSpawnWhenSacrificeArtifactEnabled = true;
            this.stealthedISC.directorCreditCost = 10;

            SpawnsCore.interactableEdits -= this.SpawnsCore_interactableEdits;
        }

        private void Main_Enable()
        {
            this.stealthedISC = Resources.Load<InteractableSpawnCard>( "spawncards/interactablespawncard/iscChest1Stealthed" );
            this.stealthedISC.skipSpawnWhenSacrificeArtifactEnabled = false;
            this.stealthedISC.directorCreditCost = 1;
            SpawnsCore.interactableEdits += this.SpawnsCore_interactableEdits;
        }

        private void SpawnsCore_interactableEdits( ClassicStageInfo stageInfo, Run runInstance, DirectorCardCategorySelection interactableSelection )
        {
            var isSacrifice = RunArtifactManager.instance.IsArtifactEnabled( RoR2Content.Artifacts.sacrificeArtifactDef );
            if( isSacrifice )
            {
                var cats = interactableSelection.categories;
                stageInfo.sceneDirectorInteractibleCredits *= 2;
                for( Int32 i = 0; i < cats.Length; ++i )
                {
                    var cat = cats[i];
                    if( cat.name == "Rare" )
                    {
                        cat.selectionWeight *= 10f;
                        foreach( var card in cat.cards )
                        {
                            if( card.spawnCard.name == "iscChest1Stealthed" )
                            {
                                card.selectionWeight *= 5;
                            } else
                            {
                                card.selectionWeight = Math.Max( 1, card.selectionWeight / 10 );
                            }
                        }
                        cats[i] = cat;
                    }
                }
                interactableSelection.categories = cats;
            }
        }
    }
}
