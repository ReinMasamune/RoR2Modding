namespace Sniper.Modules
{
	using System;
	using System.Collections.Generic;
	using System.Reflection;

    using Mono.Cecil;
    using Mono.Cecil.Cil;

	using MonoMod.Cil;

	using ReinCore;

	using RoR2;

    using Sniper.Components;
    using Sniper.ScriptableObjects;

	using UnityEngine;

    using BF = System.Reflection.BindingFlags;

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

			HooksCore.RoR2.UnlockableCatalog.Init.Il += Init_Il;
		}

		private static void Init_Il( ILContext il )
		{
			var c = new ILCursor( il );
			_ = c.GotoNext( MoveType.Before, x => x.MatchLdsfld( typeof( RoR2.UnlockableCatalog ).FullName, "nameToDefTable" ) );
			c.EmitString( "???" );
			c.EmitNewObj( typeof( UnlockableDef ).GetConstructor( Type.EmptyTypes ) );
			_ = c.EmitDelegate<Func<UnlockableDef, UnlockableDef>>( ( def ) =>
			{
				def.name = "???";
				return def;
			});
			_ = c.Emit( OpCodes.Call, typeof( UnlockableCatalog ).GetMethod( "RegisterUnlockable", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static ) );
		}

        private static void Update_Il( ILContext il )
        {
            var c = new ILCursor( il );

            var f1 = typeof( SniperCameraParams ).GetField( nameof( SniperCameraParams.standardDamp ), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );
            var f2 = typeof( SniperCameraParams ).GetField( nameof( SniperCameraParams.scopeDamp ), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );
            var f3 = typeof( SniperCameraParams ).GetField( nameof( SniperCameraParams.sprintDamp ), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );
            var camParamsField = typeof(CameraTargetParams).GetField( nameof(CameraTargetParams.cameraParams), BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance );

            FieldInfo SelectField( Single value )
            {
                switch( value )
                {
                    case 0.5f:
                    return f1;

                    case 0.4f:
                    return f2;

                    case 1f:
                    return f3;

                    default:
                    return f1;
                }
            }

            var c2 = c.Clone();
            var c3 = c.Clone();
            Single baseValue = 0.0f;


            var smoothDampMethod = typeof(Vector3).GetMethod( "SmoothDamp", BF.Public | BF.Static, null, new[] { typeof( Vector3 ), typeof( Vector3 ), typeof( Vector3 ).MakeByRefType(), typeof( Single ) }, null );

            while( c2.TryGotoNext( MoveType.AfterLabel,
                x => x.MatchLdcR4( out baseValue ),
                x => x.MatchCall( smoothDampMethod ) ) )
            {
                //c2.Index++;
                var tempCur = c2.Clone();
                var defLab = tempCur.MarkLabel();
                tempCur.Index++;
                var skipLab = tempCur.MarkLabel();

                c2
                    .MoveBeforeLabels()
                    .LdArg_(0)
                    .Emit( OpCodes.Ldfld, camParamsField )
                    .Emit( OpCodes.Isinst, typeof( SniperCameraParams ) )
                    .Emit( OpCodes.Brfalse_S, defLab )
                    .Emit( OpCodes.Ldarg_0 )
                    .Emit( OpCodes.Ldfld, camParamsField )
                    .Emit( OpCodes.Ldfld, SelectField( baseValue ) )
                    .Emit( OpCodes.Br_S, skipLab )
                ;

                c2.Index++;
            }

            var camVertDamp = typeof(SniperCameraParams).GetField( nameof(SniperCameraParams.vertDamp), BF.Public | BF.NonPublic | BF.Instance );
            var singleSmoothDamp = typeof(Mathf).GetMethod( "SmoothDamp", BF.Public | BF.Static, null, new[] { typeof(Single), typeof(Single), typeof(Single).MakeByRefType(), typeof(Single) }, null );

            while( c3.TryGotoNext( MoveType.AfterLabel,
                x => x.MatchLdcR4( 0.5f ),
                x => x.MatchCall( singleSmoothDamp ) ) )
            {
                var tempCur = c3.Clone();
                var defLab = tempCur.MarkLabel();
                tempCur.Index++;
                var skipLab = tempCur.MarkLabel();

                c3
                    .MoveBeforeLabels()
                    .LdArg_( 0 )
                    .Emit( OpCodes.Ldfld, camParamsField )
                    .Emit( OpCodes.Isinst, typeof( SniperCameraParams ) )
                    .Emit( OpCodes.Brfalse_S, defLab )
                    .Emit( OpCodes.Ldarg_0 )
                    .Emit( OpCodes.Ldfld, camParamsField )
                    .Emit( OpCodes.Ldfld, camVertDamp )
                    .Emit( OpCodes.Br_S, skipLab )
                    .Move( 1 );
            }


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
				x => x.MatchLdfld<CharacterCameraParams>( nameof( CharacterCameraParams.standardLocalCameraPos ) )
			);

			ILLabel standard = c.MarkLabel();

			_ = c.GotoPrev( MoveType.After,
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

		// FUTURE: Refactor into Core for Hud edits
		private static void Start_On( HooksCore.RoR2.CameraRigController.Start.Orig orig, CameraRigController self )
		{
			orig( self );

            _ = self.sceneCam.gameObject.AddComponent<ScopeOverlayShaderController>();

			if( self.hud == null )
			{
				return;
			}

			GameObject reloadBar = UIModule.GetRelodBar();

			Transform par = self.hud.transform.Find( "MainContainer/MainUIArea/BottomCenterCluster" );
			var barTrans = reloadBar.transform as RectTransform;
			barTrans.SetParent( par, false );
			barTrans.localPosition = new Vector3( 0f, 256f, 0f );
			barTrans.localScale = new Vector3( 0.5f, 0.5f, 1f );
		}

		internal static void AddReturnoverride( GenericSkill skill ) => slotReturnOverrides[skill] = (SkillSlot)counter++;
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
