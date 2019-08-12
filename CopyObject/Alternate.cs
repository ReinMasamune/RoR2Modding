using System;
using UnityEngine;
using BepInEx;
using System.Reflection;
using System.Collections.Generic;

namespace CreateCopy
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.CopyObject", "CopyObject", "1.0.0")]

    public class CopyObject : BaseUnityPlugin
    {
        public GameObject CreateCopy( GameObject g )
        {
            Dictionary<int, UnityEngine.Object> mask = new Dictionary<int, UnityEngine.Object>();


            return g;
        }

        public GameObject Copy( GameObject g , Dictionary<int,GameObject> mask )
        {
            if( mask[g.GetHashCode()] != null )
            {
                return mask[g.GetHashCode()];
            }

            return g;
        }


        public T CopyFields<T>( T source )
        {


            return source;
        }

    }
}
