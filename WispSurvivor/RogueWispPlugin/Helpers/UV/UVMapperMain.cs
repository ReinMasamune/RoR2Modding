using System;
using System.Diagnostics;

using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    internal static class UVMapper
    {
#if TIMER
        private static Stopwatch timer = new Stopwatch();
#endif
        internal static void Map( Mesh mesh, Boolean logging = false )
        {
#if TIMER
            timer.Start();
#endif
            var verts = mesh.vertices;
            var tris = mesh.triangles;
            var normals = mesh.normals;
            var uv = mesh.uv;


            var boneWeights = mesh.boneWeights;

            var tangents = mesh.tangents;



            var mapper = new UVMapperInst( logging, verts, tris, normals, uv, tangents, boneWeights );
#if TIMER
            LogWatch( "Init" );
#endif
            mapper.PreCache();
#if TIMER
            LogWatch( "PreCache" );
#endif
            mapper.GenerateTriangles();
#if TIMER
            LogWatch( "Triangles" );
#endif
            mapper.GenerateLinks();
#if TIMER
            LogWatch( "Links" );
#endif
            mapper.GenerateVerticies();
#if TIMER
            LogWatch( "Verticies" );
#endif
            mapper.Seperate();
#if TIMER
            LogWatch( "Seperate" );
#endif
            mapper.Seed();
#if TIMER
            LogWatch( "Seed" );
#endif
            mapper.RunMain( true );
#if TIMER
            LogWatch( "Main" );
#endif
            var newUVs = mapper.GetUVs();
#if TIMER
            LogWatch( "Get UVs" );
#endif
            mesh.uv = newUVs;
#if TIMER
            LogWatch( "Assign UVs" );
#endif

            mapper.Dispose();
#if TIMER
            LogWatch( "Dispose" );
            timer.Stop();
#endif
            //if( logging )
            //{
            //    if( verts != null && verts.Length > 0 ) Main.LogW( "Verts present" );
            //    if( tris != null && tris.Length > 0 ) Main.LogW( "Tris present" );
            //    if( normals != null && normals.Length > 0 ) Main.LogW( "Normals present" );
            //    if( uv != null && uv.Length > 0 ) Main.LogW( "Uvs present" );
            //    if( tangents != null && tangents.Length > 0 ) Main.LogW( "Tangents present" );
            //    if( boneWeights != null && boneWeights.Length > 0 ) Main.LogW( "Bones present" );
            //}

        }

#if TIMER
        private static void LogWatch( String name )
        {
            timer.Stop();
            Main.LogW( String.Format( "{0}: {1} ms", name, timer.ElapsedMilliseconds ) );
            timer.Restart();
        }
#endif
    }
}
