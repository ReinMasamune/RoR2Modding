using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal static class UVMapper
    {
        internal static void Map( Mesh mesh, Boolean logging = false )
        {
            var verts = mesh.vertices;
            var tris = mesh.triangles;
            var normals = mesh.normals;
            var uv = mesh.uv;


            var boneWeights = mesh.boneWeights;

            var tangents = mesh.tangents;



            var mapper = new UVMapperInst( logging, verts, tris, normals, uv, tangents, boneWeights );
            mapper.GenerateTriangles();
            mapper.GenerateLinks();
            mapper.GenerateVerticies();
            mapper.Seperate();

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
    }
}
