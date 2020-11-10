namespace ReinGeneralFixes
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using BF = System.Reflection.BindingFlags;

    using EntityStates;

    using Mono.Cecil.Cil;

    using MonoMod.Cil;

    using ReinCore;

    using RoR2;

    using UnityEngine;
    using EntityStates.CaptainSupplyDrop;
    using EntityStates.Captain.Weapon;
    using RoR2.Achievements.Artifacts;

    internal partial class Main
    {
        partial void HappyThings()
        {
            this.Load += this.Main_Load;
        }

        private void Main_Load()
        {
            SpawnsCore.monsterEdits += this.SpawnsCore_monsterEdits;
        }

        private void SpawnsCore_monsterEdits(ClassicStageInfo stageInfo, Run runInstance, DirectorCardCategorySelection monsterSelection)
        {
            for(Int32 i = 0; i < monsterSelection.categories.Length; ++i)
            {
                ref var cat = ref monsterSelection.categories[i];
                for(Int32 j = 0; j < cat.cards.Length; ++j)
                {
                    var card = cat.cards[j];
                    if(card.spawnCard is CharacterSpawnCard csc && csc.name?.ToLower() == "cscelectricworm") csc.noElites = false;
                }
            }
        }
    }
}
