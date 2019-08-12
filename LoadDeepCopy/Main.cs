using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using System.Collections.Generic;
using RoR2.Navigation;
using System;
using System.Reflection;

namespace CopyGameObject
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.ReinThings.LoadDeepCopy", "LoadDeepCopy", "1.0.0")]

    public class Main : BaseUnityPlugin
    {
        public GameObject Copy(this GameObject g)
        {
            GameObject obj = Instantiate(g);
            return obj;
        }

        public override bool Equals(object other)
        {
            return base.Equals(other);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
