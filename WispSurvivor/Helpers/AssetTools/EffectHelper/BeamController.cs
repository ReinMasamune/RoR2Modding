using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Linq;
using RoR2;

namespace RogueWispPlugin.Helpers
{
    internal class BeamController : MonoBehaviour
    {
        public Transform endTransform;
        public Transform midTransform;
        public Single distRateRatio;

        public ParticleSystem[] particlesForEmit = Array.Empty<ParticleSystem>();
        public ParticleSystem[] particlesForShape = Array.Empty<ParticleSystem>();
        private ParticleSystem.EmissionModule[] emitters;
        private ParticleSystem.ShapeModule[] shapes;
        public LineRenderer beamLine;

        internal LineRenderer AddBeamLine( WispSkinnedEffect skin, MaterialType matType, Single radius, Single rippleStrength = 0f, Int32 rippleCount = 0 )
        {
            this.beamLine = base.gameObject.AddComponent<LineRenderer>();
            if( matType != MaterialType.Constant )
            {
                skin.AddRenderer( this.beamLine, matType );
            }

            if( rippleStrength > 0f && rippleCount >= 0 )
            {
                var rippleMin = radius * (1f - rippleStrength );
                var rippleMax = radius * (1f + rippleStrength );
                var numSegments = 1 + rippleCount;
                var numPoints = 1 + 2*numSegments;
                var rippleDist = 1f / (numPoints - 1);
                var rippleKeys = new Keyframe[numPoints];

                rippleKeys[0] = new Keyframe( 0f, 1f, 0f, 0f );

                var loopPoints = numPoints - 2;
                var counter = 1;
                var high = false;
                while( counter <= loopPoints )
                {
                    var height = high ? rippleMax : rippleMin;
                    rippleKeys[counter] = new Keyframe( rippleDist * counter++, height, 0f, 0f );
                    high = !high;
                }

                rippleKeys[numPoints - 1] = new Keyframe( 1f, 1f, 0f, 0f );
                this.beamLine.widthCurve = new AnimationCurve( rippleKeys );
            } else
            {
                this.beamLine.endWidth = radius;
                this.beamLine.startWidth = radius;
            }

            this.beamLine.alignment = LineAlignment.View;
            this.beamLine.textureMode = LineTextureMode.Tile;

            this.beamLine.useWorldSpace = true;

            this.beamLine.numCapVertices = 64;
            this.beamLine.numCornerVertices = 64;

            return this.beamLine;
        }

        internal ParticleSystem AddParticles( ParticleSystem ps, Boolean forEmit, Boolean forShape )
        {
            if( forEmit )
            {
                var ind = this.particlesForEmit.Length;
                Array.Resize<ParticleSystem>( ref this.particlesForEmit, ind + 1 );
                this.particlesForEmit[ind] = ps;
            }
            if( forShape )
            {
                var ind = this.particlesForShape.Length;
                Array.Resize<ParticleSystem>( ref this.particlesForShape, ind + 1 );
                this.particlesForShape[ind] = ps;
            }

            return ps;
        }

        private void Awake()
        {
            var l = this.particlesForEmit.Length;
            this.emitters = new ParticleSystem.EmissionModule[l];
            for( Int32 i = 0; i < l; ++i )
            {
                this.emitters[i] = this.particlesForEmit[i].emission;
            }

            var l2 = this.particlesForShape.Length;
            this.shapes = new ParticleSystem.ShapeModule[l2];
            for( Int32 i = 0; i < l2; ++i )
            {
                this.shapes[i] = this.particlesForShape[i].shape;
            }
        }

        private void Update()
        {
            var pos1 = base.transform.position;
            var pos2 = this.endTransform.position;
            var mid = (pos1 + pos2)/2f;
            var diff = pos2 - pos1;
            var dir = diff.normalized;
            var angle = Util.QuaternionSafeLookRotation( diff.normalized );
            var dist = diff.magnitude;
            this.midTransform.position = mid;
            this.midTransform.rotation = angle;
            var scale1 = this.midTransform.localScale;
            scale1.z = dist;
            this.midTransform.localScale = scale1;
            var rateMult = dist * this.distRateRatio;
            
            for( Int32 i = 0; i < this.emitters.Length; ++i )
            {
                this.emitters[i].rateOverDistanceMultiplier = rateMult;
                this.emitters[i].rateOverTimeMultiplier = rateMult;
            }

            for( Int32 i = 0; i < this.shapes.Length; ++i )
            {
                this.shapes[i].length = dist;
            }

            if( this.beamLine )
            {
                this.beamLine.positionCount = 2;
                this.beamLine.SetPosition( 0, pos1 );
                this.beamLine.SetPosition( 1, pos2 );
            }
        }
    }
}
