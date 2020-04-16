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
        private static HashSet<ItemIndex> turretInheritBlacklist = new HashSet<ItemIndex>
        {
            ItemIndex.WardOnLevel,
            ItemIndex.CrippleWardOnLevel,
            ItemIndex.BeetleGland,
            ItemIndex.TPHealingNova,
            ItemIndex.TitanGoldDuringTP,
            ItemIndex.TreasureCache,
            ItemIndex.FocusConvergence,
        };

        private static Boolean ShouldInheritEquipment( EquipmentIndex index )
        {
            var eliteInd = EliteCatalog.GetEquipmentEliteIndex( index );
            return eliteInd != EliteIndex.None;
        }

        partial void QoLEngiTurretInheritance()
        {
            this.Enable += this.Main_Enable2;
            this.Disable += this.Main_Disable2;
        }

        private void Main_Disable2()
        {
            HooksCore.RoR2.CharacterBody.HandleConstructTurret.Il -= this.HandleConstructTurret_Il;
        }
        private void Main_Enable2()
        {
            HooksCore.RoR2.CharacterBody.HandleConstructTurret.Il += this.HandleConstructTurret_Il;
        }

        private void HandleConstructTurret_Il( ILContext il )
        {
            ILCursor c = new ILCursor( il );

            c.GotoNext( MoveType.After, x => x.MatchCallOrCallvirt<RoR2.CharacterMaster>( "get_inventory" ) );
            c.Remove();
            c.Index += 2;
            c.RemoveRange( 12 );
            c.EmitDelegate<Action<Inventory, Inventory>>( ( newInv, oldInv ) =>
            {
                newInv.CopyItemsFrom( oldInv );
                foreach( var ind in turretInheritBlacklist ) newInv.ResetItem( ind );
                if( ShouldInheritEquipment( oldInv.currentEquipmentIndex ) ) newInv.CopyEquipmentFrom( oldInv );
            });
        }
    }
}
