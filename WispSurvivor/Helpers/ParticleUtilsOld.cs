using System.Reflection;
using UnityEngine;

namespace WispSurvivor.Helpers
{
    public static class ParticleUtils
    {
        public static void SetParticleStruct<T>( T str1, T str2 ) where T : struct
        {
            foreach( PropertyInfo p in typeof( T ).GetProperties() )
            {
                if( p.CanWrite && p.CanRead ) p.SetValue( str1, p.GetValue( str2 ) );
            }
        }



        public static GameObject CreateFireBallParticle( GameObject parent, Material mat )
        {
            var info = CreateParticleBase(parent);

            //var ps = info.AddComponent<ParticleSystem>();
            //var psr = info.GetComponent<ParticleSystemRenderer>();

            //var psMain = ps.main;
            //var psEmis = ps.emission;
            //var psShape = ps.shape;
            //var psCOL = ps.colorOverLifetime;
            //var psSOL = ps.sizeOverLifetime;
            //psr.material = mat;
            info.psr.material = mat;


            var psMain = info.ps.main;
            psMain.prewarm = true;
            psMain.startLifetime = 0.5f;
            psMain.startSpeed = 0f;
            psMain.startSize = 1.25f;
            psMain.maxParticles = 100;
            psMain.emitterVelocityMode = ParticleSystemEmitterVelocityMode.Transform;
            psMain.stopAction = ParticleSystemStopAction.Destroy;
            psMain.cullingMode = ParticleSystemCullingMode.AlwaysSimulate;


            var psEmis = info.ps.emission;
            psEmis.enabled = true;
            psEmis.rateOverTime = 100f;
            psEmis.rateOverDistance = 0f;


            var psShape = info.ps.shape;
            psShape.enabled = true;
            psShape.shapeType = ParticleSystemShapeType.Sphere;
            psShape.radius = 1f;


            var psCOL = info.ps.colorOverLifetime;
            psCOL.enabled = true;
            psCOL.color = new ParticleSystem.MinMaxGradient
            {
                mode = ParticleSystemGradientMode.Gradient,
                gradient = new Gradient
                {
                    mode = GradientMode.Blend,
                    alphaKeys = new GradientAlphaKey[2]
                    {
                        new GradientAlphaKey(1f, 0f ),
                        new GradientAlphaKey(0f, 1f )
                    },
                    colorKeys = new GradientColorKey[1]
                    {
                        new GradientColorKey( Color.white, 0f )
                    }
                }
            };


            var psSOL = info.ps.sizeOverLifetime;
            psSOL.enabled = true;
            psSOL.size = new ParticleSystem.MinMaxCurve
            {
                mode = ParticleSystemCurveMode.Curve,
                curve = new AnimationCurve
                {
                    preWrapMode = WrapMode.Clamp,
                    postWrapMode = WrapMode.Clamp,
                    keys = new Keyframe[2]
                    {
                        new Keyframe( 0f, 0.5f),
                        new Keyframe( 1f, 1f)
                    }
                }
            };
            psSOL.sizeMultiplier = 1f;


            return info.obj;
        }








        private struct ParticleInfo
        {
            public GameObject obj;
            public Transform transform;
            public ParticleSystem ps;
            public ParticleSystemRenderer psr;
        }

        private static ParticleInfo CreateParticleBase( GameObject parent )
        {
            GameObject obj = new GameObject("Fireball");
            obj.transform.parent = parent.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.identity;

            //return obj;
            
            return new ParticleInfo
            {
                obj = obj,
                transform = obj.transform,
                ps = obj.AddComponent<ParticleSystem>(),
                psr = obj.GetComponent<ParticleSystemRenderer>()
            };
            
        }
    }
}
