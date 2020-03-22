using System;
using System;
using System.Collections.Generic;
using System.Text;
using BepInEx;
using ReinCore;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace Rein.AlternateArtificer
{
    internal partial class Main
    {
        partial void Projectiles()
        {
            base.awake += this.Main_awake7;
        }

        // TODO: REWRITE
        private void Main_awake7()
        {
            var novaProj = Resources.Load<GameObject>("Prefabs/Projectiles/MageLightningBombProjectile");

            var novaSimp = novaProj.GetComponent<ProjectileSimple>();
            novaSimp.velocity *= 1.25f;

            novaProj.GetComponent<Rigidbody>().useGravity = true;

            var novaImpact = novaProj.GetComponent<ProjectileImpactExplosion>();
            novaImpact.blastDamageCoefficient = 1.0f;
            novaImpact.blastRadius = 15f;

            var novaBeams = novaProj.GetComponent<ProjectileProximityBeamController>();
            novaBeams.attackRange *= 1.25f;
            novaBeams.attackInterval *= 0.9f;
            novaBeams.bounces += 1;


            AltArtiPassive.lightningProjectile = new GameObject[5];
            for( Int32 i = 0; i < 5; i++ )
            {
                this.CreateLightningSword( i );
            }

            GameObject blast = Resources.Load<GameObject>("Prefabs/NetworkedObjects/GenericDelayBlast").ClonePrefab( "IceDelayBlast", true );
            DelayBlast component = blast.GetComponent<DelayBlast>();
            component.crit = false;
            component.procCoefficient = 1.0f;
            component.maxTimer = 1.25f;
            component.falloffModel = BlastAttack.FalloffModel.None;
            component.explosionEffect = CreateIceExplosionEffect();
            component.delayEffect = CreateIceDelayEffect();
            component.damageType = DamageType.Freeze2s;
            component.baseForce = 25f;

            AltArtiPassive.iceBlast = blast;

        }

        // TODO: REWRITE
        private void CreateLightningSword( Int32 meshInd )
        {
            GameObject ghost = CreateLightningSwordGhost(meshInd);
            GameObject proj = Resources.Load<GameObject>("Prefabs/Projectiles/LunarNeedleProjectile" ).ClonePrefab( "LightningSwordProjectile" + meshInd.ToString(), true );
            ProjectileCatalog.getAdditionalEntries += ( list ) => list.Add( proj );

            AltArtiPassive.lightningProjectile[meshInd] = proj;

            var projController = proj.GetComponent<ProjectileController>();
            projController.ghostPrefab = ghost;

            var projSimple = proj.GetComponent<ProjectileSimple>();
            projSimple.enabled = true;
            projSimple.enableVelocityOverLifetime = false;
            projSimple.velocity = 60f;

            var projTargetFind = proj.GetComponent<ProjectileDirectionalTargetFinder>();
            projTargetFind.enabled = false;

            var projSteering = proj.GetComponent<ProjectileSteerTowardTarget>();
            projSteering.enabled = true;
            projSteering.rotationSpeed = 140f;

            var projStick = proj.GetComponent<ProjectileStickOnImpact>();
            projStick.alignNormals = false;

            var projExpl = proj.GetComponent<ProjectileImpactExplosion>();
            projExpl.impactEffect = Resources.Load<GameObject>( "Prefabs/Effects/LightningStakeNova" );
            projExpl.explosionSoundString = "Play_item_lunar_primaryReplace_impact";
            projExpl.lifetimeExpiredSoundString = "";
            projExpl.offsetForLifetimeExpiredSound = 0f;
            projExpl.destroyOnEnemy = false;
            projExpl.destroyOnWorld = false;
            projExpl.timerAfterImpact = true;
            projExpl.falloffModel = BlastAttack.FalloffModel.None;
            projExpl.lifetime = 10f;
            projExpl.lifetimeAfterImpact = 1f;
            projExpl.lifetimeRandomOffset = 0f;
            projExpl.blastRadius = 1f;
            projExpl.blastDamageCoefficient = 1f;
            projExpl.blastProcCoefficient = 0.2f;
            projExpl.bonusBlastForce = Vector3.zero;
            projExpl.fireChildren = false;
            projExpl.childrenProjectilePrefab = null;
            projExpl.childrenCount = 0;
            projExpl.childrenDamageCoefficient = 0f;
            projExpl.minAngleOffset = Vector3.zero;
            projExpl.maxAngleOffset = Vector3.zero;
            projExpl.transformSpace = ProjectileImpactExplosion.TransformSpace.World;
            projExpl.projectileHealthComponent = null;

            var projStimp = proj.GetComponent<ProjectileSingleTargetImpact>();
            projStimp.destroyOnWorld = false;
            projStimp.hitSoundString = "Play_item_proc_dagger_impact";
            projStimp.enemyHitSoundString = "Play_item_proc_dagger_impact";

            proj.AddComponent<Components.SoundOnAwake>().sound = "Play_item_proc_dagger_spawn";

            UnityEngine.Object.Destroy( proj.GetComponent<AwakeEvent>() );
            UnityEngine.Object.Destroy( proj.GetComponent<DelayedEvent>() );
        }
    }
}
