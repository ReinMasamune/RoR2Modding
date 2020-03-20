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
        public SeedJob( NativeArray<VertexData> verts, NativeArray<Int32> vertInds, Vector3 center, Vector3 min, Vector3 max, Single tilesPerUnit )
        {
            this.vertices = verts;
            this.vertexInds = vertInds;
            this.centerPoint = center;
            this.minVector = min;
            this.maxVector = max;
            this.sizeVector = max - min;
            this.midVector = (max + min) / 2f;

            var maxComp = Mathf.Max( this.sizeVector.x, this.sizeVector.y, this.sizeVector.z );

            this.tileVector = this.sizeVector;
            this.tileVector *= tilesPerUnit;
            this.tileFactor = tilesPerUnit;
        }

        public void Execute( Int32 index )
        {
            var ind = this.vertexInds[index];
            var vert = this.vertices[ind].reff;
            var pos = vert.position;
            pos -= this.minVector;
            pos *= this.tileFactor;
            pos.x = Mathf.Repeat( pos.x, 1f );
            //pos.y = Mathf.Repeat( pos.y, 1f );
            pos.z = Mathf.Repeat( pos.z, 1f );
            var tempV = pos.z;
            var tempU = pos.x;
            var uv = new Vector2( tempU, tempV );
            vert.uv = uv;
        }



        private NativeArray<VertexData> vertices;
        private NativeArray<Int32> vertexInds;
        private readonly Single tileFactor;
        private readonly Vector3 centerPoint;
        private readonly Vector3 minVector;
        private readonly Vector3 maxVector;
        private readonly Vector3 sizeVector;
        private readonly Vector3 midVector;
        private readonly Vector3 tileVector;

    }
}
