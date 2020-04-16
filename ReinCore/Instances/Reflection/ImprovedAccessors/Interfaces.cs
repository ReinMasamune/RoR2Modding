//using System;
//using System.ComponentModel;
//using System.Linq.Expressions;
//using System.Reflection;
//using BepInEx;
//using UnityEngine;

//namespace ReinCore
//{
//    public delegate void AccessorSetDelegate<TInstance, TValue>( ref TInstance instance, TValue value );
//    public delegate TValue AccessorGetDelegate<TInstance, TValue>( ref TInstance instance );
//    public interface IAccessor<TInstance, TValue>
//    {
//        AccessorSetDelegate<TInstance,TValue> Set { get; }
//        AccessorGetDelegate<TInstance,TValue> Get { get; }
//    }

//    public delegate void StaticAccessorSetDelegate<TValue>( TValue value );
//    public delegate TValue StaticAccessorGetDelegate<TValue>();
//    public interface IStaticAccessor<TValue>
//    {
//        StaticAccessorSetDelegate<TValue> Set { get; }
//        StaticAccessorGetDelegate<TValue> Get { get; }
//    }
//}
