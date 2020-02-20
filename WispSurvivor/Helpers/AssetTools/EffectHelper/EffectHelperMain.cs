using R2API;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RogueWispPlugin.Helpers
{
    internal static partial class EffectHelper
    {
        private static void BasicSetup( ParticleSystem ps )
        {
            ParticleSystem.EmissionModule ps1Emission = ps.emission;
            ps1Emission.enabled = false;

            ParticleSystem.ShapeModule ps1Shape = ps.shape;
            ps1Shape.enabled = false;

            ParticleSystem.VelocityOverLifetimeModule ps1VOL = ps.velocityOverLifetime;
            ps1VOL.enabled = false;

            ParticleSystem.LimitVelocityOverLifetimeModule ps1LimVOL = ps.limitVelocityOverLifetime;
            ps1LimVOL.enabled = false;

            ParticleSystem.InheritVelocityModule ps1InhVel = ps.inheritVelocity;
            ps1InhVel.enabled = false;

            ParticleSystem.ForceOverLifetimeModule ps1FOL = ps.forceOverLifetime;
            ps1FOL.enabled = false;

            ParticleSystem.ColorOverLifetimeModule ps1COL = ps.colorOverLifetime;
            ps1COL.enabled = false;

            ParticleSystem.ColorBySpeedModule ps1CBS = ps.colorBySpeed;
            ps1CBS.enabled = false;

            ParticleSystem.SizeOverLifetimeModule ps1SOL = ps.sizeOverLifetime;
            ps1SOL.enabled = false;

            ParticleSystem.SizeBySpeedModule ps1SBS = ps.sizeBySpeed;
            ps1SBS.enabled = false;

            ParticleSystem.RotationOverLifetimeModule ps1ROL = ps.rotationOverLifetime;
            ps1ROL.enabled = false;

            ParticleSystem.RotationBySpeedModule ps1RBS = ps.rotationBySpeed;
            ps1RBS.enabled = false;

            ParticleSystem.ExternalForcesModule ps1ExtFor = ps.externalForces;
            ps1ExtFor.enabled = false;

            ParticleSystem.NoiseModule ps1Noise = ps.noise;
            ps1Noise.enabled = false;

            ParticleSystem.CollisionModule ps1Collis = ps.collision;
            ps1Collis.enabled = false;

            ParticleSystem.TriggerModule ps1Trig = ps.trigger;
            ps1Trig.enabled = false;

            ParticleSystem.SubEmittersModule ps1SubEmit = ps.subEmitters;
            ps1SubEmit.enabled = false;

            ParticleSystem.TextureSheetAnimationModule ps1TexAnim = ps.textureSheetAnimation;
            ps1TexAnim.enabled = false;

            ParticleSystem.LightsModule ps1Light = ps.lights;
            ps1Light.enabled = false;

            ParticleSystem.TrailModule ps1Trails = ps.trails;
            ps1Trails.enabled = false;

            ParticleSystem.CustomDataModule ps1Cust = ps.customData;
            ps1Cust.enabled = false;
        }
    }
}


/*
Rules of thumb:

Creation Helpers:

    Name starts with Add


Edit Helpers:
    
    Name starts with Edit

    EditSimulationSpace( Boolean isLocal, 


Not in Helpers:













































































































































































































































































































































































































































    */