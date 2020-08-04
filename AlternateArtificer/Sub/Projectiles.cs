namespace AlternativeArtificer
{
    using System;

    using AlternativeArtificer.States.Main;

    using R2API;

    using RoR2;
    using RoR2.Projectile;

    using UnityEngine;

    public partial class Main
    {
        private void EditProjectiles()
        {
            this.EditNovaBomb();
            this.EditPlasmaBolt();
        }

        private void CreateProjectiles()
        {
            this.CreateLightningSwords();
            this.CreateIceExplosion();
        }

        private void EditNovaBomb()
        {
            GameObject novaProj = Resources.Load<GameObject>("Prefabs/Projectiles/MageLightningBombProjectile");

            ProjectileSimple novaSimp = novaProj.GetComponent<ProjectileSimple>();
            novaSimp.velocity *= 1.25f;

            Rigidbody rb = novaProj.GetComponent<Rigidbody>();
            rb.useGravity = true;
            rb.drag = 1f;
            novaProj.GetComponent<Rigidbody>().useGravity = true;

            ProjectileImpactExplosion novaImpact = novaProj.GetComponent<ProjectileImpactExplosion>();
            novaImpact.blastDamageCoefficient = 1.0f;
            novaImpact.falloffModel = RoR2.BlastAttack.FalloffModel.None;
            novaImpact.blastRadius = 15f;

            ProjectileProximityBeamController novaBeams = novaProj.GetComponent<ProjectileProximityBeamController>();
            novaBeams.attackRange *= 1.25f;
            novaBeams.attackInterval *= 0.9f;
            novaBeams.bounces += 1;
        }

        private void EditPlasmaBolt()
        {
            // TODO: implement plasma bolt edits
        }

        private void CreateLightningSwords()
        {
            AltArtiPassive.lightningProjectile = new GameObject[5];
            for( Int32 i = 0; i < 5; i++ )
            {
                this.CreateLightningSword( i );
            }
        }

        private void CreateLightningSword( Int32 meshInd )
        {
            GameObject ghost = this.CreateLightningSwordGhost(meshInd);
            GameObject proj = Resources.Load<GameObject>("Prefabs/Projectiles/LunarNeedleProjectile" ).InstantiateClone( "LightningSwordProjectile" + meshInd.ToString() );
            ProjectileCatalog.getAdditionalEntries += ( list ) => list.Add( proj );

            AltArtiPassive.lightningProjectile[meshInd] = proj;

            ProjectileController projController = proj.GetComponent<ProjectileController>();
            projController.ghostPrefab = ghost;
            projController.procCoefficient = 0.2f;

            ProjectileSimple projSimple = proj.GetComponent<ProjectileSimple>();
            projSimple.enabled = true;
            projSimple.enableVelocityOverLifetime = false;
            projSimple.velocity = 60f;
            

            ProjectileDirectionalTargetFinder projTargetFind = proj.GetComponent<ProjectileDirectionalTargetFinder>();
            projTargetFind.enabled = false;

            ProjectileSteerTowardTarget projSteering = proj.GetComponent<ProjectileSteerTowardTarget>();
            projSteering.enabled = true;
            projSteering.rotationSpeed = 140f;

            ProjectileStickOnImpact projStick = proj.GetComponent<ProjectileStickOnImpact>();
            //projStick.ignoreCharacters = false;
            //projStick.ignoreWorld = false;
            projStick.alignNormals = false;

            ProjectileImpactExplosion projExpl = proj.GetComponent<ProjectileImpactExplosion>();
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

            ProjectileSingleTargetImpact projStimp = proj.GetComponent<ProjectileSingleTargetImpact>();
            projStimp.destroyOnWorld = false;
            projStimp.hitSoundString = "Play_item_proc_dagger_impact";
            projStimp.enemyHitSoundString = "Play_item_proc_dagger_impact";
            

            proj.AddComponent<Components.SoundOnAwake>().sound = "Play_item_proc_dagger_spawn";

            //UnityEngine.Object.DestroyImmediate( proj.GetComponent<ProjectileSingleTargetImpact>() );
            UnityEngine.Object.Destroy( proj.GetComponent<AwakeEvent>() );
            UnityEngine.Object.Destroy( proj.GetComponent<DelayedEvent>() );
        }

        private void CreateIceExplosion()
        {
            GameObject blast = Resources.Load<GameObject>("Prefabs/NetworkedObjects/GenericDelayBlast").InstantiateClone( "IceDelayBlast" );
            DelayBlast component = blast.GetComponent<DelayBlast>();
            component.crit = false;
            component.procCoefficient = 1.0f;
            component.maxTimer = 1.25f;
            component.falloffModel = BlastAttack.FalloffModel.None;
            component.explosionEffect = this.CreateIceExplosionEffect();
            component.delayEffect = this.CreateIceDelayEffect();
            component.damageType = DamageType.Freeze2s;
            component.baseForce = 25f;

            AltArtiPassive.iceBlast = blast;
        }
    }
}
