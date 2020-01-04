﻿namespace AlternativeArtificer
{
    using BepInEx;
    using RoR2;
    using System;
    using UnityEngine;
    using R2API;
    using AlternativeArtificer.States.Main;

    public partial class Main
    {
        public BuffIndex fireBuff;
        public BuffIndex burnBuff;
        public DotController.DotIndex burnDot;
        
        private void DoBuffs()
        {
            this.AddFireBuff();
        }

        private void AddFireBuff()
        {
            this.fireBuff = (BuffIndex)ItemAPI.AddCustomBuff( new CustomBuff( "AltArtiFireBuff", new BuffDef
            {
                buffColor = new Color( 0.9f, 0.2f, 0.2f ),
                canStack = true,
                iconPath = "Textures/BuffIcons/texBuffAffixRed",
                isDebuff = false,
                name = "AltArtiFireBuff"
            } ));

            this.burnBuff = BuffIndex.OnFire;
            this.burnDot = DotController.DotIndex.Burn;
            AltArtiPassive.fireBuff = fireBuff;
        }
    }
}