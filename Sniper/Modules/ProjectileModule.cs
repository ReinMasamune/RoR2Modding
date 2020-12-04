namespace Rein.Sniper.Modules
{
    using ReinCore;

    using RoR2;
    using RoR2.Projectile;

    using Rein.Sniper.Components;

    using UnityEngine;
    using UnityEngine.Networking;

    internal static class ProjectileModule
    {
#pragma warning disable IDE1006 // Naming Styles
        private static GameObject _knifeProjectile;
#pragma warning restore IDE1006 // Naming Styles
        internal static GameObject GetKnifeProjectile()
        {
            if( _knifeProjectile == null )
            {
                _knifeProjectile = CreateKnifeProjectile();
                ProjectileCatalog.getAdditionalEntries += ( list ) => list.Add( _knifeProjectile );
            }

            return _knifeProjectile;
        }


        private static GameObject CreateKnifeProjectile()
        {
            GameObject obj = PrefabsCore.CreatePrefab( "KnifeProjectile", true );

            obj.layer = LayerIndex.projectile.intVal;
            _ = obj.AddOrGetComponent<NetworkIdentity>();
            _ = obj.AddOrGetComponent<TeamFilter>();



            ProjectileController projControl = obj.AddOrGetComponent<ProjectileController>();
            projControl.allowPrediction = false;
            projControl.catalogIndex = -1;
            projControl.ghostPrefab = null; // FUTURE: Knife projectile ghost
            projControl.ghostTransformAnchor = null;
            projControl.owner = null;
            projControl.procCoefficient = 1f;
            projControl.startSound = null; // FUTURE: Knife start sound


            Rigidbody rb = obj.AddOrGetComponent<Rigidbody>();
            rb.mass = 1f;
            rb.drag = 0.4f;
            rb.angularDrag = 0.05f;
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;


            ProjectileNetworkTransform projNetTrans = obj.AddOrGetComponent<ProjectileNetworkTransform>();
            projNetTrans.positionTransmitInterval = 0.033333334f;
            projNetTrans.interpolationFactor = 1f;
            projNetTrans.allowClientsideCollision = false;


            ProjectileStickOnImpact projStick = obj.AddOrGetComponent<ProjectileStickOnImpact>();
            projStick.alignNormals = false;
            projStick.ignoreCharacters = false;
            projStick.ignoreWorld = false;
            projStick.stickEvent = new UnityEngine.Events.UnityEvent();


            ProjectileSingleTargetImpact projImpact = obj.AddOrGetComponent<ProjectileSingleTargetImpact>();
            projImpact.destroyOnWorld = false;
            projImpact.destroyWhenNotAlive = false;
            projImpact.enemyHitSoundString = null; // FUTURE: Knife hit sound
            projImpact.hitSoundString = null; // FUTURE: Knife world hit sound
            projImpact.impactEffect = null; // FUTURE: Knife impact effect


            ProjectileSimple projSimple = obj.AddOrGetComponent<ProjectileSimple>();
            projSimple.enableVelocityOverLifetime = false;
            projSimple.lifetime = 18f;
            projSimple.updateAfterFiring = false;
            projSimple.velocity = 175f;
            projSimple.velocityOverLifetime = null;


            SphereCollider col = obj.AddOrGetComponent<SphereCollider>();
            col.center = Vector3.zero;
            col.contactOffset = 0f;
            col.isTrigger = false;
            col.material = null;
            col.radius = 0.3f;


            ProjectileDamage damage = obj.AddOrGetComponent<ProjectileDamage>();
            damage.crit = false;
            damage.damage = 0f;
            damage.damageColorIndex = DamageColorIndex.Default;
            damage.damageType = CatalogModule.sniperResetDamageType;
            damage.force = 0f;

            _ = obj.AddOrGetComponent<Deployable>();

            _ = obj.AddOrGetComponent<KnifeDeployableSync>();


            SphereCollider ownerDetection = obj.AddComponent<SphereCollider>();
            ownerDetection.isTrigger = true;
            ownerDetection.radius = 10f;



            foreach( IRuntimePrefabComponent runtimePrefabComp in obj.GetComponents<IRuntimePrefabComponent>() )
            {
                runtimePrefabComp.InitializePrefab();
            }
            return obj;
        }
    }
}
