using BepInEx;
using RoR2;
using System;
using System.Reflection;
using UnityEngine;

namespace FancyParticles
{
    [BepInDependency( R2API.R2API.PluginGUID, BepInDependency.DependencyFlags.HardDependency )]
    [BepInPlugin( "com.ReinThings.FancyParticles", "ReinFancyParticles", "1.0.0" )]
    public class Main : BaseUnityPlugin
    {
        AssetBundle bundle;

        Material pointCloudMaterial;
        Material instanceMaterial;

        ComputeShader particleCompute;

        private void Awake()
        {
            //On.RoR2.CharacterModel.Start += this.CharacterModel_Start;
            Assembly execAssembly = Assembly.GetExecutingAssembly();
            System.IO.Stream stream = execAssembly.GetManifestResourceStream( "FancyParticles.Particles.Bundle.particles" );
            this.bundle = AssetBundle.LoadFromStream( stream );
            this.pointCloudMaterial = bundle.LoadAsset<Material>( "Assets/Materials/Dust_Pointcloud.mat" );
            this.instanceMaterial = this.bundle.LoadAsset<Material>( "Assets/Materials/Dust_Instancing.mat" );
            this.particleCompute = this.bundle.LoadAsset<ComputeShader>( "Assets/Shaders/DustComputeParticles.compute" );

            var body = Resources.Load<GameObject>("Prefabs/CharacterBodies/CommandoBody").GetComponent<ModelLocator>().modelTransform.gameObject;
            var partObj = GameObject.CreatePrimitive( PrimitiveType.Sphere );
            DontDestroyOnLoad( partObj );
            DontDestroyOnLoad( body );
            Destroy( partObj.GetComponent<Collider>() );

            partObj.transform.parent = body.transform;
            partObj.transform.localPosition = Vector3.zero;
            var gps = partObj.AddComponent<Dust.DustParticleSystem>();

            var gpr = partObj.AddComponent<Dust.DustPointRenderer>();
            gpr.m_material = this.pointCloudMaterial;

            gps.Compute = this.particleCompute;

            gps.Mass = new Vector2( 0.5f, 1.0f );
            gps.Momentum = new Vector2( 0.9f, 0.99f );
            gps.Lifespan = new Vector2( 0.1f, 1f );
            gps.PreWarmFrames = 10;
            gps.InheritVelocity = 0.5f;
            gps.EmitterVelocity = 0;
            gps.GravityModifier = -0.25f;

            gps.Shape = 0;
            gps.EmissionMeshRenderer = partObj.GetComponent<MeshRenderer>();
            gps.Emission = 2400000;
            gps.InitialSpeed = 2f;
            gps.Jitter = 0f;
            gps.RandomizeDirection = 0.1f;
            gps.AlignToInitialDirection = false;

            gps.SizeOverLife = new Dust.CurveRamp
            {
                Enable = false,
                Curve = new AnimationCurve(),
                Format = TextureFormat.ARGB32,
            };

            gps.AlignToDirection = false;
            gps.RotationOverLifetime = Vector3.zero;

            gps.StartColor = new Color( 1f, 1f, 1f, 1f );
            gps.ColorOverLife = new Dust.ColorRamp
            {
                Enable = false,
                Format = TextureFormat.ARGB32,
                Gradient = new Gradient()
            };
            gps.ColorOverVelocity = new Dust.ColorRampRange
            {
                Enable = false,
                Format = TextureFormat.ARGB32,
                Gradient = new Gradient(),
                Range = 0f
            };
            gps.RandomizeColor = 0.1f;
            gps.UseMeshEmitterColor = false;

            gps.NoiseToggle = false;
        }

        private void CharacterModel_Start( On.RoR2.CharacterModel.orig_Start orig, CharacterModel self )
        {
            orig( self );
            if( self && self.gameObject )
            {
                var body = self.gameObject;
                var partObj = GameObject.CreatePrimitive( PrimitiveType.Sphere );
                Destroy( partObj.GetComponent<Collider>() );

                partObj.transform.parent = body.transform;
                partObj.transform.localPosition = Vector3.zero;
                var gps = partObj.AddComponent<Dust.DustParticleSystem>();

                var gpr = partObj.AddComponent<Dust.DustPointRenderer>();
                gpr.m_material = this.pointCloudMaterial;



                gps.Compute = this.particleCompute;

                gps.Mass = new Vector2( 0.5f, 1.0f );
                gps.Momentum = new Vector2( 0.9f, 0.99f );
                gps.Lifespan = new Vector2( 0.1f, 1f );
                gps.PreWarmFrames = 100;
                gps.InheritVelocity = 0.5f;
                gps.EmitterVelocity = 0;
                gps.GravityModifier = -0.25f;

                gps.Shape = 0;
                gps.EmissionMeshRenderer = partObj.GetComponent<MeshRenderer>();
                gps.Emission = 240000;
                gps.InitialSpeed = 2f;
                gps.Jitter = 0f;
                gps.RandomizeDirection = 0.1f;
                gps.AlignToInitialDirection = false;

                gps.SizeOverLife = new Dust.CurveRamp
                {
                    Enable = false,
                    Curve = new AnimationCurve(),
                    Format = TextureFormat.ARGB32,
                };

                gps.AlignToDirection = false;
                gps.RotationOverLifetime = Vector3.zero;

                gps.StartColor = new Color( 1f, 1f, 1f, 1f );
                gps.ColorOverLife = new Dust.ColorRamp
                {
                    Enable = false,
                    Format = TextureFormat.ARGB32,
                    Gradient = new Gradient()
                };
                gps.ColorOverVelocity = new Dust.ColorRampRange
                {
                    Enable = false,
                    Format = TextureFormat.ARGB32,
                    Gradient = new Gradient(),
                    Range = 0f
                };
                gps.RandomizeColor = 0.1f;
                gps.UseMeshEmitterColor = false;

                gps.NoiseToggle = false;
            }
        }
    }
}
