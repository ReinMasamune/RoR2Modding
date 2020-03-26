using RoR2;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    public struct Link
    {
        public Int32 index
        {
            get => this.data.index;
        }

        public Vertex vertex1
        {
            get => this._vertexBuffer[this.data.vert1].reff;
        }
        public Vertex vertex2
        {
            get => this._vertexBuffer[this.data.vert2].reff;
        }

        public Triangle triangle1
        {
            get => this._trianglesBuffer[this.data.tri1].reff;
        }
        public Triangle triangle2
        {
            get => this._trianglesBuffer[this.data.tri2].reff;
        }


        public Single distance
        {
            get => Vector3.Distance( this.vertex1.position, this.vertex2.position );
        }

        public Single uvDistance
        {
            get => Vector2.Distance( this.vertex1.uv, this.vertex2.uv );
        }



        public Link( Int32 index, NativeArray<VertexData> vertexBuffer, NativeArray<LinkData> linkBuffer, NativeArray<TriangleData> triangleBuffer )
        {
            this._index = index;
            this._vertexBuffer = vertexBuffer;
            this._linksBuffer = linkBuffer;
            this._trianglesBuffer = triangleBuffer;
        }

        private LinkData data
        {
            get => this._linksBuffer[this._index];
            set => this._linksBuffer[this._index] = value;
        }

        private Int32 _index;
        private NativeArray<VertexData> _vertexBuffer;
        private NativeArray<LinkData> _linksBuffer;
        private NativeArray<TriangleData> _trianglesBuffer;
    }
}
