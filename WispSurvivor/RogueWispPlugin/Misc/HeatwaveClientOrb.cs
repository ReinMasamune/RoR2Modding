using System;
using System.Collections;
using System.Collections.Generic;

using ReinCore;

using RoR2;

using UnityEngine;

namespace Rein.RogueWispPlugin
{
    internal partial class Main
    {
        public class HeatwaveClientOrb : BaseClientOrb
        {
            public Single speed = 200f;
            public Single damage = 1f;
            public Single scale = 1f;
            public Single procCoef = 1f;
            public Single radius = 1f;
            public Single force = 0f;
            public Single chargeRestore = 0.0f;
            public Single range = 0f;
            public Single falloffStart = 0f;
            public Single endFalloffMult = 0f;
            public UInt32 skin = 0;


            public Vector3 startPos;
            public Vector3 targetPos;
            public Vector3 worldNormal;
            public Vector3 origin;
            public Vector3 direction;
            public Boolean crit = false;
            public Boolean stopAtWorld = false;
            public Boolean useTargetPos;

            public TeamIndex team;
            public DamageColorIndex damageColor;

            public GameObject attacker;
            public CharacterBody attackerBody;
            public ProcChainMask procMask;

            private Single dist1;
            private Single dist2;

            private Boolean worldHit = false;

            private Vector3 lastPos;

            //private Vector3[] worldHits;

            private Vector3 dVec;
            private Vector3 velVec;
            private Vector3 forceVec;

            private Single totalDist;

            public HashSet<HealthComponent> mask = HashSetPool<HealthComponent>.item;
            private Dictionary<HealthComponent,Hit> bestHits = DictionaryPool<HealthComponent, Hit>.item;
            private Transform hitDetector;
            private TriggerCallbackController trigger;
            private Rigidbody hitRb;

            private struct Hit
            {
                public HurtBox box;
                public Single distance;

                public Hit( HurtBox box, Single distance )
                {
                    this.box = box;
                    this.distance = distance;
                }
            }

            public override void Begin()
            {
                PickupDef pickup = default;
                var (nameToken, colorIndex) = pickup switch
                {
                    PickupDef _ when EquipmentCatalog.GetEquipmentDef(pickup.equipmentIndex) is EquipmentDef def => (def.nameToken, def.colorIndex),
                    PickupDef _ when ItemCatalog.GetItemDef(pickup.itemIndex) is ItemDef def => (def.nameToken, def.colorIndex),
                    _ => (default, default),
                };

                if( !this.useTargetPos )
                {
                    var r = new Ray( this.origin, this.direction );
                    if( this.stopAtWorld && Util.CharacterRaycast( this.attacker, r, out RaycastHit hit, this.range, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal ) )
                    {
                        this.targetPos = hit.point + this.direction * this.radius * 2;
                        this.worldHit = true;
                    } else
                    {
                        this.targetPos = r.GetPoint( this.range );
                    }
                }

                this.dVec = this.targetPos - this.startPos;
                this.totalDist = this.dVec.magnitude;
                this.dVec = this.dVec.normalized;
                base.totalDuration = this.totalDist / this.speed;
                this.forceVec = this.dVec * this.force;

                this.trigger = TriggerCallbackController.CreateSphere( this.radius, this.startPos, true, LayerIndex.projectile );
                this.hitDetector = this.trigger.transform;
                this.hitRb = this.trigger.rb;
                this.velVec = this.dVec * this.speed;
                this.trigger.onTriggerEnter += this.ColliderEnter;


                EffectData effectData = new EffectData
                {
                    origin = this.startPos,
                    genericFloat = base.totalDuration,
                    genericBool = false,
                    start = this.targetPos,
                    genericUInt = this.skin,
                };

                //this.lastPos = this.startPos;

                EffectManager.SpawnEffect( Main.primaryOrbEffect, effectData, true );

                this.dist1 = this.range * this.falloffStart;
                this.dist2 = this.range * ( 1f - this.falloffStart );
            }
            private void ColliderEnter( Collider col )
            {
                if( col == null ) return;
                var box = col.GetComponent<HurtBox>();
                if( box == null ) return;
                var hc = box.healthComponent;
                if( hc == null || this.mask.Contains( hc ) ) return;
                if( !FriendlyFireManager.ShouldDirectHitProceed( hc, this.team ) ) return;
                if( hc == this.attackerBody.healthComponent ) return;

                var dist = (col.ClosestPoint(this.startPos) - this.startPos).magnitude;
                if( !this.bestHits.ContainsKey( hc ) || dist < this.bestHits[hc].distance )
                {
                    this.bestHits[hc] = new Hit( box, dist );
                }
            }


            public override void Tick( Single deltaT )
            {
                this.hitRb.MovePosition( Vector3.Lerp( this.startPos, this.targetPos, ( this.totalDuration - base.remainingDuration ) / this.totalDuration ) );
                Single curDist = this.speed * (this.totalDuration - base.remainingDuration );
                Single curMult = 1f;
                if( curDist > this.dist1 )
                {
                    curMult -= ( ( curDist - this.dist1 ) / this.dist2 ) * ( 1f - this.endFalloffMult );
                }

                foreach( var kv in this.bestHits )
                {
                    var hit = kv.Value;
                    var hc = kv.Key;
                    var box = hit.box;
                    if( box == null || hc == null ) continue;

                    DamageInfo dmg = new DamageInfo
                    {
                        attacker = this.attacker,
                        crit = this.crit,
                        damage = this.damage * curMult,
                        damageColorIndex = this.damageColor,
                        damageType = DamageType.Generic,
                        dotIndex = DotController.DotIndex.None,
                        force = this.forceVec * curMult,
                        inflictor = null,
                        position = box.transform.position,
                        procChainMask = default,
                        procCoefficient = this.procCoef * curMult,
                    };
                    dmg.ModifyDamageInfo( box.damageModifier );

                    ReinCore.NetworkingHelpers.DealDamage( dmg, box, true, true, true );

                    this.mask.Add( hc );

                    var targetBody = hc.body;
                    Inventory targetInv = (targetBody ? targetBody.inventory : null);

                    Single dur = curMult * this.chargeRestore * Main.WispPassiveController.GetMultiplierForBody(hc.body);
                    UInt32 stacks = 1u;
                    if( dur > 1f )
                    {
                        stacks = (UInt32)Mathf.RoundToInt( dur );
                        dur = 1f;
                    }

                    var delay = (curDist / this.speed) * 3.25f;
                    base.outer.StartCoroutine( this.DelayedBuff( delay * 0.9f, this.attackerBody, Main.RW_flameChargeBuff, dur, (Int32)stacks ) );

                    EffectData fx = new EffectData
                    {
                        origin = box.transform.position,
                        start = this.attacker.transform.position,
                        genericFloat = (curDist / this.speed) * 3.25f,
                        genericUInt = this.skin,
                        scale = 0.65f * curMult,
                        genericBool = false,
                    };
                    fx.SetHurtBoxReference( this.attacker );
                    EffectManager.SpawnEffect( Main.utilityLeech, fx, true );
                }

                this.bestHits.Clear();

                
            }
            public override void End()
            {
                Single curDist = this.speed * (this.totalDuration - base.remainingDuration );
                Single curMult = 1f;
                if( curDist > this.dist1 )
                {
                    curMult -= ( ( curDist - this.dist1 ) / this.dist2 ) * ( 1f - this.endFalloffMult );
                }


                if( this.stopAtWorld && this.worldHit )
                {
                    DamageInfo dmg = new DamageInfo
                    {
                        attacker = this.attacker,
                        crit = this.crit,
                        damage = this.damage * curMult,
                        damageColorIndex = this.damageColor,
                        damageType = DamageType.Generic,
                        dotIndex = DotController.DotIndex.None,
                        force = this.forceVec * curMult,
                        inflictor = null,
                        position = this.targetPos,
                        procChainMask = default,
                        procCoefficient = this.procCoef * curMult,
                    };

                    ReinCore.NetworkingHelpers.DealDamage( dmg, null, false, false, true );
                }
                this.trigger.Cleanup();
                HashSetPool<HealthComponent>.item = this.mask;
                this.mask = null;
                DictionaryPool<HealthComponent, Hit>.item = this.bestHits;
                this.bestHits = null;
            }

            private IEnumerator DelayedBuff( Single delay, CharacterBody body, BuffIndex buff, Single duration, Int32 stacks )
            {
                yield return new WaitForSeconds( delay );

                ReinCore.NetworkingHelpers.ApplyBuff( body, buff, stacks, duration );
            }

          


        }
    }
}