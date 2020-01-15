using BepInEx;
using RoR2;
using UnityEngine;
using R2API;
using R2API.Utils;
using System.Reflection;
using EntityStates;
using RoR2.Skills;
using System;
using RoR2.Projectile;

namespace ReinSniperRework
{
    internal partial class Main
    {
        partial void Projectiles()
        {
            this.Load += this.CreateKnifeProjectile;
        }

        private void CreateKnifeProjectile()
        {
            var obj = Resources.Load<GameObject>("Prefabs/Projectiles/Arrow").InstantiateClone("SniperKnifeProjectile");
            this.knifeProjectile = obj;
            ProjectileCatalog.getAdditionalEntries += ( list ) => list.Add( this.knifeProjectile );

            var projCallbacks = obj.AddComponent<SniperKnifeCallbacks>();
            projCallbacks.targetState = new SerializableEntityStateType( typeof( SniperKnifeSlash ) );
            projCallbacks.interruptPriority = InterruptPriority.PrioritySkill;


            var controller = obj.GetComponent<ProjectileController>();
            //Ghost


            var rigidBody = obj.GetComponent<Rigidbody>();
            rigidBody.useGravity = true;
            rigidBody.drag = 1.8f;


            var projSimple = obj.GetComponent<ProjectileSimple>();
            projSimple.velocity = 115f;
            projSimple.lifetime = 5.5f;
            projSimple.updateAfterFiring = false;


            var collider = obj.GetComponent<SphereCollider>();



            var projSingleImpact = obj.GetComponent<ProjectileSingleTargetImpact>();
            projSingleImpact.destroyOnWorld = false;
            projSingleImpact.destroyWhenNotAlive = false;


            var projDamage = obj.GetComponent<ProjectileDamage>();
            projDamage.damageType = Main.instance.resetOnKill;



            var projStick = obj.AddComponent<ProjectileStickOnImpact>();
            projStick.alignNormals = false;
            projStick.ignoreCharacters = false;
            projStick.ignoreWorld = false;


            var deployable = obj.AddComponent<Deployable>();



            var projDeployable = obj.AddComponent<ProjectileDeployToOwner>();
            projDeployable.deployableSlot = (DeployableSlot)7;



            var projCallOwner = obj.AddComponent<ProjectileCallOnOwnerNearby>();
            projCallOwner.radius = 1.65f;
            projCallOwner.onOwnerEnter = new UnityEngine.Events.UnityEvent();


            var projFuse = obj.AddComponent<ProjectileFuse>();
            projFuse.fuse = 0.4f;
            projFuse.onFuse = new UnityEngine.Events.UnityEvent();



        }
    }
}


