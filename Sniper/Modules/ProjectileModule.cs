using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using ReinCore;
using RoR2;
using RoR2.Projectile;
using Sniper.Components;
using Sniper.ScriptableObjects;
using UnityEngine;
using UnityEngine.Networking;

namespace Sniper.Modules
{
	internal static class ProjectileModule
	{
		private static GameObject _knifeProjectile;
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
			var obj = PrefabsCore.CreatePrefab( "KnifeProjectile", true );

            obj.layer = LayerIndex.projectile.intVal;

			var netId = obj.AddOrGetComponent<NetworkIdentity>();



			var teamFilter = obj.AddOrGetComponent<TeamFilter>();



			var projControl = obj.AddOrGetComponent<ProjectileController>();
            projControl.allowPrediction = false;
            projControl.catalogIndex = -1;
            projControl.ghostPrefab = null; // TODO: Knife projectile ghost
            projControl.ghostTransformAnchor = null;
            projControl.owner = null;
            projControl.procCoefficient = 1f;
            projControl.startSound = null; // TODO: Knife start sound


			var rb = obj.AddOrGetComponent<Rigidbody>();
            rb.mass = 1f;
            rb.drag = 0.1f;
            rb.angularDrag = 0.05f;
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;


			var projNetTrans = obj.AddOrGetComponent<ProjectileNetworkTransform>();
            projNetTrans.positionTransmitInterval = 0.033333334f;
            projNetTrans.interpolationFactor = 1f;
            projNetTrans.allowClientsideCollision = false;


			var projStick = obj.AddOrGetComponent<ProjectileStickOnImpact>();
            projStick.alignNormals = false;
            projStick.ignoreCharacters = false;
            projStick.ignoreWorld = false;


			var projImpact = obj.AddOrGetComponent<ProjectileSingleTargetImpact>();
            projImpact.destroyOnWorld = false;
            projImpact.destroyWhenNotAlive = false;
            projImpact.enemyHitSoundString = null; // TODO: Knife hit sound
            projImpact.hitSoundString = null; // TODO: Knife world hit sound
            projImpact.impactEffect = null; // TODO: Knife impact effect


			var projSimple = obj.AddOrGetComponent<ProjectileSimple>();
			projSimple.enableVelocityOverLifetime = false;
			projSimple.lifetime = 18f;
			projSimple.updateAfterFiring = true;
			projSimple.velocity = 100f;
			projSimple.velocityOverLifetime = null;


			var col = obj.AddOrGetComponent<SphereCollider>();
			col.center = Vector3.zero;
			col.contactOffset = 0f;
			col.isTrigger = false;
			col.material = null;
			col.radius = 0.3f;


			var damage = obj.AddOrGetComponent<ProjectileDamage>();
			damage.crit = false;
			damage.damage = 0f;
			damage.damageColorIndex = DamageColorIndex.Default;
			damage.damageType = DamageType.Generic;
			damage.force = 0f;


			var deploy = obj.AddOrGetComponent<Deployable>();



			var knifeSync = obj.AddOrGetComponent<KnifeDeployableSync>();





			foreach( var runtimePrefabComp in obj.GetComponents<IRuntimePrefabComponent>() )
			{
				runtimePrefabComp.InitializePrefab();
			}
			return obj;
		}
	}
}
