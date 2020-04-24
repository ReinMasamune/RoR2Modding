using System;
using System.Collections.Generic;
using System.Text;
using RoR2;
using UnityEngine;

namespace Sniper.ScriptableObjects
{
    internal class SniperCameraParams : CharacterCameraParams
    {
        internal static SniperCameraParams Create( Vector3 throwPosition )
        {
            var obj = ScriptableObject.CreateInstance<SniperCameraParams>();
            obj.throwLocalCameraPos = throwPosition;

            return obj;
        }

        [SerializeField]
        internal Vector3 throwLocalCameraPos;
    }
}
