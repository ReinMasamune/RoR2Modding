using BepInEx;
using R2API.Utils;
using RoR2;
using RoR2.Networking;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using RogueWispPlugin.Helpers;
using RogueWispPlugin.Modules;
//using static RogueWispPlugin.Helpers.APIInterface;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        internal static GameObject[][] genericImpactEffects = new GameObject[8][];
        internal static GameObject[] primaryOrbEffects = new GameObject[8];
        internal static GameObject[] primaryExplosionEffects = new GameObject[8];
        internal static GameObject[] secondaryExplosions = new GameObject[8];
        internal static GameObject[] utilityFlames = new GameObject[8];
        internal static GameObject[] utilityBurns = new GameObject[8];
        internal static GameObject[] utilityLeech = new GameObject[8];
        internal static GameObject[] utilityAim = new GameObject[8];
        internal static GameObject[] utilityIndicator = new GameObject[8];
        internal static GameObject[] specialCharge = new GameObject[8];
        internal static GameObject[] specialExplosion = new GameObject[8];
        internal static GameObject[] specialBeam = new GameObject[8];

        partial void RW_GenericImpactEffects();
        partial void RW_PrimaryOrbEffects();
        partial void RW_PrimaryExplosionEffects();
        partial void RW_SecondaryExplosionEffects();
        partial void RW_BlazeOrbEffects();
        partial void RW_IgnitionOrbEffects();
        partial void RW_LeechOrbEffects();
        partial void RW_UtilityAimEffects();
        partial void RW_UtilityIndicatorEffects();
        partial void RW_SpecialExplosionEffects();
        partial void RW_SpecialChargeEffects();
        partial void RW_SpecialBeamEffects();

        partial void RW_Effect()
        {
            this.RW_GenericImpactEffects();
            this.RW_PrimaryOrbEffects();
            this.RW_PrimaryExplosionEffects();
            this.RW_SecondaryExplosionEffects();
            this.RW_BlazeOrbEffects();
            this.RW_IgnitionOrbEffects();
            this.RW_LeechOrbEffects();
            this.RW_UtilityAimEffects();
            this.RW_UtilityIndicatorEffects();
            this.RW_SpecialExplosionEffects();
            this.RW_SpecialChargeEffects();
            this.RW_SpecialBeamEffects();
            this.FirstFrame += this.RW_RegisterEffects;
        }

        private void RW_RegisterEffects()
        {
            foreach( GameObject[] gs in genericImpactEffects )
            {
                foreach( GameObject g in gs )
                {
                    R2API.EffectAPI.AddEffect( g );
                }
            }
            foreach( GameObject g in primaryOrbEffects )
            {
                R2API.EffectAPI.AddEffect( g );
            }
            foreach( GameObject g in primaryExplosionEffects )
            {
                R2API.EffectAPI.AddEffect( g );
            }
            foreach( GameObject g in secondaryExplosions )
            {
                R2API.EffectAPI.AddEffect( g );
            }
            foreach( GameObject g in utilityFlames )
            {
                R2API.EffectAPI.AddEffect( g );
            }
            foreach( GameObject g in utilityBurns )
            {
                R2API.EffectAPI.AddEffect( g );
            }
            foreach( GameObject g in utilityLeech )
            {
                R2API.EffectAPI.AddEffect( g );
            }
            foreach( GameObject g in specialCharge )
            {
                R2API.EffectAPI.AddEffect( g );
            }
            foreach( GameObject g in specialExplosion )
            {
                R2API.EffectAPI.AddEffect( g );
            }

            typeof( EffectCatalog ).InvokeMethod( "CCEffectsReload", new ConCommandArgs() );


            for( Int32 i = 0; i < 8; i++ )
            {
                this.restoreIndex[i] = (UInt32)EffectCatalog.FindEffectIndexFromPrefab( utilityLeech[i] );
            }
        }

        private static void Strip( GameObject g )
        {
            foreach( Component c in g.GetComponents<Component>() )
            {
                if( !c ) continue;
                if( c.GetType() == typeof( Transform ) ) continue;

                MonoBehaviour.DestroyImmediate( c );
            }
        }

        private static void BasicSetup( ParticleSystem ps1 )
        {
            ParticleSystem.EmissionModule ps1Emission = ps1.emission;
            ps1Emission.enabled = false;

            ParticleSystem.ShapeModule ps1Shape = ps1.shape;
            ps1Shape.enabled = false;

            ParticleSystem.VelocityOverLifetimeModule ps1VOL = ps1.velocityOverLifetime;
            ps1VOL.enabled = false;

            ParticleSystem.LimitVelocityOverLifetimeModule ps1LimVOL = ps1.limitVelocityOverLifetime;
            ps1LimVOL.enabled = false;

            ParticleSystem.InheritVelocityModule ps1InhVel = ps1.inheritVelocity;
            ps1InhVel.enabled = false;

            ParticleSystem.ForceOverLifetimeModule ps1FOL = ps1.forceOverLifetime;
            ps1FOL.enabled = false;

            ParticleSystem.ColorOverLifetimeModule ps1COL = ps1.colorOverLifetime;
            ps1COL.enabled = false;

            ParticleSystem.ColorBySpeedModule ps1CBS = ps1.colorBySpeed;
            ps1CBS.enabled = false;

            ParticleSystem.SizeOverLifetimeModule ps1SOL = ps1.sizeOverLifetime;
            ps1SOL.enabled = false;

            ParticleSystem.SizeBySpeedModule ps1SBS = ps1.sizeBySpeed;
            ps1SBS.enabled = false;

            ParticleSystem.RotationOverLifetimeModule ps1ROL = ps1.rotationOverLifetime;
            ps1ROL.enabled = false;

            ParticleSystem.RotationBySpeedModule ps1RBS = ps1.rotationBySpeed;
            ps1RBS.enabled = false;

            ParticleSystem.ExternalForcesModule ps1ExtFor = ps1.externalForces;
            ps1ExtFor.enabled = false;

            ParticleSystem.NoiseModule ps1Noise = ps1.noise;
            ps1Noise.enabled = false;

            ParticleSystem.CollisionModule ps1Collis = ps1.collision;
            ps1Collis.enabled = false;

            ParticleSystem.TriggerModule ps1Trig = ps1.trigger;
            ps1Trig.enabled = false;

            ParticleSystem.SubEmittersModule ps1SubEmit = ps1.subEmitters;
            ps1SubEmit.enabled = false;

            ParticleSystem.TextureSheetAnimationModule ps1TexAnim = ps1.textureSheetAnimation;
            ps1TexAnim.enabled = false;

            ParticleSystem.LightsModule ps1Light = ps1.lights;
            ps1Light.enabled = false;

            ParticleSystem.TrailModule ps1Trails = ps1.trails;
            ps1Trails.enabled = false;

            ParticleSystem.CustomDataModule ps1Cust = ps1.customData;
            ps1Cust.enabled = false;
        }

    }

}
