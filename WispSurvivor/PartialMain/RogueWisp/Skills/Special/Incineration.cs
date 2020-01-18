using EntityStates;
using RoR2;
using RoR2.Networking;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Networking;

namespace RogueWispPlugin
{
#if ROGUEWISP
    internal partial class Main
    {
        public class Incineration : BaseState
        {
            public static System.Double chargePerTick = 20.0;

            public static System.Single baseMinDuration = 1.0f;
            public static System.Single baseDamageMult = 1.0f;
            public static System.Single baseProcCoef = 0.8f;
            public static System.Single baseTickFreq = 5f;
            public static System.Single baseMaxRange = 1000f;
            public static System.Single baseRadius = 2f;
            public static System.Single chargeScaler = 0.9f;
            public static System.Single minChargeToUse = 50f;

            public Vector3 camPos1;
            public Vector3 camPos2;

            private System.Single minDuration;
            private System.Single fireInterval;

            private System.Single fireTimer = 0f;

            private System.UInt32 skin = 0;

            private Vector3 pos;
            private Ray r;

            private Transform muzzle;
            private RaycastHit rh;
            private WispPassiveController passive;
            private GameObject beamEffect;
            private Transform beamEnd;
            private Collider[] beamCols = new Collider[512];
            private Dictionary<HealthComponent, System.Boolean> beamMask = new Dictionary<HealthComponent, System.Boolean>();
            private Dictionary<Collider, MemoizedGetComponent<HurtBox>> cacheBoxes = new Dictionary<Collider, MemoizedGetComponent<HurtBox>>();
            private MemoizedGetComponent<HurtBox> temp;
            private TeamIndex team;
            private WispAimAnimationController aimAnim;
            private BuffIndex armorBuff;

            private static NetworkWriter write = new NetworkWriter();

            private MethodInfo netStuff;


            public override void OnEnter()
            {
                base.OnEnter();
                this.armorBuff = Main.instance.RW_armorBuff;
                this.passive = this.gameObject.GetComponent<WispPassiveController>();
                this.skin = this.characterBody.skinIndex;
                this.aimAnim = this.GetComponent<WispAimAnimationController>();

                this.minDuration = baseMinDuration / this.attackSpeedStat;
                this.fireInterval = 1f / baseTickFreq;

                RoR2.Util.PlayScaledSound( "Play_titanboss_R_laser_loop", this.gameObject, 5f );

                this.muzzle = this.GetModelTransform().Find( "CannonPivot" ).Find( "BeamParent" );

                this.beamEffect = UnityEngine.Object.Instantiate<GameObject>( Main.specialBeam[this.skin], this.muzzle.position, this.muzzle.rotation );
                this.beamEffect.transform.parent = this.muzzle;
                this.beamEnd = this.beamEffect.transform.Find( "End" );
                //Create the charge and beam effects
                this.pos = Vector3.zero;

                this.team = TeamComponent.GetObjectTeam( this.gameObject );

                this.characterBody.AddBuff( this.armorBuff );

                this.cameraTargetParams.idealLocalCameraPos = this.camPos1;
                this.characterMotor.useGravity = false;
            }

            public override void Update() => base.Update();//Update the charge and beam effects

            public override void FixedUpdate()
            {
                base.FixedUpdate();
                this.skillLocator.special.rechargeStopwatch = 0f;

                this.cameraTargetParams.idealLocalCameraPos = this.camPos1;

                this.fireTimer += Time.fixedDeltaTime * this.characterBody.attackSpeed;

                this.characterBody.isSprinting = true;
                this.characterMotor.moveDirection = Vector3.zero;
                this.characterMotor.velocity = Vector3.zero;
                this.inputBank.moveVector = Vector3.zero;
                this.characterDirection.moveVector = Vector3.zero;

                this.characterBody.SetAimTimer( 0f );

                this.r = new Ray( this.muzzle.position, this.muzzle.forward );

                if( Physics.Raycast( this.r, out this.rh, baseMaxRange, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal ) )
                {
                    this.pos = this.rh.point;
                } else
                {
                    this.pos = this.r.GetPoint( baseMaxRange );
                }

                this.beamEnd.position = this.pos;

                if( this.fireTimer > this.fireInterval )
                {
                    this.Fire( this.passive.UseChargeDrain( chargePerTick, this.fireInterval / this.characterBody.attackSpeed, chargeScaler ) );
                    this.fireTimer -= this.fireInterval;
                }

                if( this.isAuthority && this.inputBank && (!this.inputBank.skill4.down || this.passive.ReadCharge() < minChargeToUse) && this.fixedAge >= this.minDuration )
                {
                    this.outer.SetNextState( new IncinerationRecovery
                    {
                        camPos1 = camPos1,
                        camPos2 = camPos2
                    } );
                }
            }

            public override void OnExit()
            {
                base.OnExit();
                if( this.fireTimer >= this.fireInterval / 2f )
                {
                    this.Fire( this.passive.UseChargeDrain( chargePerTick, this.fireInterval / this.characterBody.attackSpeed, chargeScaler ) );
                }
                RoR2.Util.PlayScaledSound( "Stop_titanboss_R_laser_loop", this.gameObject, 5f );
                if( this.beamEffect )
                {
                    Destroy( this.beamEffect );
                }

                this.characterBody.RemoveBuff( this.armorBuff );
                this.characterMotor.useGravity = true;
            }

            public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Death;

            private void Fire( WispPassiveController.ChargeState state )
            {
                //Charge consumption


                if( !this.isAuthority ) return;

                Ray r = new Ray(this.muzzle.position, this.muzzle.forward);

                this.BeamAttack( new BeamAttackInfo
                {
                    aim = r,
                    range = baseMaxRange,
                    radius = baseRadius,
                    damage = this.damageStat * baseDamageMult * state.chargeScaler,
                    force = 100f,
                    procCoef = baseProcCoef,
                    crit = this.RollCrit()
                } );
            }

            private struct BeamAttackInfo
            {
                public Ray aim;
                public System.Single range;
                public System.Single radius;
                public System.Single damage;
                public System.Single force;
                public System.Single procCoef;
                public System.Boolean crit;
            }

            private void BeamAttack( BeamAttackInfo beam )
            {
                RaycastHit beamRh;

                Vector3 end = beam.aim.origin;

                //if( Physics.Raycast( beam.aim, out beamRh, beam.range, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal ) )
                if( Util.CharacterRaycast( base.outer.gameObject, beam.aim, out beamRh, beam.range, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal ))
                {
                    end = beamRh.point;
                } else
                {
                    end = beam.aim.GetPoint( beam.range );
                }

                System.Int32 count = Physics.OverlapCapsuleNonAlloc( beam.aim.origin, end, beam.radius, this.beamCols, LayerIndex.entityPrecise.mask, QueryTriggerInteraction.UseGlobal );

                this.beamMask.Clear();

                Collider tempCol;
                HurtBox tempBox;
                HealthComponent tempHc;
                DamageInfo tempDmgInfo;
                EffectData effect = new EffectData();
                for( System.Int32 i = 0; i < count; i++ )
                {
                    if( !this.beamCols[i] ) continue;
                    tempBox = this.dicCheck( this.beamCols[i] ).Get( this.beamCols[i].gameObject );
                    if( !tempBox ) continue;
                    tempHc = tempBox.healthComponent;
                    if( !tempHc || this.beamMask.ContainsKey( tempHc ) || TeamComponent.GetObjectTeam( tempHc.gameObject ) == this.team ) continue;
                    this.beamMask[tempHc] = true;

                    tempDmgInfo = new DamageInfo
                    {
                        attacker = gameObject,
                        crit = beam.crit,
                        damage = beam.damage,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        force = beam.aim.direction * beam.force,
                        inflictor = gameObject,
                        position = tempBox.transform.position,
                        procChainMask = default( ProcChainMask ),
                        procCoefficient = beam.procCoef
                    };

                    effect.origin = tempBox.transform.position;
                    EffectManager.SpawnEffect( Main.genericImpactEffects[this.skin][0], effect, true );

                    if( NetworkServer.active )
                    {
                        tempHc.TakeDamage( tempDmgInfo );
                        GlobalEventManager.instance.OnHitEnemy( tempDmgInfo, tempHc.gameObject );
                        GlobalEventManager.instance.OnHitAll( tempDmgInfo, tempHc.gameObject );
                    } else if( ClientScene.ready )
                    {
                        write.StartMessage( 53 );
                        write.Write( tempHc.gameObject );
                        WriteDmgInfo( write, tempDmgInfo );
                        write.Write( true );
                        write.FinishMessage();
                        ClientScene.readyConnection.SendWriter( write, QosChannelIndex.defaultReliable.intVal );
                    }
                }
            }

            public MemoizedGetComponent<HurtBox> dicCheck( Collider c )
            {
                if( this.cacheBoxes.ContainsKey( c ) ) return this.cacheBoxes[c];

                this.cacheBoxes[c] = new MemoizedGetComponent<HurtBox>();
                return this.cacheBoxes[c];
            }

            public static void WriteDmgInfo( NetworkWriter writer, DamageInfo damageInfo )
            {
                writer.Write( damageInfo.damage );
                writer.Write( damageInfo.crit );
                writer.Write( damageInfo.attacker );
                writer.Write( damageInfo.inflictor );
                writer.Write( damageInfo.position );
                writer.Write( damageInfo.force );
                writer.Write( damageInfo.procChainMask.mask );
                writer.Write( damageInfo.procCoefficient );
                writer.Write( (System.Byte)damageInfo.damageType );
                writer.Write( (System.Byte)damageInfo.damageColorIndex );
                writer.Write( (System.Byte)(damageInfo.dotIndex + 1) );
            }
        }
    }
#endif
}