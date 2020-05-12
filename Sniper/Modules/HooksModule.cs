namespace Sniper.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Reflection;
    using BepInEx.Logging;
    using Mono.Cecil.Cil;
    using MonoMod.Cil;
    using ReinCore;
    using RoR2;
    using Sniper.Components;
    using Sniper.ScriptableObjects;
    using UnityEngine;

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
            HooksCore.RoR2.CameraTargetParams.Update.Il += Update_Il;
        }

        private static void Update_Il( ILContext il )
        {
            var c = new ILCursor( il );
            Int32 vecLocation = 1;

            _ = c.GotoNext( MoveType.After,
                x => x.MatchLdcR4( -8f ),
                x => x.MatchStfld<Vector3>( nameof( Vector3.z ) ),
                x => x.MatchLdloca( out vecLocation ),
                x => x.MatchLdcR4( 1f ),
                x => x.MatchStfld<Vector3>( nameof( Vector3.y ) ),
                x => x.MatchLdloca( vecLocation ),
                x => x.MatchLdcR4( 1f ),
                x => x.MatchStfld<Vector3>( nameof( Vector3.x ) )
            );

            ILLabel end = c.MarkLabel();

            _ = c.GotoPrev( MoveType.AfterLabel,
                x => x.MatchLdfld<CharacterCameraParams>( nameof(CharacterCameraParams.standardLocalCameraPos) )
            );

            ILLabel standard = c.MarkLabel();

            _= c.GotoPrev( MoveType.After,
                x => x.MatchLdarg( 0 ),
                x => x.MatchLdfld<CameraTargetParams>( nameof( CameraTargetParams.cameraParams ) )
            );
            _ = c.Emit( OpCodes.Dup );
            _ = c.Emit( OpCodes.Isinst, typeof( SniperCameraParams ) );
            _ = c.Emit( OpCodes.Brfalse_S, standard );
            _ = c.Emit( OpCodes.Ldfld, typeof( SniperCameraParams ).GetField( nameof( SniperCameraParams.throwLocalCameraPos ), BindingFlags.Instance | BindingFlags.NonPublic ) );
            _ = c.Emit( OpCodes.Stloc, vecLocation );
            _ = c.Emit( OpCodes.Br_S, end );
        }

        // TODO: Refactor into Core for Hud edits
        private static void Start_On( HooksCore.RoR2.CameraRigController.Start.Orig orig, CameraRigController self )
        {
            orig( self );

            if( self.hud == null )
            {
                return;
            }

            GameObject reloadBar = UIModule.GetRelodBar();

            Transform par = self.hud.transform.Find( "MainContainer/MainUIArea/BottomCenterCluster" );
            var barTrans = reloadBar.transform as RectTransform;
            barTrans.SetParent(par, false);
            barTrans.localPosition = new Vector3( 0f, 256f, 0f );
            barTrans.localScale = new Vector3( 0.5f, 0.5f, 1f );
        }

        internal static void AddReturnoverride( GenericSkill skill ) => slotReturnOverrides[skill] = (SkillSlot) counter++ ;
        private static Int32 counter = (Int32)SkillSlot.Special + 1;
        private static readonly Dictionary<GenericSkill,SkillSlot> slotReturnOverrides = new Dictionary<GenericSkill, SkillSlot>();
        private static void FindSkillSlot_Il( ILContext il )
        {
            var c = new ILCursor( il );
            ILLabel label = null;
            _ = c.GotoNext( x => x.MatchBrtrue( out label ) );
            _ = c.GotoLabel( label );
            _ = c.Emit( OpCodes.Nop );
            ILLabel newLabel = c.MarkLabel();
            _ = c.GotoLabel( label );
            _ = c.Emit( OpCodes.Ldarg_1 );
            _ = c.EmitDelegate<Func<GenericSkill, Boolean>>( ( skill ) => slotReturnOverrides.ContainsKey( skill ) );
            _ = c.Emit( OpCodes.Brfalse_S, newLabel );
            _ = c.Emit( OpCodes.Ldarg_1 );
            _ = c.EmitDelegate<Func<GenericSkill, SkillSlot>>( ( skill ) => slotReturnOverrides[skill] );
            _ = c.Emit( OpCodes.Ret );
        }

        private static void FromSkillSlot_Il( MonoMod.Cil.ILContext il )
        {
            var c = new ILCursor( il );
            var c2 = new ILCursor( il );
            ILLabel endSwitchLabel = null;
            _ = c.GotoNext( x => x.MatchStloc( 3 ), x => x.MatchBr( out endSwitchLabel ) );
            ILLabel[] switchLabels = null;
            _ = c.GotoPrev( MoveType.Before, x => x.MatchSwitch( out switchLabels ) );

            _ = c2.GotoNext( MoveType.Before, x => x.MatchLdstr( "LOADOUT_SKILL_MISC" ) );
            ILLabel newLabel1 = c2.MarkLabel();
            _ = c2.Emit( OpCodes.Ldstr, Properties.Tokens.LOADOUT_SNIPER_AMMO );
            _ = c2.Emit( OpCodes.Stloc_2 );
            _ = c2.Emit( OpCodes.Ldc_I4_0 );
            _ = c2.Emit( OpCodes.Stloc_3 );
            _ = c2.Emit( OpCodes.Br_S, endSwitchLabel );

            ILLabel newLabel2 = c2.MarkLabel();
            _ = c2.Emit( OpCodes.Ldstr, Properties.Tokens.LOADOUT_SNIPER_PASSIVE );
            _ = c2.Emit( OpCodes.Stloc_2 );
            _ = c2.Emit( OpCodes.Ldc_I4_0 );
            _ = c2.Emit( OpCodes.Stloc_3 );
            _ = c2.Emit( OpCodes.Br_S, endSwitchLabel );

            Array.Resize<ILLabel>( ref switchLabels, switchLabels.Length + 2 );
            switchLabels[switchLabels.Length - 2] = newLabel1;
            switchLabels[switchLabels.Length - 1] = newLabel2;

            _ = c.Remove();
            _ = c.Emit( OpCodes.Switch, switchLabels );
        }

        private static void Example( ILContext il )
        {
            var cursor = new ILCursor( il );

            ILLabel myBlockStart, myBlockEnd;
            myBlockStart = myBlockEnd = null;

            _ = cursor.GotoNext( MoveType.After,
                instr => instr.MatchBrfalse( out myBlockStart ),
                instr => instr.MatchLdcR4( 3f ),
                instr => instr.MatchLdcR4( 1f ),
                instr => instr.MatchLdarg( 0 )
            );

            _ = cursor.GotoLabel( myBlockStart );
            _ = cursor.Emit( OpCodes.Nop );
            myBlockEnd = cursor.Clone().MarkLabel(); //Clone needed because MarkLabel moves cursor

            //Your IL here
            //Anything that should exit your block should do this or something similar
            _ = cursor.Emit( OpCodes.Brfalse_S, myBlockEnd );
        }
    }

}
