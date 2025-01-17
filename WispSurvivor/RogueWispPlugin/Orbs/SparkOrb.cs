﻿using System.Collections.Generic;

using RoR2;
using RoR2.Orbs;

using UnityEngine;

namespace Rein.RogueWispPlugin
{
    internal partial class Main
    {
        internal class SparkOrb : RoR2.Orbs.Orb
        {
            public System.Single speed = 200f;
            public System.Single damage = 1f;
            public System.Single scale = 1f;
            public System.Single procCoef = 1f;
            public System.Single radius = 1f;
            public System.Single stepHeight = 1f;
            public System.Single stepDist = 1f;
            public System.Single maxFall = 5f;
            public System.Single minDistThreshold = 1f;
            public System.Single height = 10f;
            public System.Single vOffset = 5f;
            public System.Single innerRadScale = 0.67f;
            public System.Single edgePenaltyMult = 0.35f;

            public System.Int32 stepsLeft = 0;
            public System.UInt32 skin = 0;

            public Vector3 direction;

            public System.Boolean isFirst = false;
            public System.Boolean crit = false;


            public TeamIndex team;
            public DamageColorIndex damageColor;

            public GameObject attacker;
            public ProcChainMask procMask;

            private Vector3 dest;

            private System.Boolean last = false;

            public HashSet<HealthComponent> hitMask;

            public override void Begin()
            {
                if( this.hitMask == null ) this.hitMask = new HashSet<HealthComponent>();

                Vector3 intermediate = this.origin + new Vector3(0f, this.stepHeight, 0f);
                intermediate += this.direction * this.stepDist * ( this.isFirst ? 0.5f : 1.0f );
                Vector3 dir = intermediate - this.origin;
                System.Single dist = dir.magnitude;
                dir = Vector3.Normalize( dir );

                Ray r1 = new Ray
                {
                    origin = origin,
                    direction = dir
                };

                RaycastHit rh1;

                Vector3 top;

                if( Physics.SphereCast( r1, 0.25f, out rh1, dist, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal ) )
                {
                    top = rh1.point + rh1.normal;
                } else
                {
                    top = r1.GetPoint( dist );
                }

                Ray r2 = new Ray
                {
                    origin = top,
                    direction = Vector3.down
                };

                RaycastHit rh2;

                if( Physics.SphereCast( r2, 0.25f, out rh2, this.maxFall, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal ) )
                {
                    this.dest = rh2.point + rh2.normal;
                } else
                {
                    this.dest = r2.GetPoint( this.maxFall );
                }

                System.Single distance = Vector3.Distance( this.dest, this.origin );

                this.duration = distance / this.speed;

                if( !this.isFirst && distance < this.minDistThreshold ) this.last = true;

                //Effect is created here
            }

            public override void OnArrival()
            {
                //Explosion effect
                EffectData effect = new EffectData
                {
                    origin = dest,
                    genericUInt = this.skin
                };

                EffectManager.SpawnEffect( Main.secondaryExplosion, effect, true );

                if( this.attacker )
                {
                    HashSet<HealthComponent> mask = new HashSet<HealthComponent>();
                    HurtBox box;

                    DamageInfo bdmg = new DamageInfo();
                    bdmg.damage = this.damage;
                    bdmg.attacker = this.attacker;
                    bdmg.crit = this.crit;
                    bdmg.damageColorIndex = this.damageColor;
                    bdmg.damageType = DamageType.Generic;
                    bdmg.force = ( this.direction + Vector3.up ) * 50f;
                    bdmg.inflictor = this.attacker;
                    bdmg.position = this.dest;
                    bdmg.procChainMask = this.procMask;
                    bdmg.procCoefficient = this.procCoef;
                    GlobalEventManager.instance.OnHitAll( bdmg, null );


                    //Collider[] cols = Physics.OverlapCapsule(this.dest, this.dest + new Vector3(0f, 20f, 0f), this.radius * this.innerRadScale, LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal);

                    //foreach( Collider col in cols )
                    //{
                    //    if( !col ) continue;
                    //    box = col.GetComponent<HurtBox>();
                    //    if( !box ) continue;
                    //    HealthComponent hcomp = box.healthComponent;
                    //    if( !hcomp || mask.Contains( hcomp ) || TeamComponent.GetObjectTeam( hcomp.gameObject ) == this.team ) continue;

                    //    DamageInfo dmg = new DamageInfo();
                    //    dmg.damage = this.damage;
                    //    dmg.attacker = this.attacker;
                    //    dmg.crit = this.crit;
                    //    dmg.damageColorIndex = this.damageColor;
                    //    dmg.damageType = DamageType.Generic;
                    //    dmg.force = (this.direction + Vector3.up) * 50f;
                    //    dmg.inflictor = this.attacker;
                    //    dmg.position = col.transform.position;
                    //    dmg.procChainMask = this.procMask;
                    //    dmg.procCoefficient = this.procCoef;

                    //    hcomp.TakeDamage( dmg );
                    //    GlobalEventManager.instance.OnHitEnemy( dmg, hcomp.gameObject );
                    //    GlobalEventManager.instance.OnHitAll( dmg, hcomp.gameObject );
                    //    mask.Add( hcomp );
                    //}

                    var cols = Physics.OverlapCapsule( this.dest, this.dest + new Vector3( 0f, 20f, 0f ), this.radius, LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal );

                    foreach( Collider col in cols )
                    {
                        if( !col ) continue;
                        box = col.GetComponent<HurtBox>();
                        if( !box ) continue;
                        HealthComponent hcomp = box.healthComponent;
                        if( !hcomp || mask.Contains( hcomp ) || !FriendlyFireManager.ShouldDirectHitProceed( hcomp, this.team ) || hcomp.gameObject == this.attacker ) continue;

                        DamageInfo dmg = new DamageInfo();
                        dmg.damage = this.damage;
                        dmg.attacker = this.attacker;
                        dmg.crit = this.crit;
                        dmg.damageColorIndex = this.damageColor;
                        dmg.damageType = DamageType.Generic;
                        dmg.force = ( this.direction + Vector3.up ) * 10f;
                        dmg.inflictor = this.attacker;
                        dmg.position = col.transform.position;
                        dmg.procChainMask = this.procMask;
                        dmg.procCoefficient = this.procCoef * this.edgePenaltyMult;

                        if( this.hitMask.Contains( hcomp ) )
                        {
                            dmg.damage *= this.edgePenaltyMult;
                            dmg.procCoefficient *= this.edgePenaltyMult;
                        }

                        hcomp.TakeDamage( dmg );
                        GlobalEventManager.instance.OnHitEnemy( dmg, hcomp.gameObject );
                        GlobalEventManager.instance.OnHitAll( dmg, hcomp.gameObject );
                        mask.Add( hcomp );
                        this.hitMask.Add( hcomp );
                    }
                }

                if( this.stepsLeft > 0 && !this.last )
                {
                    SparkOrb nextOrb = new SparkOrb();
                    nextOrb.maxFall = this.maxFall;
                    nextOrb.attacker = this.attacker;
                    nextOrb.crit = this.crit;
                    nextOrb.damage = this.damage;
                    nextOrb.damageColor = this.damageColor;
                    nextOrb.direction = this.direction;
                    nextOrb.origin = this.dest;
                    nextOrb.procCoef = this.procCoef;
                    nextOrb.procMask = this.procMask;
                    nextOrb.isFirst = false;
                    nextOrb.radius = this.radius;
                    nextOrb.scale = this.scale;
                    nextOrb.speed = this.speed;
                    nextOrb.stepDist = this.stepDist;
                    nextOrb.stepHeight = this.stepHeight;
                    nextOrb.stepsLeft = this.stepsLeft - 1;
                    nextOrb.target = this.target;
                    nextOrb.team = this.team;
                    nextOrb.skin = this.skin;
                    nextOrb.hitMask = this.hitMask;

                    OrbManager.instance.AddOrb( nextOrb );
                }
            }


        }
    }
}