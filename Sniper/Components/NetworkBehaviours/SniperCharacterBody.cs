using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using UnityEngine;
using Sniper.Skills;

namespace Sniper.Components
{
    internal class SniperCharacterBody : CharacterBody
    {
        internal SniperAmmoSkillDef ammo
        {
            get
            {
                if( this._ammo == null )
                {
                    var slot = this.ammoSlot;
                    this._ammo = slot.skillDef as SniperAmmoSkillDef;
                }
                return this._ammo;
            }
        }
        private SniperAmmoSkillDef _ammo;

        private GenericSkill ammoSlot
        {
            get
            {
                if( this._ammoSlot == null )
                {
                    this._ammoSlot = base.skillLocator.GetSkillAtIndex( 0 );
                    this._ammoSlot.onSkillChanged += ( slot ) => this._ammo = slot.skillDef as SniperAmmoSkillDef;
                }
                return this._ammoSlot;
            }
        }
        private GenericSkill _ammoSlot;


        internal SniperPassiveSkillDef passive
        {
            get
            {
                if( this._passive == null )
                {
                    var slot = this.passiveSlot;
                    this._passive = slot.skillDef as SniperPassiveSkillDef;
                }
                return this._passive;
            }
        }
        private SniperPassiveSkillDef _passive;

        private GenericSkill passiveSlot
        {
            get
            {
                if( this._passiveSlot == null )
                {
                    this._passiveSlot = base.skillLocator.GetSkillAtIndex( 1 );
                    this._passiveSlot.onSkillChanged += ( slot ) => this._passive = slot.skillDef as SniperPassiveSkillDef;
                }
                return this._passiveSlot;
            }
        }
        private GenericSkill _passiveSlot;
    }
}
