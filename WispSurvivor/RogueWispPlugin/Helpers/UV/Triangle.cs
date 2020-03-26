using RoR2;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    public struct Triangle
    {
        public Vertex vertex1
        {
            get => this._vertexBuffer[this.data.vert1].reff;
        }
        public Vertex vertex2
        {
            get => this._vertexBuffer[this.data.vert2].reff;
        }
        public Vertex vertex3
        {
            get => this._vertexBuffer[this.data.vert3].reff;
        }

        public Link link12
        {
            get => this._linksBuffer[this.data.link12].reff;
        }
        public Link link23
        {
            get => this._linksBuffer[this.data.link23].reff;
        }
        public Link link31
        {
            get => this._linksBuffer[this.data.link31].reff;
        }


        public Triangle( Int32 index, NativeArray<VertexData> vertexBuffer, NativeArray<LinkData> linkBuffer, NativeArray<TriangleData> triangleBuffer )
        {
            this._index = index;
            this._vertexBuffer = vertexBuffer;
            this._linksBuffer = linkBuffer;
            this._trianglesBuffer = triangleBuffer;
        }

        private TriangleData data
        {
            get => this._trianglesBuffer[this._index];
            set => this._trianglesBuffer[this._index] = value;
        }

        private Int32 _index;
        private NativeArray<VertexData> _vertexBuffer;
        private NativeArray<LinkData> _linksBuffer;
        private NativeArray<TriangleData> _trianglesBuffer;
    }
}
