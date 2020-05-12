namespace ReinCore
{
    using System;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using System.Reflection;
    using BepInEx;
    using UnityEngine;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public static class Invoker
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public static TSignature CreateDelegate<TSignature>( MethodInfo method ) where TSignature : Delegate => (TSignature)Delegate.CreateDelegate( typeof( TSignature ), method );
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
