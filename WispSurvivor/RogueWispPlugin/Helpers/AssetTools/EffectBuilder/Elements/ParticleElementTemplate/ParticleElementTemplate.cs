using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Rein.RogueWispPlugin.Helpers
{
    internal abstract class ParticleElementTemplate
    {
        internal ParticleElement element;
        internal ParticleSystem partSys;
        internal ParticleSystemRenderer partRend;

        internal ParticleSystem.MainModule main;
        internal ParticleSystem.CollisionModule collision;
        internal ParticleSystem.ColorBySpeedModule colorBySpeed;
        internal ParticleSystem.ColorOverLifetimeModule colorOverLifetime;
        internal ParticleSystem.CustomDataModule customData;
        internal ParticleSystem.EmissionModule emission;
        internal ParticleSystem.ExternalForcesModule externalForces;
        internal ParticleSystem.ForceOverLifetimeModule forceOverLifetime;
        internal ParticleSystem.InheritVelocityModule inheritVelocity;
        internal ParticleSystem.LightsModule lights;
        internal ParticleSystem.LimitVelocityOverLifetimeModule limitVelocityOverLifetime;
        internal ParticleSystem.NoiseModule noise;
        internal ParticleSystem.RotationBySpeedModule rotationBySpeed;
        internal ParticleSystem.RotationOverLifetimeModule rotationOverLifetime;
        internal ParticleSystem.ShapeModule shape;
        internal ParticleSystem.SizeBySpeedModule sizeBySpeed;
        internal ParticleSystem.SizeOverLifetimeModule sizeOverLifetime;
        internal ParticleSystem.SubEmittersModule subEmitters;
        internal ParticleSystem.TextureSheetAnimationModule textureSheetAnimation;
        internal ParticleSystem.TrailModule trails;
        internal ParticleSystem.TriggerModule trigger;
        internal ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime;

        internal ParticleElementTemplate( ParticleElement element )
        {
            this.element = element;
            this.partSys = element.partSys;
            this.partRend = element.partRend;


            this.main = this.partSys.main;


            this.collision = this.partSys.collision;
            this.collision.enabled = false;

            this.colorBySpeed = this.partSys.colorBySpeed;
            this.colorBySpeed.enabled = false;

            this.colorOverLifetime = this.partSys.colorOverLifetime;
            this.colorOverLifetime.enabled = false;

            this.customData = this.partSys.customData;
            this.customData.enabled = false;

            this.emission = this.partSys.emission;
            this.emission.enabled = false;

            this.externalForces = this.partSys.externalForces;
            this.externalForces.enabled = false;

            this.forceOverLifetime = this.partSys.forceOverLifetime;
            this.forceOverLifetime.enabled = false;

            this.inheritVelocity = this.partSys.inheritVelocity;
            this.inheritVelocity.enabled = false;

            this.lights = this.partSys.lights;
            this.lights.enabled = false;

            this.limitVelocityOverLifetime = this.partSys.limitVelocityOverLifetime;
            this.limitVelocityOverLifetime.enabled = false;

            this.noise = this.partSys.noise;
            this.noise.enabled = false;

            this.rotationBySpeed = this.partSys.rotationBySpeed;
            this.rotationBySpeed.enabled = false;

            this.rotationOverLifetime = this.partSys.rotationOverLifetime;
            this.rotationOverLifetime.enabled = false;

            this.shape = this.partSys.shape;
            this.shape.enabled = false;

            this.sizeBySpeed = this.partSys.sizeBySpeed;
            this.sizeBySpeed.enabled = false;

            this.sizeOverLifetime = this.partSys.sizeOverLifetime;
            this.sizeOverLifetime.enabled = false;

            this.subEmitters = this.partSys.subEmitters;
            this.subEmitters.enabled = false;

            this.textureSheetAnimation = this.partSys.textureSheetAnimation;
            this.textureSheetAnimation.enabled = false;

            this.trails = this.partSys.trails;
            this.trails.enabled = false;

            this.trigger = this.partSys.trigger;
            this.trigger.enabled = false;

            this.velocityOverLifetime = this.partSys.velocityOverLifetime;
            this.velocityOverLifetime.enabled = false;
        }
    }
}
