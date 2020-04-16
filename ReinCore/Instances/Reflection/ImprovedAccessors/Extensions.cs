//using System;
//using System.ComponentModel;
//using System.Linq.Expressions;
//using System.Reflection;
//using BepInEx;
//using UnityEngine;

//namespace ReinCore
//{
//    public static class CachedReflection
//    {
//        //public static TValue GetValue<TValue>( this System.Object instance, String memberName )
//        //{
//        //    return default;
//        //}
//        //public static void SetValue<TValue>( this System.Object instance, String memberName, TValue value )
//        //{
//        //}
//        //public static System.Object GetValue( this System.Object instance, String memberName )
//        //{
//        //    return default;
//        //}
//        //public static void SetValue( this System.Object instance, String memberName, System.Object value )
//        //{
//        //}
//        public static TValue GetValue<TValue>( this System.Type type, String memberName )
//        {
//            return default;
//        }
//        public static void SetValue<TValue>( this Type type, String memberName, TValue value )
//        {
//        }
//        public static System.Object GetValue( this Type type, String memberName )
//        {
//            return default;
//        }
//        public static void SetValue( this Type type, String memberName, System.Object value )
//        {
//        }
//        public static TValue GetValue<TInstance,TValue>( this TInstance instance, String memberName ) where TInstance : class
//        {
//            return default;
//        }
//        public static void SetValue<TInstance,TValue>( this TInstance instance, String memberName, TValue value ) where TInstance : class
//        {
//        }
//        public static System.Object GetValue<TInstance>( this TInstance instance, String memberName ) where TInstance : class
//        {
//            return default;
//        }
//        public static void SetValue<TInstance>( this TInstance instance, String memberName, System.Object value ) where TInstance : class
//        {
//        }
//        public static TValue GetValue<TInstance, TValue>( ref this TInstance instance, String memberName ) where TInstance : struct
//        {
//            return default;
//        }
//        public static void SetValue<TInstance, TValue>( ref this TInstance instance, String memberName, TValue value  ) where TInstance : struct
//        {
//        }
//        public static System.Object GetValue<TInstance>( ref this TInstance instance, String memberName ) where TInstance : struct
//        {
//            return default;
//        }
//        public static void SetValue<TInstance>( ref this TInstance instance, String memberName, System.Object value ) where TInstance : struct
//        {
//        }
//    }
//}
