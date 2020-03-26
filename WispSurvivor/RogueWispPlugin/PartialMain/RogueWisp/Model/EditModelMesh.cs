#if ROGUEWISP
using RoR2;
using System;
using System.Collections.Generic;
using UnityEngine;
using Rein.RogueWispPlugin.Helpers;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        partial void RW_EditModelMesh() => this.Load += this.RW_DoModelMeshEdits;

        private void RW_DoModelMeshEdits()
        {
            Mesh m = this.RW_body.GetComponent<ModelLocator>().modelTransform.Find("AncientWispMesh").GetComponent<SkinnedMeshRenderer>().sharedMesh;

            UVMapper.Map( m, true );
            
            //Vector2[] newUvs = new Vector2[m.vertexCount];
            //Vector3[] verts = m.vertices;
            //Vector3[] norms = m.normals;
            //BoneWeight[] boneWeights = m.boneWeights;
            //Single xMin = Single.MaxValue;
            //Single xMax = Single.MinValue;
            //Single yMin = Single.MaxValue;
            //Single yMax = Single.MinValue;
            //Single zMin = Single.MaxValue;
            //Single zMax = Single.MinValue;


            //Vector3Range globalRange = Vector3Range.New();

            //Dictionary<Int32,Vector3Range> boneRanges = new Dictionary<Int32, Vector3Range>();
            //Dictionary<Int32,Int32> boneLookup = new Dictionary<Int32, Int32>();

            //for( Int32 i = 0; i < m.vertexCount; ++i )
            //{
            //    var pos = verts[i];
            //    globalRange.Update( pos );

            //    var bone = boneWeights[i];
            //    var ind = GetDominantBone( bone );
            //    if( ind == -1 )
            //    {
            //        Main.LogW( String.Format( "No bones for vert ind {0}", i ) );
            //    } else if( ind == -100 )
            //    {
            //        Main.LogW( String.Format( "Multiple Bones for vert ind {0}", i ) );
            //    } else
            //    {
            //        //Main.LogW( String.Format( "Dominant bone for vertex {0} is {1}", i, ind ) );
            //        Vector3Range vec = default;
            //        if( !boneRanges.TryGetValue( ind, out vec ) )
            //        {
            //            vec = boneRanges[ind] = Vector3Range.New();
            //        }
            //        vec.Update( pos );
            //        boneRanges[ind] = vec;
            //        boneLookup[i] = ind;
            //    }
            //}

            //Single xTiles = 5f;
            //Single yTiles = 5f;
            //Single zTiles = 5f;

            //for( Int32 i = 0; i < m.vertexCount; i++ )
            //{
            //    Vector3Range rangeVec = globalRange;
            //    if( boneLookup.TryGetValue( i, out var ind ) )
            //    {
            //        rangeVec = boneRanges[ind];
            //    }

            //    var vec = verts[i] - rangeVec.center;
            //    var normal = norms[i];

            //    //vec.x /= rangeVec.xRange;
            //    //vec.y /= rangeVec.yRange;
            //    //vec.z /= rangeVec.zRange;

            //    //vec.x += 1f;
            //    //vec.y += 1f;
            //    //vec.z += 1f;

            //    //vec.x /= 2f;
            //    //vec.y /= 2f;
            //    //vec.z /= 2f;

            //    //vec.x /= xTiles;
            //    //vec.y /= yTiles;
            //    //vec.z /= zTiles;

            //    //vec -= normal / 3f;

            //    var tempU = Mathf.Atan( vec.y / vec.x ) / 2f / Mathf.PI;
            //    //var tempV = Mathf.Atan2( vec.z, vec.x );
            //    var tempV = Mathf.Atan( ((vec.x * vec.x) + (vec.y * vec.y) ) / vec.z ) / 2f / Mathf.PI;

            //    newUvs[i] = new Vector2( tempU, tempV );
            //}
            //m.uv = newUvs;
        }

        private static Int32 GetDominantBone( BoneWeight weight )
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

        internal struct Vector3Range
        {
            internal static Vector3Range New()
            {
                var vec = new Vector3Range();
                vec.xMin = vec.yMin = vec.zMin = Single.MaxValue;
                vec.xMax = vec.yMax = vec.zMax = Single.MinValue;
                vec.xRange = vec.yRange = vec.zRange = 0f;
                vec.center = Vector3.zero;

                return vec;
            }

            internal Single xRange { get; private set; }
            internal Single yRange { get; private set; }
            internal Single zRange { get; private set; }
            internal Vector3 center { get; private set; }

            internal void Update( Vector3 vec )
            {
                var shouldRecalc = false;

                if( vec.x < this.xMin )
                {
                    shouldRecalc = true;
                    this.xMin = vec.x;
                }
                if( vec.x > this.xMax )
                {
                    shouldRecalc = true;
                    this.xMax = vec.x;
                }
                if( vec.y < this.yMin )
                {
                    shouldRecalc = true;
                    this.yMin = vec.y;
                }
                if( vec.y > this.yMax )
                {
                    shouldRecalc = true;
                    this.yMax = vec.y;
                }
                if( vec.z < this.zMin )
                {
                    shouldRecalc = true;
                    this.zMin = vec.z;
                }
                if( vec.z > this.zMax )
                {
                    shouldRecalc = true;
                    this.zMax = vec.z;
                }

                if( shouldRecalc )
                {
                    this.Recalculate();
                }
            }

            private Single xMin;
            private Single xMax;
            private Single yMin;
            private Single yMax;
            private Single zMin;
            private Single zMax;


            private void Recalculate()
            {
                this.xRange = this.xMax - this.xMin;
                this.yRange = this.yMax - this.yMin;
                this.zRange = this.zMax - this.zMin;

                this.center = 0.5f * (new Vector3( xMin, yMin, zMin ) + new Vector3( xMax, yMax, zMax ));
            }
        }
    }

}
#endif