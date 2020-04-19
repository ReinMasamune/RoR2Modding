using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReinCore;
using RoR2;
using Sniper.Components;
using UnityEngine;

namespace Sniper.Modules
{
    internal static class HooksModule
    {
        internal static void Remove()
        {

        }
        internal static void Add()
        {
            HooksCore.RoR2.UI.LoadoutPanelController.Row.FromSkillSlot.Il += FromSkillSlot_Il;
            HooksCore.RoR2.SkillLocator.FindSkillSlot.Il += FindSkillSlot_Il;

            HooksCore.RoR2.CameraRigController.Start.On += Start_On;
        }

        // TODO: Refactor into Core for Hud edits
        private static void Start_On( HooksCore.RoR2.CameraRigController.Start.Orig orig, CameraRigController self )
        {
            orig( self );

            if( self.hud == null )
            {
                return;
            }

            var reloadBar = UIModule.GetRelodBar();

            var par = self.hud.transform.Find( "MainContainer/MainUIArea/BottomCenterCluster" );
            var barTrans = reloadBar.transform as RectTransform;
            barTrans.SetParent(par, false);
            barTrans.localPosition = new Vector3( 0f, 256f, 0f );
            barTrans.localScale = new Vector3( 0.5f, 0.5f, 1f );
        }

        internal static void AddReturnoverride( GenericSkill skill )
        {
            slotReturnOverrides[skill] = (SkillSlot)(counter++);
        }
        private static Int32 counter = (Int32)SkillSlot.Special + 1;
        private static Dictionary<GenericSkill,SkillSlot> slotReturnOverrides = new Dictionary<GenericSkill, SkillSlot>();
        private static void FindSkillSlot_Il( ILContext il )
        {
            ILCursor c = new ILCursor( il );
            ILLabel label = null;
            c.GotoNext( x => x.MatchBrtrue( out label ) );
            c.GotoLabel( label );
            c.Emit( OpCodes.Nop );
            var newLabel = c.MarkLabel();
            c.GotoLabel( label );
            c.Emit( OpCodes.Ldarg_1 );
            c.EmitDelegate<Func<GenericSkill, Boolean>>( ( skill ) => slotReturnOverrides.ContainsKey( skill ) );
            c.Emit( OpCodes.Brfalse_S, newLabel );
            c.Emit( OpCodes.Ldarg_1 );
            c.EmitDelegate<Func<GenericSkill, SkillSlot>>( ( skill ) => slotReturnOverrides[skill] );
            c.Emit( OpCodes.Ret );
        }

        private static void FromSkillSlot_Il( MonoMod.Cil.ILContext il )
        {
            ILCursor c = new ILCursor( il );
            ILCursor c2 = new ILCursor( il );
            ILLabel endSwitchLabel = null;
            c.GotoNext( x => x.MatchStloc( 3 ), x => x.MatchBr( out endSwitchLabel ) );
            ILLabel[] switchLabels = null;
            c.GotoPrev( MoveType.Before, x => x.MatchSwitch( out switchLabels ) );

            c2.GotoNext( MoveType.Before, x => x.MatchLdstr( "LOADOUT_SKILL_MISC" ) );
            var newLabel1 = c2.MarkLabel();
            c2.Emit( OpCodes.Ldstr, Properties.Tokens.LOADOUT_SNIPER_AMMO );
            c2.Emit( OpCodes.Stloc_2 );
            c2.Emit( OpCodes.Ldc_I4_0 );
            c2.Emit( OpCodes.Stloc_3 );
            c2.Emit( OpCodes.Br_S, endSwitchLabel );

            var newLabel2 = c2.MarkLabel();
            c2.Emit( OpCodes.Ldstr, Properties.Tokens.LOADOUT_SNIPER_PASSIVE );
            c2.Emit( OpCodes.Stloc_2 );
            c2.Emit( OpCodes.Ldc_I4_0 );
            c2.Emit( OpCodes.Stloc_3 );
            c2.Emit( OpCodes.Br_S, endSwitchLabel );

            Array.Resize<ILLabel>( ref switchLabels, switchLabels.Length + 2 );
            switchLabels[switchLabels.Length - 2] = newLabel1;
            switchLabels[switchLabels.Length - 1] = newLabel2;

            c.Remove();
            c.Emit( OpCodes.Switch, switchLabels );
        }
    }

}
