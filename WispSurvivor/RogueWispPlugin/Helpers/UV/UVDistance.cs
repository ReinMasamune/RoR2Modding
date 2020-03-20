using RoR2;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    public struct UVDistance
    {
        public UVDistance( Vector2 uv1, Vector2 uv2 )
        {
            this.uv1 = uv1;
            this.uv2 = uv2;

            var x = 0f;
            var y = 0f;
            var dx = Mathf.Abs(uv1.x - uv2.x);
            var dy = Mathf.Abs(uv1.y - uv2.y);
            var dx2 = 1f - dx;
            var dy2 = 1f - dy;

            if( dx2 < dx )
            {
                this.loopX = true;
                x = dx2;
            } else
            {
                this.loopX = false;
                x = dx;
            }

            if( dy2 < dy )
            {
                this.loopY = true;
                y = dy2;
            } else
            {
                this.loopY = false;
                y = dy;
            }

            this.difference = new Vector2( x, y );
            this.distance = this.difference.magnitude;
            this.direction12 = this.difference / this.distance;
            this.direction21 = this.direction12 * -1;

        }

        public Vector2 uv1 { get; private set; }
        public Vector2 uv2 { get; private set; }
        public Vector2 difference { get; private set; }
        public Vector2 direction12 { get; private set; }
        public Vector2 direction21 { get; private set; }
        public Single distance { get; private set; }
        public Boolean loopX { get; private set; }
        public Boolean loopY { get; private set; }
    }
}
