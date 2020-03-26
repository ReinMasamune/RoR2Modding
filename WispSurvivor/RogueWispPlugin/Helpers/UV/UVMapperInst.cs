using RoR2;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    internal class UVMapperInst : IDisposable
    {
        public void Dispose()
        {
            if( this.globalVerticies != null )
            {
                for( Int32 i = 0; i < this.globalVerticies.Length; ++i )
                {
                    this.globalVerticies[i].Dispose();
                }
            }

            if( this.boneVerts != null )
            {
                for( Int32 i = 0; i < this.boneVerts.Count; ++i )
                {
                    this.boneVerts[i].Dispose();
                }
            }

            if( this.boneTris != null )
            {
                for( Int32 i = 0; i < this.boneTris.Count; ++i )
                {
                    this.boneTris[i].Dispose();
                }
            }

            if( this.boneLinks != null )
            {
                for( Int32 i = 0; i < this.boneLinks.Count; ++i )
                {
                    this.boneLinks[i].Dispose();
                }
            }

            this.nativeVerts.Dispose();
            this.nativeLinks.Dispose();
            this.nativeTris.Dispose();
        }


        private readonly Vector3[] verticies;
        private readonly Int32[] triangles;
        private readonly Vector3[] normals;
        private readonly Vector2[] uvs;
        private readonly Vector4[] tangents;
        private readonly BoneWeight[] boneWeights;

        private VertexData[] globalVerticies;
        private LinkData[] globalLinks;
        private TriangleData[] globalTriangles;

        private Int32 linkCounter;

        private readonly List<VertVertLink> linkList = new List<VertVertLink>();


        private NativeArray<VertexData> nativeVerts;
        private NativeArray<LinkData> nativeLinks;
        private NativeArray<TriangleData> nativeTris;

        //private readonly Dictionary<Int32, Int32> vertexToBone = new Dictionary<Int32, Int32>();
        private readonly Int32[] vertexToBone;
        private readonly List<Int32>[] boneToVertex = new List<Int32>[50];
        private readonly List<Int32>[] boneToTriangles = new List<Int32>[50];
        private readonly List<Int32>[] boneToLinks = new List<Int32>[50];
        private readonly List<Int32>[] vertexTriangleSets;
        private readonly List<Int32>[] vertexLinkSets;
        private readonly List<BoneMapper> boneMappers = new List<BoneMapper>();
        private readonly List<NativeArray<Int32>> boneVerts = new List<NativeArray<Int32>>();
        private readonly List<NativeArray<Int32>> boneTris = new List<NativeArray<Int32>>();
        private readonly List<NativeArray<Int32>> boneLinks = new List<NativeArray<Int32>>();

        private readonly Queue<JobHandle> activeJobs = new Queue<JobHandle>();

        //private readonly Dictionary<Int32, List<Int32>> boneToVertex = new Dictionary<Int32, List<Int32>>();

        //private readonly Dictionary<Int32, List<Int32>> boneToTriangles = new Dictionary<Int32, List<Int32>>();
        //private readonly Dictionary<Int32, List<Int32>> boneToLinks = new Dictionary<Int32, List<Int32>>();
        //private readonly Dictionary<Int32, HashSet<Int32>> vertexTriangleSets = new Dictionary<Int32, HashSet<Int32>>();
        //private readonly Dictionary<Int32, HashSet<Int32>> vertexLinkSets = new Dictionary<Int32, HashSet<Int32>>();
        private readonly Dictionary<VertVertLink, Int32> linkMapping = new Dictionary<VertVertLink, Int32>();
        private readonly Dictionary<VertVertLink, VertVertLink> linkTriangles = new Dictionary<VertVertLink, VertVertLink>();

        internal UVMapperInst( Boolean logging, Vector3[] verticies, Int32[] triangles, Vector3[] normals, Vector2[] uvs, Vector4[] tangents, BoneWeight[] boneWeights )
        {
            this.verticies = verticies;
            this.triangles = triangles;
            this.normals = normals;
            this.uvs = uvs;
            this.tangents = tangents;
            this.boneWeights = boneWeights;
            this.vertexToBone = new Int32[this.verticies.Length];
            this.vertexTriangleSets = new List<Int32>[this.verticies.Length];
            this.vertexLinkSets = new List<Int32>[this.verticies.Length];
        }

        internal void PreCache()
        {
            
            for( Int32 i = 0; i < this.verticies.Length; ++i )
            {
                var bone = GetBoneIndexFromWeights( this.boneWeights[i] );
                this.vertexToBone[i] = bone;

                var virtList = this.boneToVertex[bone];
                if( virtList == null )
                {
                    virtList = new List<Int32>();
                    this.boneToVertex[bone] = virtList;
                }
                virtList.Add( i );
            }
        }

        internal void GenerateTriangles()
        {
            var triCount = this.triangles.Length / 3;
            this.globalTriangles = new TriangleData[triCount];
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

                this.globalTriangles[triInd] = new TriangleData( triInd, ind1, ind2, ind3, link1Index, link2Index, link3Index );

                var boneTriList = this.boneToTriangles[boneInd];
                if( boneTriList == null )
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
            this.globalLinks = new LinkData[linkCount];

            for( Int32 i = 0; i < linkCount; ++i )
            {
                var vertexKvp = this.linkList[i];
                this.AddVertexLinkAssociation( i, vertexKvp.Key, vertexKvp.Value );
                var boneInd = this.GetBoneIndex( vertexKvp.Key, vertexKvp.Value );
                var triKvp = this.linkTriangles[vertexKvp];

                this.globalLinks[i] = new LinkData( i, vertexKvp.Key, vertexKvp.Value, triKvp.Key, triKvp.Value );

                var boneLinkList = this.boneToLinks[boneInd];
                if( boneLinkList == null )
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
            this.globalVerticies = new VertexData[vertCount];
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

                this.globalVerticies[i] = new VertexData( i, this.verticies[i], this.normals[i], this.tangents[i], this.uvs[i], linkArray, triArray );
            }
        }

        internal void Seperate()
        {
            this.nativeVerts = new NativeArray<VertexData>( this.globalVerticies, Allocator.TempJob );
            this.nativeLinks = new NativeArray<LinkData>( this.globalLinks, Allocator.TempJob );
            this.nativeTris = new NativeArray<TriangleData>( this.globalTriangles, Allocator.TempJob );

            for( Int32 i = 0; i < this.nativeVerts.Length; ++i )
            {
                var vert = this.nativeVerts[i];
                vert.AssignBuffers( this.nativeVerts, this.nativeLinks, this.nativeTris );
                this.nativeVerts[i] = vert;
            }

            for( Int32 i = 0; i < this.nativeLinks.Length; ++i )
            {
                var link = this.nativeLinks[i];
                link.AssignBuffers( this.nativeVerts, this.nativeLinks, this.nativeTris );
                this.nativeLinks[i] = link;
            }

            for( Int32 i = 0; i < this.nativeTris.Length; ++i )
            {
                var tri = this.nativeTris[i];
                tri.AssignBuffers( this.nativeVerts, this.nativeLinks, this.nativeTris );
                this.nativeTris[i] = tri;
            }


            for( Int32 i = 0; i < this.boneToVertex.Length; ++i )
            {
                var v = this.boneToVertex[i];
                if( v == null ) continue;
                var verts = new NativeArray<Int32>( v.ToArray(), Allocator.TempJob );
                var tris = new NativeArray<Int32>( this.boneToTriangles[i].ToArray(), Allocator.TempJob );
                var links = new NativeArray<Int32>( this.boneToLinks[i].ToArray(), Allocator.TempJob );
                this.boneTris.Add( tris );
                this.boneVerts.Add( verts );
                this.boneLinks.Add( links );

                this.boneMappers.Add( new BoneMapper( i, this.nativeVerts, this.nativeLinks, this.nativeTris, verts, links, tris ) );
            }
        }

        internal void Seed()
        {
            for( Int32 i = 0; i < this.boneMappers.Count; ++i )
            {
                this.activeJobs.Enqueue( this.boneMappers[i].GetSeedJob() );
            }

            this.WaitForJobs();
        }

        internal void RunMain( Boolean forceComplete = false )
        {
            for( Int32 i = 0; i < this.boneMappers.Count; ++i )
            {
                this.activeJobs.Enqueue( this.boneMappers[i].Schedule() );
            }

            if( forceComplete )
            {
                this.WaitForJobs();
            }
        }

        internal Vector2[] GetUVs()
        {
            this.WaitForJobs();
            //var finalizer = new FinalizeJob( this.nativeVerts ).Schedule( this.nativeVerts.Length, 1 );
            //finalizer.Complete();


            var uvs = new Vector2[this.nativeVerts.Length];
            for( Int32 i = 0; i < this.nativeVerts.Length; ++i )
            {
                uvs[i] = this.nativeVerts[i].uv;
            }


            return uvs;
        }

        
        private void WaitForJobs()
        {
            while( this.activeJobs.Count > 0 )
            {
                this.activeJobs.Dequeue().Complete();
            }
        }

        private Int32 AddLink( Int32 triangleIndex, VertVertLink linkKvp )
        {
            Int32 index;
            if( !this.linkMapping.TryGetValue( linkKvp, out index ) )
            {
                index = this.linkCounter;
                this.linkCounter++;
                this.linkMapping[linkKvp] = index;
                this.linkList.Add( linkKvp );
            }

            VertVertLink triKvp;
            if( !this.linkTriangles.TryGetValue( linkKvp, out triKvp ) )
            {
                triKvp = new VertVertLink( triangleIndex, -1 );
                this.linkTriangles[linkKvp] = triKvp;
            } else
            {
                if( triKvp.Value != -1 )
                {
                    Main.LogE( "Tried to add third triangle to a link" );
                    return index;
                }

                triKvp = new VertVertLink( triKvp.Key, triangleIndex );
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
            Int32 bone = this.vertexToBone[vert];
            //if( !this.vertexToBone.TryGetValue( vert, out bone ) )
            //{
            //    bone = GetBoneIndexFromWeights( this.boneWeights[vert] );
            //    this.vertexToBone[vert] = bone;

            //    List<Int32> vertList;
            //    if( !this.boneToVertex.TryGetValue( bone, out vertList ) )
            //    {
            //        vertList = new List<Int32>();
            //        this.boneToVertex[bone] = vertList;
            //    }
            //    this.boneToVertex[bone].Add( vert );
            //}
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
            var triSet = this.vertexTriangleSets[vertIndex];
            if( triSet == null )
            {
                triSet = new List<Int32>();
                this.vertexTriangleSets[vertIndex] = triSet;
            }
            triSet.Add( triIndex );
        }

        private void AddVertexLinkAssociationSingle( Int32 linkIndex, Int32 vertIndex )
        {
            var linkSet = this.vertexLinkSets[vertIndex];
            if( linkSet == null )
            {
                linkSet = new List<Int32>();
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

        private static VertVertLink MakeLinkKvp( Int32 ind1, Int32 ind2 )
        {
            return new VertVertLink( Mathf.Min( ind1, ind2 ), Mathf.Max( ind1, ind2 ) );
        }

        internal struct VertVertLink
        {
            internal VertVertLink( Int32 from, Int32 to )
            {
                this.Key = from;
                this.Value = to;
            }

            public override Int32 GetHashCode()
            {
                return (this.Key, this.Value).GetHashCode();
            }

            internal Int32 Key;
            internal Int32 Value;
        }
    }
}
