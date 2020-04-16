//using System;
//using System.ComponentModel;
//using System.Linq.Expressions;
//using System.Reflection;
//using BepInEx;

//namespace ReinCore
//{
//    public abstract class BaseAccessor
//    {
//        #region Public Static End
//        public static IAccessor<TInstance, TValue> GetInstanceAccess<TInstance, TValue>(String memberName)
//        {
//            return null;
//        }
//        public static IAccessor<System.Object,TValue> GetInstanceAccess<TValue>( Type instanceType, String memberName )
//        {
//            return null;
//        }
//        public static IAccessor<TInstance,System.Object> GetInstanceAccess<TInstance>( String memberName )
//        {
//            return null;
//        }
//        public static IAccessor<System.Object, System.Object> GetInstanceAccess( Type instanceType, Type memberType, String memberName )
//        {
//            return null;
//        }
//        public static IStaticAccessor<TValue> GetStaticAccess<TValue>( Type staticType, String memberName )
//        {
//            var obj = new UnityEngine.Object();
//            var thing = obj.GetValue<UnityEngine.Object,Int32>("Val");


//            return null;
//        }
//        public static IStaticAccessor<System.Object> GetStaticAccess( Type staticType, Type valueType, String memberName )
//        {
//            return null;
//        }



//        #endregion
//    }
//}
