using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    internal static partial class EffectHelper
    {
        private static Dictionary<GameObject,UInt32> rotatorCounter = new Dictionary<GameObject, UInt32>();
        internal static Transform AddRotator( GameObject mainObj, Vector3 spinSpeed, Vector3 spinAxis, Single radius, params Transform[] transforms )
        {
            if( !rotatorCounter.ContainsKey( mainObj ) ) rotatorCounter[mainObj] = 0u;
            var obj = new GameObject( "Rotator" + rotatorCounter[mainObj]++ );
            obj.transform.parent = mainObj.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.forward = mainObj.transform.TransformDirection(spinAxis);


            var rotation = obj.AddComponent<RotateObject>();
            rotation.rotationSpeed = spinSpeed;


            var count = transforms.Length;
            if( count == 0 )
            {
                throw new ArgumentException( "Must include at least one transform" );
            }
            var angStep = 2f * Mathf.PI / (count);
            for( Int32 i = 0; i < count; ++i )
            {
                var trans = transforms[i];
                var ang = angStep * i;
                var pos = new Vector3( radius * Mathf.Cos(ang), radius * Mathf.Sin(ang), 0f );
                trans.parent = obj.transform;
                trans.localPosition = pos;
                trans.localRotation = Quaternion.identity;
            }

            return obj.transform;
        }
    }
}
