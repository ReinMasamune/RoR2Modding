using RoR2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal static class UVMapper
    {
        private static Stopwatch timer = new Stopwatch();
        internal static void Map( Mesh mesh, Boolean logging = false )
        {
            timer.Start();
            var verts = mesh.vertices;
            var tris = mesh.triangles;
            var normals = mesh.normals;
            var uv = mesh.uv;


            var boneWeights = mesh.boneWeights;

            var tangents = mesh.tangents;



            var mapper = new UVMapperInst( logging, verts, tris, normals, uv, tangents, boneWeights );
            LogWatch( "Init" );

            mapper.PreCache();
            LogWatch( "PreCache" );
            mapper.GenerateTriangles();
            LogWatch( "Triangles" );
            mapper.GenerateLinks();
            LogWatch( "Links" );
            mapper.GenerateVerticies();
            LogWatch( "Verticies" );
            mapper.Seperate();
            LogWatch( "Seperate" );
            mapper.Seed();
            LogWatch( "Seed" );
            mapper.RunMain( true );
            LogWatch( "Main" );
            var newUVs = mapper.GetUVs();
            LogWatch( "Get UVs" );

            mapper.Dispose();
            LogWatch( "Dispose" );
            timer.Stop();
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

        private static void LogWatch( String name )
        {
            timer.Stop();
            Main.LogW( String.Format( "{0}: {1} ms", name, timer.ElapsedMilliseconds ) );
            timer.Restart();
        }
    }
}
