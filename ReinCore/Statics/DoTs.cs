namespace ReinCore
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    using Mono.Cecil;
    using Mono.Cecil.Cil;

    using MonoMod.Cil;

    using RoR2;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class DoTsCore
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static Boolean loaded { get; internal set; } = false;
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public delegate void CustomDoTDamageDelegate( HealthComponent victim, DamageInfo damage );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static DotController.DotIndex AddDotType( DoTDef dot, Boolean blockMergeTicks = false, CustomDoTDamageDelegate customDamage = null )
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if( !loaded )
            {
                throw new CoreNotLoadedException( nameof( DoTsCore ) );
            }

            if( dot == null )
            {
                throw new ArgumentNullException( nameof( dot ) );
            }

            DotController.DotIndex ind = currentIndex++;

            AddNewDotDef( dot );
            if( customDamage != null )
            {
                customDoTDamages[ind] = customDamage;
            }
            if( blockMergeTicks )
            {
                _ = mergeBlocked.Add( ind );
            }

            return ind;
        }



        static DoTsCore()
        {
            BindingFlags allFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;
            Type controllerType = typeof(DotController);
            Type dotDefType = controllerType.GetNestedType( "DotDef", allFlags );
            Type dotDefArrayType = dotDefType.MakeArrayType();
            arrayField = controllerType.GetField( "dotDefs", allFlags );
            FieldInfo intervalField = dotDefType.GetField( "interval", allFlags );
            FieldInfo damageCoefField = dotDefType.GetField( "damageCoefficient", allFlags );
            FieldInfo damageColorIndexField = dotDefType.GetField( "damageColorIndex", allFlags );
            FieldInfo associatedBuffField = dotDefType.GetField( "associatedBuff", allFlags );
            MemberInfo custInterval = typeof(DoTDef).GetMember( nameof(DoTDef.interval), allFlags )[0];
            MemberInfo custDamageCoef = typeof(DoTDef).GetMember( nameof(DoTDef.damageCoefficient), allFlags )[0];
            MemberInfo custDamageColor = typeof(DoTDef).GetMember( nameof(DoTDef.damageColorIndex), allFlags )[0];
            MemberInfo custAssociatedBuff = typeof(DoTDef).GetMember( nameof(DoTDef.associatedBuff), allFlags )[0];
            Type constructionFuncType = typeof(Func<,>).MakeGenericType( typeof(DoTDef), dotDefType );

            ParameterExpression inputParameter = Expression.Parameter( typeof(DoTDef), "NewDotDef" );
            MemberExpression inputInterval = Expression.MakeMemberAccess( inputParameter, custInterval );
            MemberExpression inputDamageCoef = Expression.MakeMemberAccess( inputParameter, custDamageCoef );
            MemberExpression inputDamageColor = Expression.MakeMemberAccess( inputParameter, custDamageColor );
            MemberExpression inputAssociatedBuff = Expression.MakeMemberAccess( inputParameter, custAssociatedBuff );
            MemberAssignment intervalBind = Expression.Bind( intervalField, inputInterval );
            MemberAssignment damageCoefBind = Expression.Bind( damageCoefField, inputDamageCoef );
            MemberAssignment damageColorBind = Expression.Bind( damageColorIndexField, inputDamageColor );
            MemberAssignment associatedBuffBind = Expression.Bind( associatedBuffField, inputAssociatedBuff );
            NewExpression newDotDef = Expression.New( dotDefType );
            MemberInitExpression newDotDefInit = Expression.MemberInit(newDotDef, intervalBind, damageCoefBind, damageColorBind, associatedBuffBind );
            LambdaExpression newDotDefFunc = Expression.Lambda( constructionFuncType, newDotDefInit, inputParameter );
            InvocationExpression newDotDefInvoke = Expression.Invoke( newDotDefFunc, inputParameter );
            MemberExpression origArray = Expression.Field( null, arrayField );
            UnaryExpression origLength = Expression.ArrayLength( origArray );
            BinaryExpression length = Expression.Add( origLength, Expression.Constant(1) );
            NewArrayExpression initArray = Expression.NewArrayBounds(dotDefType, length);
            LabelTarget breakLabel = Expression.Label( dotDefArrayType, "return" );
            ParameterExpression iteratorVar = Expression.Variable( typeof(Int32), "iterator" );
            ParameterExpression newArrayVar = Expression.Variable( dotDefArrayType, "newArray" );
            BinaryExpression compare1 = Expression.LessThan(iteratorVar, origLength);
            IndexExpression accessOld = Expression.ArrayAccess(origArray, iteratorVar );
            IndexExpression accessNew = Expression.ArrayAccess(newArrayVar, iteratorVar );
            BinaryExpression assignFromOldToNew = Expression.Assign( accessNew, accessOld );
            ConstantExpression iteratorInitialValue = Expression.Constant( 0 );
            BinaryExpression assignInitialValueToIterator = Expression.Assign( iteratorVar, iteratorInitialValue );

            UnaryExpression incrementIterator = Expression.Increment( iteratorVar );
            BinaryExpression assignInc = Expression.Assign( iteratorVar, incrementIterator );
            BinaryExpression storeNewArrayInVar = Expression.Assign( newArrayVar, initArray );
            BlockExpression loopBlock1 = Expression.Block
            (
                assignFromOldToNew,
                assignInc
            );
            BinaryExpression assignNewInputToNew = Expression.Assign( accessNew, newDotDefInvoke );
            GotoExpression returnExpression = Expression.Return( breakLabel, newArrayVar );
            BlockExpression loopBlock3 = Expression.Block
            (
                assignNewInputToNew,
                returnExpression
            );

            ConditionalExpression branch1 = Expression.IfThenElse( compare1, loopBlock1, loopBlock3 );
            LoopExpression mainLoop = Expression.Loop( branch1, breakLabel );
            BlockExpression mainBlock = Expression.Block
            (   dotDefArrayType, new[] { iteratorVar, newArrayVar },
                storeNewArrayInVar,
                assignInitialValueToIterator,
                mainLoop
            );
            BinaryExpression finalAssignment = Expression.Assign( origArray, mainBlock );
            AddNewDotDef = Expression.Lambda<AddNewDotDefDelegate>( finalAssignment, inputParameter ).Compile();


            HooksCore.RoR2.DotController.AddDot.Il += AddDot_Il;
            HooksCore.RoR2.DotController.FixedUpdate.Il += FixedUpdate_Il;
            HooksCore.RoR2.DotController.EvaluateDotStacksForType.Il += EvaluateDotStacksForType_Il;
            HooksCore.RoR2.DotController.Awake.Il += Awake_Il;

            loaded = true;
        }



        private static readonly DotController.DotIndex startingIndex = EnumExtensions.GetMax<DotController.DotIndex>();
        private static DotController.DotIndex currentIndex = startingIndex;
        private static readonly FieldInfo arrayField;

        private static readonly Dictionary<DotController.DotIndex, CustomDoTDamageDelegate> customDoTDamages = new Dictionary<DotController.DotIndex, CustomDoTDamageDelegate>();
        private static readonly HashSet<DotController.DotIndex> mergeBlocked = new HashSet<DotController.DotIndex>();

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
                if( val != null )
                {
                    return val;
                }

                var array = (Array)r;
                return array.GetValue( index );
            } );
            _ = cursor.Emit( OpCodes.Stloc_1 );

            _ = cursor.GotoNext( MoveType.AfterLabel, x => x.MatchCallOrCallvirt<DotController>( "AddPendingDamageEntry" ) );
            ILLabel passLabel = cursor.MarkLabel();
            cursor.Index++;
            ILLabel overLabel = cursor.MarkLabel();
            cursor.Index--;
            _ = cursor.MoveBeforeLabels();


            _ = cursor.EmitReference<HashSet<DotController.DotIndex>>( mergeBlocked );
            _ = cursor.Emit( OpCodes.Ldarg_1 );
            _ = cursor.Emit<HashSet<DotController.DotIndex>>( OpCodes.Call, "Contains" );
            _ = cursor.Emit( OpCodes.Brfalse, passLabel );

            Type dotStackType = typeof(DotController).GetNestedType( "DotStack", BindingFlags.NonPublic );
            FieldInfo attackerField = dotStackType.GetField( "attackerObject" );
            FieldInfo damageField = dotStackType.GetField( "damage" );
            FieldInfo damageTypeField = dotStackType.GetField( "damageType" );


            Type pendingDamageType = typeof(DotController).GetNestedType("PendingDamage", BindingFlags.NonPublic );
            FieldInfo pendingAttackerField = pendingDamageType.GetField( "attackerObject" );
            FieldInfo pendingDamageField = pendingDamageType.GetField( "totalDamage" );
            FieldInfo pendingDamageTypeField = pendingDamageType.GetField( "damageType" );


            MethodInfo addMethod = typeof(List<>).MakeGenericType(pendingDamageType).GetMethod( "Add" );

            _ = cursor.Emit( OpCodes.Pop );
            _ = cursor.Emit( OpCodes.Pop );
            _ = cursor.Emit( OpCodes.Pop );
            _ = cursor.Emit( OpCodes.Newobj, pendingDamageType.GetConstructor( Array.Empty<Type>() ) );
            _ = cursor.Emit( OpCodes.Dup );
            _ = cursor.Emit( OpCodes.Ldloc_3 );
            _ = cursor.Emit( OpCodes.Ldfld, attackerField );
            _ = cursor.Emit( OpCodes.Stfld, pendingAttackerField );
            _ = cursor.Emit( OpCodes.Dup );
            _ = cursor.Emit( OpCodes.Ldloc_3 );
            _ = cursor.Emit( OpCodes.Ldfld, damageField );
            _ = cursor.Emit( OpCodes.Stfld, pendingDamageField );
            _ = cursor.Emit( OpCodes.Dup );
            _ = cursor.Emit( OpCodes.Ldloc_3 );
            _ = cursor.Emit( OpCodes.Ldfld, damageTypeField );
            _ = cursor.Emit( OpCodes.Stfld, pendingDamageTypeField );
            _ = cursor.Emit( OpCodes.Callvirt, addMethod );
            _ = cursor.Emit( OpCodes.Br_S, overLabel );

            MethodReference method = null;
            Int32 damageLoc = 0;

            _ = cursor.GotoNext( MoveType.AfterLabel,
                x => x.MatchLdarg( 0 ),
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
                if( val != null )
                {
                    return val;
                }

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
