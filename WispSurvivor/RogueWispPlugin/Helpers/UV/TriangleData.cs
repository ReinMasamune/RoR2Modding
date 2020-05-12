using System;

using Unity.Collections;

namespace Rein.RogueWispPlugin.Helpers
{
    public struct TriangleData
    {
        public readonly Int32 index;
        public readonly Int32 vert1;
        public readonly Int32 vert2;
        public readonly Int32 vert3;
        public readonly Int32 link12;
        public readonly Int32 link23;
        public readonly Int32 link31;

        public TriangleData( Int32 index, Int32 vert1, Int32 vert2, Int32 vert3, Int32 link12, Int32 link23, Int32 link31 )
        {
            this.index = index;
            this.vert1 = vert1;
            this.vert2 = vert2;
            this.vert3 = vert3;

            this.link12 = link12;
            this.link23 = link23;
            this.link31 = link31;

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

        public Triangle reff
        {
            get => new Triangle( this.index, this.vertexBuffer, this.linksBuffer, this.trianglesBuffer );
        }

        private NativeArray<VertexData> vertexBuffer;
        private NativeArray<LinkData> linksBuffer;
        private NativeArray<TriangleData> trianglesBuffer;
    }
}
