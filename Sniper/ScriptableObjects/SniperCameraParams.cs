namespace Sniper.ScriptableObjects
{
    using System;

    using RoR2;

    using UnityEngine;

    internal class SniperCameraParams : CharacterCameraParams
    {
        internal static SniperCameraParams Create( Vector3 throwPosition, Single standardDamp, Single scopeDamp, Single sprintDamp, Single vertDamp )
        {
            SniperCameraParams obj = ScriptableObject.CreateInstance<SniperCameraParams>();
            obj.throwLocalCameraPos = throwPosition;
            obj.standardDamp = standardDamp;
            obj.scopeDamp = scopeDamp;
            obj.sprintDamp = sprintDamp;
            obj.vertDamp = vertDamp;
            return obj;
        }

        [SerializeField]
        internal Vector3 throwLocalCameraPos;

        [SerializeField]
        internal Single vertDamp;

        [SerializeField]
        internal Single standardDamp;

        [SerializeField]
        internal Single scopeDamp;

        [SerializeField]
        internal Single sprintDamp;
    }
}
