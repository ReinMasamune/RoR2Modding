using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal struct TransformParams
    {
        private Vector3 position;
        private Vector3 rotation;
        private Vector3 scale;

        internal static TransformParams identity = new TransformParams( Vector3.zero, Vector3.zero, Vector3.one );

        internal TransformParams( Vector3 position, Vector3 rotation, Vector3 scale )
        {
            this.position = position;
            this.rotation = rotation;
            this.scale = scale;
        }

        internal void Apply( Transform t )
        {
            t.localPosition = this.position;
            t.localEulerAngles = this.rotation;
            t.localScale = this.scale;
        }

        internal void Apply( ParticleSystem.ShapeModule shape )
        {
            shape.scale = this.scale;
            shape.position = this.position;
            shape.rotation = this.rotation;
        }
    }
}
