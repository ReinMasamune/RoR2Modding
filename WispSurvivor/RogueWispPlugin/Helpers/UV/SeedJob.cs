using RoR2;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    public struct SeedJob : IJobParallelFor
    {
        public SeedJob( NativeArray<Vertex> verts, NativeArray<Int32> vertInds, Vector3 center )
        {
            this.vertices = verts;
            this.vertexInds = vertInds;
            this.centerPoint = center;
        }

        public void Execute( Int32 index )
        {
            var ind = this.vertexInds[index];
            var pos = this.vertices[ind].position;
            pos -= this.centerPoint;
            var length = pos.magnitude;
            pos /= length;
            pos += Vector3.one;
            pos /= 2f;
            


            var tempU = pos.z;
            var tempV = Mathf.Atan2( pos.y, pos.x ) / Mathf.PI / 2f;

            //tempU = Mathf.Abs( tempU );
            //if( tempU > 1f ) tempU -= Mathf.FloorToInt( tempU );

            //tempV = Mathf.Abs( tempV );
            //if( tempV > 1f ) tempV -= Mathf.FloorToInt( tempV );

            var uv = new Vector2( tempU, tempV );

            var vert = this.vertices[ind];
            vert.SetUV( uv );
            this.vertices[ind] = vert;
        }



        private NativeArray<Vertex> vertices;
        private NativeArray<Int32> vertexInds;
        private Vector3 centerPoint;

    }
}
