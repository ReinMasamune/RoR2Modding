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

        public BoneMapper( Int32 boneIndex, NativeArray<Vertex> verts, NativeArray<Link> links, NativeArray<Triangle> tris, NativeArray<Int32> vertInds, NativeArray<Int32> linkInds, NativeArray<Int32> triInds )
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
            do
            {

            } while( this.ContinueIteration() );
        }

        public JobHandle GetSeedJob()
        {
            var total = Vector3.zero;
            for( Int32 i = 0; i < this.vertexInds.Length; ++i )
            {
                total += this.verticies[this.vertexInds[i]].position;
            }
            total /= this.vertexInds.Length;


            return new SeedJob(this.verticies, this.vertexInds, total ).Schedule(this.vertexInds.Length, 1);
        }

        
        private NativeArray<Vertex> verticies;
        private NativeArray<Link> links;
        private NativeArray<Triangle> triangles;

        private NativeArray<Int32> vertexInds;
        private NativeArray<Int32> linkInds;
        private NativeArray<Int32> triangleInds;

        private Int32 iterationCounter;

        private Boolean ContinueIteration()
        {
            if( this.iterationCounter++ < maxIterations )
            {



            }
            return false;
        }
    }
}
