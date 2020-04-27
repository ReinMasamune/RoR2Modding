using System;
using System.Linq.Expressions;
using System.Reflection;
using BepInEx;
using UnityEngine;

namespace ReinCore
{
    public static class GenericUnityExtensions
    {

        public static TObject Instantiate<TObject>( this TObject obj ) where TObject : UnityEngine.Object => UnityEngine.Object.Instantiate( obj );
    }
}
