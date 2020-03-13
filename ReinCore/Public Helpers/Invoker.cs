using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using BepInEx;
using UnityEngine;

namespace ReinCore
{
    public static class Invoker
    {
        public static TSignature CreateDelegate<TSignature>( MethodInfo method ) where TSignature : Delegate
        {
            return (TSignature)Delegate.CreateDelegate( typeof( TSignature ), method );
        }
    }
}
