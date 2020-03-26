#if ANCIENTWISP
using RoR2;
using RoR2.Orbs;
using RoR2.Projectile;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        [RequireComponent( typeof( ProjectileController) )]
        [RequireComponent( typeof( ProjectileDamage ) )]
        internal class ProjectileColliderDamageZone : MonoBehaviour
        {
            public Single damageMult = 1f;
            public Single damageInterval = 1f;
            public Single procCoef = 1f;
            public Single radialForce = 0f;
            public Single normalForce = 0f;
            public Single tangentForce = 0f;
           



            private ProjectileController projectileController;
            private ProjectileDamage projectileDamage;
            private Dictionary<HealthComponent,Single> targetTimers = new Dictionary<HealthComponent, Single>();
            private HashSet<HealthComponent> hitThisFrame = new HashSet<HealthComponent>();
            private TeamIndex team;
            private GameObject owner;
            private Single deltaT = 0f;
            private Boolean crit = false;

            private void Awake()
            {
                this.projectileController = base.GetComponent<ProjectileController>();
                this.projectileDamage = base.GetComponent<ProjectileDamage>();

            }
            private void Start()
            {
                this.owner = this.projectileController.owner;
                this.team = TeamComponent.GetObjectTeam( this.owner );
                this.crit = this.projectileDamage.crit;
            }

            private void FixedUpdate()
            {
                this.hitThisFrame.Clear();
                this.deltaT = Time.fixedDeltaTime;
            }

            private void OnTriggerStay( Collider other )
            {
                if( !NetworkServer.active ) return;
                var hb = other.GetComponent<HurtBox>();
                if( hb == null ) return;

                if( hb.teamIndex == this.team ) return;

                var hc = hb.healthComponent;
                if( hc == null ) return;

                if( this.hitThisFrame.Contains( hc ) ) return;

                this.hitThisFrame.Add( hc );
                if( !this.targetTimers.ContainsKey( hc ) )
                {
                    this.targetTimers[hc] = 0f;
                }
                this.targetTimers[hc] += this.deltaT;
                while( this.targetTimers[hc] >= this.damageInterval )
                {
                    this.TickDamage( hc );
                    this.targetTimers[hc] -= this.damageInterval;
                }
            }

            private void TickDamage( HealthComponent hc )
            {
                var posDiff = hc.transform.position - base.transform.position;
                var dist = posDiff.magnitude;
                var dir = posDiff.normalized;
                var normalForce = Vector3.up * this.normalForce;
                var radialForce = dir * this.radialForce;
                var tangentForce = Vector3.zero * this.tangentForce;
                var info = new DamageInfo
                {
                    attacker = this.owner,
                    crit = this.projectileDamage.crit,
                    damage = this.projectileDamage.damage * this.damageMult,
                    damageColorIndex = DamageColorIndex.Default,
                    damageType = this.projectileDamage.damageType,
                    dotIndex = DotController.DotIndex.None,
                    force = (normalForce + radialForce + tangentForce ) * this.projectileDamage.force,
                    inflictor = base.gameObject,
                    position = hc.transform.position,
                    procChainMask = this.projectileController.procChainMask,
                    procCoefficient = this.projectileController.procCoefficient * this.procCoef,
                };


                hc.TakeDamage( info );
                GlobalEventManager.instance.OnHitEnemy( info, hc.gameObject );
                GlobalEventManager.instance.OnHitAll( info, hc.gameObject );
            }
        }
    }

}
#endif