using RoR2;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    public struct Vertex
    {
        public Int32 index
        {
            get => this.data.index;
        }
        public Vector3 position
        {
            get => this.data.position;
        }
        public Vector3 normal
        {
            get => this.data.normal;
        }
        public Vector4 tangent
        {
            get => this.data.tangent;
        }
        public Vector2 uv
        {
            get => this.data.uv;
            set
            {
                var temp = this.data;
                temp.uv = value;
                this.data = temp;
            }
        }
        public Vector2 averageOfConnectedUVs
        {
            get
            {
                var avg = Vector2.zero;
                Int32 totalAdded = 0;
                for( Int32 i = 0; i < this.data.links.Length; ++i )
                {
                    var link = this._linksBuffer[this.data.links[i]].reff;
                    Vertex other = ( link.vertex1.index == this.index ) ? link.vertex2 : link.vertex1;
                    if( other.uv == this.uv ) continue;

                    totalAdded++;
                    avg += other.uv;
                }
                return avg / totalAdded;
            }
        }
        public Vertex( Int32 index, NativeArray<VertexData> vertexBuffer, NativeArray<LinkData> linkBuffer, NativeArray<TriangleData> triangleBuffer )
        {
            this._index = index;
            this._vertexBuffer = vertexBuffer;
            this._linksBuffer = linkBuffer;
            this._trianglesBuffer = triangleBuffer;
        }
        private VertexData data
        {
            get => this._vertexBuffer[this._index];
            set => this._vertexBuffer[this._index] = value;
        }
        private readonly Int32 _index;
        private NativeArray<VertexData> _vertexBuffer;
        private NativeArray<LinkData> _linksBuffer;
        private NativeArray<TriangleData> _trianglesBuffer;
    }
}
