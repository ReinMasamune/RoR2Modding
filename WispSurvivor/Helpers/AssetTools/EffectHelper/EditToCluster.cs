using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal static partial class EffectHelper
    {
        internal static void EditToCluster( ParticleSystem ps, TransformParams shapeInfo, Single clusterRadius, Single clusterScale, Int32 clusterCount )
        {
            var psMain = ps.main;
            psMain.startSizeMultiplier *= clusterScale;
            psMain.maxParticles *= clusterCount;


            var psEmis = ps.emission;
            var bursts = new ParticleSystem.Burst[psEmis.burstCount];
            psEmis.GetBursts( bursts );
            for( Int32 i = 0; i < bursts.Length; ++i )
            {
                var burst = bursts[i];
                var curve = burst.count;
                switch( curve.mode )
                {
                    default:
                        Main.LogE( "Unhanded curve mode: " + curve.mode.ToString() );
                        break;
                    case ParticleSystemCurveMode.Constant:
                        curve.constant *= clusterCount;
                        break;
                    case ParticleSystemCurveMode.Curve:
                        curve.curveMultiplier *= clusterCount;
                        break;
                    case ParticleSystemCurveMode.TwoConstants:
                        curve.constantMin *= clusterCount;
                        curve.constantMax *= clusterCount;
                        break;
                    case ParticleSystemCurveMode.TwoCurves:
                        curve.curveMultiplier *= clusterCount;
                        break;
                }
                burst.count = curve;
                bursts[i] = burst;
            }
            psEmis.SetBursts( bursts );
            psEmis.rateOverDistanceMultiplier *= clusterCount;
            psEmis.rateOverTimeMultiplier *= clusterCount;

            var psShape = ps.shape;
            if( psShape.enabled )
            {
                psShape.radius = clusterRadius;
                
            } else
            {
                psShape.enabled = true;
                psShape.shapeType = ParticleSystemShapeType.Cone;
                psShape.angle = 25f;
                psShape.radiusThickness = 1f;
                psShape.length = 0.01f;
                shapeInfo.Apply( psShape );
            }
        }
    }
}
