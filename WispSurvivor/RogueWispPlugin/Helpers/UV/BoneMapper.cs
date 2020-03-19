using RoR2;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    public struct BoneMapper : IJob
    {
        public static readonly Int32 maxIterations = 100;
        public readonly Int32 boneIndex;

        public BoneMapper( Int32 boneIndex, NativeArray<VertexData> verts, NativeArray<LinkData> links, NativeArray<TriangleData> tris, NativeArray<Int32> vertInds, NativeArray<Int32> linkInds, NativeArray<Int32> triInds )
        {
            this.boneIndex = boneIndex;
            this.verticies = verts;
            this.links = links;
            this.triangles = tris;
            this.vertexInds = vertInds;
            this.linkInds = linkInds;
            this.triangleInds = triInds;
            this.iterationCounter = 0;
        }

        public void Execute()
        {
            var linksQueue = new Queue<LinkData>();
            var trisQueue = new Queue<TriangleData>();
            do
            {

            } while( this.ContinueIteration( linksQueue, trisQueue ) );
        }

        public JobHandle GetSeedJob()
        {
            var total = Vector3.zero;
            var min = new Vector3( Single.MaxValue, Single.MaxValue, Single.MaxValue );
            var max = new Vector3( Single.MinValue, Single.MinValue, Single.MinValue );
            for( Int32 i = 0; i < this.vertexInds.Length; ++i )
            {
                var pos = this.verticies[this.vertexInds[i]].position;
                min.x = Mathf.Min( min.x, pos.x );
                min.y = Mathf.Min( min.y, pos.y );
                min.z = Mathf.Min( min.z, pos.z );
                max.x = Mathf.Max( max.x, pos.x );
                max.y = Mathf.Max( max.y, pos.y );
                max.z = Mathf.Max( max.z, pos.z );
                total += this.verticies[this.vertexInds[i]].position;
            }
            total /= this.vertexInds.Length;


            return new SeedJob(this.verticies, this.vertexInds, total, min, max, 0.75f ).Schedule(this.vertexInds.Length, 1);
        }

        
        private NativeArray<VertexData> verticies;
        private NativeArray<LinkData> links;
        private NativeArray<TriangleData> triangles;

        private NativeArray<Int32> vertexInds;
        private NativeArray<Int32> linkInds;
        private NativeArray<Int32> triangleInds;

        private Int32 iterationCounter;

        private Boolean ContinueIteration(Queue<LinkData> linksQueue, Queue<TriangleData> trisQueue )
        {
            if( this.iterationCounter++ < maxIterations )
            {
                for( Int32 i = 0; i < this.linkInds.Length; ++i )
                {
                    var l = this.links[this.linkInds[i]];


                }


            }
            return false;
        }
    }
}
