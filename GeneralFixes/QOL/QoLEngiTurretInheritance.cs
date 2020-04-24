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
        private static Boolean ShouldInheritEquipment( EquipmentIndex index )
        {
            EliteIndex eliteInd = EliteCatalog.GetEquipmentEliteIndex( index );
            return eliteInd != EliteIndex.None;
        }

        partial void QoLEngiTurretInheritance()
        {
            this.Enable += this.Main_Enable2;
            this.Disable += this.Main_Disable2;
        }

        private void Main_Disable2() => HooksCore.RoR2.CharacterBody.HandleConstructTurret.Il -= this.HandleConstructTurret_Il;
        private void Main_Enable2() => HooksCore.RoR2.CharacterBody.HandleConstructTurret.Il += this.HandleConstructTurret_Il;

        private void HandleConstructTurret_Il( ILContext il )
        {
            var c = new ILCursor( il );

            _ = c.GotoNext( MoveType.After, x => x.MatchCallOrCallvirt<CharacterMaster>( "get_inventory" ) );
            _ = c.Emit( OpCodes.Dup );
            c.Index += 3;
            //_ = c.Emit( OpCodes.Dup );
            _ = c.EmitDelegate<Func<Inventory, Inventory, Inventory>>( ( newInv, oldInv ) =>
              {
                  if( ShouldInheritEquipment( oldInv.currentEquipmentIndex ) ) newInv.CopyEquipmentFrom( oldInv );
                  return newInv;
              } );
        }
    }
}
