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
using ReinCore;

namespace ReinGeneralFixes
{
    internal partial class Main
    {
        partial void FixBandolier()
        {
            this.Enable += this.ApplyBandolierFix;
            this.Disable += this.RemoveBandolierFix;
        }

        private void RemoveBandolierFix()
        {
            HooksCore.RoR2.SkillLocator.ApplyAmmoPack.Il -= this.ApplyAmmoPack_Il;
        }
        private void ApplyBandolierFix()
        {
            HooksCore.RoR2.SkillLocator.ApplyAmmoPack.Il += this.ApplyAmmoPack_Il;
        }

        private void ApplyAmmoPack_Il( ILContext il )
        {
            ILCursor c = new ILCursor( il );
            c.GotoNext( MoveType.Before, x => x.MatchCallOrCallvirt<RoR2.GenericSkill>( "AddOneStock" ) );
            c.Remove();
            c.EmitDelegate<Action<GenericSkill>>( ( skill ) =>
            {
                var tempStock = skill.stock;
                tempStock += skill.rechargeStock;
                skill.stock = Mathf.Min( tempStock, skill.maxStock );
                if( skill.stock >= skill.maxStock )
                {
                    skill.rechargeStopwatch = 0f;
                }
            } );
        }
    }
}
