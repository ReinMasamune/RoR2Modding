//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq.Expressions;
//using Mono.Cecil;
//using RoR2;
//using RoR2.Networking;
//using UnityEngine.Networking;
//using System.Linq;

//namespace ReinCore
//{
//    public class Buffs
//    {
//        //Fast wrapper for HasBuff, GetBuffCount, etc, if needed.
//    }

//    public delegate Single StatDelegate( Single inStat, Inventory inventory, Buffs buffs );
//    public static class StatsThings
//    {
//        // It may be worth switching this to be backed by a linked list of some kind, would make the transformation into one function faster.
//        public static ObservableCollection<Expression<StatDelegate>> calculateHealth = new ObservableCollection<Expression<StatDelegate>>();
//        internal static StatDelegate healthCalculateDelegate
//        {
//            get
//            {
//                //Null check not actually needed for this setup specifically since it defaults to dirty
//                if( healthDirty || backingHealthCalculate == null )
//                {
//                    backingHealthCalculate = CompileStatDelegate( calculateHealth );
//                }
//                return backingHealthCalculate;
//            }
//        }
//        private static StatDelegate backingHealthCalculate;
//        private static Boolean healthDirty;


//        private static List<Expression> tempCalculationList = new List<Expression>();
//        private static StatDelegate CompileStatDelegate( IEnumerable<Expression<StatDelegate>> expressions )
//        {
//            ParameterExpression inputStat = Expression.Parameter( typeof(Single), "input" );
//            ParameterExpression inventory = Expression.Parameter( typeof(Inventory), "inventory" );
//            ParameterExpression buffs  = Expression.Parameter( typeof(Buffs), "buffs" );
//            ParameterExpression runningOutputStat = Expression.Variable( typeof(Single), "result" );
//            IEnumerable<ParameterExpression> parameters = new[] { runningOutputStat, inventory, buffs };

//            tempCalculationList.Clear();
//            tempCalculationList.Add( Expression.Assign( runningOutputStat, inputStat ) );

//            //It may be worth doing a sort of some kind here to order the expressions based on addition/multiplication/etc to get more consistent behaviour.
//            tempCalculationList.AddRange( expressions.Select<Expression<StatDelegate>, Expression>(
//                ( expr ) => Expression.Assign( runningOutputStat, expr.Update( expr, parameters ) )
//            ) );

//            var mainBlock = Expression.Block( typeof(Single), new[] { inputStat, inventory, buffs, runningOutputStat }, tempCalculationList );

//            var lambda = Expression.Lambda<StatDelegate>( mainBlock, inputStat, inventory, buffs );

//            return lambda.Compile();
//        }

//        static StatsThings()
//        {
//            healthDirty = true;
//            calculateHealth.CollectionChanged += ( _1, _2 ) => healthDirty = true;

//            //Example usage
//            //The only trick here is that the expression being added is required to be a single line of code, but conditional expressiosn are allowed.
//            calculateHealth.Add( ( inStat, inventory, buffs ) => inStat + ( 40f * inventory.GetItemCount( ItemIndex.Knurl ) ) );
//        }
//    }
//}
