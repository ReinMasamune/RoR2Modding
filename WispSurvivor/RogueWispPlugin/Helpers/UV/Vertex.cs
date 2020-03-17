using RoR2;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    public struct Vertex : IDisposable
    {
        public readonly Int32 index;
        public readonly Vector3 position;
        public readonly Vector3 normal;
        public readonly Vector4 tangent;

        public Vector2 uv { get; private set; }

        private NativeArray<Int32> links;
        private NativeArray<Int32> triangles;

        public Vertex( Int32 index, Vector3 position, Vector3 normal, Vector4 tangent, Vector2 uv, Int32[] links, Int32[] triangles )
        {
            this.index = index;
            this.position = position;
            this.normal = normal;
            this.tangent = tangent;
            this.uv = uv;

            this.links = new NativeArray<Int32>( links, Allocator.TempJob );
            this.triangles = new NativeArray<Int32>( triangles, Allocator.TempJob );
        }

        public void Dispose()
        {
            Main.LogW( "Disposing" );
            this.links.Dispose();
            this.triangles.Dispose();
        }
    }
}
