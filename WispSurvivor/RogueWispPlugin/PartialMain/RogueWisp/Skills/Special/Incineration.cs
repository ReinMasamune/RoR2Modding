#if ROGUEWISP
using System.Collections.Generic;
using System.Reflection;

using EntityStates;

using Rein.RogueWispPlugin.Helpers;

using RoR2;

using UnityEngine;
using UnityEngine.Networking;

namespace Rein.RogueWispPlugin
{

    internal partial class Main
    {
        public class Incineration : BaseState
        {
            public static System.Double chargePerTick = 20;

            public static System.Single baseMinDuration = 0.25f;
            public static System.Single baseDamageMult = 1.0f;
            public static System.Single baseProcCoef = 0.8f;
            public static System.Single baseTickFreq = 5f;
            public static System.Single baseMaxRange = 1000f;
            public static System.Single baseRadius = 2f;
            public static System.Single chargeScaler = 0.85f;
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
            private Transform beamEndSub;
            private readonly Collider[] beamCols = new Collider[512];
            private readonly HashSet<HealthComponent> beamMask = new HashSet<HealthComponent>();
            private readonly Dictionary<Collider, MemoizedGetComponent<HurtBox>> cacheBoxes = new Dictionary<Collider, MemoizedGetComponent<HurtBox>>();
            private MemoizedGetComponent<HurtBox> temp;
            private TeamIndex team;
            private WispAimAnimationController aimAnim;
            private BuffIndex armorBuff;

            private static readonly NetworkWriter write = new NetworkWriter();

            private readonly MethodInfo netStuff;


            public override void OnEnter()
            {
                base.OnEnter();

                Main.instance.RW_BlockSprintCrosshair.Add( base.characterBody );

                this.armorBuff = Main.RW_armorBuff;
                this.passive = this.gameObject.GetComponent<WispPassiveController>();
                this.skin = this.characterBody.skinIndex;
                this.aimAnim = this.GetComponent<WispAimAnimationController>();

                this.minDuration = baseMinDuration / this.attackSpeedStat;
                this.fireInterval = 1f / baseTickFreq;

                RoR2.Util.PlayScaledSound( "Play_titanboss_R_laser_loop", this.gameObject, 0.5f );

                this.muzzle = this.GetModelTransform().Find( "CannonPivot" ).Find( "BeamParent" );

                this.beamEffect = UnityEngine.Object.Instantiate<GameObject>( Main.specialBeam, this.muzzle.position, this.muzzle.rotation );
                this.beamEffect.GetComponent<BitSkinController>().Apply( WispBitSkin.GetWispSkin( this.skin ) );
                this.beamEffect.transform.parent = this.muzzle;
                this.beamEnd = this.beamEffect.transform.Find( "End" );
                //this.beamEndSub = this.beamEnd.Find( "Sub" );
                //this.beamEndSub.gameObject.SetActive( false );
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
                this.characterBody.isSprinting = true;


                this.fireTimer += Time.fixedDeltaTime * this.characterBody.attackSpeed;


                this.characterMotor.moveDirection = Vector3.zero;
                this.characterMotor.velocity = Vector3.zero;
                this.inputBank.moveVector = Vector3.zero;
                this.characterDirection.moveVector = Vector3.zero;

                this.characterBody.SetAimTimer( 0f );

                this.r = new Ray( this.muzzle.position, this.muzzle.forward );

                if( Physics.Raycast( this.r, out this.rh, baseMaxRange, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal ) )
                {
                    this.pos = this.rh.point;
                    //this.beamEndSub.gameObject.SetActive( true );
                } else
                {
                    this.pos = this.r.GetPoint( baseMaxRange );
                    //this.beamEndSub.gameObject.SetActive( false );
                }

                this.beamEnd.position = this.pos;

                if( this.fireTimer > this.fireInterval )
                {
                    this.Fire( this.passive.UseChargeDrain( chargePerTick, this.fireInterval / Mathf.Sqrt( base.characterBody.attackSpeed ), chargeScaler ) );
                    this.fireTimer -= this.fireInterval;
                }

                if( this.isAuthority && this.inputBank && ( !this.inputBank.skill4.down || this.passive.ReadCharge() < minChargeToUse ) && this.fixedAge >= this.minDuration )
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
                    this.Fire( this.passive.UseChargeDrain( chargePerTick, this.fireInterval / Mathf.Sqrt( base.characterBody.attackSpeed ), chargeScaler ) );
                }
                RoR2.Util.PlayScaledSound( "Stop_titanboss_R_laser_loop", this.gameObject, 5f );
                if( this.beamEffect )
                {
                    Destroy( this.beamEffect );
                }

                base.characterBody.RemoveBuff( this.armorBuff );
                base.characterMotor.useGravity = true;
                Main.instance.RW_BlockSprintCrosshair.Remove( base.characterBody );
            }

            public override InterruptPriority GetMinimumInterruptPriority() => InterruptPriority.Frozen;

            private void Fire( WispPassiveController.ChargeState state )
            {
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
                if( Util.CharacterRaycast( base.outer.gameObject, beam.aim, out beamRh, beam.range, LayerIndex.world.mask, QueryTriggerInteraction.UseGlobal ) )
                {
                    end = beamRh.point;
                    var dmg = new DamageInfo
                    {
                        attacker = gameObject,
                        crit = beam.crit,
                        damage = beam.damage,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        force = beam.aim.direction * beam.force,
                        inflictor = gameObject,
                        position = end,
                        procChainMask = default( ProcChainMask ),
                        procCoefficient = beam.procCoef
                    };
                    DoNetworkedDamage( dmg, null );
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
                //EffectData effect = new EffectData();
                for( System.Int32 i = 0; i < count; i++ )
                {
                    if( !this.beamCols[i] ) continue;
                    tempBox = this.dicCheck( this.beamCols[i] ).Get( this.beamCols[i].gameObject );
                    if( !tempBox ) continue;
                    tempHc = tempBox.healthComponent;
                    if( !tempHc || this.beamMask.Contains( tempHc ) || !FriendlyFireManager.ShouldDirectHitProceed( tempHc, this.team ) || tempHc == base.healthComponent ) continue;
                    this.beamMask.Add( tempHc );

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

                    //effect.origin = tempBox.transform.position;
                    //EffectManager.SpawnEffect( Main.genericImpactEffects[this.skin][0], effect, true );

                    DoNetworkedDamage( tempDmgInfo, tempBox );
                }
            }

            public MemoizedGetComponent<HurtBox> dicCheck( Collider c )
            {
                if( this.cacheBoxes.ContainsKey( c ) ) return this.cacheBoxes[c];

                this.cacheBoxes[c] = new MemoizedGetComponent<HurtBox>();
                return this.cacheBoxes[c];
            }

            public static void DoNetworkedDamage( DamageInfo info, HurtBox target )
            {
#if NETWORKING
                ReinCore.NetworkingHelpers.DealDamage( info, target, true, true, true );
#else
				HealthComponent targetHc = null;
				GameObject targetObj = null;

				if( target != null )
				{
					targetHc = target.healthComponent;
				}
				if( targetHc != null )
				{
					targetObj = targetHc.gameObject;
				}
				if( NetworkServer.active )
				{
					if( targetObj != null )
					{
						target.TakeDamage( info );
						GlobalEventManager.instance.OnHitEnemy( info, targetObj );
					}
					GlobalEventManager.instance.OnHitAll( info, targetObj );
				} else if( ClientScene.ready )
				{
					write.StartMessage( RoR2.Networking.UmsgType.BulletDamage );
					write.Write( targetObj );
					WriteDmgInfo( write, info );
					write.Write( targetObj != null );
					write.FinishMessage();
					ClientScene.readyConnection.SendWriter( write, (Int32)QosType.Reliable );
				}
#endif
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
                writer.Write( (System.Byte)( damageInfo.dotIndex + 1 ) );
            }
        }
    }
}
#endif