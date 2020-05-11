using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using BepInEx;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using UnityEngine;

namespace ReinCore
{
    public static class DoTsCore
    {
        public static Boolean loaded { get; internal set; } = false;

        public delegate void CustomDoTDamageDelegate( HealthComponent victim, DamageInfo damage );

        public static DotController.DotIndex AddDotType( DoTDef dot, CustomDoTDamageDelegate customDamage = null )
        {
            if( !loaded ) throw new CoreNotLoadedException( nameof( DoTsCore ) );
            if( dot == null ) throw new ArgumentNullException( nameof( dot ) );

            var ind = currentIndex++;

            AddNewDotDef( dot );
            if( customDamage != null )
            {
                customDoTDamages[ind] = customDamage;
            }

            return ind;
        }



        static DoTsCore()
        {
            var allFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;
            var controllerType = typeof(DotController);
            var dotDefType = controllerType.GetNestedType( "DotDef", allFlags );
            var dotDefArrayType = dotDefType.MakeArrayType();
            arrayField = controllerType.GetField( "dotDefs", allFlags );
            var intervalField = dotDefType.GetField( "interval", allFlags );
            var damageCoefField = dotDefType.GetField( "damageCoefficient", allFlags );
            var damageColorIndexField = dotDefType.GetField( "damageColorIndex", allFlags );
            var associatedBuffField = dotDefType.GetField( "associatedBuff", allFlags );
            var custInterval = typeof(DoTDef).GetMember( nameof(DoTDef.interval), allFlags )[0];
            var custDamageCoef = typeof(DoTDef).GetMember( nameof(DoTDef.damageCoefficient), allFlags )[0];
            var custDamageColor = typeof(DoTDef).GetMember( nameof(DoTDef.damageColorIndex), allFlags )[0];
            var custAssociatedBuff = typeof(DoTDef).GetMember( nameof(DoTDef.associatedBuff), allFlags )[0];
            var constructionFuncType = typeof(Func<,>).MakeGenericType( typeof(DoTDef), dotDefType );

            var inputParameter = Expression.Parameter( typeof(DoTDef), "NewDotDef" );
            var inputInterval = Expression.MakeMemberAccess( inputParameter, custInterval );
            var inputDamageCoef = Expression.MakeMemberAccess( inputParameter, custDamageCoef );
            var inputDamageColor = Expression.MakeMemberAccess( inputParameter, custDamageColor );
            var inputAssociatedBuff = Expression.MakeMemberAccess( inputParameter, custAssociatedBuff );
            var intervalBind = Expression.Bind( intervalField, inputInterval );
            var damageCoefBind = Expression.Bind( damageCoefField, inputDamageCoef );
            var damageColorBind = Expression.Bind( damageColorIndexField, inputDamageColor );
            var associatedBuffBind = Expression.Bind( associatedBuffField, inputAssociatedBuff );
            var newDotDef = Expression.New( dotDefType );
            var newDotDefInit = Expression.MemberInit(newDotDef, intervalBind, damageCoefBind, damageColorBind, associatedBuffBind );
            var newDotDefFunc = Expression.Lambda( constructionFuncType, newDotDefInit, inputParameter );
            var newDotDefInvoke = Expression.Invoke( newDotDefFunc, inputParameter );
            var origArray = Expression.Field( null, arrayField );
            var origLength = Expression.ArrayLength( origArray );
            var length = Expression.Add( origLength, Expression.Constant(1) );
            var initArray = Expression.NewArrayBounds(dotDefType, length);
            var breakLabel = Expression.Label( dotDefArrayType, "return" );
            var iteratorVar = Expression.Variable( typeof(Int32), "iterator" );
            var newArrayVar = Expression.Variable( dotDefArrayType, "newArray" );
            var compare1 = Expression.LessThan(iteratorVar, origLength);
            var accessOld = Expression.ArrayAccess(origArray, iteratorVar );
            var accessNew = Expression.ArrayAccess(newArrayVar, iteratorVar );
            var assignFromOldToNew = Expression.Assign( accessNew, accessOld );
            var iteratorInitialValue = Expression.Constant( 0 );
            var assignInitialValueToIterator = Expression.Assign( iteratorVar, iteratorInitialValue );

            var incrementIterator = Expression.Increment( iteratorVar );
            var assignInc = Expression.Assign( iteratorVar, incrementIterator );
            var storeNewArrayInVar = Expression.Assign( newArrayVar, initArray );
            var loopBlock1 = Expression.Block
            (
                assignFromOldToNew,
                assignInc
            );
            var assignNewInputToNew = Expression.Assign( accessNew, newDotDefInvoke );
            var returnExpression = Expression.Return( breakLabel, newArrayVar );
            var loopBlock3 = Expression.Block
            (
                assignNewInputToNew,
                returnExpression
            );

            var branch1 = Expression.IfThenElse( compare1, loopBlock1, loopBlock3 );
            var mainLoop = Expression.Loop( branch1, breakLabel );
            var mainBlock = Expression.Block
            (   dotDefArrayType, new[] { iteratorVar, newArrayVar },
                storeNewArrayInVar,
                assignInitialValueToIterator,
                mainLoop
            );
            var finalAssignment = Expression.Assign( origArray, mainBlock );
            AddNewDotDef = Expression.Lambda<AddNewDotDefDelegate>( finalAssignment, inputParameter ).Compile();


            HooksCore.RoR2.DotController.AddDot.Il += AddDot_Il;
            HooksCore.RoR2.DotController.FixedUpdate.Il += FixedUpdate_Il;
            HooksCore.RoR2.DotController.EvaluateDotStacksForType.Il += EvaluateDotStacksForType_Il;
            HooksCore.RoR2.DotController.Awake.Il += Awake_Il;

            loaded = true;
        }



        private static readonly DotController.DotIndex startingIndex = EnumExtensions.GetMax<DotController.DotIndex>();
        private static DotController.DotIndex currentIndex = startingIndex;
        private static FieldInfo arrayField;

        private static readonly Dictionary<DotController.DotIndex, CustomDoTDamageDelegate> customDoTDamages = new Dictionary<DotController.DotIndex, CustomDoTDamageDelegate>();


        private delegate void AddNewDotDefDelegate( DoTDef dot );


#pragma warning disable IDE1006 // Naming Styles
        private static readonly AddNewDotDefDelegate AddNewDotDef;
#pragma warning restore IDE1006 // Naming Styles


        private static void EvaluateDotStacksForType_Il( ILContext il )
        {
            var cursor = new ILCursor( il );

            _ = cursor.GotoNext( MoveType.After, x => x.MatchStloc( 1 ) );
            _ = cursor.Emit( OpCodes.Ldloc_1 );
            _ = cursor.Emit( OpCodes.Ldsfld, arrayField );
            _ = cursor.Emit( OpCodes.Ldarg_1 );
            _ = cursor.EmitDelegate<Func<System.Object, System.Object, Int32, System.Object>>( ( val, r, index ) =>
            {
                if( val != null ) return val;

                var array = (Array)r;
                return array.GetValue( index );
            } );
            _ = cursor.Emit( OpCodes.Stloc_1 );

            MethodReference method = null;
            Int32 damageLoc = 0;

            _ = cursor.GotoNext( MoveType.AfterLabel,
                x => x.MatchLdarg(0),
                x => x.MatchCallOrCallvirt( out method ),
                x => x.MatchLdloc( out damageLoc ),
                x => x.MatchCallOrCallvirt<RoR2.HealthComponent>( "TakeDamage" )
            );
            ILLabel breakLabel = cursor.MarkLabel();
            cursor.Index += 4;
            ILLabel skipLabel = cursor.MarkLabel();
            cursor.Index -= 4;
            _ = cursor.MoveBeforeLabels();

            Int32 ind = cursor.EmitReference<Dictionary<DotController.DotIndex, CustomDoTDamageDelegate>>( customDoTDamages );
            _ = cursor.Emit( OpCodes.Ldarg_1 );
            _ = cursor.Emit( OpCodes.Call, typeof( Dictionary<DotController.DotIndex, CustomDoTDamageDelegate> ).GetMethod( "ContainsKey" ) );
            _ = cursor.Emit( OpCodes.Brfalse_S, breakLabel );

            cursor.EmitGetReference<Dictionary<DotController.DotIndex, CustomDoTDamageDelegate>>( ind );
            _ = cursor.Emit( OpCodes.Ldarg_1 );
            _ = cursor.Emit<Dictionary<DotController.DotIndex, CustomDoTDamageDelegate>>( OpCodes.Call, "get_Item" );
            _ = cursor.Emit( OpCodes.Ldarg_0 );
            _ = cursor.Emit( OpCodes.Call, method );
            _ = cursor.Emit( OpCodes.Ldloc, damageLoc );
            _ = cursor.Emit<CustomDoTDamageDelegate>( OpCodes.Callvirt, "Invoke" );
            _ = cursor.Emit( OpCodes.Br_S, skipLabel );
        }


        private static void FixedUpdate_Il( MonoMod.Cil.ILContext il )
        {
            var cursor = new ILCursor( il );

            _ = cursor.GotoNext( MoveType.AfterLabel, x => x.MatchStloc( 2 ) );
            _ = cursor.Emit( OpCodes.Ldsfld, arrayField );
            _ = cursor.Emit( OpCodes.Ldloc_1 );
            _ = cursor.EmitDelegate<Func<System.Object, System.Object, Int32, System.Object>>( ( val, r, ind ) =>
            {
                if( val != null ) return val;

                var array = (Array)r;
                return array.GetValue( ind );
            } );

            _ = cursor.GotoNext( MoveType.AfterLabel, x => x.MatchLdcI4( (Int32)startingIndex ) );
            _ = cursor.Remove();
            _ = cursor.Emit( OpCodes.Ldsfld, arrayField );
            _ = cursor.Emit( OpCodes.Ldlen );
            _ = cursor.Emit( OpCodes.Conv_I4 );
        }


        private static void AddDot_Il( MonoMod.Cil.ILContext il )
        {
            var cursor = new ILCursor( il );

            _ = cursor.GotoNext( MoveType.AfterLabel, x => x.MatchLdcI4( (Int32)startingIndex ) );
            _ = cursor.Remove();
            _ = cursor.Emit( OpCodes.Ldsfld, arrayField );
            _ = cursor.Emit( OpCodes.Ldlen );
            _ = cursor.Emit( OpCodes.Conv_I4 );
        }

        private static void Awake_Il( ILContext il )
        {
            var cursor = new ILCursor( il );

            _ = cursor.GotoNext( MoveType.AfterLabel, x => x.MatchLdcI4( (Int32)startingIndex ) );
            _ = cursor.Remove();
            _ = cursor.Emit( OpCodes.Ldsfld, arrayField );
            _ = cursor.Emit( OpCodes.Ldlen );
            _ = cursor.Emit( OpCodes.Conv_I4 );
        }
    }
}
