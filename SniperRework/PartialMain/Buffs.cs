using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using System;

namespace ReinSniperRework
{
    internal partial class Main
    {
        partial void Buffs()
        {
            this.Load += this.AddResetDebuff;
            this.FirstFrame += this.Main_FirstFrame;
        }

        private void Main_FirstFrame()
        {
            this.resetDebuff = BuffCatalog.FindBuffIndex( "SniperResetDebuff" );
        }

        private void AddResetDebuff()
        {
            var resetdef = new BuffDef
            {
                buffColor = new Color( 0.2f, 0.2f, 0.2f, 1f ),
                canStack = false,
                eliteIndex = EliteIndex.None,
                iconPath = "Textures/BuffIcons/texBuffFullCritIcon",
                isDebuff = true,
                name = "SniperResetDebuff"
            };
            BuffCatalog.modHelper.getAdditionalEntries += ( list ) => list.Add( resetdef );
        }
    }
}


