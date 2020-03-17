using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal class UVMapperInst
    {
        private readonly Vector3[] verticies;
        private readonly Int32[] triangles;
        private readonly Vector3[] normals;
        private readonly Vector2[] uvs;
        private readonly Vector4[] tangents;
        private readonly BoneWeight[] boneWeights;

        private Vertex[] globalVerticies;
        private Link[] globalLinks;
        private Triangle[] globalTriangles;

        private Int32 linkCounter;

        private readonly List<KeyValuePair<Int32,Int32>> linkList = new List<KeyValuePair<Int32, Int32>>();

        private readonly Dictionary<Int32, Int32> vertexToBone = new Dictionary<Int32, Int32>();
        private readonly Dictionary<Int32, Int32> boneToVertex = new Dictionary<Int32, Int32>();
        private readonly Dictionary<Int32, List<Int32>> boneToTriangles = new Dictionary<Int32, List<Int32>>();
        private readonly Dictionary<Int32, List<Int32>> boneToLinks = new Dictionary<Int32, List<Int32>>();
        private readonly Dictionary<Int32, BoneMapper> boneMappers = new Dictionary<Int32, BoneMapper>();
        private readonly Dictionary<Int32, HashSet<Int32>> vertexTriangleSets = new Dictionary<Int32, HashSet<Int32>>();
        private readonly Dictionary<Int32, HashSet<Int32>> vertexLinkSets = new Dictionary<Int32, HashSet<Int32>>();
        private readonly Dictionary<KeyValuePair<Int32,Int32>, Int32> linkMapping = new Dictionary<KeyValuePair<Int32, Int32>, Int32>();
        private readonly Dictionary<KeyValuePair<Int32,Int32>, KeyValuePair<Int32,Int32>> linkTriangles = new Dictionary<KeyValuePair<Int32, Int32>, KeyValuePair<Int32, Int32>>();

        internal UVMapperInst( Boolean logging, Vector3[] verticies, Int32[] triangles, Vector3[] normals, Vector2[] uvs, Vector4[] tangents, BoneWeight[] boneWeights )
        {
            this.verticies = verticies;
            this.triangles = triangles;
            this.normals = normals;
            this.uvs = uvs;
            this.tangents = tangents;
            this.boneWeights = boneWeights;
        }

        internal void GenerateTriangles()
        {
            var triCount = this.triangles.Length / 3;
            this.globalTriangles = new Triangle[triCount];
            for( Int32 i = 0; i < this.triangles.Length; i += 3 )
            {
                var triInd = i / 3;

                var ind1 = this.triangles[i];
                var ind2 = this.triangles[i+1];
                var ind3 = this.triangles[i+2];

                this.AddVertexTriangleAssociation( triInd, ind1, ind2, ind3 );

                var boneInd = this.GetBoneIndex( ind1, ind2, ind3 );

                var link1Kvp = MakeLinkKvp( ind1, ind2 );
                var link2Kvp = MakeLinkKvp( ind2, ind3 );
                var link3Kvp = MakeLinkKvp( ind3, ind1 );

                var link1Index = this.AddLink( triInd, link1Kvp );
                var link2Index = this.AddLink( triInd, link2Kvp );
                var link3Index = this.AddLink( triInd, link3Kvp );

                this.globalTriangles[triInd] = new Triangle( triInd, ind1, ind2, ind3, link1Index, link2Index, link3Index );

                List<Int32> boneTriList;
                if( !this.boneToTriangles.TryGetValue( boneInd, out boneTriList ) )
                {
                    boneTriList = new List<Int32>();
                    this.boneToTriangles[boneInd] = boneTriList; 
                }
                boneTriList.Add( triInd );
            }
        }

        internal void GenerateLinks()
        {
            var linkCount = this.linkCounter;
            this.globalLinks = new Link[linkCount];

            for( Int32 i = 0; i < linkCount; ++i )
            {
                var vertexKvp = this.linkList[i];
                this.AddVertexLinkAssociation( i, vertexKvp.Key, vertexKvp.Value );
                var boneInd = this.GetBoneIndex( vertexKvp.Key, vertexKvp.Value );
                var triKvp = this.linkTriangles[vertexKvp];

                this.globalLinks[i] = new Link( i, vertexKvp.Key, vertexKvp.Value, triKvp.Key, triKvp.Value );

                List<Int32> boneLinkList;
                if( !this.boneToLinks.TryGetValue( boneInd, out boneLinkList ) )
                {
                    boneLinkList = new List<Int32>();
                    this.boneToLinks[boneInd] = boneLinkList;
                }
                boneLinkList.Add( i );
            }
        }

        internal void GenerateVerticies()
        {
            var vertCount = this.verticies.Length;
            this.globalVerticies = new Vertex[vertCount];
            for( Int32 i = 0; i < vertCount; ++i )
            {
                var triSet = this.vertexTriangleSets[i];
                var triCount = triSet.Count;
                var triArray = new Int32[triCount];
                triSet.CopyTo( triArray );

                var linkSet = this.vertexLinkSets[i];
                var linkCount = linkSet.Count;
                var linkArray = new Int32[linkCount];
                linkSet.CopyTo( linkArray );

                this.globalVerticies[i] = new Vertex( i, this.verticies[i], this.normals[i], this.tangents[i], this.uvs[i], linkArray, triArray );
            }
        }

        internal void Seperate()
        {
        }

        

        private Int32 AddLink( Int32 triangleIndex, KeyValuePair<Int32,Int32> linkKvp )
        {
            Int32 index;
            if( !this.linkMapping.TryGetValue( linkKvp, out index ) )
            {
                index = this.linkCounter;
                this.linkCounter++;
                this.linkMapping[linkKvp] = index;
                this.linkList.Add( linkKvp );
            }

            KeyValuePair<Int32,Int32> triKvp;
            if( !this.linkTriangles.TryGetValue( linkKvp, out triKvp ) )
            {
                triKvp = new KeyValuePair<Int32, Int32>( triangleIndex, -1 );
                this.linkTriangles[linkKvp] = triKvp;
            } else
            {
                if( triKvp.Value != -1 )
                {
                    Main.LogE( "Tried to add third triangle to a link" );
                    return index;
                }

                triKvp = new KeyValuePair<Int32, Int32>( triKvp.Key, triangleIndex );
                this.linkTriangles[linkKvp] = triKvp;
            }

            return index;
        }

        private Int32 GetBoneIndex( params Int32[] verts )
        {
            Int32 index = -1;
            for( Int32 i = 0; i < verts.Length; ++i )
            {
                var temp = GetBoneIndexSingle( verts[i] );
                if( index == -1 )
                {
                    index = temp;
                    continue;
                }

                if( index != temp )
                {
                    Main.LogE( "Verticies were not from same bone" );
                }
            }

            return index;
        }

        private Int32 GetBoneIndexSingle( Int32 vert )
        {
            Int32 bone;
            if( !this.vertexToBone.TryGetValue( vert, out bone ) )
            {
                bone = GetBoneIndexFromWeights( this.boneWeights[vert] );
                this.vertexToBone[vert] = bone;
                this.boneToVertex[bone] = vert;
            }
            return bone;
        }

        private void AddVertexTriangleAssociation( Int32 triIndex, params Int32[] verts )
        {
            for( Int32 i = 0; i < verts.Length; ++i )
            {
                this.AddVertexTriangleAssociationSingle( triIndex, verts[i] );
            }
        }

        private void AddVertexLinkAssociation( Int32 linkIndex, params Int32[] verts )
        {
            for( Int32 i = 0; i < verts.Length; ++i )
            {
                this.AddVertexLinkAssociationSingle( linkIndex, verts[i] );
            }
        }

        private void AddVertexTriangleAssociationSingle( Int32 triIndex, Int32 vertIndex )
        {
            HashSet<Int32> triSet = null;
            if( !this.vertexTriangleSets.TryGetValue( vertIndex, out triSet ) )
            {
                triSet = new HashSet<Int32>();
                this.vertexTriangleSets[vertIndex] = triSet;
            }
            triSet.Add( triIndex );
        }

        private void AddVertexLinkAssociationSingle( Int32 linkIndex, Int32 vertIndex )
        {
            HashSet<Int32> linkSet = null;
            if( !this.vertexLinkSets.TryGetValue( vertIndex, out linkSet ) )
            {
                linkSet = new HashSet<Int32>();
                this.vertexLinkSets[vertIndex] = linkSet;
            }
            linkSet.Add( linkIndex );
        }

        private static Int32 GetBoneIndexFromWeights( BoneWeight weight )
        {
            var maxWeight = 0f;
            var outInd = -1;
            Boolean multiWeight = false;
            if( weight.weight0 > 0f )
            {
                if( maxWeight > 0f )
                {
                    multiWeight = true;
                }
                maxWeight = weight.weight0;
                outInd = weight.boneIndex0;
            }
            if( weight.weight1 > 0f )
            {
                if( maxWeight > 0f )
                {
                    multiWeight = true;
                }
                maxWeight = weight.weight1;
                outInd = weight.boneIndex1;
            }
            if( weight.weight2 > 0f )
            {
                if( maxWeight > 0f )
                {
                    multiWeight = true;
                }
                maxWeight = weight.weight2;
                outInd = weight.boneIndex2;

            }
            if( weight.weight3 > 0f )
            {
                if( maxWeight > 0f )
                {
                    multiWeight = true;
                }
                maxWeight = weight.weight3;
                outInd = weight.boneIndex3;
            }

            if( multiWeight )
            {
                return -100;
            } else return outInd;
        }

        private static KeyValuePair<Int32,Int32> MakeLinkKvp( Int32 ind1, Int32 ind2 )
        {
            return new KeyValuePair<Int32, Int32>( Mathf.Min( ind1, ind2 ), Mathf.Max( ind1, ind2 ) );
        }
    }
}
