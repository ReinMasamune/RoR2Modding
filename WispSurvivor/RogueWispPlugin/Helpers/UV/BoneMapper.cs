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
        }

        public void Execute()
        {

        }

        public JobHandle GetSeedJob()
        {
            return new SeedJob().Schedule(this.vertexInds.Length, 1);
        }

        
        private NativeArray<Vertex> verticies;
        private NativeArray<Link> links;
        private NativeArray<Triangle> triangles;

        private NativeArray<Int32> vertexInds;
        private NativeArray<Int32> linkInds;
        private NativeArray<Int32> triangleInds;
    }
}
