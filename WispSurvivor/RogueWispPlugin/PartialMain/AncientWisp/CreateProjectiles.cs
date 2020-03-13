#if ANCIENTWISP
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;
using ReinCore;

namespace RogueWispPlugin
{
    internal partial class Main
    {
        internal static GameObject AW_primaryProjectile;
        internal static GameObject AW_utilityProjectile;
        internal static GameObject AW_utilityZoneProjectile;
        partial void AW_CreateProjectiles()
        {
            this.Load += this.AW_CreatePrimaryProjectile;
            this.Load += this.AW_CreateUtilityZoneProjectile;
            this.Load += this.AW_CreateUtilityProjectile;


        }

        private void AW_CreateUtilityZoneProjectile()
        {
            var obj = PrefabsCore.CreatePrefab("AWPrimaryProjectile", true );

            obj.layer = LayerIndex.projectile.intVal;

            var netID = obj.GetComponent<NetworkIdentity>();
            netID.localPlayerAuthority = true;
            netID.serverOnly = false;

            var projControl = obj.AddComponent<ProjectileController>();
            projControl.catalogIndex = -1;
            projControl.ghostPrefab = AW_utilityZoneGhost;
            projControl.isPrediction = false;
            projControl.allowPrediction = true;
            projControl.procChainMask = default;
            projControl.procCoefficient = 1f;


            var rb = obj.AddComponent<Rigidbody>();
            rb.mass = 1f;
            rb.drag = 0f;
            rb.angularDrag = 0.05f;
            rb.useGravity = false;
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;


            var projNetTransform = obj.AddComponent<ProjectileNetworkTransform>();
            projNetTransform.positionTransmitInterval = 0.03333334f;
            projNetTransform.interpolationFactor = 1f;
            projNetTransform.allowClientsideCollision = false;

            var projSimple = obj.AddComponent<ProjectileSimple>();
            projSimple.velocity = 0f;
            projSimple.lifetime = 10f;
            projSimple.updateAfterFiring = false;
            projSimple.enableVelocityOverLifetime = false;


            var collider = obj.AddComponent<SphereCollider>();
            collider.isTrigger = true;
            collider.material = null;
            collider.center = new Vector3( 0f, 0f, 0f );
            collider.radius = 1f;

            var colliderRadScale = obj.AddComponent<ScaleColliderRadiusOverTime>();
            colliderRadScale.startRadius = 1f;
            colliderRadScale.endRadius = 75f;
            colliderRadScale.duration = 10f;
            colliderRadScale.durationFrac = 1f;


            var damage = obj.AddComponent<ProjectileDamage>();
            damage.damage = 0f;
            damage.crit = false;
            damage.force = 0f;
            damage.damageColorIndex = DamageColorIndex.Default;
            damage.damageType = DamageType.Generic;

            var teamFilter = obj.AddComponent<TeamFilter>();


            var healOrb = obj.AddComponent<ProjectileUniversalHealOrbOnDamage>();
            healOrb.effectPrefab = AW_utilityOrbEffect;
            healOrb.healTarget = UniversalHealOrb.HealTarget.Barrier;
            healOrb.healType = UniversalHealOrb.HealType.PercentMissing;
            healOrb.value = 0.05f;
            healOrb.useSkin = true;


            var damageZone = obj.AddComponent<ProjectileColliderDamageZone>();
            damageZone.damageInterval = 0.5f;
            damageZone.damageMult = 0.5f;
            damageZone.procCoef = 0.5f;


            AW_utilityZoneProjectile = obj;
            RegisterProjectile( AW_utilityZoneProjectile );
        }
        private void AW_CreateUtilityProjectile()
        {
            var obj = PrefabsCore.CreatePrefab("AWUtilityProjectile", true );

            obj.layer = LayerIndex.projectile.intVal;

            var netID = obj.GetComponent<NetworkIdentity>();
            netID.localPlayerAuthority = true;
            netID.serverOnly = false;

            var projControl = obj.AddComponent<ProjectileController>();
            projControl.catalogIndex = -1;
            projControl.ghostPrefab = AW_utilityProjGhost;
            projControl.isPrediction = false;
            projControl.allowPrediction = true;
            projControl.procChainMask = default;
            projControl.procCoefficient = 1f;


            var rb = obj.AddComponent<Rigidbody>();
            rb.mass = 1f;
            rb.drag = 0f;
            rb.angularDrag = 0.05f;
            rb.useGravity = false;
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;


            var projNetTransform = obj.AddComponent<ProjectileNetworkTransform>();
            projNetTransform.positionTransmitInterval = 0.03333334f;
            projNetTransform.interpolationFactor = 1f;
            projNetTransform.allowClientsideCollision = false;

            var projSimple = obj.AddComponent<ProjectileSimple>();
            projSimple.velocity = 40f;
            projSimple.lifetime = 30f;
            projSimple.updateAfterFiring = false;
            projSimple.enableVelocityOverLifetime = false;


            var collider = obj.AddComponent<SphereCollider>();
            collider.isTrigger = false;
            collider.material = null;
            collider.center = Vector3.zero;
            collider.radius = 1f;


            var damage = obj.AddComponent<ProjectileDamage>();
            damage.damage = 0f;
            damage.crit = false;
            damage.force = 0f;
            damage.damageColorIndex = DamageColorIndex.Default;
            damage.damageType = DamageType.Generic;

            var teamFilter = obj.AddComponent<TeamFilter>();

            var impactExplosion = obj.AddComponent<ProjectileImpactExplosion>();
            impactExplosion.impactEffect = null;
            impactExplosion.explosionSoundString = Main.skinnedProjExplosionString;
            impactExplosion.lifetimeExpiredSoundString = "";
            impactExplosion.offsetForLifetimeExpiredSound = 0f;
            impactExplosion.destroyOnEnemy = true;
            impactExplosion.destroyOnWorld = true;
            impactExplosion.timerAfterImpact = false;
            impactExplosion.falloffModel = BlastAttack.FalloffModel.Linear;
            impactExplosion.lifetime = 5f;
            impactExplosion.lifetimeAfterImpact = 0f;
            impactExplosion.lifetimeRandomOffset = 0f;
            impactExplosion.blastRadius = 0f;
            impactExplosion.blastDamageCoefficient = 0f;
            impactExplosion.blastProcCoefficient = 0f;
            impactExplosion.bonusBlastForce = new Vector3( 0f, 0f, 0f );
            impactExplosion.fireChildren = true;
            impactExplosion.childrenProjectilePrefab = AW_utilityZoneProjectile;
            impactExplosion.childrenCount = 1;
            impactExplosion.childrenDamageCoefficient = 1f;
            impactExplosion.minAngleOffset = Vector3.zero;
            impactExplosion.maxAngleOffset = Vector3.zero;
            impactExplosion.transformSpace = ProjectileImpactExplosion.TransformSpace.World;
            impactExplosion.projectileHealthComponent = null;


            AW_utilityProjectile = obj;
            RegisterProjectile( AW_utilityProjectile );
        }

        private void AW_CreatePrimaryProjectile()
        {
            var obj = PrefabsCore.CreatePrefab("AWPrimaryProjectile", true );

            obj.layer = LayerIndex.projectile.intVal;

            var netID = obj.GetComponent<NetworkIdentity>();
            netID.localPlayerAuthority = true;
            netID.serverOnly = false;

            var projControl = obj.AddComponent<ProjectileController>();
            projControl.catalogIndex = -1;
            projControl.ghostPrefab = AW_primaryProjGhost;
            projControl.isPrediction = false;
            projControl.allowPrediction = true;
            projControl.procChainMask = default;
            projControl.procCoefficient = 1f;
            

            var rb = obj.AddComponent<Rigidbody>();
            rb.mass = 1f;
            rb.drag = 0f;
            rb.angularDrag = 0.05f;
            rb.useGravity = false;
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            

            var projNetTransform = obj.AddComponent<ProjectileNetworkTransform>();
            projNetTransform.positionTransmitInterval = 0.03333334f;
            projNetTransform.interpolationFactor = 1f;
            projNetTransform.allowClientsideCollision = false;

            var projSimple = obj.AddComponent<ProjectileSimple>();
            projSimple.velocity = 76f;
            projSimple.lifetime = 5f;
            projSimple.updateAfterFiring = false;
            projSimple.enableVelocityOverLifetime = false;


            var collider = obj.AddComponent<SphereCollider>();
            collider.isTrigger = false;
            collider.material = null;
            collider.center = Vector3.zero;
            collider.radius = 1.5f;


            var damage = obj.AddComponent<ProjectileDamage>();
            damage.damage = 0f;
            damage.crit = false;
            damage.force = 0f;
            damage.damageColorIndex = DamageColorIndex.Default;
            damage.damageType = DamageType.Generic;

            var teamFilter = obj.AddComponent<TeamFilter>();

            var impactExplosion = obj.AddComponent<ProjectileImpactExplosion>();
            impactExplosion.impactEffect = AW_primaryExplosionEffect;
            impactExplosion.explosionSoundString = Main.skinnedProjExplosionString;
            impactExplosion.lifetimeExpiredSoundString = "";
            impactExplosion.offsetForLifetimeExpiredSound = 0f;
            impactExplosion.destroyOnEnemy = true;
            impactExplosion.destroyOnWorld = true;
            impactExplosion.timerAfterImpact = false;
            impactExplosion.falloffModel = BlastAttack.FalloffModel.Linear;
            impactExplosion.lifetime = 5f;
            impactExplosion.lifetimeAfterImpact = 0f;
            impactExplosion.lifetimeRandomOffset = 0f;
            impactExplosion.blastRadius = 14f;
            impactExplosion.blastDamageCoefficient = 1f;
            impactExplosion.blastProcCoefficient = 1f;
            impactExplosion.bonusBlastForce = new Vector3( 0f, 750f, 0f );
            impactExplosion.fireChildren = false;
            impactExplosion.childrenProjectilePrefab = null;
            impactExplosion.childrenCount = 0;
            impactExplosion.childrenDamageCoefficient = 0f;
            impactExplosion.minAngleOffset = Vector3.zero;
            impactExplosion.maxAngleOffset = Vector3.zero;
            impactExplosion.transformSpace = ProjectileImpactExplosion.TransformSpace.World;
            impactExplosion.projectileHealthComponent = null;


            AW_primaryProjectile = obj;
            RegisterProjectile( AW_primaryProjectile);
        }
    }
}
#endif
