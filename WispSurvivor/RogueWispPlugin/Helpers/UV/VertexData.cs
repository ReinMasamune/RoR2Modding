using RoR2;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    public struct VertexData : IDisposable
    {
        public readonly Int32 index;
        public readonly Vector3 position;
        public readonly Vector3 normal;
        public readonly Vector4 tangent;
        public Vector2 uv;

        public VertexData( Int32 index, Vector3 position, Vector3 normal, Vector4 tangent, Vector2 uv, Int32[] links, Int32[] triangles )
        {
            this.index = index;
            this.position = position;
            this.normal = normal;
            this.tangent = tangent;
            this.uv = uv;

            this.links = new NativeArray<Int32>( links, Allocator.TempJob );
            this.triangles = new NativeArray<Int32>( triangles, Allocator.TempJob );

            this.linksBuffer = default;
            this.vertexBuffer = default;
            this.trianglesBuffer = default;
        }

        public void AssignBuffers( NativeArray<VertexData> vertexBuffer, NativeArray<LinkData> linksBuffer, NativeArray<TriangleData> trianglesBuffer )
        {
            this.vertexBuffer = vertexBuffer;
            this.linksBuffer = linksBuffer;
            this.trianglesBuffer = trianglesBuffer;
        }

        public Vertex reff
        {
            get => new Vertex( this.index, this.vertexBuffer, this.linksBuffer, this.trianglesBuffer );
        }

        public void Dispose()
        {
            this.links.Dispose();
            this.triangles.Dispose();
        }

        private NativeArray<Int32> links;
        private NativeArray<Int32> triangles;

        private NativeArray<VertexData> vertexBuffer;
        private NativeArray<LinkData> linksBuffer;
        private NativeArray<TriangleData> trianglesBuffer;
    }
}
