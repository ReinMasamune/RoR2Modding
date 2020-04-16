//using System;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using BepInEx;
//using ReinCore;
//using RoR2;
//using UnityEngine;

//namespace Rein.AlternateArtificer
//{
//    internal partial class Main
//    {
//        partial void Buffs()
//        {
//            base.awake += this.Main_awake2;
//            base.start += this.Main_start;
//        }

//        private void Main_start()
//        {
//            burnBuff = BuffIndex.OnFire;
//            fireBuff = BuffCatalog.FindBuffIndex( "AltArtiFireBuff" );
//            burnDot = DotController.DotIndex.Burn;
//        }

//        private void Main_awake2()
//        {
//            BuffsCore.getAdditionalEntries += this.BuffsCore_getAdditionalEntries;
//        }

//        private void BuffsCore_getAdditionalEntries( List<BuffDef> buffList )
//        {
//            buffList.Add( new BuffDef
//            {
//                buffColor = new Color( 0.9f, 0.2f, 0.2f ),
//                canStack = true,
//                iconPath = "Textures/BuffIcons/texBuffAffixRed",
//                isDebuff = false,
//                name = "AltArtiFireBuff",
//            } );
//        }
//    }
//}