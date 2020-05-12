namespace ReinGeneralFixes
{
    using System;

    using MonoMod.Cil;

    using ReinCore;

    using RoR2;

    using UnityEngine;

    internal partial class Main
    {
        partial void FixBandolier()
        {
            this.Enable += this.ApplyBandolierFix;
            this.Disable += this.RemoveBandolierFix;
        }

        private void RemoveBandolierFix() => HooksCore.RoR2.SkillLocator.ApplyAmmoPack.Il -= this.ApplyAmmoPack_Il;
        private void ApplyBandolierFix() => HooksCore.RoR2.SkillLocator.ApplyAmmoPack.Il += this.ApplyAmmoPack_Il;

        private void ApplyAmmoPack_Il( ILContext il )
        {
            var c = new ILCursor( il );
            _ = c.GotoNext( MoveType.Before, x => x.MatchCallOrCallvirt<GenericSkill>( "AddOneStock" ) );
            _ = c.Remove();
            _ = c.EmitDelegate<Action<GenericSkill>>( ( skill ) =>
              {
                  Int32 tempStock = skill.stock;
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
