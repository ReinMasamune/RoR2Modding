namespace ReinGeneralFixes
{
    using System;

    using MonoMod.Cil;

    using ReinCore;

    using RoR2;

    using UnityEngine;

    internal partial class Main
    {
        partial void BalanceOSP()
        {
            this.Enable += this.AddOSPFix;
            this.Disable += this.RemoveOSPFix;
        }

        private void RemoveOSPFix() => HooksCore.RoR2.HealthComponent.TakeDamage.Il -= this.TakeDamage_Il;
        private void AddOSPFix() => HooksCore.RoR2.HealthComponent.TakeDamage.Il += this.TakeDamage_Il;

        private void TakeDamage_Il( ILContext il )
        {
            var c = new ILCursor( il );

            _ = c.GotoNext( MoveType.After, x => x.MatchCallOrCallvirt<HealthComponent>( "get_hasOneshotProtection" ) );
            _ = c.GotoNext( MoveType.Before, x => x.MatchCallOrCallvirt<HealthComponent>( "get_fullCombinedHealth" ) );
            _ = c.RemoveRange( 2 );
            _ = c.GotoNext( MoveType.Before, x => x.MatchLdfld<HealthComponent>( "barrier" ) );
            _ = c.RemoveRange( 2 );
            _ = c.GotoNext( MoveType.Before, x => x.MatchMul() );
            _ = c.RemoveRange( 2 );
            _ = c.EmitDelegate<Func<Single, HealthComponent, Single, Single>>( ( num, healthComp, osp ) =>
              {
                  Single temp = num;
                  temp -= healthComp.shield;
                  temp -= healthComp.barrier;

                  Single protection = healthComp.fullHealth * osp;
                  if( temp <= protection )
                  {
                      return num;
                  }
                  temp -= healthComp.fullHealth * ( 1f + ( 1f / osp ) );
                  temp = Mathf.Max( 0f, temp );
                  temp += protection;
                  temp += healthComp.shield;
                  temp += healthComp.barrier;

                  return temp;
              } );
        }
    }
}
/*
Changelist:
Commando Phase round 2s cooldown, phase blast 3s cooldown



















*/
