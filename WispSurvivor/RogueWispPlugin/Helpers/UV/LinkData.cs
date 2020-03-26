using RoR2;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    public struct LinkData
    {
        public readonly Int32 index;
        public readonly Int32 vert1;
        public readonly Int32 vert2;
        public readonly Int32 tri1;
        public readonly Int32 tri2;

        public LinkData( Int32 index, Int32 vert1, Int32 vert2, Int32 tri1, Int32 tri2 )
        {
            this.index = index;
            this.vert1 = vert1;
            this.vert2 = vert2;
            this.tri1 = tri1;
            this.tri2 = tri2;

            this.vertexBuffer = default;
            this.linksBuffer = default;
            this.trianglesBuffer = default;
        }

        public void AssignBuffers( NativeArray<VertexData> vertexBuffer, NativeArray<LinkData> linkBuffer, NativeArray<TriangleData> triangleBuffer )
        {
            this.vertexBuffer = vertexBuffer;
            this.linksBuffer = linkBuffer;
            this.trianglesBuffer = triangleBuffer;
        }

        public Link reff
        {
            get => new Link( this.index, this.vertexBuffer, this.linksBuffer, this.trianglesBuffer );
        }

        private NativeArray<VertexData> vertexBuffer;
        private NativeArray<LinkData> linksBuffer;
        private NativeArray<TriangleData> trianglesBuffer;
    }
}
