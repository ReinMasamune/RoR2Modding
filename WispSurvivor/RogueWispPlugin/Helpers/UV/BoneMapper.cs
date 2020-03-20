using RoR2;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    public struct BoneMapper : IJob
    {
        public static readonly Int32 maxIterations = 0;
        public readonly Int32 boneIndex;

        public BoneMapper( Int32 boneIndex, NativeArray<VertexData> verts, NativeArray<LinkData> links, NativeArray<TriangleData> tris, NativeArray<Int32> vertInds, NativeArray<Int32> linkInds, NativeArray<Int32> triInds )
        {
            this.boneIndex = boneIndex;
            this.verticies = verts;
            this.links = links;
            this.triangles = tris;
            this.vertexInds = vertInds;
            this.linkInds = linkInds;
            this.triangleInds = triInds;
            this.iterationCounter = 0;
            this.linkLoops = 10;
            this.triLoops = 10;
            this.uvDistPerUnit = 0.75f;
            this.currentAcceptableErrorCoef = 0.00001f;
            this.errorMult = 1f;
            this.currentStepLength = 0.5f;
            this.stepMult = 1f;  
        }

        public void Execute()
        {
            while( this.iterationCounter < maxIterations )
            {
                for( Int32 i = 0; i < this.linkLoops; ++i )
                {
                    this.IterateLinks();
                }
                for( Int32 i = 0; i < this.triLoops; ++i )
                {
                    this.IterateTris();
                }
                this.iterationCounter++;
                this.currentStepLength *= this.stepMult;
                this.currentAcceptableErrorCoef *= this.errorMult;
            }
        }

        public JobHandle GetSeedJob()
        {
            var total = Vector3.zero;
            var min = new Vector3( Single.MaxValue, Single.MaxValue, Single.MaxValue );
            var max = new Vector3( Single.MinValue, Single.MinValue, Single.MinValue );
            for( Int32 i = 0; i < this.vertexInds.Length; ++i )
            {
                var pos = this.verticies[this.vertexInds[i]].position;
                min.x = Mathf.Min( min.x, pos.x );
                min.y = Mathf.Min( min.y, pos.y );
                min.z = Mathf.Min( min.z, pos.z );
                max.x = Mathf.Max( max.x, pos.x );
                max.y = Mathf.Max( max.y, pos.y );
                max.z = Mathf.Max( max.z, pos.z );
                total += this.verticies[this.vertexInds[i]].position;
            }
            total /= this.vertexInds.Length;
            return new SeedJob(this.verticies, this.vertexInds, total, min, max, this.uvDistPerUnit ).Schedule(this.vertexInds.Length, 1);
        }

        
        private NativeArray<VertexData> verticies;
        private NativeArray<LinkData> links;
        private NativeArray<TriangleData> triangles;

        private NativeArray<Int32> vertexInds;
        private NativeArray<Int32> linkInds;
        private NativeArray<Int32> triangleInds;

        private Int32 iterationCounter;
        private Int32 linkLoops;
        private Int32 triLoops;
        private Single uvDistPerUnit;
        private Single currentAcceptableErrorCoef;
        private Single errorMult;
        private Single currentStepLength;
        private Single stepMult;

        private void IterateLinks()
        {
            Single totalError = 0f;
            Int32 passCounter = 0;
            Int32 failedNudgeCounter = 0;
            for( Int32 i = 0; i < this.linkInds.Length; ++i )
            {
                var link = this.links[this.linkInds[i]].reff;
                var vert1 = link.vertex1;
                var vert2 = link.vertex2;
                var pos1 = vert1.position;
                var pos2 = vert2.position;
                var diff = pos1 - pos2;
                var vDist = Mathf.Abs( diff.z );
                var uDist = ((Vector2)diff).magnitude;
                var idealUV = new Vector2( Mathf.Repeat( uDist * this.uvDistPerUnit, 1f ), Mathf.Repeat( vDist * this.uvDistPerUnit, 1f ) );
                var uvDist = new UVDistance( vert1.uv, vert2.uv );
                var error = idealUV - uvDist.difference;

                totalError += error.magnitude;
                if( !SingleWithinMargin( error.magnitude, 0f, this.currentAcceptableErrorCoef ) )
                {
                    var nudgeDirection = error;
                    var uv1Nudge = nudgeDirection * this.currentStepLength;
                    var uv2Nudge = -nudgeDirection * this.currentStepLength;

                    var tempUv1 = vert1.uv;
                    tempUv1 += uv1Nudge;
                    tempUv1.x = Mathf.Repeat( tempUv1.x, 1f );
                    tempUv1.y = Mathf.Repeat( tempUv1.y, 1f );

                    var tempUv2 = vert2.uv;
                    tempUv2 += uv2Nudge;
                    tempUv2.x = Mathf.Repeat( tempUv2.x, 1f );
                    tempUv2.y = Mathf.Repeat( tempUv2.y, 1f );

                    var verifyDiff = new UVDistance( tempUv1, tempUv2 );
                    var verifyError = idealUV - verifyDiff.difference;

                    if( verifyError.sqrMagnitude > error.sqrMagnitude )
                    {
                        failedNudgeCounter++;
                        continue;
                    }
                    if( verifyError.sqrMagnitude == error.sqrMagnitude )
                    {
                        //Main.LogE( "No change" );
                        continue;
                    }



                    vert1.uv = tempUv1;
                    vert2.uv = tempUv2;


                } else passCounter++;
            }
            
            if( this.boneIndex == 0 )
            {
                Main.LogW( String.Format( "{0}% of links passed. {1} total error, {2}% failed nudges. Bone: {3} Iteration: {4}",
                    100f * (Single)passCounter / (Single)this.linkInds.Length,
                    totalError,
                    100f * (Single)failedNudgeCounter / (Single)this.linkInds.Length,
                    this.boneIndex,
                    this.iterationCounter ) );
            }
        }

        private void IterateTris()
        {
        }


        private static Vector2 LoopUVs( Vector2 input )
        {
            input.x = Mathf.Repeat( input.x, 1f );
            if( input.x == 1f ) input.x = 0f;
            input.y = Mathf.Repeat( input.y, 1f );
            if( input.y == 1f ) input.y = 0f;
            return input;
        }

        public static Single LoopDistance( Single input )
        {
            return Mathf.Repeat( input, Mathf.Sqrt( 2f ) );
        }

        private static Boolean SingleWithinMargin( Single input, Single target, Single margin )
        {
            if( target == 0f )
            {
                return Mathf.Abs( input ) <= margin;
            } else
            {
                return SingleWithinMargin( (input / target) - 1f, 0f, margin );
            }
        }
    }
}
