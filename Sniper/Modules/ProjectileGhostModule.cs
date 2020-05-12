using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using Sniper.ScriptableObjects;
using UnityEngine;

namespace Sniper.Modules
{
    internal static class ProjectileGhostModule
    {
        private static GameObject _baseKnifeGhost;
        private static GameObject GetBaseKnifeGhost()
        {
            if( _baseKnifeGhost == null )
            {
                _baseKnifeGhost = CreateBaseKnifeGhost();
            }

            return _baseKnifeGhost;
        }
        private static GameObject CreateBaseKnifeGhost()
        {
            var obj = PrefabsCore.CreatePrefab( "KnifeGhost", false );


            var knifeMesh = new GameObject( "KnifeMesh" ).transform;
            knifeMesh.parent = obj.transform;
            knifeMesh.localPosition = Vector3.zero;
            knifeMesh.localScale = Vector3.one;
            knifeMesh.localRotation = Quaternion.identity;



            var knifeTrail = new GameObject( "Trail" ).transform;
            knifeTrail.parent = obj.transform;
            knifeTrail.localPosition = Vector3.zero;
            knifeTrail.localScale = Vector3.one;
            knifeTrail.localRotation = Quaternion.identity;


            return obj;
        }
    }
}
