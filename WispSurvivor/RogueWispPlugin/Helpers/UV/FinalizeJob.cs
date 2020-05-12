using System;

using Unity.Collections;
using Unity.Jobs;

using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    public struct FinalizeJob : IJobParallelFor
    {
        public FinalizeJob( NativeArray<VertexData> verts )
        {
            this.vertices = verts;
        }

        public void Execute( Int32 index )
        {
            var vert = this.vertices[index].reff;
            var uv = vert.uv;
            uv.x = Mathf.Repeat( uv.x, 1f );
            uv.y = Mathf.Repeat( uv.y, 1f );
            vert.uv = uv;
        }

        private NativeArray<VertexData> vertices;
    }
}
